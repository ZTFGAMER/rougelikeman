using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class AndroidRateUsPopUp : MonoBehaviour
{
    public string title;
    public string message;
    public string rate;
    public string remind;
    public string declined;
    public string appLink;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event OnRateUSPopupComplete onRateUSPopupComplete;

    public static AndroidRateUsPopUp Create() => 
        Create("Like the Game?", "Rate US");

    public static AndroidRateUsPopUp Create(string title, string message) => 
        Create(title, message, "Rate Now", "Ask me later", "No, thanks");

    public static AndroidRateUsPopUp Create(string title, string message, string rate, string remind, string declined)
    {
        AndroidRateUsPopUp up = new GameObject("AndroidRateUsPopUp").AddComponent<AndroidRateUsPopUp>();
        up.title = title;
        up.message = message;
        up.rate = rate;
        up.remind = remind;
        up.declined = declined;
        up.init();
        return up;
    }

    public void init()
    {
        AndroidNative.showRateUsPopUP(this.title, this.message, this.rate, this.remind, this.declined);
    }

    public void OnRatePopUpCallBack(string buttonIndex)
    {
        switch (Convert.ToInt16(buttonIndex))
        {
            case 0:
                AndroidNative.RedirectToAppStoreRatingPage(this.appLink);
                this.RaiseOnOnRateUSPopupComplete(MessageState.RATED);
                break;

            case 1:
                this.RaiseOnOnRateUSPopupComplete(MessageState.REMIND);
                break;

            case 2:
                this.RaiseOnOnRateUSPopupComplete(MessageState.DECLINED);
                break;
        }
        UnityEngine.Object.Destroy(base.gameObject);
    }

    private void RaiseOnOnRateUSPopupComplete(MessageState state)
    {
        if (onRateUSPopupComplete != null)
        {
            onRateUSPopupComplete(state);
        }
    }

    public delegate void OnRateUSPopupComplete(MessageState state);
}

