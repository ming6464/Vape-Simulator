using System;
using UnityEngine;

// Vape 

[CreateAssetMenu(menuName = "SO/ScifiGunDataSO",fileName = "ScifiGunDataSO",order = 0)]
public class ScifiGunDataSO : ScriptableObject
{
    public ScifiGunInfo[] objectInfos;
}

[Serializable]
public class ScifiGunInfo : SimulationObjectInfo
{
}

// Vape
