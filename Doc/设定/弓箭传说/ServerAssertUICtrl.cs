using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using UnityEngine.UI;

public class ServerAssertUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Content;
    private ServerAssertProxy.Transfer mTransfer;

    private void InitUI()
    {
        string str = string.Empty;
        DateTime time = new DateTime(this.mTransfer.assertendtime);
        DateTime now = DateTime.Now;
        TimeSpan span = (TimeSpan) (time - now);
        if (span.Days >= 1)
        {
            object[] objArray1 = new object[] { span.Days };
            str = GameLogic.Hold.Language.GetLanguageByTID("time_day", objArray1);
        }
        else if (span.Hours >= 1)
        {
            object[] objArray2 = new object[] { span.Hours };
            str = GameLogic.Hold.Language.GetLanguageByTID("time_hour", objArray2);
        }
        else
        {
            int num = MathDxx.Clamp(span.Minutes, 10, span.Minutes);
            object[] objArray3 = new object[] { num };
            str = GameLogic.Hold.Language.GetLanguageByTID("time_hour", objArray3);
        }
        object[] args = new object[] { str };
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("serverassert_content", args);
        this.Text_Content.text = languageByTID;
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("serverassert_title", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("ServerAssertProxy");
        if ((proxy == null) || (proxy.Data == null))
        {
            SdkManager.Bugly_Report("ServerAssertUICtrl", "proxy is null.");
        }
        if (!(proxy.Data is ServerAssertProxy.Transfer))
        {
            SdkManager.Bugly_Report("ServerAssertUICtrl", "proxy.Data is not a ServerAssertProxy.Transfer.");
        }
        this.mTransfer = proxy.Data as ServerAssertProxy.Transfer;
        if ((this.mTransfer == null) || (this.mTransfer.assertendtime == 0L))
        {
            SdkManager.Bugly_Report("ServerAssertUICtrl", "mTransfer is invalid.");
        }
        this.InitUI();
    }
}

