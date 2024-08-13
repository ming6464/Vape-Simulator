using System;
using _Game._Scripts.Support;
using BlackBoardSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _Game._Scripts.UI.Layer.GameModeLayer
{
    public class GameModeLayer_Ctrl : LayerBase
    {
        [Tab("Game Mode Layer")]
        [SerializeField]
        private ButtonSelectMode[] _buttonSelectModes;

        protected override void OnEnable()
        {
            foreach (var buttonSelectMode in _buttonSelectModes)
            {
                buttonSelectMode.button.onClick.AddListener(() => LoadMode(buttonSelectMode.mode));
            }
        }

        protected override void OnDisable()
        {
            foreach (var buttonSelectMode in _buttonSelectModes)
            {
                buttonSelectMode.button.onClick.RemoveListener(() => LoadMode(buttonSelectMode.mode));
            }
        }

        private void LoadMode(SimulationMode mode)
        {
            BlackBoard.Instance.SetValue(BlackBoardKEY.SimulationMode,mode);
            this.PostEvent(EventID.OpenMenuSelectionLayer);
        }
    }

    [Serializable]
    public struct ButtonSelectMode
    {
        public Button         button;
        public SimulationMode mode;
    }
    
}