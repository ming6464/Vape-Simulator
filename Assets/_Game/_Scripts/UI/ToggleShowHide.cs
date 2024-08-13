using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ToggleShowHide : ButtonBase
{
    [Tab("Show Hide Togglr")]
    public bool currentState;
    [Foldout("Reference")]
    [SerializeField]
    private GameObject[] _showHideObjs;
    [EndFoldout]
    [EndTab]
    public override void OnClick()
    {
        currentState = !currentState;

        foreach (var objSet in _showHideObjs)
        {
            objSet.SetActive(currentState);
        }
        
    }
}
