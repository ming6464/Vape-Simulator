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

        private int _idObject;

        public void SetData(Sprite icon, int id)
        {
            _idObject         = id;
            _iconImage.sprite = icon;
        }

        public void OnClick()
        {
            EventDispatcher.Instance.PostEvent(EventID.LoadSceneMain);
        }
    }
}