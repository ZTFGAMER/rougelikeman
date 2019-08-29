using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BattleLoadUICtrl : MediatorCtrlBase
{
    private const string ShowAnimationName = "LoadShow";
    private const string MissAnimationName = "LoadMiss";
    public Animator ani;
    public CanvasGroup ani_canvasgroup;
    public Text Text_Content;
    public GameObject loadingparent;
    private float anitime;
    private BattleLoadProxy.BattleLoadData loaddata;
    private WaitForSecondsRealtime wait01;
    private WaitForSecondsRealtime opentime;
    private int startframe;
    private bool bStart;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache0;

    private void InitUI()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("BattleLoadProxy");
        if (proxy == null)
        {
            throw new Exception(Utils.FormatString("BattleLoadMediator.Proxy is null", Array.Empty<object>()));
        }
        this.loaddata = proxy.Data as BattleLoadProxy.BattleLoadData;
        if (this.loaddata == null)
        {
            throw new Exception(Utils.FormatString("BattleLoadMediator.Proxy BattleLoadData is null", Array.Empty<object>()));
        }
        GameLogic.SetPause(true);
        this.ani_canvasgroup.alpha = 0f;
        this.ani.enabled = true;
        this.loadingparent.SetActive(this.loaddata.showLoading);
        this.PlayOpen();
    }

    protected override void OnClose()
    {
        this.ani.enabled = false;
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.anitime = this.ani.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        this.wait01 = new WaitForSecondsRealtime(0.1f);
        this.opentime = new WaitForSecondsRealtime(this.anitime);
    }

    public override void OnLanguageChange()
    {
        object[] args = new object[] { GameLogic.Hold.Language.GetLanguageByTID("battleloading_content", Array.Empty<object>()) };
        this.Text_Content.text = Utils.FormatString("{0}...", args);
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void PlayClose()
    {
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayClose>m__3));
        TweenSettingsExtensions.AppendInterval(sequence, 0.03f);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = new TweenCallback(null, <PlayClose>m__4);
        }
        TweenSettingsExtensions.AppendCallback(sequence, <>f__am$cache0);
        TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayClose>m__5));
        TweenSettingsExtensions.AppendInterval(sequence, 0.03f);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayClose>m__6));
        TweenSettingsExtensions.AppendInterval(sequence, this.anitime);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayClose>m__7));
    }

    private void PlayOpen()
    {
        base.transform.parent.SetAsLastSibling();
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
        TweenSettingsExtensions.AppendInterval(sequence, 0.01f);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayOpen>m__0));
        TweenSettingsExtensions.AppendInterval(sequence, this.anitime);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayOpen>m__1));
        TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayOpen>m__2));
    }

    private void show_camera(bool value)
    {
        if (this.loaddata.showLoading)
        {
            GameNode.m_Camera.enabled = value;
            GameNode.m_UICamera.enabled = value;
        }
    }

    [DebuggerHidden]
    private IEnumerator start_play() => 
        new <start_play>c__Iterator0 { $this = this };

    private void Update()
    {
        if (this.bStart)
        {
            switch ((Time.frameCount - this.startframe))
            {
                case 12:
                    if (this.loaddata.LoadingDo != null)
                    {
                        this.loaddata.LoadingDo();
                    }
                    return;

                case 14:
                    this.show_camera(false);
                    GameLogic.UpdateResolution();
                    return;

                case 15:
                    this.show_camera(true);
                    this.ani.Play("LoadMiss");
                    if (this.loaddata.LoadEnd1Do != null)
                    {
                        this.loaddata.LoadEnd1Do();
                    }
                    GameLogic.SetPause(false);
                    return;

                case 0:
                    this.ani.Play("LoadShow");
                    break;

                case 0x1b:
                    if (this.loaddata.LoadEnd2Do != null)
                    {
                        this.loaddata.LoadEnd2Do();
                    }
                    WindowUI.CloseWindow(WindowID.WindowID_BattleLoad);
                    this.bStart = false;
                    return;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <start_play>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal BattleLoadUICtrl $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_0204;

                case 1:
                    this.$this.ani.Play("LoadShow");
                    this.$current = this.$this.opentime;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_0204;

                case 2:
                    if (this.$this.loaddata.LoadingDo != null)
                    {
                        this.$this.loaddata.LoadingDo();
                    }
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 3;
                    }
                    goto Label_0204;

                case 3:
                    this.$this.show_camera(false);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 4;
                    }
                    goto Label_0204;

                case 4:
                    GameLogic.UpdateResolution();
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 5;
                    }
                    goto Label_0204;

                case 5:
                    this.$this.ani.Play("LoadMiss");
                    if (this.$this.loaddata.LoadEnd1Do != null)
                    {
                        this.$this.loaddata.LoadEnd1Do();
                    }
                    GameLogic.SetPause(false);
                    this.$current = this.$this.opentime;
                    if (!this.$disposing)
                    {
                        this.$PC = 6;
                    }
                    goto Label_0204;

                case 6:
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 7;
                    }
                    goto Label_0204;

                case 7:
                    this.$this.show_camera(true);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 8;
                    }
                    goto Label_0204;

                case 8:
                    if (this.$this.loaddata.LoadEnd2Do != null)
                    {
                        this.$this.loaddata.LoadEnd2Do();
                    }
                    WindowUI.CloseWindow(WindowID.WindowID_BattleLoad);
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0204:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

