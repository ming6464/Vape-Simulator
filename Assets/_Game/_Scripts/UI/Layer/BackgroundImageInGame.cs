using _Game._Scripts.Data;
using ComponentUtilitys.UI;

namespace _Game._Scripts.UI.Layer
{
    public class BackgroundImageInGame : ResizeBackgroundImage
    {
        protected virtual void OnEnable()
        {
            this.RegisterListener(EventID.ApplyBackground,ApplyBackground);
        }

        protected virtual void OnDisable()
        {
            this.RemoveListener(EventID.ApplyBackground,ApplyBackground);
        }
        
        protected virtual void ApplyBackground(object obj)
        {
            var bgInfo = (BackgroundInfo)obj;
            _resolution = bgInfo.resolution;
            _sprite     = bgInfo.sprite;
            ApplySpriteResolution();
        }
        
    }
}