using DG.Tweening;
using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class VideoUICtrl : MediatorCtrlBase
{
    public Image Image_Boss;
    public Image Image_Hero;
    public Text Text_1;
    public Text Text_2;
    private bool bStartLogin;
    private bool bShowNet;
    private int mLoginSate;
    private Sequence seq;
    private Sequence seq_login;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache0;

    private void InitUI()
    {
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    protected override void OnClose()
    {
        this.KillSequence();
        this.ShowNetDoing(false);
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.mLoginSate = 0;
        LocalSave.Instance.DoLogin_Start(new Action(this.OnLoginCallback));
        this.bStartLogin = false;
        this.Text_1.set_color(new Color(1f, 1f, 1f, 0f));
        this.Text_2.set_color(new Color(1f, 1f, 1f, 0f));
        this.Image_Boss.set_color(new Color(1f, 1f, 1f, 0f));
        this.Image_Hero.set_color(new Color(1f, 1f, 1f, 0f));
        this.Image_Hero.get_rectTransform().anchoredPosition = new Vector2(0f, -300f);
        this.KillSequence();
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnInit>m__0));
        TweenSettingsExtensions.AppendInterval(this.seq, 2f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnInit>m__1));
        TweenSettingsExtensions.Append(this.seq, ShortcutExtensions46.DOFade(this.Image_Hero, 1f, 1f));
        TweenSettingsExtensions.Join(this.seq, ShortcutExtensions46.DOAnchorPosY(this.Image_Hero.get_rectTransform(), 0f, 1f, false));
        TweenSettingsExtensions.Append(this.seq, ShortcutExtensions46.DOFade(this.Text_2, 1f, 1f));
        TweenSettingsExtensions.AppendInterval(this.seq, 1f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnInit>m__2));
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnInit>m__3));
    }

    public override void OnLanguageChange()
    {
        this.Text_1.text = GameLogic.Hold.Language.GetLanguageByTID("cg_text_1", Array.Empty<object>());
        this.Text_2.text = GameLogic.Hold.Language.GetLanguageByTID("cg_text_2", Array.Empty<object>());
    }

    private void OnLoginCallback()
    {
        if (LocalSave.Instance.GetServerUserID() <= 0L)
        {
            this.mLoginSate = 2;
        }
        else
        {
            this.mLoginSate = 1;
        }
    }

    private void OnLoginCallback_Retry()
    {
        if (LocalSave.Instance.GetServerUserID() <= 0L)
        {
            this.ShowRetry();
        }
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void ShowNetDoing(bool value)
    {
        if ((value && (LocalSave.Instance.GetServerUserID() <= 0L)) && !this.bShowNet)
        {
            this.bShowNet = true;
            WindowUI.ShowNetDoing(true, NetDoingType.netdoing_http);
        }
        if (!value && this.bShowNet)
        {
            WindowUI.ShowNetDoing(false, NetDoingType.netdoing_http);
        }
    }

    private void ShowRetry()
    {
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("first_login_title", Array.Empty<object>());
        string content = GameLogic.Hold.Language.GetLanguageByTID("first_login_content", Array.Empty<object>());
        string sure = GameLogic.Hold.Language.GetLanguageByTID("common_retry", Array.Empty<object>());
        WindowUI.ShowPopWindowOneUI(languageByTID, content, sure, false, () => TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1f), new TweenCallback(this, this.<ShowRetry>m__6)));
    }

    private void Update()
    {
        if (this.bStartLogin && (LocalSave.Instance.GetServerUserID() > 0L))
        {
            WindowUI.CloseWindow(WindowID.WindowID_VideoPlay);
            WindowUI.ShowWindow(WindowID.WindowID_Login);
        }
    }
}

