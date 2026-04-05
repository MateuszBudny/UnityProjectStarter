using NodeCanvas.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UISetAsSelected : MonoBehaviour
{
    [SerializeField]
    private Selectable toSetAsSelected;

    public void SetAsSelected()
    {
        EventSystem.current.SetSelectedGameObject(toSetAsSelected.gameObject);
    }
}
