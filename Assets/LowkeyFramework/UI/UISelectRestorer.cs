using NodeCanvas.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UISelectRestorer : MonoBehaviour
{
    [SerializeField]
    private SignalDefinition inputDeviceChangedSignal;

    private EventSystem CurrentEventSystem => EventSystem.current;

    private InputDevice currentInputDevice;
    private GameObject lastSelected;

    private void OnEnable()
    {
        inputDeviceChangedSignal.onInvoke += OnInputDeviceChanged;
    }

    private void OnDisable()
    {
        inputDeviceChangedSignal.onInvoke -= OnInputDeviceChanged;
    }

    void Update()
    {
        // Save last selected while navigating with controller
        if(CurrentEventSystem.currentSelectedGameObject)
        {
            lastSelected = CurrentEventSystem.currentSelectedGameObject;
        }

        if(currentInputDevice is Mouse)
        {
            CurrentEventSystem.SetSelectedGameObject(null);
        }
    }

    private void OnInputDeviceChanged(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        currentInputDevice = args[0] as InputDevice;
        if(currentInputDevice is Keyboard || currentInputDevice is Gamepad)
        {
            // Restore last selected when switching to controller
            if(lastSelected != null && CurrentEventSystem.currentSelectedGameObject == null)
            {
                CurrentEventSystem.SetSelectedGameObject(lastSelected);
            }
        }
        else
        {
            CurrentEventSystem.SetSelectedGameObject(null);
        }
    }
}
