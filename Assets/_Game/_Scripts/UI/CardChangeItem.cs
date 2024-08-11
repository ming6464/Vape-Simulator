using System.Collections;
using System.Collections.Generic;
using _Game._Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class CardChangeItem : CardBase,IOnClickNoVariable
{
    [Tab("Card")]
    [Foldout("Reference")]
    [SerializeField]
    private Image _icon;
    [EndFoldout]
    //
    private object  _obj;
    private EventID _eventID;

    public void SetData(object obj,Sprite icon,EventID eventID)
    {
        _obj         = obj;
        _icon.sprite = icon;
        _eventID     = eventID;
    }
    
    public void OnClick()
    {
        this.PostEvent(_eventID,_obj);
    }
}
