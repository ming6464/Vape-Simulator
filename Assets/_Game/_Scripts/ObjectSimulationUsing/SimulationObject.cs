using System;
using BlackBoardSystem;
using UnityEngine;
using VInspector;

namespace _Game._Scripts.SimulateObjectUsing
{
    public abstract class SimulationObject : MonoBehaviour
    {
        protected SimulationObjectInfo _myInfo;
        protected AbilityMode          _currentAbilityMode;
        protected bool                 _hasInput;
        private   bool                 _isBusy;
        protected virtual void Awake()
        {
            _myInfo = BlackBoard.Instance.GetValue<SimulationObjectInfo>(BlackBoardKEY.ObjectSelectionUsing);
            _currentAbilityMode = _myInfo.defaultAbility;
        }

        protected virtual void OnEnable()
        {
            if (InputInGamePlay.Instance)
            {
                _hasInput                            =  true;
                InputInGamePlay.Instance.onClick     += OnPlayActionClick;
                InputInGamePlay.Instance.onShake     += OnPlayActionShake;
                InputInGamePlay.Instance.onStartHold += OnStartHold;
                InputInGamePlay.Instance.onEndHold   += onEndHold;
                EventDispatcher.Instance.RegisterListener(EventID.OnRotateMode,OnRotateMode);
                EventDispatcher.Instance.RegisterListener(EventID.OnExpandMode,OnExpandMode);
                EventDispatcher.Instance.RegisterListener(EventID.OnBackToDefaultLayerMain,OnBackToDefaultLayerMain);
            }
            else
            {
                Debug.LogError("Input bị null rồi");
            }
            EventDispatcher.Instance.RegisterListener(EventID.OnChangeModeSingle,OnChangeModeSingle);
            EventDispatcher.Instance.RegisterListener(EventID.OnChangeModeAuto,OnChangeModeAuto);
            EventDispatcher.Instance.RegisterListener(EventID.OnChangeModeBurst,OnChangeModeBurst);
            EventDispatcher.Instance.RegisterListener(EventID.OnChangeModeShake,OnChangeModeShake);
        }

        private void OnChangeModeShake(object obj)
        {
            _currentAbilityMode = AbilityMode.Shake;
        }

        protected virtual void OnDisable()
        {
            if (_hasInput &&InputInGamePlay.Instance)
            {
                InputInGamePlay.Instance.onClick     -= OnPlayActionClick;
                InputInGamePlay.Instance.onShake     -= OnPlayActionShake;
                InputInGamePlay.Instance.onStartHold -= OnStartHold;
                InputInGamePlay.Instance.onEndHold   -= onEndHold;
                EventDispatcher.Instance.RemoveListener(EventID.OnRotateMode,OnRotateMode);
                EventDispatcher.Instance.RemoveListener(EventID.OnExpandMode,OnExpandMode);
                EventDispatcher.Instance.RemoveListener(EventID.OnBackToDefaultLayerMain,OnBackToDefaultLayerMain);
            }
            EventDispatcher.Instance.RemoveListener(EventID.OnChangeModeSingle,OnChangeModeSingle);
            EventDispatcher.Instance.RemoveListener(EventID.OnChangeModeAuto,OnChangeModeAuto);
            EventDispatcher.Instance.RemoveListener(EventID.OnChangeModeBurst,OnChangeModeBurst);
            EventDispatcher.Instance.RemoveListener(EventID.OnChangeModeShake,OnChangeModeShake);
        }


        #region Func projector
        
        private void OnChangeModeBurst(object obj)
        {
            _currentAbilityMode = AbilityMode.Burst;
        }

        private void OnChangeModeAuto(object obj)
        {
            _currentAbilityMode = AbilityMode.Auto;
        }

        private void OnChangeModeSingle(object obj)
        {
            _currentAbilityMode = AbilityMode.Single;
        }
        
        protected virtual void OnStartHold()
        {
            if(_isBusy || _currentAbilityMode != AbilityMode.Auto) return;
            RunAction();
        }
        
        protected virtual void onEndHold()
        {
            if(_isBusy || _currentAbilityMode != AbilityMode.Auto) return;
            RunAction();
        }

        protected virtual void OnPlayActionClick()
        {
            if(_isBusy || _currentAbilityMode is AbilityMode.Shake or AbilityMode.Auto) return;
            RunAction();
        }
        
        protected virtual void OnPlayActionShake()
        {
            if(_isBusy || _currentAbilityMode != AbilityMode.Shake) return;
            RunAction();
        }

        protected abstract void RunAction(float timeHold = 0);

        #endregion

        #region Func Event

        protected virtual void OnExpandMode(object obj)
        {
            _isBusy = true;
        }

        protected virtual void OnRotateMode(object obj)
        {
            _isBusy = true;
        }
        
        protected virtual void OnBackToDefaultLayerMain(object obj)
        {
            _isBusy = false;
        }

        #endregion

    }
}