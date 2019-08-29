using System;
using UnityEngine;

public class MainBottomUICtrl : MonoBehaviour
{
    public RectTransform bottomline;

    private void Start()
    {
        float bottomHeight = PlatformHelper.GetBottomHeight();
        (base.transform as RectTransform).anchoredPosition = new Vector2(0f, bottomHeight);
        if (this.bottomline != null)
        {
            this.bottomline.anchoredPosition = new Vector2(0f, -bottomHeight / GameLogic.WidthScaleAll);
        }
    }
}

