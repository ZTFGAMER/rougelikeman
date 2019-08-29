using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LanguageUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public LanguageInfinity mInfinity;
    public GameObject copyitems;
    private List<string> mList;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.mInfinity.SetItemCount(this.mList.Count);
        this.mInfinity.Refresh();
    }

    private void OnClickLanguage(LanguageOneCtrl one)
    {
        GameLogic.Hold.Language.ChangeLanguage(one.mLanguage);
        this.mInfinity.Refresh();
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
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Language);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
        this.mInfinity.Init(1);
        this.mInfinity.updatecallback = new Action<int, LanguageOneCtrl>(this.UpdateChildCallBack);
        this.mList = SettingLanguageCtrl.GetLanguageList();
        this.copyitems.SetActive(false);
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("设置_语言", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
        this.InitUI();
    }

    private void UpdateChildCallBack(int index, LanguageOneCtrl one)
    {
        one.Init(index, this.mList[index]);
        one.OnClickButton = new Action<LanguageOneCtrl>(this.OnClickLanguage);
    }
}

