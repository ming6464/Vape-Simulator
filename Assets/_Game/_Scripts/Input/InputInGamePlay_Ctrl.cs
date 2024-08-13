using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInGamePlay_Ctrl : Singleton<InputInGamePlay_Ctrl>
{
    //
    public Vector3 MousePosition     { get; private set; }
    public Vector3 PassMousePosition { get; private set; }

    private float _holdTime;
    private bool  _canCatchActionMouse = true;

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.OnRotateMode,CanCatchActionMouse);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnRotateMode,CanCatchActionMouse);
    }

    private void CanCatchActionMouse(object obj)
    {
        var value = !(bool)obj;
        if(_canCatchActionMouse == value) return;
        _holdTime            = 0;
        _canCatchActionMouse = value;
    }

    private void Update()
    {
        UpdateMousePosition();
        
    }

    private void UpdateMousePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MousePosition     = Input.mousePosition;
            PassMousePosition = MousePosition;

            if (_canCatchActionMouse)
            {
                _holdTime = 0;
                EventDispatcher.Instance.PostEvent(EventID.OnClick);
            }
            
        }

        if (Input.GetMouseButton(0))
        {
            PassMousePosition = MousePosition;
            MousePosition     = Input.mousePosition;

            if (_canCatchActionMouse)
            {
                _holdTime += Time.deltaTime;
                EventDispatcher.Instance.PostEvent(EventID.OnHold);
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            PassMousePosition = MousePosition;
        }
    }
}
