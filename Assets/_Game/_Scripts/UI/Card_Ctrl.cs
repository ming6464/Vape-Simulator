using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Ctrl : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Image _icon;
    //
    private GameObject _obj;

    public void SetData(GameObject obj,Sprite icon)
    {
        _obj         = obj;
        _icon.sprite = icon;
    }
    
    public void ApplyObj()
    {
        EventDispatcher.Instance.PostEvent(EventID.ApplyObject,_obj);
    }
}
