using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using UnityEngine.UI;

public class ChangeAccountUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Content;
    public Text Text_Sure;
    public Text Text_Refuse;
    public ButtonCtrl Button_Sure;
    public ButtonCtrl Button_Refuse;
    private ChangeAccountProxy.Transfer mTransfer;

    private void InitUI()
    {
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
        this.Button_Sure.onClick = delegate {
            if ((this.mTransfer != null) && (this.mTransfer.callback_sure != null))
            {
                this.mTransfer.callback_sure();
            }
        };
        this.Button_Refuse.onClick = delegate {
            WindowUI.CloseWindow(WindowID.WindowID_ChangeAccount);
            if ((this.mTransfer != null) && (this.mTransfer.callback_confirm != null))
            {
                this.mTransfer.callback_confirm();
            }
        };
    }

    public override void OnLanguageChange()
    {
        this.Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗确定", Array.Empty<object>());
        this.Text_Refuse.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗取消", Array.Empty<object>());
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("title_warning", Array.Empty<object>());
        object[] args = new object[] { LocalSave.Instance.GetUserName() };
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("changeaccount_content", args);
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("ChangeAccountProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen ChangeAccountProxy is null");
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen ChangeAccountProxy.Data is null");
        }
        else
        {
            this.mTransfer = proxy.Data as ChangeAccountProxy.Transfer;
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

