using System;
using UnityEngine;
using UnityEngine.UI;

public class StageListSkillsCtrl : MonoBehaviour
{
    public RectTransform imageRect;
    public Text Text_Value;

    public void Refresh(string value)
    {
        this.Text_Value.text = value;
        float x = (this.imageRect.sizeDelta.x + this.Text_Value.preferredWidth) + 5f;
        float num2 = -x / 2f;
        float num3 = (num2 + this.imageRect.sizeDelta.x) + 5f;
        this.imageRect.anchoredPosition = new Vector2(num2, this.imageRect.anchoredPosition.y);
        this.Text_Value.get_rectTransform().anchoredPosition = new Vector2(num3, this.Text_Value.get_rectTransform().anchoredPosition.y);
        RectTransform transform = base.transform as RectTransform;
        transform.sizeDelta = new Vector2(x, this.imageRect.sizeDelta.y);
    }
}

