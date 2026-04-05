using NodeCanvas.Framework;
using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalInvoker : MonoBehaviour
{
    [SerializeField]
    private SignalDefinition signalToInvoke;
    [SerializeField]
    private bool useCooldown;
    [ShowIf(nameof(useCooldown))]
    [SerializeField]
    private float cooldownDuration = 1f;

    private float invokedTimestamp = -1f;

    public void InvokeSignal()
    {
        if(useCooldown && Time.time < invokedTimestamp + cooldownDuration)
            return;

        signalToInvoke.Invoke(transform, null, true);
        invokedTimestamp = Time.time;
    }
}
