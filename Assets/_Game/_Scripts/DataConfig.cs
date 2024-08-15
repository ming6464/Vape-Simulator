using _Game._Scripts.Data;
using BlackBoardSystem;
using UnityEngine;
using VInspector;

public class DataConfig : Singleton<DataConfig>
{
    //
    [Foldout("Data")]
        
    [SerializeField]
    private VapeDataSO _vapeSo;
        
    [SerializeField]
    private MachineGunDataSO _machineGunSo;
        
    [SerializeField]
    private ScifiGunDataSO _scifiGun;
        
    [SerializeField]
    private LightSaberDataSO _lightSaber;
        
    [SerializeField]
    private BackgroundSO _backgroundSo;

    [Foldout("Asset")]
    [SerializeField]
    private GameObject _dontDestroyPrefab;
        
    [Foldout("Define")]
    [SerializeField]
    private string _mainSceneName;

    [SerializeField]
    private string _startSceneName;

    [SerializeField]
    private string _loadingSceneName;

    private bool _isLoadDataShare;
        
    public override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        LoadDataShare();
    }

    private void LoadDataShare()
    {
        if(_isLoadDataShare) return;
        _isLoadDataShare = true;
        BlackBoard.Instance.SetValue(BlackBoardKEY.VapeData,_vapeSo.objectInfos as SimulationObjectInfo[]);
        BlackBoard.Instance.SetValue(BlackBoardKEY.MachineGunData,_machineGunSo.objectInfos  as SimulationObjectInfo[]);
        BlackBoard.Instance.SetValue(BlackBoardKEY.ScifiGunData,_scifiGun.objectInfos  as SimulationObjectInfo[]);
        BlackBoard.Instance.SetValue(BlackBoardKEY.LightSaberData,_lightSaber.objectInfos  as SimulationObjectInfo[]);
        BlackBoard.Instance.SetValue(BlackBoardKEY.BackgroundData,_backgroundSo.backgrounds);
        BlackBoard.Instance.SetValue(BlackBoardKEY.MainSceneName,_mainSceneName);
        BlackBoard.Instance.SetValue(BlackBoardKEY.StartSceneName,_startSceneName);
        BlackBoard.Instance.SetValue(BlackBoardKEY.LoadingSceneName,_loadingSceneName);
        BlackBoard.Instance.SetValue(BlackBoardKEY.DontDestroyPrefab,_dontDestroyPrefab);
    }
}