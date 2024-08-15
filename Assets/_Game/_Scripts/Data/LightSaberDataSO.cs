// Vape 

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/LightSaberDataSO",fileName = "LightSaberDataSO",order = 0)]
public class LightSaberDataSO : ScriptableObject
{
    public LightSaberInfo[] objectInfos;
}

[Serializable]
public class LightSaberInfo : SimulationObjectInfo
{
}

// Vape