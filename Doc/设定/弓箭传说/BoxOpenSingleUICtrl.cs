using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenSingleUICtrl : MediatorCtrlBase
{
    public BoxOpenBoxAniCtrl mBoxCtrl;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public GameObject effect_light;
    public GameObject titleparent;
    public CanvasGroup equipparent;
    public CanvasGroup nameparent;
    public Text Text_Quality;
    public Text Text_Name;
    public Text Text_Info;
    public Image Image_BG;
    public Image Image_Icon;
    public BoxOpenSingleRetryCtrl mRetryCtrl;
    private LocalSave.EquipOne equipdata;
    private SequencePool mSeqPool = new SequencePool();
    private BoxOpenSingleProxy.Transfer mTransfer;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache1;

    private void InitUI()
    {
        this.mBoxCtrl.Init();
        SdkManager.send_event_equipment("GET", this.mTransfer.data.id, this.mTransfer.data.count, 1, this.mTransfer.source, 0);
        this.mRetryCtrl.transform.localScale = Vector3.zero;
        this.equipdata = new LocalSave.EquipOne();
        this.equipdata.EquipID = this.mTransfer.data.id;
        this.effect_light.SetActive(false);
        this.Text_Name.text = this.equipdata.NameOnlyString;
        this.Text_Quality.text = this.equipdata.QualityString;
        this.Text_Name.set_color(this.equipdata.qualityColor);
        this.Text_Quality.set_color(this.equipdata.qualityColor);
        this.Text_Info.text = this.equipdata.InfoString;
        this.Text_Info.set_color(new Color(1f, 1f, 1f, 0f));
        this.titleparent.SetActive(false);
        this.equipparent.alpha = 0f;
        this.nameparent.alpha = 0f;
        this.nameparent.transform.localScale = Vector3.one * 1.5f;
        this.equipparent.transform.localScale = Vector3.one;
        this.show_close(false);
        this.mBoxCtrl.ShowOpenEffect(false);
        this.mBoxCtrl.transform.localScale = Vector3.one;
        this.mBoxCtrl.ShowBoxOpeningEffect(false);
        this.mBoxCtrl.ShowBoxOneEffect(false);
        GameLogic.Hold.Sound.PlayUI(0xf4249);
        this.mBoxCtrl.Play(BoxOpenBoxAniCtrl.BoxState.BoxOpenShow, this.mTransfer.boxtype);
        Sequence sequence = this.mSeqPool.Get();
        TweenSettingsExtensions.AppendInterval(sequence, 0.9f);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<InitUI>m__2));
    }

    protected override void OnClose()
    {
        this.mSeqPool.Clear();
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        base.mWindowID = WindowID.WindowID_BoxOpen;
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_BoxOpenSingle);
        }
        this.Button_Close.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
        this.mRetryCtrl.OnLanguageChange();
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("BoxOpenSingleProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("BoxOpenSingleUICtrl", Utils.FormatString("proxy is null", Array.Empty<object>()));
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("BoxOpenSingleUICtrl", Utils.FormatString("proxy.Data is null", Array.Empty<object>()));
        }
        else if (!(proxy.Data is BoxOpenSingleProxy.Transfer))
        {
            SdkManager.Bugly_Report("BoxOpenSingleUICtrl", Utils.FormatString("proxy is not a BoxOpenSingleProxy.Transfer.", Array.Empty<object>()));
        }
        else
        {
            this.mTransfer = proxy.Data as BoxOpenSingleProxy.Transfer;
            if (this.mTransfer.data == null)
            {
                SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("Transfer.data is null.", Array.Empty<object>()));
            }
            else
            {
                this.mRetryCtrl.onRetry = delegate {
                    if ((this.mTransfer != null) && (this.mTransfer.retry_callback != null))
                    {
                        if ((LocalSave.Instance.GetDiamondBoxFreeCount(this.mTransfer.boxtype) == 0) && (LocalSave.Instance.GetDiamond() < this.mTransfer.GetCurrentDiamond()))
                        {
                            WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, null);
                        }
                        else
                        {
                            this.mTransfer.retry_callback();
                        }
                    }
                };
                this.InitUI();
            }
        }
    }

    private void show_close(bool value)
    {
        this.Button_Close.transform.parent.gameObject.SetActive(value);
    }
}

