using UnityEngine;

namespace _Game._Scripts.ObjectSimulationUsing
{
    public abstract class Projector : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener(EventID.None,Play);
        }

        protected virtual void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.None,Play);
        }

        protected abstract void Play(object obj);
    }
}