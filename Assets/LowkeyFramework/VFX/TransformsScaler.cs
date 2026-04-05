using Alchemy.Inspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TransformsScaler : MonoBehaviour
{
    [SerializeField]
    private List<TransformToScaleRecord> transformsToScale;

    public void SetTransformsScale(float scale)
    {
        transformsToScale.ForEach(t => t.Scale(scale));
    }

    [Serializable]
    private class TransformToScaleRecord
    {
        public Transform transformToScale;
        public bool useConstScalingMultiplier = true;
        [ShowIf(nameof(useConstScalingMultiplier))]
        public float scalingMultiplier = 1f;
        [HideIf(nameof(useConstScalingMultiplier))]
        public AnimationCurve scalingCurveMultiplier = AnimationCurve.Constant(0f, 1f, 1f);

        public void Scale(float scale)
        {
            float scaleMultiplied = useConstScalingMultiplier ? scale * scalingMultiplier : scale * scalingCurveMultiplier.Evaluate(scale);
            transformToScale.localScale = new Vector3(scaleMultiplied, scaleMultiplied, scaleMultiplied);
        }
    }
}
