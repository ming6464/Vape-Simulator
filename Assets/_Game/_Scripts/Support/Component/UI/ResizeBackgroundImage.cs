using System;
using _Game._Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace ComponentUtilitys.UI
{
    public class ResizeBackgroundImage : MonoBehaviour
    {
        [Foldout("Info")]
        [SerializeField,Variants(0,1)]
        private int _match = 1;
        [SerializeField]
        private Vector2 _resolutionReference = new (1080,1920);
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

        private void Awake()
        {
            _imageRtf = _image.rectTransform;
        }
    
        private void OnEnable()
        {
            this.RegisterListener(EventID.ApplyBackground,ApplyBackground);
        }

        private void OnDisable()
        {
            this.RemoveListener(EventID.ApplyBackground,ApplyBackground);
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
            float width                = 0;
            float height               = 0;
            float screenHeightCalculate = 0;
            float screenWidthCalculate = 0;
            

            if (_match - 1 == 0)
            {
                screenHeightCalculate = _resolutionReference.y;
                var ratio           = screenHeightCalculate / Screen.height;
                screenWidthCalculate = Screen.width * ratio;
            }
            else
            {
                screenWidthCalculate = _resolutionReference.x;
                var ratio = screenWidthCalculate / Screen.width;
                screenHeightCalculate = Screen.height * ratio;
            }

            float subtractW = screenWidthCalculate - _resolution.x;
            float subtractH = screenHeightCalculate - _resolution.y;
    
            if (subtractW > subtractH)
            {
                width = screenWidthCalculate;
                var ratio = width / _resolution.x;
                height = _resolution.y * ratio;
            }
            else
            {
                height = screenHeightCalculate;
                var ratio = height / _resolution.y;
                width = _resolution.x * ratio;
            }
    
            _imageRtf.sizeDelta = new Vector2(width, height);
            _image.sprite       = _sprite;
        }

    }
}