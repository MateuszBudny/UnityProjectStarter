using System.Collections;
using UnityEngine;

public class CursorManager : SingleBehaviour<CursorManager>
{
    [SerializeField]
    private CursorLockMode startingCursorLockMode = CursorLockMode.Locked;

    public CursorLockMode CurrentCursorLockMode
    {
        get => currentCursorLockMode;
        set
        {
            currentCursorLockMode = value;
            if(!controllerMode)
            {
                Cursor.lockState = value;
            }
        }
    }

    private CursorLockMode currentCursorLockMode = CursorLockMode.Locked;
    private bool controllerMode = false;

    private void Start()
    {
        CurrentCursorLockMode = startingCursorLockMode;
    }

    public void ControllerMode(bool isUsingController)
    {
        controllerMode = isUsingController;
        if(controllerMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CurrentCursorLockMode;
        }
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus)
        {
            Cursor.lockState = CurrentCursorLockMode;
        }
    }
}
