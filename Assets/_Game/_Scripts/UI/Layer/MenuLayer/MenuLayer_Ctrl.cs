using System;
using BlackBoardSystem;
using UnityEngine;
using VInspector;

namespace _Game._Scripts.UI.Layer.MenuLayer
{
    public class MenuLayer_Ctrl : LayerBase
    {
        // [Tab("Menu Layer")]
        // [Foldout("Reference")]
        [SerializeField]
        private GameObject _parentCardSelection;
        // [Foldout("Asset")]
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

        private void OpenMenuSelection(object obj)
        {
            if (!BlackBoard.Instance.TryGetValue(BlackBoardKEY.SimulationMode, out SimulationMode mode))
            {
                return;
            }

            SimulateObjectInfo[] data = null;
            switch (mode)
            { 
                case SimulationMode.Vape:
                    _vapeGunData ??= BlackBoard.Instance.GetValue<SimulateObjectInfo[]>(BlackBoardKEY.VapeData);
                    data         =   _vapeGunData;
                    break;
                case SimulationMode.MachineGun:
                    _machineGunData ??= BlackBoard.Instance.GetValue<SimulateObjectInfo[]>(BlackBoardKEY.VapeData);
                    data            =   _machineGunData;
                    break;
                case SimulationMode.ScifiGun:
                    _scifiGunData ??= BlackBoard.Instance.GetValue<SimulateObjectInfo[]>(BlackBoardKEY.VapeData);
                    data          =   _scifiGunData;
                    break;
                case SimulationMode.LightSaber:
                    _lightSaberData ??= BlackBoard.Instance.GetValue<SimulateObjectInfo[]>(BlackBoardKEY.VapeData);
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
            RemoveAllCard();

            foreach (var simulateObjInfo in simulateObjectInfos)
            {
                var cardNew = Instantiate(_cardPrefab, _parentCardSelection.transform, false);
                cardNew.SetData(simulateObjInfo.icon,simulateObjInfo.prefab);
            }
            _content.gameObject.SetActive(true);
        }

        private void RemoveAllCard()
        {
            foreach (Transform tfChild in _parentCardSelection.transform)
            {
                Destroy(tfChild.gameObject);
            }
        }
    }
}