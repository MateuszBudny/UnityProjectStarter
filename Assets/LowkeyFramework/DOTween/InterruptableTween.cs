using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptableTween
{
    private Tween tween;

    public Tween PlayInterruptable(Tween tweenToPlay, bool completeOnInterruption = false)
    {
        if(tweenToPlay == null)
            return null;

        tween?.Kill(completeOnInterruption);
        tween = tweenToPlay.Play();

        return tween;
    }
}
