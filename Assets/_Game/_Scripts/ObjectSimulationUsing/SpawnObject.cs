using BlackBoardSystem;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    //Property
    public  GameObject      currentObject { get; private set; }
    [SerializeField]
    private Transform _parentObj;
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.ApplyObject,ApplyObj);
    }
    

    private void OnDisable()
    {
        this.RemoveListener(EventID.ApplyObject,ApplyObj);
    }
    

    private void ApplyObj(object obj)
    {
        if (obj is not SimulationObjectInfo simulationObjectInfo)
        {
            Debug.LogError("Simulation object info bị null");
            return;
        }
        
        BlackBoard.Instance.SetValue(BlackBoardKEY.ObjectSelectionUsing, simulationObjectInfo);
        
        var gObj = simulationObjectInfo.prefab;
        
        if (currentObject)
        {
            DestroyImmediate(currentObject);
        }

        _parentObj.localRotation = Quaternion.identity;
        var objApply = Instantiate(gObj, _parentObj, true);
        objApply.transform.localPosition = Vector3.zero;
        objApply.transform.localRotation = Quaternion.identity;
        currentObject                    = objApply;
    }
}
