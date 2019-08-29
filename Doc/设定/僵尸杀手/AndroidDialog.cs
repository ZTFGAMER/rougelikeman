using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class AndroidDialog : MonoBehaviour
{
    public string title;
    public string message;
    public string yes;
    public string no;
    public string urlString;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event OnDialogPopupComplete onDialogPopupComplete;

    public static AndroidDialog Create(string title, string message) => 
        Create(title, message, "Yes", "No");

    public static AndroidDialog Create(string title, string message, string yes, string no)
    {
        AndroidDialog dialog = new GameObject("AndroidDialogPopup").AddComponent<AndroidDialog>();
        dialog.title = title;
        dialog.message = message;
        dialog.yes = yes;
        dialog.no = no;
        dialog.init();
        return dialog;
    }

    public void init()
    {
        AndroidNative.showDialog(this.title, this.message, this.yes, this.no);
    }

    public void OnDialogPopUpCallBack(string buttonIndex)
    {
        switch (Convert.ToInt16(buttonIndex))
        {
            case 0:
                AndroidNative.RedirectToWebPage(this.urlString);
                this.RaiseOnOnDialogPopupComplete(MessageState.YES);
                break;

            case 1:
                this.RaiseOnOnDialogPopupComplete(MessageState.NO);
                break;
        }
        UnityEngine.Object.Destroy(base.gameObject);
    }

    private void RaiseOnOnDialogPopupComplete(MessageState state)
    {
        if (onDialogPopupComplete != null)
        {
            onDialogPopupComplete(state);
        }
    }

    public delegate void OnDialogPopupComplete(MessageState state);
}

