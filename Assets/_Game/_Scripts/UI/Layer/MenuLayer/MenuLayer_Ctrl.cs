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

        private SimulationObjectInfo[] _vapeGunData;
        private SimulationObjectInfo[] _machineGunData;
        private SimulationObjectInfo[] _scifiGunData;
        private SimulationObjectInfo[]   _lightSaberData;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            this.RegisterListener(EventID.OpenMenuSelectionLayer,OpenMenuSelection);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.RemoveListener(EventID.OpenMenuSelectionLayer,OpenMenuSelection);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.V))
            {
                BlackBoard.Instance.SetValue(BlackBoardKEY.SimulationMode,SimulationMode.MachineGun);
                this.PostEvent(EventID.OpenMenuSelectionLayer);
            }
        }

        private void OpenMenuSelection(object obj)
        {
            if (!BlackBoard.Instance.TryGetValue(BlackBoardKEY.SimulationMode, out SimulationMode mode))
            {
                return;
            }

            SimulationObjectInfo[] data = null;
            switch (mode)
            { 
                case SimulationMode.Vape:
                    _vapeGunData ??= BlackBoard.Instance.GetValue<SimulationObjectInfo[]>(BlackBoardKEY.VapeData);
                    data         =   _vapeGunData;
                    break;
                case SimulationMode.MachineGun:
                    _machineGunData ??= BlackBoard.Instance.GetValue<SimulationObjectInfo[]>(BlackBoardKEY.MachineGunData);
                    data            =   _machineGunData;
                    break;
                case SimulationMode.ScifiGun:
                    _scifiGunData ??= BlackBoard.Instance.GetValue<SimulationObjectInfo[]>(BlackBoardKEY.ScifiGunData);
                    data          =   _scifiGunData;
                    break;
                case SimulationMode.LightSaber:
                    _lightSaberData ??= BlackBoard.Instance.GetValue<SimulationObjectInfo[]>(BlackBoardKEY.LightSaberData);
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

        private void InitData(SimulationObjectInfo[] simulateObjectInfos)
        {
            RemoveAllCard();

            for (int i = 0; i < simulateObjectInfos.Length; i++)
            {
                var simulateObjInfo = simulateObjectInfos[i];
                var cardNew         = Instantiate(_cardPrefab, _parentCardSelection.transform, false);
                cardNew.SetData(simulateObjInfo.icon,i);
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