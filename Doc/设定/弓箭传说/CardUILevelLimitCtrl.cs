using System;
using UnityEngine;
using UnityEngine.UI;

public class CardUILevelLimitCtrl : MonoBehaviour
{
    public GameObject child;
    public RectTransform levelrect;
    public Text Text_Level;
    public Text Text_Info1;
    public Text Text_Info2;
    private float interval = 10f;

    public void Init(int level)
    {
        this.Text_Level.text = level.ToString();
        this.Text_Info1.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_UpgradeLevel1", Array.Empty<object>());
        this.Text_Info2.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_UpgradeLevel2", Array.Empty<object>());
        float preferredWidth = this.Text_Info1.preferredWidth;
        float x = this.levelrect.sizeDelta.x;
        float num3 = this.Text_Info2.preferredWidth;
        float num4 = -(((preferredWidth + x) + num3) + (this.interval * 2f)) / 2f;
        this.Text_Info1.get_rectTransform().anchoredPosition = new Vector2(num4 + (preferredWidth / 2f), 0f);
        this.levelrect.anchoredPosition = new Vector2(((this.Text_Info1.get_rectTransform().anchoredPosition.x + (preferredWidth / 2f)) + (x / 2f)) + this.interval, 0f);
        this.Text_Info2.get_rectTransform().anchoredPosition = new Vector2(((this.levelrect.anchoredPosition.x + (x / 2f)) + (num3 / 2f)) + this.interval, 0f);
    }

    public void Show(bool value)
    {
        this.child.SetActive(value);
    }
}

