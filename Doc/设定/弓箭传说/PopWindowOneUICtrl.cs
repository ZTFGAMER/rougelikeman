using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class PopWindowOneUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Content;
    public Text Text_Sure;
    public ButtonCtrl Button_Sure;
    public ButtonCtrl Button_Close;
    private PopWindowOneProxy.Transfer mTransfer;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.Text_Title.text = this.mTransfer.title;
        this.Text_Content.text = this.mTransfer.content;
        this.Text_Sure.text = this.mTransfer.sure;
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
            WindowUI.CloseWindow(WindowID.WindowID_PopWindowOne);
            if ((this.mTransfer != null) && (this.mTransfer.callback != null))
            {
                this.mTransfer.callback();
            }
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_PopWindowOne);
        }
        this.Button_Close.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("PopWindowOneProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("PopWindowOneUICtrl", "OnOpen PopWindowOneProxy is null");
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("PopWindowOneUICtrl", "OnOpen PopWindowOneProxy.Data is null");
        }
        else
        {
            this.mTransfer = proxy.Data as PopWindowOneProxy.Transfer;
            if (this.mTransfer == null)
            {
                SdkManager.Bugly_Report("PopWindowOneUICtrl", "OnOpen PopWindowOneProxy.Data is null");
            }
            else
            {
                this.Button_Close.gameObject.SetActive(this.mTransfer.showclosebutton);
                this.InitUI();
            }
        }
    }
}

