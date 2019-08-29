using Dxx.Util;
using System;
using UnityEngine;

public class CurrencyExpCtrl : MonoBehaviour
{
    private ProgressTextCtrl mProgressTextCtrl;

    private void Awake()
    {
        Transform transform = base.transform.Find("fg/ProgressTextBar");
        if (transform != null)
        {
            this.mProgressTextCtrl = transform.GetComponent<ProgressTextCtrl>();
        }
    }

    public void SetProgress(int current, int max)
    {
        if (max <= 0)
        {
            object[] args = new object[] { current, max };
            SdkManager.Bugly_Report("CurrencyExpCtrl.cs", Utils.FormatString("SetProgress({0}, {1}) is invalid!", args));
        }
        else if (this.mProgressTextCtrl != null)
        {
            this.mProgressTextCtrl.current = current;
            this.mProgressTextCtrl.max = max;
        }
    }
}

