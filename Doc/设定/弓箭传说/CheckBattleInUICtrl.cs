using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class CheckBattleInUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Content;
    public ButtonCtrl Button_Sure;
    public ButtonCtrl Button_Refuse;
    public Text Text_Sure;
    public Text Text_Refuse;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;
    [CompilerGenerated]
    private static Action <>f__am$cache2;

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
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate {
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = () => LocalSave.Instance.BattleIn_Check();
                }
                WindowUI.ShowLoading(<>f__am$cache2, null, null, BattleLoadProxy.LoadingType.eMiss);
            };
        }
        this.Button_Sure.onClick = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate {
                LocalSave.Instance.BattleIn_DeInit();
                WindowUI.CloseWindow(WindowID.WindowID_CheckBattleIn);
            };
        }
        this.Button_Refuse.onClick = <>f__am$cache1;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("UICommon_Tip", Array.Empty<object>());
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗", Array.Empty<object>());
        this.Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗确定", Array.Empty<object>());
        this.Text_Refuse.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗取消", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }
}

