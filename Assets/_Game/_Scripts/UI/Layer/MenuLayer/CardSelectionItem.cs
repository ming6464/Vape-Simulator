using BlackBoardSystem;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _Game._Scripts.UI.Layer.MenuLayer
{
    public class CardSelectionItem : CardBase,IOnClickNoVariable
    {
        [Tab("Card Selection")]
        [Foldout("Reference")]
        [SerializeField]
        private Image _iconImage;

        private GameObject _gObjData;
        
        public void SetData(Sprite icon, GameObject gObjData)
        {
            _gObjData         = gObjData;
            _iconImage.sprite = icon;
        }

        public void OnClick()
        {
            BlackBoard.Instance.SetValue(BlackBoardKEY.ObjSelectionDefault,_gObjData);
            this.PostEvent(EventID.LoadSceneMain);
        }
    }
}