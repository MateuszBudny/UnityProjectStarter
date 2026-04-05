using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : SingleBehaviour<BlackScreen>
{
    [SerializeField]
    private bool showBlackScreenOnStart = true;

    private Image blackImage;

    protected override void Awake()
    {
        base.Awake();
        blackImage = GetComponent<Image>();

        if(showBlackScreenOnStart)
        {
            FadeIn(0f);
        }
        else
        {
            FadeOut(0f);
        }
    }

    /// <summary>
    /// Black screen fades in (from transparent to black)
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="forceStartFromBeginning"></param>
    /// <param name="onComplete"></param>
    public void FadeIn(float duration, bool forceStartFromBeginning = false, Action onComplete = null)
    {
        Fade(duration, 1f, forceStartFromBeginning, onComplete);
    }

    /// <summary>
    /// Black screen fades out (from black to transparent)
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="forceStartFromBeginning"></param>
    /// <param name="onComplete"></param>
    public void FadeOut(float duration, bool forceStartFromBeginning = false, Action onComplete = null)
    {
        Fade(duration, 0f, forceStartFromBeginning, onComplete);
    }

    private void Fade(float duration, float toAlphaValue, bool forceStartFromBeginning, Action onComplete)
    {
        DOTween.Kill(this);

        if(forceStartFromBeginning)
        {
            blackImage.color = new Color(0f, 0f, 0f, 1f - toAlphaValue);
        }

        DOTween.To(() => blackImage.color.a, setter => blackImage.color = new Color(0f, 0f, 0f, setter), toAlphaValue, duration)
            .SetEase(Ease.InOutSine)
            .SetTarget(this)
            .OnComplete(() => onComplete?.Invoke());
    }
}
