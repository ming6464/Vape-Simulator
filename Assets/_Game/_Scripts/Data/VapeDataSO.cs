using System;
using UnityEngine;

// Vape 

[CreateAssetMenu(menuName = "SO/VapeDataSO",fileName = "VapeDataSO",order = 0)]
public class VapeDataSO : ScriptableObject
{
    public VapeInfo[] objectInfos;
}

[Serializable]
public class VapeInfo : SimulationObjectInfo
{
}

// Vape
