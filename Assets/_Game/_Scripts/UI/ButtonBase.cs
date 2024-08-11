using _Game._Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public abstract class ButtonBase : MonoBehaviour,IOnClickNoVariable
{
    [Tab("Base")]
    [SerializeField]
    private bool _addEventOnCode;
    [SerializeField,ShowIf("_addEventOnCode",true)]
    private Button _button;
    [EndIf]
    [EndTab]
    //
    private EventDispatcher _eventDispatcher;

    private void OnValidate()
    {
        if (!_button)
        {
            _button = GetComponent<Button>();
        }
    }

    protected virtual void OnEnable()
    {
        if (!_eventDispatcher)
        {
            _eventDispatcher = EventDispatcher.Instance;
        }

        if (_addEventOnCode)
        {
            _button.onClick.AddListener(OnClick);
        }
    }

    public abstract void OnClick();
}
