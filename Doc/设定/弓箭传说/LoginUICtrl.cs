using DG.Tweening;
using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LoginUICtrl : MediatorCtrlBase
{
    public GameObject loginobj;
    public Image Image_BG;
    public Image Image_Splash1;
    public CanvasGroup mCanvasGroup;
    private Sequence seq_loading;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache0;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache1;

    private void KillSequence()
    {
        if (this.seq_loading != null)
        {
            TweenExtensions.Kill(this.seq_loading, false);
            this.seq_loading = null;
        }
    }

    protected override void OnClose()
    {
        this.KillSequence();
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
    }

    protected override void OnInitBefore()
    {
        base.bInitSize = false;
    }

    public override void OnLanguageChange()
    {
    }

    private void OnLogin()
    {
        this.KillSequence();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = new TweenCallback(null, <OnLogin>m__0);
        }
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = new TweenCallback(null, <OnLogin>m__1);
        }
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.2f), <>f__am$cache0), 0.3f), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOFade(this.mCanvasGroup, 0f, 0.5f), 1)), <>f__am$cache1);
    }

    protected override void OnOpen()
    {
        this.OnLogin();
    }
}

