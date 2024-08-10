using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Ctrl : Singleton<Input_Ctrl>
{
    public bool    fakeInput;
    public Vector3 mousePositionFake;
    public Vector3 mousePosition     { get; private set; }
    public Vector3 passMousePosition { get; private set; }
    
    private void Update()
    {
        UpdateMousePosition();

    }

    private void UpdateMousePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition     = Input.mousePosition;
            passMousePosition = mousePosition;
            mousePositionFake = mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            passMousePosition = mousePosition;
            mousePosition     = Input.mousePosition;
            mousePositionFake = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            passMousePosition = mousePosition;
        }

        if (fakeInput)
        {
            passMousePosition = mousePosition;
            mousePosition     = mousePositionFake;
        }
    }
}
