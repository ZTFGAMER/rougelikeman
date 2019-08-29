using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using TableTool;

public class BoxOpenOneUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Close;
    public BoxOpenOneCurrencyCtrl mCurrencyCtrl;
    public BoxOpenOneEquipCtrl mEquipCtrl;
    private int state;
    private Drop_DropModel.DropData mTransfer;
    private Sequence seq;
    private Sequence seq_close;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache0;

    private void CloseUI()
    {
        WindowUI.CloseWindow(WindowID.WindowID_BoxOpenOne);
    }

    private void DelayClose()
    {
        this.seq_close = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1f), new TweenCallback(this, this.<DelayClose>m__2));
    }

    private void InitUI()
    {
        this.mCurrencyCtrl.gameObject.SetActive(false);
        this.mEquipCtrl.gameObject.SetActive(false);
        this.seq = DOTween.Sequence();
        PropType type = this.mTransfer.type;
        if (type == PropType.eCurrency)
        {
            this.mCurrencyCtrl.gameObject.SetActive(true);
            TweenSettingsExtensions.Append(this.seq, this.mCurrencyCtrl.Init(this.mTransfer));
        }
        else if (type == PropType.eEquip)
        {
            this.mEquipCtrl.gameObject.SetActive(true);
            LocalSave.EquipOne equip = new LocalSave.EquipOne {
                EquipID = this.mTransfer.id,
                Level = 1,
                Count = 1
            };
            TweenSettingsExtensions.Append(this.seq, this.mEquipCtrl.Init(equip, this.mTransfer.count));
        }
        else
        {
            object[] args = new object[] { this.mTransfer.ToString() };
            SdkManager.Bugly_Report("BoxOpenOneUICtrl", Utils.FormatString("InitUI {0}", args));
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = new TweenCallback(null, <InitUI>m__0);
        }
        TweenSettingsExtensions.AppendCallback(this.seq, <>f__am$cache0);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<InitUI>m__1));
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
        if (this.seq_close != null)
        {
            TweenExtensions.Kill(this.seq_close, false);
            this.seq_close = null;
        }
    }

    private void OnClickButton()
    {
        switch (this.state)
        {
            case 0:
                this.state = 1;
                if (this.seq != null)
                {
                    TweenExtensions.Complete(this.seq, true);
                    this.seq = null;
                }
                break;

            case 1:
                this.CloseUI();
                break;
        }
    }

    protected override void OnClose()
    {
        this.KillSequence();
        if (this.mTransfer.OnClose != null)
        {
            this.mTransfer.OnClose();
        }
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.Button_Close.onClick = new Action(this.OnClickButton);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.state = 0;
        IProxy proxy = Facade.Instance.RetrieveProxy("BoxOpenOneProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("BoxOpenOneCtrl", Utils.FormatString("BoxOpenOneProxy is null.", Array.Empty<object>()));
            this.CloseUI();
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("BoxOpenOneCtrl", Utils.FormatString("BoxOpenOneProxy.Data is null.", Array.Empty<object>()));
            this.CloseUI();
        }
        else if (!(proxy.Data is Drop_DropModel.DropData))
        {
            SdkManager.Bugly_Report("BoxOpenOneCtrl", Utils.FormatString("BoxOpenOneProxy is not Drop_DropModel.DropData.", Array.Empty<object>()));
            this.CloseUI();
        }
        else
        {
            this.mTransfer = proxy.Data as Drop_DropModel.DropData;
            this.InitUI();
        }
    }
}

