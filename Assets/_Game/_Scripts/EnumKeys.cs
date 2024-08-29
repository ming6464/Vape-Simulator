using System;



// A
[Serializable]
public enum AxisRotate
{
    Up,Down,Right,Left,Forward,Backward
}

[Serializable]
public enum AbilityMode
{
    Single,
    Burst,
    Auto,
    Shake
}

// F
[Serializable]
public enum FireType
{
    single,
    burst,
    auto,
    shake
}


// S
[Serializable]
public enum SimulationMode
{
    Vape,
    MachineGun,
    ScifiGun,
    LightSaber
}


// E
[Serializable]
public enum EventID
{
    None = 0,
    ApplyObject,
    ApplyBackground,
    LoadSceneByName,
    LoadSceneByIndex,
    LoadSceneMain,
    LoadSceneStart,
    OpenSettingLayer,
    OpenMenuSelectionLayer,
    OpenChangeCharacterLayer,
    OnRotateMode,
    OnExpandMode,
    OnActionProjector,
    OnSelectionSimulationObject,
    OnSelectionBackground,
    OnBackToDefaultLayerMain,
    OnChangeModeSingle,
    OnChangeModeAuto,
    OnChangeModeBurst,
    OnChangeModeShake,
    ProgressLoading,
    FinishLoading,
}

[Serializable]
public enum PrimitiveDataType
{
    Default,Int,Float,String,Bool
}

