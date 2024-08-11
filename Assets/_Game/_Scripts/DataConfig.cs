using System;
using _Game._Scripts.Data;
using _Game._Scripts.Support;
using UnityEditor;
using UnityEngine;
using VInspector;

namespace _Game._Scripts
{
    public class DataConfig : Singleton<DataConfig>
    {
        //
        [Foldout("Data")]
        
        [SerializeField]
        private SimulateObjectSO _vapeSo;
        
        [SerializeField]
        private SimulateObjectSO _machineGunSo;
        
        [SerializeField]
        private SimulateObjectSO _scifiGun;
        
        [SerializeField]
        private SimulateObjectSO _lightSaber;
        
        [SerializeField]
        private BackgroundSO _backgroundSo;

        [Foldout("Define")]
        [SerializeField]
        private string _mainSceneName;

        [SerializeField]
        private string _startSceneName;

        [SerializeField]
        private string _loadingSceneName;

        public override void Awake()
        {
            base.Awake();
            LoadDataShare();
        }

        private void LoadDataShare()
        {
            DataShare.Instance.SetData(DataShareKey.VapeData,_vapeSo.objectInfos);
            DataShare.Instance.SetData(DataShareKey.MachineGunData,_machineGunSo.objectInfos);
            DataShare.Instance.SetData(DataShareKey.ScifiGunData,_scifiGun.objectInfos);
            DataShare.Instance.SetData(DataShareKey.LightSaberData,_lightSaber.objectInfos);
            DataShare.Instance.SetData(DataShareKey.BackgroundData,_backgroundSo.backgrounds);
            DataShare.Instance.SetData(DataShareKey.MainSceneName,_mainSceneName);
            DataShare.Instance.SetData(DataShareKey.StartSceneName,_startSceneName);
            DataShare.Instance.SetData(DataShareKey.LoadingSceneName,_loadingSceneName);
        }
    }
    
}