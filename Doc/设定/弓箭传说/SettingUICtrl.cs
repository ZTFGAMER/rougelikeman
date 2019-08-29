using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SettingUICtrl : MediatorCtrlBase
{
    public GameObject loginObj;
    public GameObject logoutObj;
    public Text Text_Title;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public Text Text_Music;
    public Text Text_Sound;
    public Text Text_Language;
    public Text Text_Producter;
    public Text Text_Login;
    public Text Text_ButtonLogin;
    public Text Text_Logout;
    public Text Text_Quality;
    public ButtonCtrl Button_Producter;
    public ButtonCtrl Button_Logout;
    public ButtonCtrl Button_Login;
    public SettingMusicCtrl mMusicCtrl;
    public SettingSoundCtrl mSoundCtrl;
    public SettingLanguageCtrl mLanguageCtrl;
    public SettingQualityCtrl mQualityCtrl;
    public Text Text_Version;
    public Text Text_UserID;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;
    [CompilerGenerated]
    private static Action <>f__am$cache2;
    [CompilerGenerated]
    private static Action <>f__am$cache3;

    private void InitUI()
    {
        LoginType loginType = LocalSave.Instance.GetLoginType();
        if (string.IsNullOrEmpty(LocalSave.Instance.GetUserID()) || (loginType == LoginType.eInvalid))
        {
            this.loginObj.SetActive(true);
            this.logoutObj.SetActive(false);
        }
        else
        {
            this.loginObj.SetActive(false);
            this.logoutObj.SetActive(true);
            string languageByTID = string.Empty;
            switch (loginType)
            {
                case LoginType.eGP:
                    languageByTID = GameLogic.Hold.Language.GetLanguageByTID("设置_登录GP", Array.Empty<object>());
                    break;

                case LoginType.eWeiXin:
                    languageByTID = GameLogic.Hold.Language.GetLanguageByTID("设置_登录微信", Array.Empty<object>());
                    break;

                case LoginType.eGameCenter:
                    languageByTID = GameLogic.Hold.Language.GetLanguageByTID("设置_登录GameCenter", Array.Empty<object>());
                    break;
            }
            if (!string.IsNullOrEmpty(languageByTID))
            {
                object[] args = new object[] { languageByTID };
                this.Text_Login.text = GameLogic.Hold.Language.GetLanguageByTID("设置_已登录", args);
            }
        }
        this.loginObj.SetActive(false);
        this.logoutObj.SetActive(false);
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if ((name == null) || (name == "PUB_UI_UPDATE_PING"))
        {
        }
    }

    protected override void OnInit()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Setting);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => WindowUI.ShowWindow(WindowID.WindowID_Producer);
        }
        this.Button_Producter.onClick = <>f__am$cache1;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = delegate {
                LocalSave.Instance.SetUserID(LoginType.eInvalid, string.Empty, string.Empty);
                SdkManager.OnLogin();
            };
        }
        this.Button_Logout.onClick = <>f__am$cache2;
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = delegate {
            };
        }
        this.Button_Login.onClick = <>f__am$cache3;
    }

    public override void OnLanguageChange()
    {
        ulong serverUserID = LocalSave.Instance.GetServerUserID();
        if (serverUserID != 0L)
        {
            object[] objArray1 = new object[] { GameLogic.Hold.Language.GetLanguageByTID("设置_玩家ID", Array.Empty<object>()), serverUserID };
            this.Text_UserID.text = Utils.FormatString("{0}:{1}", objArray1);
        }
        else
        {
            this.Text_UserID.text = string.Empty;
        }
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("设置_标题", Array.Empty<object>());
        this.Text_Music.text = GameLogic.Hold.Language.GetLanguageByTID("设置_音乐", Array.Empty<object>());
        this.Text_Sound.text = GameLogic.Hold.Language.GetLanguageByTID("设置_音效", Array.Empty<object>());
        this.Text_Language.text = GameLogic.Hold.Language.GetLanguageByTID("设置_语言", Array.Empty<object>());
        this.Text_Producter.text = GameLogic.Hold.Language.GetLanguageByTID("设置_制作人员", Array.Empty<object>());
        this.Text_Logout.text = GameLogic.Hold.Language.GetLanguageByTID("设置_退出登录", Array.Empty<object>());
        this.Text_ButtonLogin.text = GameLogic.Hold.Language.GetLanguageByTID("设置_登录", Array.Empty<object>());
        this.Text_Quality.text = GameLogic.Hold.Language.GetLanguageByTID("设置_画面质量", Array.Empty<object>());
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("AppVersion", Array.Empty<object>());
        object[] args = new object[] { languageByTID, PlatformHelper.GetAppVersionName().ToString() };
        string str2 = Utils.FormatString("{0}:{1}", args);
        this.Text_Version.text = str2;
        this.mMusicCtrl.UpdateLanguage();
        this.mSoundCtrl.UpdateLanguage();
        this.mLanguageCtrl.UpdateLanguage();
        this.mQualityCtrl.UpdateLanguage();
    }

    protected override void OnOpen()
    {
        GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
        this.InitUI();
    }
}

