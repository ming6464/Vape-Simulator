using System;
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

        private EventDispatcher _eventDispatcher;
        private void OnEnable()
        {
            if (!_eventDispatcher)
            {
                _eventDispatcher = EventDispatcher.Instance;
            }

            foreach (var buttonSelectMode in _buttonSelectModes)
            {
                buttonSelectMode.button.onClick.AddListener(() => LoadMode(buttonSelectMode.mode));
            }
        }

        private void OnDisable()
        {
            foreach (var buttonSelectMode in _buttonSelectModes)
            {
                buttonSelectMode.button.onClick.RemoveListener(() => LoadMode(buttonSelectMode.mode));
            }
        }

        private void LoadMode(SimulationMode mode)
        {
            DataConfig.Instance.SetSimulationMode(mode);
            _eventDispatcher.PostEvent(EventID.None,mode);
        }
    }

    [Serializable]
    public struct ButtonSelectMode
    {
        public Button         button;
        public SimulationMode mode;
    }
    
}