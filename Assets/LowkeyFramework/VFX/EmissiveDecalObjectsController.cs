using DG.Tweening;
using Newtonsoft.Json.Bson;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissiveDecalObjectsController : MonoBehaviour
{
    [SerializeField]
    private bool isChangingEmissivePropertyOnSignalActive = true;
    [SerializeField]
    private SignalDefinition startEmissivePropertyChangingSignal;
    [SerializeField]
    private SignalDefinition stopEmissivePropertyChangingSignal;
    [SerializeField]
    private List<EmissiveDecalObject> emissiveDecals;
    [SerializeField]
    private List<EmissiveDecalObjectChangeOnEventRecord> changesOnEvents;

    private void Awake()
    {
        changesOnEvents.ForEach(changes => changes.signalForChange.onInvoke += OnSignalForChangePropertyValue);
        if(stopEmissivePropertyChangingSignal)
        {
            stopEmissivePropertyChangingSignal.onInvoke += OnSignalToStopChangingEmissiveProperty;
        }
        if(startEmissivePropertyChangingSignal)
        {
            startEmissivePropertyChangingSignal.onInvoke += OnSignalToStartChangingEmissiveProperty;
        }
    }

    private void Start()
    {
        emissiveDecals.ForEach(decal =>
        {
            decal.DecalProjector.fadeFactor = 0f;
            decal.EmissiveStrength = 0f;
        });
    }

    private void OnDestroy()
    {
        changesOnEvents.ForEach(changes => changes.signalForChange.onInvoke -= OnSignalForChangePropertyValue);
        if(stopEmissivePropertyChangingSignal)
        {
            stopEmissivePropertyChangingSignal.onInvoke -= OnSignalToStopChangingEmissiveProperty;
        }
        if(startEmissivePropertyChangingSignal)
        {
            startEmissivePropertyChangingSignal.onInvoke -= OnSignalToStartChangingEmissiveProperty;
        }
    }

    private void OnSignalForChangePropertyValue(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        if(!isChangingEmissivePropertyOnSignalActive)
            return;

        changesOnEvents.ForEach(changes =>
        {
            emissiveDecals.ForEach(decal =>
            {
                changes.ChangeValueSmooth(decal);
            });
        });
    }

    private void OnSignalToStartChangingEmissiveProperty(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        isChangingEmissivePropertyOnSignalActive = true;
    }

    private void OnSignalToStopChangingEmissiveProperty(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        isChangingEmissivePropertyOnSignalActive = false;
    }

    [Serializable]
    private class EmissiveDecalObjectChangeOnEventRecord
    {
        public EmissiveDecalObjectProperty propertyToChange;
        public SignalDefinition signalForChange;
        [Tooltip("How much should be added to the chosen EmissiveDecalObject property.")]
        public float changeValue;
        public float changeDuration = 1f;
        public Ease changeEase = Ease.Linear;

        private Dictionary<EmissiveDecalObject, InterruptableTween> tweens = new Dictionary<EmissiveDecalObject, InterruptableTween>();

        public void ChangeValueSmooth(EmissiveDecalObject decal)
        {
            if(!tweens.ContainsKey(decal))
            {
                tweens[decal] = new InterruptableTween();
            }

            Accessor<float> propertyAccessor = propertyToChange switch
            {
                EmissiveDecalObjectProperty.FadeFactor => new Accessor<float>(() => decal.DecalProjector.fadeFactor),
                EmissiveDecalObjectProperty.EmissiveStrength => new Accessor<float>(() => decal.EmissiveStrength),
                _ => throw new ArgumentOutOfRangeException(),
            };

            tweens[decal].PlayInterruptable(DOTween.To(propertyAccessor.Get, propertyAccessor.Set, Mathf.Clamp01(propertyAccessor.Get() + changeValue), changeDuration));
        }
    }

    private enum EmissiveDecalObjectProperty
    {
        FadeFactor,
        EmissiveStrength
    }
}