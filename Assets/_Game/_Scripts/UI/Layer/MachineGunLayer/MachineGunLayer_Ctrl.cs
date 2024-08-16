using System;
using _Game._Scripts.Data;
using _Game._Scripts.UI.Layer;
using BlackBoardSystem;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class MachineGunLayer_Ctrl : LayerBaseInGame
{
    [Foldout("Reference")]
    [SerializeField]
    private Transform _listWeaponTf;

    [SerializeField]
    private Transform _listBackgroundTf;
    
    [SerializeField]
    private AbilityInfo[] _abilityInfos;
    
    [Foldout("Asset")]
    [SerializeField]
    private CardChangeItem _weaponCardPrefab;

    [SerializeField]
    private CardChangeItem _backgroundCardPrefab;

    [EndFoldout]
    private SimulationObjectInfo[] _simulationObjectInfos;
    private BackgroundInfo[]       _backgroundInfos;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventDispatcher.Instance.RegisterListener(EventID.ApplyObject,UpdateModeState);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventDispatcher.Instance.RemoveListener(EventID.ApplyObject,UpdateModeState);
    }

    private void UpdateModeState(object obj)
    {
        var simulationObj = (MachineGunInfo)obj;
        foreach (var ability in _abilityInfos)
        {
            var index = Array.IndexOf(simulationObj.abilities, ability.abilityMode);
            ability.parent.SetActive(index >= 0);

            if (ability.abilityMode.Equals(simulationObj.defaultAbility))
            {
                ability.toggle.isOn = true;
            }
        }
    }

    protected override void Start()
    {
        Init();
    }

    private void Init()
    {
        //Init list
        if (BlackBoard.Instance.TryGetValue(BlackBoardKEY.MachineGunData, out _simulationObjectInfos))
        {

            var idDefault = BlackBoard.Instance.GetValue<int>(BlackBoardKEY.IdDefaultObjectSelection);
            
            for (var i = 0; i < _simulationObjectInfos.Length; i++)
            {
                var weaponInfo = _simulationObjectInfos[i];
                var card       = Instantiate(_weaponCardPrefab, _listWeaponTf,false);
                card.SetData(i,weaponInfo.icon,EventID.OnSelectionSimulationObject);
                card.name = weaponInfo.name;

                if (i == idDefault)
                {
                    EventDispatcher.Instance.PostEvent(EventID.ApplyObject,weaponInfo);
                }
            }

            if (!BlackBoard.Instance.TryGetValue(BlackBoardKEY.BackgroundData, out _backgroundInfos))
            {
                return;
            }

            {
                for(var i = 0; i < _backgroundInfos.Length; i++)
                {
                    var bgInfo = _backgroundInfos[i];
                    var card   = Instantiate(_backgroundCardPrefab, _listBackgroundTf,false);
                    card.SetData(i,bgInfo.icon,EventID.OnSelectionBackground);
                }
            }

        }
    }
    
    protected override void OnSelectionBackground(object obj)
    {
        var id = (int)obj;
        for(var i = 0; i < _backgroundInfos.Length; i++)
        {
            if(i != id) continue;
            EventDispatcher.Instance.PostEvent(EventID.ApplyBackground,_backgroundInfos[i]);
            break;
        }
        
    }

    protected override void OnSelectionSimulationObject(object obj)
    {
        var id = (int)obj;
        for (var i = 0; i < _simulationObjectInfos.Length; i++)
        {
            if (i != id)
            {
                continue;
            }

            EventDispatcher.Instance.PostEvent(EventID.ApplyObject,_simulationObjectInfos[i]);
            break;
        }
    }
}

[Serializable]
public struct AbilityInfo
{
    public GameObject parent;
    public Toggle     toggle;
    public AbilityMode abilityMode;
}
