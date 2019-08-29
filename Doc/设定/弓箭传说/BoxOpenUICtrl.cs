using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenUICtrl : MediatorCtrlBase
{
    public BoxOpenBoxCtrl mBoxCtrl;
    public BoxOpenGetCtrl mGetCtrl;
    public ButtonCtrl mButtonRetry;
    public Text Text_Retry;
    public GoldTextCtrl mGoldCtrl;
    public ButtonCtrl mButtonClose;
    public TapToCloseCtrl mTapCloseCtrl;
    public ButtonCtrl Button_Shadow;
    public GameObject copyitems;
    private BoxOpenProxy.Transfer mTransfer;
    private List<Drop_DropModel.DropData> mEquipTransfer = new List<Drop_DropModel.DropData>();
    private int currentIndex;
    private EState mState;

    private void ChangeState(EState state)
    {
        this.mState = state;
        if (this.mState == EState.eDone)
        {
            this.show_close(true);
        }
    }

    private void ExcuteEquips()
    {
        this.mEquipTransfer.Clear();
        int num = 0;
        int count = this.mTransfer.list.Count;
        while (num < count)
        {
            if (this.mTransfer.list[num].type == PropType.eEquip)
            {
                Drop_DropModel.DropData item = new Drop_DropModel.DropData {
                    type = this.mTransfer.list[num].type
                };
                int haveCount = this.GetHaveCount(this.mTransfer.list[num].id);
                item.id = this.mTransfer.list[num].id;
                item.count = this.mTransfer.list[num].count;
                this.mEquipTransfer.Add(item);
                SdkManager.send_event_equipment("GET", item.id, item.count, 1, this.mTransfer.source, 0);
            }
            else
            {
                this.mEquipTransfer.Add(this.mTransfer.list[num]);
            }
            num++;
        }
    }

    private int GetHaveCount(int id)
    {
        int num = 0;
        int num2 = 0;
        int count = this.mEquipTransfer.Count;
        while (num2 < count)
        {
            if (this.mEquipTransfer[num2].id == id)
            {
                num += this.mEquipTransfer[num2].count;
            }
            num2++;
        }
        return num;
    }

    private void InitUI()
    {
        this.show_close(false);
        this.mTapCloseCtrl.Show(false);
        this.mState = EState.eDoing;
        this.mGetCtrl.Show(false);
        this.mBoxCtrl.mBoxCtrl.ShowOpenEffect(false);
        this.mBoxCtrl.gameObject.SetActive(true);
        this.mBoxCtrl.PlayScrollShow(false);
        this.mBoxCtrl.mBoxCtrl.ShowBoxOpeningEffect(false);
        this.mBoxCtrl.mBoxCtrl.ShowBoxOneEffect(false);
        this.currentIndex = 0;
        int num = 0;
        int count = this.mEquipTransfer.Count;
        while (num < count)
        {
            this.mEquipTransfer[num].OnClose = new Action(this.OnOneUIClose);
            num++;
        }
        GameLogic.Hold.Sound.PlayUI(0xf4249);
        this.mBoxCtrl.mBoxCtrl.Play("BoxOpenShow");
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(sequence, 0.9f);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<InitUI>m__2));
    }

    private void OnClickShadow()
    {
        if (this.mState != EState.eDone)
        {
        }
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
        base.mWindowID = WindowID.WindowID_BoxOpen;
        this.copyitems.SetActive(false);
        this.mBoxCtrl.mScrollCtrl.OnScrollEnd = new Action(this.OnScrollEnd);
        this.Button_Shadow.onClick = new Action(this.OnClickShadow);
        this.mButtonClose.onClick = () => WindowUI.CloseWindow(base.mWindowID);
        this.mButtonRetry.onClick = delegate {
            if ((this.mTransfer != null) && (this.mTransfer.retry_callback != null))
            {
                if (LocalSave.Instance.GetDiamond() < this.mTransfer.GetCurrentDiamond())
                {
                    WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, null);
                }
                else
                {
                    this.mTransfer.retry_callback();
                }
            }
        };
    }

    public override void OnLanguageChange()
    {
    }

    private void OnOneUIClose()
    {
        this.currentIndex++;
        this.PlayCurrent();
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("BoxOpenProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("proxy is null", Array.Empty<object>()));
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("proxy.Data is null", Array.Empty<object>()));
        }
        else if (!(proxy.Data is BoxOpenProxy.Transfer))
        {
            SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("proxy is not a BoxOpenProxy.Transfer.", Array.Empty<object>()));
        }
        else
        {
            this.mTransfer = proxy.Data as BoxOpenProxy.Transfer;
            if (this.mTransfer.list == null)
            {
                SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("proxy.list is null.", Array.Empty<object>()));
            }
            else
            {
                this.ExcuteEquips();
                this.InitUI();
            }
        }
    }

    private void OnScrollEnd()
    {
        this.mBoxCtrl.mBoxCtrl.Play("BoxOpenOpen");
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.15f), new TweenCallback(this, this.<OnScrollEnd>m__4)), 0.2f), new TweenCallback(this, this.<OnScrollEnd>m__5));
    }

    private void PlayCurrent()
    {
        if (this.currentIndex < this.mEquipTransfer.Count)
        {
            TweenSettingsExtensions.AppendCallback(DOTween.Sequence(), new TweenCallback(this, this.<PlayCurrent>m__3));
        }
        else
        {
            this.PlayGet();
        }
    }

    private void PlayGet()
    {
        this.mBoxCtrl.gameObject.SetActive(false);
        this.mGetCtrl.Show(true);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Append(DOTween.Sequence(), this.mGetCtrl.Init(this.mEquipTransfer)), new TweenCallback(this, this.<PlayGet>m__6));
    }

    private void show_close(bool value)
    {
        this.mButtonRetry.transform.parent.gameObject.SetActive(value);
        this.mButtonClose.transform.parent.gameObject.SetActive(value);
        if (value)
        {
            this.update_retry_button();
        }
    }

    private void update_retry_button()
    {
        this.mGoldCtrl.SetCurrencyType(CurrencyType.Diamond);
        this.mGoldCtrl.SetValue(this.mTransfer.GetCurrentDiamond());
    }

    private enum EState
    {
        eDoing,
        eDone
    }
}

