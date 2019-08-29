using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class RewardShowUICtrl : MediatorCtrlBase
{
    public BoxOpenGetCtrl mGetCtrl;
    public ButtonCtrl mButtonClose;
    public TapToCloseCtrl mTapCloseCtrl;
    public ButtonCtrl Button_Shadow;
    public GameObject copyitems;
    private RewardShowProxy.Transfer mTransfer;
    private List<Drop_DropModel.DropData> mEquipTransfer = new List<Drop_DropModel.DropData>();
    private int currentIndex;
    private EState mState;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache1;

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
        this.currentIndex = 0;
        int num = 0;
        int count = this.mEquipTransfer.Count;
        while (num < count)
        {
            this.mEquipTransfer[num].OnClose = new Action(this.OnOneUIClose);
            num++;
        }
        GameLogic.Hold.Sound.PlayUI(0xf4249);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.2f), new TweenCallback(this, this.<InitUI>m__1));
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
        this.copyitems.SetActive(false);
        this.Button_Shadow.onClick = new Action(this.OnClickShadow);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_RewardShow);
        }
        this.mButtonClose.onClick = <>f__am$cache0;
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
        IProxy proxy = Facade.Instance.RetrieveProxy("RewardShowProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("RewardShowUICtrl", Utils.FormatString("proxy is null", Array.Empty<object>()));
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("RewardShowUICtrl", Utils.FormatString("proxy.Data is null", Array.Empty<object>()));
        }
        else if (!(proxy.Data is RewardShowProxy.Transfer))
        {
            SdkManager.Bugly_Report("RewardShowUICtrl", Utils.FormatString("proxy is not a RewardShowProxy.Transfer.", Array.Empty<object>()));
        }
        else
        {
            this.mTransfer = proxy.Data as RewardShowProxy.Transfer;
            if (this.mTransfer.list == null)
            {
                SdkManager.Bugly_Report("RewardShowUICtrl", Utils.FormatString("proxy.list is null.", Array.Empty<object>()));
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
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = new TweenCallback(null, <OnScrollEnd>m__3);
        }
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.15f), <>f__am$cache1), 0.2f), new TweenCallback(this, this.<OnScrollEnd>m__4));
    }

    private void PlayCurrent()
    {
        if (this.currentIndex < this.mEquipTransfer.Count)
        {
            TweenSettingsExtensions.AppendCallback(DOTween.Sequence(), new TweenCallback(this, this.<PlayCurrent>m__2));
        }
        else
        {
            this.PlayGet();
        }
    }

    private void PlayGet()
    {
        this.mGetCtrl.Show(true);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Append(DOTween.Sequence(), this.mGetCtrl.Init(this.mEquipTransfer)), new TweenCallback(this, this.<PlayGet>m__5));
    }

    private void show_close(bool value)
    {
        this.mButtonClose.transform.parent.gameObject.SetActive(value);
    }

    private enum EState
    {
        eDoing,
        eDone
    }
}

