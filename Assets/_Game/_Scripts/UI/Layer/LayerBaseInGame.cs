namespace _Game._Scripts.UI.Layer
{
    public abstract class LayerBaseInGame : LayerBase
    {
        protected new virtual void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RegisterListener(EventID.OnSelectionSimulationObject,OnSelectionSimulationObject);
            EventDispatcher.Instance.RegisterListener(EventID.OnSelectionBackground,OnSelectionBackground);
        }
   
        protected new virtual void OnDisable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RemoveListener(EventID.OnSelectionSimulationObject,OnSelectionSimulationObject);
            EventDispatcher.Instance.RemoveListener(EventID.OnSelectionBackground,OnSelectionBackground);
        }
        
        
        #region Func Event

        protected abstract void OnSelectionBackground(object obj);

        protected abstract void OnSelectionSimulationObject(object obj);

        #endregion
    }
}