using _Game._Scripts.Data;
using UnityEngine;

namespace _Game._Scripts
{
    public class DataConfig : Singleton<DataConfig>
    {
        //Property
        [SerializeField]
        private SimulateObjectSO _weaponSo;
        //
        public SimulateObjectInfo[] WeaponInfos => _weaponSo.objectInfos;
    }
}