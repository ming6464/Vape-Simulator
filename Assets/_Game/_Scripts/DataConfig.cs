using System;
using _Game._Scripts.Data;
using UnityEngine;
using VInspector;

namespace _Game._Scripts
{
    public class DataConfig : Singleton<DataConfig>
    {
        //Property
        public SimulateObjectInfo[] WeaponInfos => _weaponSo.objectInfos;
        public BackgroundInfo[] BackgroundSoInfos => _backgroundSo.backgrounds;

        public int IdObjectDefault => _idObjectDefault;

        public SimulationMode CurrentSimulationMode => _simulationMode;

        public string StartScene   => _startSceneName;
        public string MainScene    => _mainSceneName;
        public string LoadingScene => _loadingSceneName;
        
        //
        [Foldout("Data")]
        [SerializeField]
        private SimulateObjectSO _weaponSo;

        [SerializeField]
        private BackgroundSO _backgroundSo;

        [Foldout("Define")]
        [SerializeField]
        private string _mainSceneName;

        [SerializeField]
        private string _startSceneName;

        [SerializeField]
        private string _loadingSceneName;

        private int _idObjectDefault;

        private SimulationMode _simulationMode;

        public void SetIdObjectDefault(int id)
        {
            _idObjectDefault = id;
        }

        public void SetSimulationMode(SimulationMode simulationMode)
        {
            _simulationMode = simulationMode;
        }
    }

    [Serializable]
    public enum SimulationMode
    {
        Vape,
        MachineGun,
        ScifiGun,
        LightSaber
    }
}