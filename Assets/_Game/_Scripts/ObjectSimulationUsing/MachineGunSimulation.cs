using System;
using System.Threading.Tasks;
using _Game._Scripts.SimulateObjectUsing;
using UnityEngine;
using VInspector;

namespace _Game._Scripts.ObjectSimulationUsing
{
    public class MachineGunSimulation : SimulationObject
    {
        [Foldout("Reference")]
        [SerializeField]
        private Transform _pivotSpawnShell;
        //
        private float          _cooldownDelta;
        private bool           _canPlayAction;
        private MachineGunInfo _myData;
        private float          _capacity;
        private GameObject     _shell;
        private bool           _canSpawnShell;
        private bool           _onReload;
        
        private void Start()
        {
            _myData        = _myInfo as MachineGunInfo;
            _capacity      = _myData.capacity;
            _shell         = _myData.shell;
            _canSpawnShell = _shell && _pivotSpawnShell;
            _canPlayAction = true;

            switch (_myData.defaultAbility)
            {
                case AbilityMode.Single:
                    EventDispatcher.Instance.PostEvent(EventID.OnChangeModeSingle);
                    break;
                case AbilityMode.Auto:
                    EventDispatcher.Instance.PostEvent(EventID.OnChangeModeAuto);
                    break;
                case AbilityMode.Burst:
                    EventDispatcher.Instance.PostEvent(EventID.OnChangeModeBurst);
                    break;
                case AbilityMode.Shake:
                    EventDispatcher.Instance.PostEvent(EventID.OnChangeModeShake);
                    break;
                
            }
            
        }

        private void Update()
        {
            UpdateActionState();
            UpdateStateObject();
        }

        private void UpdateStateObject()
        {
            if (_capacity <= 0 && !_onReload)
            {
                OnReload();
            }
        }

        private void UpdateActionState()
        {
            if (_canPlayAction) return;
            _cooldownDelta += Time.deltaTime;
            if(_cooldownDelta <= _myData.cooldown) return;
            _cooldownDelta = 0;
            _canPlayAction = true;
        }

        private void OnReload()
        {
            Debug.Log("On Reload");
            _onReload = true;
            _capacity = _myData.capacity;
            _onReload = false;
        }

        private async Task OnActionBurstMode()
        {
            for (var i = 0; i < _myData.burstCount; i++)
            {
                SpawnShell();
                OnAction();
                await Task.Delay(Mathf.FloorToInt(_myData.timeDelay * 1000));
            }
        }
        
        protected override void RunAction(float timeHold = 0)
        {
            if(!_canPlayAction || _onReload) return;
            
            if (_currentAbilityMode == AbilityMode.Burst)
            {
                OnActionBurstMode();
            }
            else
            {
                SpawnShell();
                OnAction();
            }
            
        }

        private void OnAction()
        {
            _capacity--;
            _canPlayAction = false;
            Debug.Log("On Action Projector");
            EventDispatcher.Instance.PostEvent(EventID.OnActionProjector);
        }

        private void SpawnShell()
        {
            if(!_canSpawnShell) return;
            Debug.Log("Spawn Shell");
        }

        protected override void OnRotateMode(object obj)
        {
            base.OnRotateMode(obj);
        }

        protected override void OnExpandMode(object obj)
        {
            base.OnExpandMode(obj);
        }

        protected override void OnBackToDefaultLayerMain(object obj)
        {
            base.OnBackToDefaultLayerMain(obj);
        }
    }
}