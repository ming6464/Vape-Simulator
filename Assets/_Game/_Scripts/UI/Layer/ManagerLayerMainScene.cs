using System;
using BlackBoardSystem;
using UnityEngine;
using VInspector;

namespace _Game._Scripts.UI.Layer
{
    public class ManagerLayerMainScene : MonoBehaviour
    {
        [Foldout("Asset")]
        [SerializeField]
        private GameObject _vapeLayerPrefab;
        [SerializeField]
        private GameObject _machineGunLayerPrefab;
        [SerializeField]
        private GameObject _scifiGunLayerPrefab;
        [SerializeField]
        private GameObject _lightSaberLayerPrefab;

        [Foldout("Reference")]
        [SerializeField]
        private Transform _parentLayer;

        private void Start()
        {
            LoadLayer();
            
        }

        private void LoadLayer()
        {
            if (!BlackBoard.Instance.TryGetValue(BlackBoardKEY.SimulationMode, out SimulationMode mode)) return;

            switch (mode)
            {
                case SimulationMode.Vape:
                    Instantiate(_vapeLayerPrefab, _parentLayer, false);
                    break;
                case SimulationMode.MachineGun:
                    Instantiate(_machineGunLayerPrefab, _parentLayer, false);
                    break;
                case SimulationMode.ScifiGun:
                    Instantiate(_scifiGunLayerPrefab, _parentLayer, false);
                    break;
                case SimulationMode.LightSaber:
                    Instantiate(_lightSaberLayerPrefab, _parentLayer, false);
                    break;
            }
        }
    }
}