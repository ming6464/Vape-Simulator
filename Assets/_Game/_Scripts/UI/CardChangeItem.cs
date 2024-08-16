using System.Collections;
using System.Collections.Generic;
using _Game._Scripts.UI;
using ComponentUtilitys.UI;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class CardChangeItem : CardBase,IOnClick
{
    [Tab("Card")]
    [Foldout("Reference")]
    [SerializeField]
    private Image _icon;

    [EndFoldout]
    //
    private int _id;

    private EventID _event;

    public void SetData(int id,Sprite icon,EventID eventID)
    {
        _id          = id;
        _icon.sprite = icon;
        _event       = eventID;
    }
    
    public void OnClick()
    {
        this.PostEvent(_event,_id);
    }
}
