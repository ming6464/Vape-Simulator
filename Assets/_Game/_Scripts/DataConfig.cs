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
    }
}