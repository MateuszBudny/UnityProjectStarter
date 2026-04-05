using DG.Tweening;
using Alchemy.Inspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TweenController : MonoBehaviour
{
    [SerializeField]
    private TweenType tweenType;
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    [ShowIf(nameof(ShowShake))]
    [Tooltip("Shake instead of normal target tween. This only works for Move, Rotate and Scale tweens.")]
    private bool shake = false;
    [SerializeField]
    [ShowIf(nameof(ShowEase))]
    private Ease ease = Ease.Linear;
    [SerializeField]
    [ShowIf(nameof(ShowTargetAsObject))]
    [FormerlySerializedAs("targetAsTransform")]
    private bool targetAsObject;
    [SerializeField]
    [ShowIf(nameof(ShowLocal))]
    [Tooltip("This does nothing for scaling tweens - all scale tweens are local.")]
    private bool local;
    [SerializeField]
    [FormerlySerializedAs("useThisTransformToTween")]
    private bool useThisObjectToTween = true;

    [SerializeField]
    [ShowIf(nameof(ShowTransformToTween))]
    private Transform otherTransformToTween;

    [SerializeField]
    [ShowIf(nameof(ShowColorComponentType))]
    private ColorComponentType colorComponentType;
    [SerializeField]
    [ShowIf(nameof(ShowTextMeshProColorComponentToTween))]
    private TMP_Text otherTextMeshProToTween;
    [SerializeField]
    [ShowIf(nameof(ShowSpriteColorComponentToTween))]
    private SpriteRenderer otherSpriteToTween;
    [SerializeField]
    [ShowIf(nameof(ShowImageColorComponentToTween))]
    private Image otherImageToTween;

    [SerializeField]
    [ShowIf(nameof(ShowCanvasGroupToTween))]
    private CanvasGroup otherCanvasGroupToTween;

    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTransformTarget))]
    private Transform targetTransform;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTextMeshProTarget))]
    private TMP_Text targetTextMeshPro;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowSpriteTarget))]
    private SpriteRenderer targetSprite;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowImageTarget))]
    private Image targetImage;

    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetPosition))]
    private Vector3 targetPosition;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowUseEulerForRotation))]
    private bool useEulerForRotation;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetQuaternionRotation))]
    private Vector4 targetQuaternionRotation;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetEulerRotation))]
    private Vector3 targetEulerRotation;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetScale))]
    private Vector3 targetScale;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetColor))]
    private Color targetColor;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetAlpha))]
    private float targetAlpha;

    [BoxGroup("Shake Parameters")]
    [SerializeField]
    [ShowIf(nameof(IsShakeActivated))]
    private bool simpleShakeStrength = true;
    [BoxGroup("Shake Parameters")]
    [SerializeField]
    [ShowIf(nameof(ShowShakeStrengthAsFloat))]
    private float shakeStrength = 1f;
    [BoxGroup("Shake Parameters")]
    [SerializeField]
    [ShowIf(nameof(ShowShakeStrengthAsVector3))]
    private Vector3 shakeStrengthVector3 = new Vector3(1f, 1f, 1f);
    [BoxGroup("Shake Parameters")]
    [SerializeField]
    [ShowIf(nameof(IsShakeActivated))]
    private int shakeVibrato = 10;
    [BoxGroup("Shake Parameters")]
    [SerializeField]
    [ShowIf(nameof(IsShakeActivated))]
    private float shakeRandomness = 90f;
    [BoxGroup("Shake Parameters")]
    [SerializeField]
    [ShowIf(nameof(IsShakeActivated))]
    private bool shakeFadeOut = true;
    [BoxGroup("Shake Parameters")]
    [SerializeField]
    [ShowIf(nameof(ShowShakeSnapping))]
    private bool shakeSnapping = false;

    public Ease Ease { get => ease; set => ease = value; }
    public Transform TargetTransform { get => targetTransform; set => targetTransform = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public bool UseEulerForRotation { get => useEulerForRotation; set => useEulerForRotation = value; }
    public Vector4 TargetQuaternionRotation { get => targetQuaternionRotation; set => targetQuaternionRotation = value; }
    public Vector3 TargetEulerRotation { get => targetEulerRotation; set => targetEulerRotation = value; }
    public Vector3 TargetScale { get => targetScale; set => targetScale = value; }
    public Color TargetColor { get => targetColor; set => targetColor = value; }
    public float TargetAlpha { get => targetAlpha; set => targetAlpha = value; }

    private bool ShowShake => tweenType.HasFlag(TweenType.Move) || tweenType.HasFlag(TweenType.Rotate) || tweenType.HasFlag(TweenType.Scale);
    private bool ShowEase => !IsShakeActivated || tweenType.HasFlag(TweenType.Color);
    private bool ShowTargetAsObject => !IsShakeActivated;
    private bool ShowLocal => !tweenType.HasFlag(TweenType.Scale) && !tweenType.HasFlag(TweenType.Color) && !tweenType.HasFlag(TweenType.CanvasGroupAlpha) && !IsShakeActivated;
    private bool ShowTransformToTween => !useThisObjectToTween && IsTransformBasedTween;
    private bool ShowColorComponentType => tweenType.HasFlag(TweenType.Color);
    private bool ShowTextMeshProColorComponentToTween => !useThisObjectToTween && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.TextMeshPro);
    private bool ShowSpriteColorComponentToTween => !useThisObjectToTween && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.Sprite);
    private bool ShowImageColorComponentToTween => !useThisObjectToTween && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.Image);

    private bool ShowCanvasGroupToTween => !useThisObjectToTween && tweenType.HasFlag(TweenType.CanvasGroupAlpha) && !IsShakeActivated;

    private bool ShowTransformTarget => targetAsObject && IsTransformBasedTween && !IsShakeActivated;
    private bool ShowTextMeshProTarget => targetAsObject && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.TextMeshPro);
    private bool ShowSpriteTarget => targetAsObject && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.Sprite);
    private bool ShowImageTarget => targetAsObject && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.Image);

    private bool ShowTargetPosition => !targetAsObject && tweenType.HasFlag(TweenType.Move) && !IsShakeActivated;
    private bool ShowUseEulerForRotation => !targetAsObject && tweenType.HasFlag(TweenType.Rotate) && !IsShakeActivated;
    private bool ShowTargetQuaternionRotation => !targetAsObject && !useEulerForRotation && tweenType.HasFlag(TweenType.Rotate) && !IsShakeActivated;
    private bool ShowTargetEulerRotation => !targetAsObject && useEulerForRotation && tweenType.HasFlag(TweenType.Rotate) && !IsShakeActivated;
    private bool ShowTargetScale => !targetAsObject && tweenType.HasFlag(TweenType.Scale) && !IsShakeActivated;
    private bool ShowTargetColor => !targetAsObject && tweenType.HasFlag(TweenType.Color) && !IsShakeActivated;
    private bool ShowTargetAlpha => !targetAsObject && tweenType.HasFlag(TweenType.CanvasGroupAlpha) && !IsShakeActivated;

    private bool ShowShakeStrengthAsFloat => IsShakeActivated && simpleShakeStrength;
    private bool ShowShakeStrengthAsVector3 => IsShakeActivated && !simpleShakeStrength;
    private bool ShowShakeSnapping => IsShakeActivated && tweenType.HasFlag(TweenType.Move);

    private bool IsShakeActivated => ShowShake && shake;
    private bool IsLocalTween => local;

    private Transform TransformToTween => useThisObjectToTween ? transform : otherTransformToTween;
    private TMP_Text TextMeshProToTween => useThisObjectToTween ? GetComponent<TMP_Text>() : otherTextMeshProToTween;
    private SpriteRenderer SpriteToTween => useThisObjectToTween ? GetComponent<SpriteRenderer>() : otherSpriteToTween;
    private Image ImageToTween => useThisObjectToTween ? GetComponent<Image>() : otherImageToTween;
    private CanvasGroup CanvasGroupToTween => useThisObjectToTween ? GetComponent<CanvasGroup>() : otherCanvasGroupToTween;
    private bool IsTransformBasedTween => tweenType.HasFlag(TweenType.Move) || tweenType.HasFlag(TweenType.Scale) || tweenType.HasFlag(TweenType.Rotate);

    private Sequence currentSequence;

    public void Play() => PlayTween(null);

    public Tween PlayTween() => PlayTween(null);

    public Tween PlayTween(Action onComplete)
    {
        currentSequence?.Kill();
        currentSequence = DOTween.Sequence();

        bool onCompleteInvoked = false;
        void onSequenceComplete()
        {
            if(onCompleteInvoked)
                return;

            onCompleteInvoked = true;
            onComplete?.Invoke();
        }

        if(tweenType.HasFlag(TweenType.Move))
        {
            if(IsShakeActivated)
            {
                if(simpleShakeStrength)
                {
                    currentSequence.Insert(0f, TransformToTween.DOShakePosition(duration, shakeStrength, shakeVibrato, shakeRandomness, shakeSnapping, shakeFadeOut).OnComplete(onSequenceComplete));
                }
                else
                {
                    currentSequence.Insert(0f, TransformToTween.DOShakePosition(duration, shakeStrengthVector3, shakeVibrato, shakeRandomness, shakeSnapping, shakeFadeOut).OnComplete(onSequenceComplete));
                }
            }
            else
            {
                Vector3 target = targetAsObject ? (IsLocalTween ? TargetTransform.localPosition : TargetTransform.position) : targetPosition;
                if(IsLocalTween)
                {
                    currentSequence.Insert(0f, TransformToTween.DOLocalMove(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
                else
                {
                    currentSequence.Insert(0f, TransformToTween.DOMove(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
            }
        }

        if(tweenType.HasFlag(TweenType.Rotate))
        {
            if(IsShakeActivated)
            {
                if(simpleShakeStrength)
                {
                    currentSequence.Insert(0f, TransformToTween.DOShakeRotation(duration, shakeStrength, shakeVibrato, shakeRandomness, shakeFadeOut).OnComplete(onSequenceComplete));
                }
                else
                {
                    currentSequence.Insert(0f, TransformToTween.DOShakeRotation(duration, shakeStrengthVector3, shakeVibrato, shakeRandomness, shakeFadeOut).OnComplete(onSequenceComplete));
                }
            }
            else if(useEulerForRotation)
            {
                Vector3 target = targetAsObject ? (IsLocalTween ? TargetTransform.localEulerAngles : TargetTransform.eulerAngles) : targetEulerRotation;
                if(IsLocalTween)
                {
                    currentSequence.Insert(0, TransformToTween.DOLocalRotate(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
                else
                {
                    currentSequence.Insert(0, TransformToTween.DORotate(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
            }
            else
            {
                Quaternion target = targetAsObject ? TargetTransform.rotation : new Quaternion(targetQuaternionRotation.x, targetQuaternionRotation.y, targetQuaternionRotation.z, targetQuaternionRotation.w);
                if(IsLocalTween)
                {
                    currentSequence.Insert(0, TransformToTween.DOLocalRotateQuaternion(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
                else
                {
                    currentSequence.Insert(0, TransformToTween.DORotateQuaternion(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
            }
        }

        if(tweenType.HasFlag(TweenType.Scale))
        {
            if(IsShakeActivated)
            {
                if(simpleShakeStrength)
                {
                    currentSequence.Insert(0f, TransformToTween.DOShakeScale(duration, shakeStrength, shakeVibrato, shakeRandomness, shakeFadeOut).OnComplete(onSequenceComplete));
                }
                else
                {
                    currentSequence.Insert(0f, TransformToTween.DOShakeScale(duration, shakeStrengthVector3, shakeVibrato, shakeRandomness, shakeFadeOut).OnComplete(onSequenceComplete));
                }
            }
            else
            {
                Vector3 target = targetAsObject ? targetTransform.localScale : TargetScale;
                currentSequence.Insert(0, TransformToTween.DOScale(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
            }
        }

        if(tweenType.HasFlag(TweenType.Color))
        {
            Color target;
            if(colorComponentType == ColorComponentType.TextMeshPro)
            {
                target = targetAsObject ? targetTextMeshPro.color : TargetColor;
                currentSequence
                    .Insert(0, DOTween.To(() => TextMeshProToTween.color, value => TextMeshProToTween.color = value, target, duration)
                        .SetEase(ease)
                        .OnComplete(onSequenceComplete));
            }
            if(colorComponentType == ColorComponentType.Sprite)
            {
                target = targetAsObject ? targetSprite.color : TargetColor;
                currentSequence
                    .Insert(0, DOTween.To(() => SpriteToTween.color, value => SpriteToTween.color = value, target, duration)
                        .SetEase(ease)
                        .OnComplete(onSequenceComplete));

            }
            if(colorComponentType == ColorComponentType.Image)
            {
                target = targetAsObject ? targetImage.color : TargetColor;
                currentSequence
                    .Insert(0, DOTween.To(() => ImageToTween.color, value => ImageToTween.color = value, target, duration)
                        .SetEase(ease)
                        .OnComplete(onSequenceComplete));
            }
        }

        if(tweenType.HasFlag(TweenType.CanvasGroupAlpha))
        {
            float target = targetAsObject ? otherCanvasGroupToTween.alpha : TargetAlpha;
            currentSequence
                    .Insert(0, DOTween.To(() => CanvasGroupToTween.alpha, value => CanvasGroupToTween.alpha = value, target, duration)
                        .SetEase(ease)
                        .OnComplete(onSequenceComplete));
        }

        return currentSequence.Play();
    }

    [Button]
    private void OpenEasesVisualization()
    {
        Application.OpenURL("https://p1-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/862a7322e00e446f806517dc8c7edf4e~tplv-k3u1fbpfcp-zoom-in-crop-mark:4536:0:0:0.image");
    }

    [Flags]
    public enum TweenType
    {
        None = 0,
        Move = 1,
        Rotate = 1 << 1,
        Scale = 1 << 2,
        Color = 1 << 3,
        CanvasGroupAlpha = 1 << 4,
    }

    [Flags]
    public enum ColorComponentType
    {
        None = 0,
        TextMeshPro = 1,
        Sprite = 1 << 2,
        Image = 1 << 3,
    }
}
