using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ShowHideButton : ButtonBase
{
    [Tab("Show Hide Button")]
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
