using System;
using UnityEngine;

namespace _Game._Scripts.Data
{
    [CreateAssetMenu(menuName = "SimulateObjectSO",fileName = "SimulateObjectSO",order = 0)]
    public class SimulateObjectSO : ScriptableObject
    {
        public SimulateObjectInfo[] objectInfos;
    }

    [Serializable]
    public struct SimulateObjectInfo
    {
        public string     name;
        public Sprite     icon;
        public GameObject prefab;
    }
}