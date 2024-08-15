using System;
using UnityEngine;

public class InputInGamePlay : Singleton<InputInGamePlay>
{
    public Action<float> onMouseHold;
    public Action        onMouseClick;
    public Action        onShake;
    public Vector3       MousePosition     { get; private set; }
    public Vector3       PassMousePosition { get; private set; }

    private float _holdTime;

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
            _holdTime         = 0;
            
            onMouseClick?.Invoke();
        }

        if (Input.GetMouseButton(0))
        {
            PassMousePosition = MousePosition;
            MousePosition     = Input.mousePosition;

            _holdTime += Time.deltaTime;
            onMouseHold?.Invoke(_holdTime);
        }

        if (Input.GetMouseButtonUp(0))
        {
            PassMousePosition = MousePosition;
        }
    }
}
