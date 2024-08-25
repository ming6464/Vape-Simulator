using System;
using BlackBoardSystem;
using UnityEngine;
using VInspector;

public class ContainerObjectCtrl : MonoBehaviour
{
    //Property
    public  GameObject      CurrentObject { get; private set; }
    //
    [Foldout("Reference")]
    [SerializeField]
    private Transform _parentObj;
    [SerializeField]
    private RenderTexture _renderTexture;
    [EndFoldout]
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.ApplyObject,ApplyObj);
    }
    

    private void OnDisable()
    {
        this.RemoveListener(EventID.ApplyObject,ApplyObj);
    }

    private void Start()
    {
        _renderTexture.Release();
        _renderTexture.width = Screen.width;
        _renderTexture.height = Screen.height;
        _renderTexture.Create();
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
        
        if (CurrentObject)
        {
            DestroyImmediate(CurrentObject);
        }

        _parentObj.localRotation = Quaternion.identity;
        var objApply = Instantiate(gObj, _parentObj, true);
        objApply.transform.localPosition = Vector3.zero;
        objApply.transform.localRotation = Quaternion.identity;
        CurrentObject                    = objApply;
    }
}
