using _Game._Scripts.Data;
using UnityEngine;
using VInspector;

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
    protected override void Start()
    {
        Init();
    }

    private void Init()
    {
        if (this.TryGetData(DataShareKey.MachineGunData, out SimulateObjectInfo[] weaponInfos))
        {
            foreach (var weaponInfo in weaponInfos)
            {
                var card = Instantiate(_weaponCardPrefab, _listWeaponTf,false);
                card.SetData(weaponInfo.prefab,weaponInfo.icon,EventID.ApplyObject);
                card.name = weaponInfo.name;
            }
        }

        if (this.TryGetData(DataShareKey.BackgroundData, out BackgroundInfo[] backgroundInfos))
        {
            foreach (var bgInfo in backgroundInfos)
            {
                var card = Instantiate(_backgroundCardPrefab, _listBackgroundTf,false);
                card.SetData(bgInfo,bgInfo.icon,EventID.ApplyBackground);
            }
        }
    }
}
