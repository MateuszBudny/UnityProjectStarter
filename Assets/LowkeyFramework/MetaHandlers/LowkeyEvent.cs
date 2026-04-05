using NodeCanvas.Framework;
using Alchemy.Inspector;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class LowkeyEvent
{
    [SerializeField]
    private EventType eventType = EventType.Signal;
    [SerializeField]
    [ShowIf(nameof(IsEventTypeSignal))]
    private SignalDefinition signal;
    [SerializeField]
    [ShowIf(nameof(IsEventTypeUnity))]
    private UnityEvent unityEvent;

    private bool IsEventTypeUnity => eventType == EventType.UnityEvent;
    private bool IsEventTypeSignal => eventType == EventType.Signal;

    public void AddListener(Action onInvoke)
    {
        switch(eventType)
        {
            case EventType.UnityEvent:
                unityEvent.AddListener(() => onInvoke());
                break;
            case EventType.Signal:
                signal.onInvoke += (_, _, _, _) => onInvoke();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
        }
    }

    // TODO: test if this naive solution works. I have doubts about it.
    public void RemoveListener(Action onInvoke)
    {
        switch(eventType)
        {
            case EventType.UnityEvent:
                unityEvent.RemoveListener(() => onInvoke());
                break;
            case EventType.Signal:
                signal.onInvoke -= (_, _, _, _) => onInvoke();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
        }
    }

    public void Invoke(params object[] args)
    {
        switch(eventType)
        {
            case EventType.UnityEvent:
                unityEvent.Invoke();
                break;
            case EventType.Signal:
                signal.Invoke(null, null, true, args);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
        }
    }

    private enum EventType
    {
        UnityEvent,
        Signal,
    }
}
