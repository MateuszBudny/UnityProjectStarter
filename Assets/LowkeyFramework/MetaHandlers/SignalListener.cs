using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    [SerializeField]
    private bool listenEvenIfObjectIsInactive;
    [SerializeField]
    private SignalDefinition signalToListenFor;
    [SerializeField]
    private UnityEvent onSignalInvoked;

    private bool initialized = false;

    private void OnEnable()
    {
        if(initialized && listenEvenIfObjectIsInactive)
            return;

        signalToListenFor.onInvoke += OnSignalInvoked;
        initialized = true;
    }

    private void OnDisable()
    {
        if(!listenEvenIfObjectIsInactive)
            return;

        signalToListenFor.onInvoke -= OnSignalInvoked;
    }

    private void OnSignalInvoked(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        onSignalInvoked.Invoke();
    }
}
