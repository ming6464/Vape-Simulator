using UnityEngine;
using VInspector;

namespace _Game._Scripts.UI
{
    public class ButtonPostEvent : ButtonBase
    {
        [Tab("Event")]
        [SerializeField]
        private EventID _eventID;

        [Tab("Value Post")]
        [SerializeField,Variants("Null","Int", "Float", "String", "Bool")]
        private string _valuePost;
        [SerializeField,ShowIf("_valuePost","Int")]
        private int _int;

        [SerializeField,ShowIf("_valuePost","Float")]
        private float _float;

        [SerializeField,ShowIf("_valuePost","String")]
        private string _string;

        [SerializeField,ShowIf("_valuePost","Bool")]
        private bool _bool;
        [EndIf]
        [EndTab]
        public override void OnClick()
        {
            if(_eventID == EventID.None) return;
            object value = null;

            switch (_valuePost)
            {
                case "Int":
                    value = _int;
                    break;
                case "Float":
                    value = _float;
                    break;
                case "String":
                    value = _string;
                    break;
                case "Bool":
                    value = _bool;
                    break;
            }
            this.PostEvent(_eventID,value);
        }
    }
}