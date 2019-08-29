namespace Dxx.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("UI/TiledImage")]
    public class TiledImage : RawImage
    {
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Vector2 sizeDelta = base.get_rectTransform().sizeDelta;
            base.set_uvRect(new Rect(0f, 0f, (sizeDelta.x / ((float) base.get_texture().width)) * base.get_canvas().scaleFactor, (sizeDelta.y / ((float) base.get_texture().height)) * base.get_canvas().scaleFactor));
        }
    }
}

