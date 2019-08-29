using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AndroidNative
{
    public static void CallStatic(string methodName, params object[] args)
    {
        <CallStatic>c__AnonStorey1 storey = new <CallStatic>c__AnonStorey1 {
            methodName = methodName,
            args = args
        };
        try
        {
            <CallStatic>c__AnonStorey0 storey2 = new <CallStatic>c__AnonStorey0 {
                <>f__ref$1 = storey
            };
            string className = "com.tag.nativepopup.PopupManager";
            storey2.bridge = new AndroidJavaObject(className, new object[0]);
            AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            object[] objArray1 = new object[] { new AndroidJavaRunnable(storey2.<>m__0) };
            class2.GetStatic<AndroidJavaObject>("currentActivity").Call("runOnUiThread", objArray1);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(exception.Message);
        }
    }

    public static void RedirectToAppStoreRatingPage(string appLink)
    {
        object[] args = new object[] { appLink };
        CallStatic("OpenAppRatingPage", args);
    }

    public static void RedirectToWebPage(string urlString)
    {
        object[] args = new object[] { urlString };
        CallStatic("OpenWebPage", args);
    }

    public static void showDialog(string title, string message, string yes, string no)
    {
        object[] args = new object[] { title, message, yes, no };
        CallStatic("ShowDialogPopup", args);
    }

    public static void showMessage(string title, string message, string ok)
    {
        object[] args = new object[] { title, message, ok };
        CallStatic("ShowMessagePopup", args);
    }

    public static void showRateUsPopUP(string title, string message, string rate, string remind, string declined)
    {
        object[] args = new object[] { title, message, rate, remind, declined };
        CallStatic("ShowRatePopup", args);
    }

    [CompilerGenerated]
    private sealed class <CallStatic>c__AnonStorey0
    {
        internal AndroidJavaObject bridge;
        internal AndroidNative.<CallStatic>c__AnonStorey1 <>f__ref$1;

        internal void <>m__0()
        {
            this.bridge.CallStatic(this.<>f__ref$1.methodName, this.<>f__ref$1.args);
        }
    }

    [CompilerGenerated]
    private sealed class <CallStatic>c__AnonStorey1
    {
        internal string methodName;
        internal object[] args;
    }
}

