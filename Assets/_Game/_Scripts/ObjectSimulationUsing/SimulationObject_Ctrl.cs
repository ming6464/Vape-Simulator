using System;
using System.Collections;
using System.Collections.Generic;
using BlackBoardSystem;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SimulationObject_Ctrl : MonoBehaviour
{
    //Property
    public  GameObject      currentObject { get; private set; }
    [SerializeField]
    private Transform _parentObj;

    private float _timeHold;
    
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
        if (BlackBoard.Instance.TryGetValue(BlackBoardKEY.ObjSelectionDefault, out GameObject gObj))
        {
            ApplyObj(gObj);
        }
        
    }

    private void ApplyObj(object obj)
    {
        var gObj = obj as GameObject;

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
