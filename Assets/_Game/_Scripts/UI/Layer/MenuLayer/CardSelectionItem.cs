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

        private int _id;
        
        public void SetData(Sprite icon, int id)
        {
            _id         = id;
            _iconImage.sprite = icon;
        }

        public void OnClick()
        {
            BlackBoard.Instance.SetValue(BlackBoardKEY.IdDefaultObjectSelection,_id);
            this.PostEvent(EventID.LoadSceneMain);
        }
    }
}