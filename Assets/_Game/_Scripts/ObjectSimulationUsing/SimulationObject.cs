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
        protected virtual void Awake()
        {
            _myInfo = BlackBoard.Instance.GetValue<SimulationObjectInfo>(BlackBoardKEY.ObjectSelectionUsing);
            _currentAbilityMode = _myInfo.defaultAbility;
        }

        protected virtual void OnEnable()
        {
            if (InputInGamePlay.Instance)
            {
                _hasInput                             =  true;
                InputInGamePlay.Instance.onMouseClick += OnPlayActionClick;
                InputInGamePlay.Instance.onShake      += OnPlayActionShake;
                InputInGamePlay.Instance.onMouseHold  += OnPlayActionHold;
            }
            else
            {
                Debug.LogError("Input bị null rồi");
            }
            EventDispatcher.Instance.RegisterListener(EventID.OnChangeAbilityMode,OnChangeAbilityMode);
        }
        

        protected virtual void OnDisable()
        {
            if (_hasInput &&InputInGamePlay.Instance)
            {
                InputInGamePlay.Instance.onMouseClick -= OnPlayActionClick;
                InputInGamePlay.Instance.onShake      -= OnPlayActionShake;
                InputInGamePlay.Instance.onMouseHold  -= OnPlayActionHold;
            }
            EventDispatcher.Instance.RemoveListener(EventID.OnChangeAbilityMode,OnChangeAbilityMode);
        }


        #region Func projector

        
        protected virtual void OnChangeAbilityMode(object obj)
        {
            _currentAbilityMode = (AbilityMode)obj;
        }

        protected virtual void OnPlayActionHold(float timeHold)
        {
            if(_currentAbilityMode != AbilityMode.Auto) return;
            RunAction(timeHold);
        }

        protected virtual void OnPlayActionClick()
        {
            if(_currentAbilityMode is AbilityMode.Shake or AbilityMode.Auto) return;
            RunAction();
        }
        
        protected virtual void OnPlayActionShake()
        {
            if(_currentAbilityMode != AbilityMode.Shake) return;
            RunAction();
        }

        protected abstract void RunAction(float timeHold = 0);

        #endregion

    }
}