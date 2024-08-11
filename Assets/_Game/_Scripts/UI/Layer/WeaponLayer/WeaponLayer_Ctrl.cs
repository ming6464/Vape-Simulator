using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts;
using _Game._Scripts.Data;
using UnityEngine;
using VInspector;
using Random = UnityEngine.Random;

public class WeaponLayer_Ctrl : LayerBase
{
    [Foldout("Reference")]
    [SerializeField]
    private Transform _listWeaponTf;

    [SerializeField]
    private Transform _listBackgroundTf;
    
    [Foldout("Asset")]
    [SerializeField]
    private CardChangeItem _weaponCardPrefab;

    [SerializeField]
    private CardChangeItem _backgroundCardPrefab;
    [EndFoldout]
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        var weaponInfos     = DataConfig.Instance.WeaponInfos;
        var backgroundInfos = DataConfig.Instance.BackgroundSoInfos;
        
        foreach (var weaponInfo in weaponInfos)
        {
            var card = Instantiate(_weaponCardPrefab, _listWeaponTf,false);
            card.SetData(weaponInfo.prefab,weaponInfo.icon,EventID.ApplyObject);
            card.name = weaponInfo.name;
        }

        foreach (var bgInfo in backgroundInfos)
        {
            var card = Instantiate(_backgroundCardPrefab, _listBackgroundTf,false);
            card.SetData(bgInfo,bgInfo.icon,EventID.ApplyBackground);
        }
    }
}
