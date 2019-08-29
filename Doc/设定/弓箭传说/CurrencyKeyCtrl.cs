using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyKeyCtrl : MonoBehaviour
{
    [NonSerialized]
    public ProgressCtrl mProgressCtrl;
    [NonSerialized]
    public ProgressTextCtrl mProgressTextCtrl;
    private int mBeforeKey = 1;
    private Image Image_Key;

    private void Awake()
    {
        Transform transform = base.transform.Find("fg/ProgressTextBar");
        if (transform != null)
        {
            this.mProgressTextCtrl = transform.GetComponent<ProgressTextCtrl>();
        }
        Transform transform2 = base.transform.Find("fg/ProgressBar");
        if (transform2 != null)
        {
            this.mProgressCtrl = transform2.GetComponent<ProgressCtrl>();
        }
        this.Image_Key = base.transform.Find("fg/Image/Image").GetComponent<Image>();
    }

    private void ChangeImage(int current)
    {
        if ((this.mBeforeKey * current) <= 0)
        {
            this.mBeforeKey = current;
            this.Image_Key.set_sprite(SpriteManager.GetUICommon((current < 0) ? "Currency_Key2" : "Currency_Key"));
        }
    }

    public void SetProgress(string text)
    {
    }

    public void SetProgress(int current, int max)
    {
        if (max <= 0)
        {
            object[] args = new object[] { current, max };
            SdkManager.Bugly_Report("CurrencyKeyCtrl.cs", Utils.FormatString("SetProgress({0}, {1}) is invalid!", args));
        }
        else if (this.mProgressTextCtrl != null)
        {
            this.mProgressTextCtrl.max = max;
            if (current > 0)
            {
                this.mProgressTextCtrl.current = current;
                this.mProgressCtrl.Value = 0f;
            }
            else
            {
                this.mProgressTextCtrl.current = 0;
                this.mProgressTextCtrl.SetText(-current.ToString());
                if (this.mProgressCtrl != null)
                {
                    this.mProgressCtrl.Value = ((float) -current) / ((float) max);
                }
            }
            this.ChangeImage(current);
        }
    }
}

