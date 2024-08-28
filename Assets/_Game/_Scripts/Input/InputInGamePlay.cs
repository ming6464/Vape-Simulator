using System;
using UnityEngine;

public class InputInGamePlay : Singleton<InputInGamePlay>
{
    public Action onStartHold;
    public Action onEndHold;
    public Action onClick;
    public Action onShake;

    private float _longPressTime = 0f;
    private Vector3 _lastAcceleration;
    private const float ShakeThreshold = 2.0f; // Adjust this threshold as needed

    private void OnDisable()
    {
        onStartHold = null;
        onEndHold = null;
        onClick = null;
        onShake = null;
    }

    private void Update()
    {
        DetectShake();
    }

    public void onPointerDown()
    {
        onStartHold?.Invoke();
        onClick?.Invoke();
    }

    public void onPointerUp()
    {
        onEndHold?.Invoke();
    }

    private void DetectShake()
    {
        Vector3 acceleration = Input.acceleration;
        Vector3 deltaAcceleration = acceleration - _lastAcceleration;

        if (deltaAcceleration.sqrMagnitude >= ShakeThreshold * ShakeThreshold)
        {
            onShake?.Invoke();
        }

        _lastAcceleration = acceleration;
    }
}