using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CountDownCtrl : MonoBehaviour
{
    public GameObject child;
    public Image Image_Fill;
    public Text Text_Time;
    public Image Image_Arrow;
    private bool bShow = true;
    private string timestring;

    public string GetTimeString() => 
        this.timestring;

    public void Refresh(long time, float percent)
    {
        TimeSpan span = Utils.GetTime(time);
        this.Image_Fill.fillAmount = percent;
        this.Image_Arrow.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - (percent * 360f));
        if (span.Days > 0)
        {
            object[] args = new object[] { span.Days.ToString(), span.Hours.ToString() };
            this.Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("倒计时_dh", args);
        }
        else if (span.Hours > 0)
        {
            object[] args = new object[] { span.Hours.ToString(), span.Minutes.ToString() };
            this.Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("倒计时_hm", args);
        }
        else if (span.Minutes > 0)
        {
            object[] args = new object[] { span.Minutes.ToString(), span.Seconds.ToString() };
            this.Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("倒计时_ms", args);
        }
        else
        {
            object[] args = new object[] { span.Seconds.ToString() };
            this.Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("倒计时_s", args);
        }
        this.timestring = this.Text_Time.text;
    }

    public void Show(bool show)
    {
        if (this.bShow != show)
        {
            this.bShow = show;
            this.child.SetActive(show);
        }
    }
}

