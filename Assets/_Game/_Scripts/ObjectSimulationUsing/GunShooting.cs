using UnityEngine;
using VInspector;

namespace _Game._Scripts.SimulateObjectUsing
{
    public class GunShooting : MonoBehaviour
    {
        [Foldout("Reference")]
        [SerializeField]
        private Transform _pivotFire;

        [Foldout("Asset")]
        [SerializeField]
        private GameObject _shellPrefab;
        
        [EndFoldout]
        //
        private InputInGamePlay_Ctrl _input;
        
        
        
    }
}