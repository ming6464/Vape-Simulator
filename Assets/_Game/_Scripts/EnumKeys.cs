using System;



// A
[Serializable]
public enum AxisRotate
{
    Up,Down,Right,Left,Forward,Backward
}
  
// D
[Serializable]
public enum DataShareKey
{
    none,
    SimulationMode,
    MachineGunData,
    VapeData,
    ScifiGunData,
    LightSaberData,
    BackgroundData,
    MainSceneName,
    StartSceneName,
    LoadingSceneName,
    ObjSelectionDefault,
    DontDestroyPrefab
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
}

