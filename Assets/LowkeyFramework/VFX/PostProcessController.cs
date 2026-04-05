using Unity.Cinemachine;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
    [SerializeField]
    private Volume volume;

    [Header("Vignette")]
    [SerializeField]
    [Range(0f, 2f)]
    private float minVignetteValue = 1f;
    [SerializeField]
    [Range(0f, 2f)]
    private float maxVignetteValue = 2f;
    [SerializeField]
    private float vignetteDefaultChangeDuration = 1.5f;
    [SerializeField]
    private Material insanityMaterial;
    [SerializeField]
    private AnimationCurve insanityIncreaseCurve;
    [SerializeField]
    private AnimationCurve insanityDecreaseCurve;

    [Header("Color adjustments")]
    [SerializeField]
    private AnimationCurve postExposureSinglePulseCurve;
    [SerializeField]
    private float postExposureSinglePulseDuration = 1f;
    [SerializeField]
    private float postExposureSinglePulseMaxValue = 2f;
    [SerializeField]
    private float chromaticAberrationSinglePulseMaxValue = 0.85f;

    [Header("Camera shake")]
    [SerializeField]
    private CinemachineNoiseMovement vCamNoiseMovement;

    private float InsanityMaterialIntensity
    {
        get => insanityMaterial.GetFloat("_VignetteIntensity");
        set => insanityMaterial.SetFloat("_VignetteIntensity", Mathf.Clamp(value, 0f, 10f));
    }

    private Color InsanityMaterialColor
    {
        get => insanityMaterial.GetColor("_VignietteColor");
        set => insanityMaterial.SetColor("_VignietteColor", value);
    }

    private Vignette vignette;
    private ColorAdjustments colorAdjustments;
    private ChromaticAberration chromaticAberration;

    private Tween vignetteColorTween;
    private Sequence vignetteIntensitySequence;
    private Sequence pulsingVignetteSequence;
    private Sequence postExposureSinglePulseSequence;

    private void Start()
    {
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out chromaticAberration);
    }

    private void OnDestroy()
    {
        InsanityMaterialIntensity = 0f;
        pulsingVignetteSequence?.Kill(true);
        vignetteColorTween?.Kill(true);
    }

    public void Init(Func<CinemachineVirtualCamera> getCurrentVCam)
    {
        vCamNoiseMovement.Init(getCurrentVCam);
    }

    public void SetVignetteSmoothlyAsMaxPercentage(float endValueAsMaxPercentage, float duration = -1f)
    {
        bool addPulsing = endValueAsMaxPercentage > 0.75f;
        SetVignetteSmoothly(Mathf.Lerp(minVignetteValue, maxVignetteValue, endValueAsMaxPercentage), addPulsing, duration);
    }

    public void SetVignetteSmoothly(float endValue, bool addPulsing = false, float duration = -1f)
    {
        vignetteIntensitySequence?.Kill(true);
        pulsingVignetteSequence?.Kill(true);
        vignetteIntensitySequence = DOTween.Sequence();
        float vignetteIntensityDelay = endValue > InsanityMaterialIntensity ? 0f : 0.75f;
        AnimationCurve vignetteIntensityEase = endValue > InsanityMaterialIntensity ? insanityIncreaseCurve : insanityDecreaseCurve;
        duration = Mathf.Approximately(duration, -1f) ? vignetteDefaultChangeDuration : duration;

        if(endValue > InsanityMaterialIntensity)
        {
            vignetteIntensitySequence.Insert(vignetteIntensityDelay, DOTween.To(() => InsanityMaterialIntensity, (value) => InsanityMaterialIntensity = value, endValue, duration)
            .SetEase(vignetteIntensityEase)
            .OnComplete(() =>
            {
                if(addPulsing)
                {
                    pulsingVignetteSequence = DOTween.Sequence();
                    pulsingVignetteSequence.Append(DOTween.To(() => InsanityMaterialIntensity, (value) => InsanityMaterialIntensity = value, endValue - 0.3f, 0.8f));
                    pulsingVignetteSequence.Append(DOTween.To(() => InsanityMaterialIntensity, (value) => InsanityMaterialIntensity = value, endValue, 0.1f));
                    pulsingVignetteSequence.SetLoops(-1);
                    pulsingVignetteSequence.Play();
                }
            }))
            .Play();
        }
        else
        {
            vignetteIntensitySequence.Insert(vignetteIntensityDelay, DOTween.To(() => InsanityMaterialIntensity, (value) => InsanityMaterialIntensity = value, 0f, vignetteDefaultChangeDuration)
                .SetEase(Ease.OutQuad));
            vignetteIntensitySequence.Insert(vignetteIntensityDelay + vignetteDefaultChangeDuration - 0.5f, DOTween.To(() => InsanityMaterialIntensity, (value) => InsanityMaterialIntensity = value, endValue, vignetteDefaultChangeDuration / 1.5f)
                .SetEase(Ease.Linear));
            vignetteIntensitySequence.Play();
        }
    }

    public void SetVignette(float value)
    {
        vignette.intensity.value = value;
    }

    public void SetVCamNoiseMovementProgress(float progress)
    {
        vCamNoiseMovement.UpdateProgress(progress);
    }

    public void PlayVignetteLoweringSaturationAndBack()
    {
        vignetteColorTween?.Kill(true);
        Color.RGBToHSV(InsanityMaterialColor, out float startingHue, out float startingSaturation, out float startingBrightness);
        vignetteColorTween = DOTween.To(() =>
        {
            Color.RGBToHSV(InsanityMaterialColor, out float _, out float saturation, out float __);
            return saturation;
        }, value => InsanityMaterialColor = Color.HSVToRGB(startingHue, value, startingBrightness), 0f, 1.75f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => InsanityMaterialColor = Color.HSVToRGB(startingHue, startingSaturation, startingBrightness));
    }

    public void PlayPostExposureSinglePulse()
    {
        postExposureSinglePulseSequence?.Kill(true);
        postExposureSinglePulseSequence = DOTween.Sequence();
        postExposureSinglePulseSequence.Append(DOTween.To(() => colorAdjustments.postExposure.value, value => colorAdjustments.postExposure.value = value, postExposureSinglePulseMaxValue, postExposureSinglePulseDuration)
            .SetEase(postExposureSinglePulseCurve));
        postExposureSinglePulseSequence.Insert(0f, DOTween.To(() => chromaticAberration.intensity.value, value => chromaticAberration.intensity.value = value, chromaticAberrationSinglePulseMaxValue, postExposureSinglePulseDuration)
            .SetEase(postExposureSinglePulseCurve));
        postExposureSinglePulseSequence.Play();
    }
}
