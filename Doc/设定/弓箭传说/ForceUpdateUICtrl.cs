using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class ForceUpdateUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Content;
    public Text Text_Sure;
    public ButtonCtrl Button_Sure;
    private ChangeAccountProxy.Transfer mTransfer;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

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
            <>f__am$cache0 = () => RateUrlManager.OpenAppUrl();
        }
        this.Button_Sure.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
        this.Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗确定", Array.Empty<object>());
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("title_warning", Array.Empty<object>());
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("forceupdate_content", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }
}

