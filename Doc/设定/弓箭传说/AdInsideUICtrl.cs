using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AdInsideUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_shadow;
    public RawImage image;
    public VideoPlayer mPlayer;
    public AudioSource mAudioSource;
    public AdInsideTimeCtrl mTimeCtrl;
    private AdInsideProxy.Transfer mTransfer;
    private bool bSoundOpen;
    private bool bMusicOpen;
    private float updatetime;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.mPlayer.add_loopPointReached(new VideoPlayer.EventHandler(this, this.OnLoopPointReached));
        base.StartCoroutine(this.play_video());
    }

    protected override void OnClose()
    {
        GameLogic.Hold.Sound.ResumeBackgroundMusic();
        this.mPlayer.remove_loopPointReached(new VideoPlayer.EventHandler(this, this.OnLoopPointReached));
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
            <>f__am$cache0 = () => RateUrlManager.OpenAdUrl();
        }
        this.Button_shadow.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
    }

    private void OnLoopPointReached(VideoPlayer video)
    {
        WindowUI.CloseWindow(WindowID.WindowID_AdInside);
        if (this.mTransfer != null)
        {
            this.mTransfer.finish_callback();
        }
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("AdInsideProxy");
        if (((proxy == null) || (proxy.Data == null)) || !(proxy.Data is AdInsideProxy.Transfer))
        {
            SdkManager.Bugly_Report("AdInsideUICtrl", "AdInsideProxy is invalid.");
            this.OnLoopPointReached(null);
        }
        else
        {
            this.updatetime = 0f;
            GameLogic.Hold.Sound.PauseBackgroundMusic();
            this.mTransfer = proxy.Data as AdInsideProxy.Transfer;
            this.mTimeCtrl.SetMax((float) this.mPlayer.get_clip().get_length());
            this.InitUI();
        }
    }

    [DebuggerHidden]
    private IEnumerator play_video() => 
        new <play_video>c__Iterator0 { $this = this };

    private void Update()
    {
        if (((this.mPlayer != null) && (this.mPlayer.get_texture() != null)) && (this.image != null))
        {
            this.updatetime += Time.unscaledDeltaTime;
            this.image.set_texture(this.mPlayer.get_texture());
            this.mTimeCtrl.SetCurrent((float) this.mPlayer.get_time());
        }
    }

    [CompilerGenerated]
    private sealed class <play_video>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal AdInsideUICtrl $this;
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
                    this.$this.mPlayer.EnableAudioTrack(0, true);
                    this.$this.mPlayer.SetTargetAudioSource(0, this.$this.mAudioSource);
                    this.$this.mPlayer.Prepare();
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_00D8;

                default:
                    goto Label_00F4;
            }
            if (!this.$this.mPlayer.get_isPrepared())
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_00F6;
            }
            this.$this.mPlayer.Play();
            this.$this.mAudioSource.Play();
        Label_00D8:
            while (this.$this.mPlayer.get_isPlaying())
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_00F6;
            }
            this.$PC = -1;
        Label_00F4:
            return false;
        Label_00F6:
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

