using DG.Tweening;
using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class GameOverMatchDefenceTimeCtrl : GameOverModeCtrlBase
{
    public TapToCloseCtrl mCloseCtrl;
    public Text Text_Result;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void OnClickClose()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.ShowWindow(WindowID.WindowID_Main);
        }
        WindowUI.ShowLoading(<>f__am$cache0, null, null, BattleLoadProxy.LoadingType.eMiss);
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        base.OnGetEvent(eventName);

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.mCloseCtrl.OnClose = new Action(this.OnClickClose);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.UpdateUI();
    }

    private void UpdateUI()
    {
        this.mCloseCtrl.Show(false);
        if (GameLogic.Hold.BattleData.Win)
        {
            this.Text_Result.text = "You win!";
        }
        else
        {
            this.Text_Result.text = "You loss.";
        }
        TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1f), new TweenCallback(this, this.<UpdateUI>m__0)), true);
    }
}

