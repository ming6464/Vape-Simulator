using UnityEngine;
using VInspector;

public class SimulationObjectInfo
{
    [Tab("Base")]
    [Header("Base Info")]
    public string name;
    public Sprite     icon;
    public GameObject prefab;
    public float      capacity;
    
    [Header("Ability")]
    public AbilityMode defaultAbility;
    public AbilityMode[] abilities;
}




