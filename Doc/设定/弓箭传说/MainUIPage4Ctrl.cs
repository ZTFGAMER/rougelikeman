using Dxx.Util;
using PureMVC.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUIPage4Ctrl : MediatorCtrlBase
{
    public GameObject window;
    public RectTransform titleparent;
    public Text Text_Setting;
    public Text Text_UserID;
    public Text Text_Version;
    public SettingMusicCtrl mMusicCtrl;
    public SettingSoundCtrl mSoundCtrl;
    public SettingLanguageCtrl mLanguageCtrl;
    public SettingQualityCtrl mQualityCtrl;
    public SettingProducterCtrl mProducterCtrl;
    public SettingReportCtrl mReportCtrl;
    private bool bOpened;
    private bool userid_showlong = true;

    private void InitUI()
    {
        this.OnLanguageChange();
    }

    protected override void OnClose()
    {
        this.bOpened = false;
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        float fringeHeight = PlatformHelper.GetFringeHeight();
        this.titleparent.anchoredPosition = new Vector2(0f, this.titleparent.anchoredPosition.y + fringeHeight);
        this.window.SetActive(false);
    }

    public override void OnLanguageChange()
    {
        this.Text_Setting.text = GameLogic.Hold.Language.GetLanguageByTID("设置_标题", Array.Empty<object>());
        this.mMusicCtrl.UpdateLanguage();
        this.mSoundCtrl.UpdateLanguage();
        this.mLanguageCtrl.UpdateLanguage();
        this.mQualityCtrl.UpdateLanguage();
        this.mProducterCtrl.UpdateLanguage();
        this.mReportCtrl.UpdateLanguage();
        this.update_userid();
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("AppVersion", Array.Empty<object>());
        object[] args = new object[] { languageByTID, PlatformHelper.GetAppVersionName().ToString() };
        string str2 = Utils.FormatString("{0}: {1}", args);
        this.Text_Version.text = str2;
    }

    protected override void OnOpen()
    {
        this.bOpened = true;
        this.window.SetActive(true);
        this.InitUI();
    }

    private void update_userid()
    {
        if (LocalSave.Instance.GetServerUserID() != 0L)
        {
            string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("设置_玩家ID", Array.Empty<object>());
            object[] args = new object[] { languageByTID, LocalSave.Instance.GetServerUserIDSub() };
            this.Text_UserID.text = Utils.FormatString("{0}: {1:D4}", args);
        }
        else
        {
            this.Text_UserID.text = string.Empty;
        }
    }
}

