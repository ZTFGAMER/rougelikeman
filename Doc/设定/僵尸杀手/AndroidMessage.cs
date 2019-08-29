using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class AndroidMessage : MonoBehaviour
{
    public string title;
    public string message;
    public string ok;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event OnMessagePopupComplete onMessagePopupComplete;

    public static AndroidMessage Create(string title, string message) => 
        Create(title, message, "Ok");

    public static AndroidMessage Create(string title, string message, string ok)
    {
        AndroidMessage message2 = new GameObject("AndroidMessagePopup").AddComponent<AndroidMessage>();
        message2.title = title;
        message2.message = message;
        message2.ok = ok;
        message2.init();
        return message2;
    }

    public void init()
    {
        AndroidNative.showMessage(this.title, this.message, this.ok);
    }

    public void OnMessagePopUpCallBack(string buttonIndex)
    {
        this.RaiseOnMessagePopupComplete(MessageState.OK);
        UnityEngine.Object.Destroy(base.gameObject);
    }

    private void RaiseOnMessagePopupComplete(MessageState state)
    {
        if (onMessagePopupComplete != null)
        {
            onMessagePopupComplete(state);
        }
    }

    public delegate void OnMessagePopupComplete(MessageState state);
}

