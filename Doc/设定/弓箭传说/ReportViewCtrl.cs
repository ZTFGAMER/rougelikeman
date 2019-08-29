using System;
using UnityEngine;
using UnityEngine.UI;

public class ReportViewCtrl : MonoBehaviour
{
    public Canvas mCanvas;
    public CanvasScaler mScaler;
    public RectTransform view;

    private void Awake()
    {
        this.mCanvas.worldCamera = GameNode.m_UICamera;
    }

    public void Init(float width, float height)
    {
        this.mScaler.set_referenceResolution(new Vector2((float) Screen.width, (float) Screen.height));
        float x = (width * Screen.width) / ((float) GameLogic.DesignWidth);
        float y = (height * Screen.height) / ((float) GameLogic.DesignHeight);
        float num3 = (Screen.width - x) / 2f;
        float num4 = (Screen.height - y) / 2f;
        this.view.sizeDelta = new Vector2(x, y);
        this.view.anchoredPosition = new Vector2(num3, num4);
    }
}

