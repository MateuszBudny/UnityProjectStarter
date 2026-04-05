using NodeCanvas.Framework;
using Alchemy.Inspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NodeCanvas.Framework.SignalDefinition;

public class AnalyticsFeeder : MonoBehaviour
{
    [SerializeField]
    private AnalyticsHandler analyticsHandler;
    [SerializeField]
    private List<AnalyticsRecord> analyticsRecords;

    private IAnalytics Analytics => analyticsHandler.Analytics;

    private Dictionary<SignalDefinition, InvokeArguments> signalHandlers;

    private bool IsNotPlaying => !Application.isPlaying;

    private void OnEnable()
    {
        signalHandlers = new Dictionary<SignalDefinition, InvokeArguments>();

        analyticsRecords.ForEach(record =>
        {
            void handler(Transform sender, Transform receiver, bool isGlobal, object[] args) =>
                OnEventSignalInvoked(record, sender, receiver, isGlobal, args);

            signalHandlers[record.eventSignal] = handler;
            record.eventSignal.onInvoke += handler;
        });
    }

    private void OnDisable()
    {
        analyticsRecords.ForEach(record =>
        {
            if(signalHandlers.TryGetValue(record.eventSignal, out InvokeArguments handler))
            {
                record.eventSignal.onInvoke -= handler;
            }
        });

        signalHandlers.Clear();
    }

    public static string GetComponentHierarchy(Component component)
    {
        if(component == null)
            return "null";

        return GetTransformHierarchy(component.transform);
    }

    public static string GetGameObjectHierarchy(GameObject go)
    {
        if(go == null)
            return "null";

        return GetTransformHierarchy(go.transform);
    }

    public static string GetTransformHierarchy(Transform transform)
    {
        if(transform == null)
            return "null";

        List<string> hierarchy = new List<string>();
        Transform current = transform;

        while(current != null)
        {
            hierarchy.Add(current.gameObject.name);
            current = current.parent;
        }

        return string.Join("/", hierarchy);
    }

    private void OnEventSignalInvoked(AnalyticsRecord record, Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>();
        eventData["Sender"] = sender != null ? GetTransformHierarchy(sender) : "null";

        if(args != null && args.Length > 0)
        {
            for(int i = 0; i < args.Length; i++)
            {
                eventData[$"{record.eventSignal.parameters[i].name}"] = args[i];
            }
        }

        string eventName = ConvertSignalNameToEventName(record.eventSignal.name);
        StartCoroutine(SendEventWhenAnalyticsAreInitializedEnumerator(eventName, eventData));
    }

    private IEnumerator SendEventWhenAnalyticsAreInitializedEnumerator(string eventName, Dictionary<string, object> eventData)
    {
        while(!analyticsHandler.IsAnalyticsInitialized)
        {
            yield return null;
        }

        Analytics.SendEvent(eventName, eventData);
    }

    private string ConvertSignalNameToEventName(string signalName)
    {
        string signalStandardSuffix = "_Signal";
        if(signalName.EndsWith(signalStandardSuffix))
        {
            return signalName[..^signalStandardSuffix.Length];
        }

        return signalName;
    }

    [Serializable]
    private struct AnalyticsRecord
    {
        [HorizontalGroup("InvokeButton")]
        public SignalDefinition eventSignal;

        [Button]
        [DisableInEditMode]
        [HorizontalGroup("InvokeButton")]
        public void InvokeSignal()
        {
            object[] args = GenerateParametersSchema();
            eventSignal.Invoke(null, null, true, args);
        }

        public object[] GenerateParametersSchema()
        {
            object[] args = new object[eventSignal.parameters.Count];
            for(int i = 0; i < args.Length; i++)
            {
                Type paramType = eventSignal.parameters[i].type;
                args[i] = paramType.IsValueType ? Activator.CreateInstance(paramType) : null;
            }

            return args;
        }
    }

#if UNITY_EDITOR
    [Button]
    [DisableInEditMode]
    private void SendAllEvents()
    {
        analyticsRecords.ForEach(record =>
        {
            object[] args = record.GenerateParametersSchema();
            OnEventSignalInvoked(record, null, null, true, args);
        });

        Debug.Log($"All events sent.");
    }
#endif
}
