using System;
using UnityEngine;
using VInspector;

namespace _Game._Scripts.UI.Layer.MenuLayer
{
    public class MenuLayer_Ctrl : LayerBase
    {
        [Tab("Menu Layer")]
        public bool check;
        [Foldout("Reference")]
        [SerializeField]
        private Transform _parentCardSelection;
        [Foldout("Asset")]
        [SerializeField]
        private CardSelectionItem _cardPrefab;

        private SimulateObjectInfo[] _vapeGunData;
        private SimulateObjectInfo[] _machineGunData;
        private SimulateObjectInfo[] _scifiGunData;
        private SimulateObjectInfo[] _lightSaberData;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            this.RegisterListener(EventID.OpenMenuSelectionLayer,OpenMenuSelection);
        }

        protected override void Update()
        {
            base.Update();

            if (!_parentCardSelection)
            {
                Debug.Log("Null");
            }
            else
            {
                Debug.Log("Not Null");
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (!_parentCardSelection)
            {
                Debug.Log("Null");
            }
            else
            {
                Debug.Log("Not Null");
            }
        }

        private void OpenMenuSelection(object obj)
        {
            if (!this.TryGetData(DataShareKey.SimulationMode, out SimulationMode mode))
            {
                return;
            }

            SimulateObjectInfo[] data = null;
            switch (mode)
            { 
                case SimulationMode.Vape:
                    _vapeGunData ??= this.GetData<SimulateObjectInfo[]>(DataShareKey.VapeData);
                    data         =   _vapeGunData;
                    break;
                case SimulationMode.MachineGun:
                    _machineGunData ??= this.GetData<SimulateObjectInfo[]>(DataShareKey.VapeData);
                    data            =   _machineGunData;
                    break;
                case SimulationMode.ScifiGun:
                    _scifiGunData ??= this.GetData<SimulateObjectInfo[]>(DataShareKey.VapeData);
                    data          =   _scifiGunData;
                    break;
                case SimulationMode.LightSaber:
                    _lightSaberData ??= this.GetData<SimulateObjectInfo[]>(DataShareKey.VapeData);
                    data            =   _lightSaberData;
                    break;
                    
            }

            if (data == null)
            {
                Debug.LogError("Data bị null rồi !");
                return;
            }
            InitData(data);
        }

        private void InitData(SimulateObjectInfo[] simulateObjectInfos)
        {
            if (check)
            {
                return;
            }
            
            RemoveAllCard();

            for (int i = 0; i < simulateObjectInfos.Length; i++)
            {
                var simulateObjInfo = simulateObjectInfos[i];
                var cardNew         = Instantiate(_cardPrefab, _parentCardSelection, false);
                cardNew.SetData(simulateObjInfo.icon,i);
            }
            _content.gameObject.SetActive(true);
        }

        private void RemoveAllCard()
        {
            if (!_parentCardSelection)
            {
                Debug.Log("Null");
            }
            else
            {
                Debug.Log("Not Null");
            }
            foreach (Transform tfChild in _parentCardSelection)
            {
                Destroy(tfChild.gameObject);
            }
        }
    }
}