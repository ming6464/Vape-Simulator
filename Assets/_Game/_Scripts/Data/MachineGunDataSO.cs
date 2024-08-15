using System;
using UnityEngine;
using VInspector;

//Machine Gun
[CreateAssetMenu(menuName = "SO/MachineGunData",fileName = "MachineGunDataSO",order = 0)]
public class MachineGunDataSO : ScriptableObject
{
    public MachineGunInfo[] objectInfos;
}

[Serializable]
public class MachineGunInfo : SimulationObjectInfo
{
    [Tab("Data")]
    [Header("Info")]
    public float cooldown;
    [Header("Burst Mode Info")]
    public int burstCount;
    public float timeDelay;
    
    [Header("Asset")]
    public GameObject shell;
}
// MachineGun