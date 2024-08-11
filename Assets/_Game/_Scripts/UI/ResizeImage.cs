using _Game._Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class ResizeImage : MonoBehaviour
{
    [Foldout("Reference")]
    [SerializeField]
    private Image _image;
    [Foldout("Data")]
    [SerializeField]
    private Sprite _sprite;

    [SerializeField]
    private Vector2 _resolution;
    [EndFoldout]
    //
    private RectTransform _imageRtf;

    private EventDispatcher _eventDispatcher;
    
    private void Awake()
    {
        _imageRtf = _image.rectTransform;
    }

    private void OnEnable()
    {
        if (!_eventDispatcher)
        {
            _eventDispatcher = EventDispatcher.Instance;
        }
        _eventDispatcher.RegisterListener(EventID.ApplyBackground,ApplyBackground);
    }

    private void ApplyBackground(object obj)
    {
        var bgInfo = (BackgroundInfo)obj;
        _resolution = bgInfo.resolution;
        _sprite     = bgInfo.sprite;
        ApplySpriteResolution();
    }
    void Start()
    {
        ApplySpriteResolution();
    }
    private void ApplySpriteResolution()
    {
        float height    = 0;
        float width     = 0;
        float subtractW = Screen.width - _resolution.x;
        float subtractH = Screen.height - _resolution.y;

        if (subtractW > subtractH)
        {
            width = Screen.width;
            var ratio = width / _resolution.x;
            height = _resolution.y * ratio;
        }
        else
        {
            height = Screen.height;
            var ratio = height / _resolution.y;
            width = _resolution.x * ratio;
        }

        _imageRtf.sizeDelta = new Vector2(width, height);
        _image.sprite       = _sprite;
    }
    
}
