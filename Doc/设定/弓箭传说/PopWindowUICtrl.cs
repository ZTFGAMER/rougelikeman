using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class PopWindowUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Content;
    public Text Text_Sure;
    public Text Text_Refuse;
    public ButtonCtrl Button_Sure;
    public ButtonCtrl Button_Refuse;
    private PopWindowProxy.Transfer mTransfer;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.Text_Title.text = this.mTransfer.title;
        this.Text_Content.text = this.mTransfer.content;
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
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_PopWindow);
        }
        this.Button_Refuse.onClick = <>f__am$cache0;
        this.Button_Sure.onClick = delegate {
            this.Button_Refuse.onClick();
            if ((this.mTransfer != null) && (this.mTransfer.callback != null))
            {
                this.mTransfer.callback();
            }
        };
    }

    public override void OnLanguageChange()
    {
        this.Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("popwindow_sure", Array.Empty<object>());
        this.Text_Refuse.text = GameLogic.Hold.Language.GetLanguageByTID("popwindow_cancel", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("PopWindowProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen PopWindowProxy is null");
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen PopWindowProxy.Data is null");
        }
        else
        {
            this.mTransfer = proxy.Data as PopWindowProxy.Transfer;
            if (this.mTransfer == null)
            {
                SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen ChangeAccountProxy.Data is null");
            }
            else
            {
                this.InitUI();
            }
        }
    }
}

