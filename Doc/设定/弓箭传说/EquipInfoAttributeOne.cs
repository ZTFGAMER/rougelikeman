using Dxx.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoAttributeOne : MonoBehaviour
{
    public Text Text_Attr;
    public OutLineDxx outline;
    public Image Image_Icon;

    public float GetTextHeight()
    {
        if (this.Text_Attr != null)
        {
            return this.Text_Attr.preferredHeight;
        }
        return 0f;
    }

    public void SetText(string value)
    {
        if (this.Text_Attr != null)
        {
            this.Text_Attr.text = value;
        }
        this.SetUnlock(true);
    }

    public void SetUnlock(bool value)
    {
        if (this.Text_Attr != null)
        {
            this.Text_Attr.set_color(!value ? Color.gray : Color.white);
        }
        this.Image_Icon.gameObject.SetActive(!value);
        this.ShowOutLine(value);
    }

    private void ShowOutLine(bool value)
    {
        if (this.outline != null)
        {
            this.outline.enabled = value;
        }
    }
}

