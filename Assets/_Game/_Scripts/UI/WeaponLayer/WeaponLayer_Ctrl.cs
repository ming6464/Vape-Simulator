using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts;
using _Game._Scripts.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponLayer_Ctrl : LayerBase
{
    [Header("Reference")]
    [SerializeField]
    private Transform _listWeaponTf;
    [Header("Asset")]
    [SerializeField]
    private Card_Ctrl _weaponCardPrefab;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        var weaponInfos = DataConfig.Instance.WeaponInfos;

        foreach (var weaponInfo in weaponInfos)
        {
            var card = Instantiate(_weaponCardPrefab, _listWeaponTf,false);
            card.SetData(weaponInfo.prefab,weaponInfo.icon);
            card.name = weaponInfo.name;
        }
    }
}
