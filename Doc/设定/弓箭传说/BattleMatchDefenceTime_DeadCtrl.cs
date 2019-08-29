using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleMatchDefenceTime_DeadCtrl : MonoBehaviour
{
    public GameObject child;
    public Text Text_DeadContent;
    public Text Text_DeadTime;
    private int mTime;
    private int mCurrentTime;
    private Action mCallback;
    private Sequence seq;

    public void Deinit()
    {
        this.KillSequence();
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    public void OnLanguageChange()
    {
        this.Text_DeadContent.text = "复活倒计时";
    }

    private void OnUpdateSecond()
    {
        this.mCurrentTime--;
        if (this.mCurrentTime <= 0)
        {
            this.KillSequence();
            if (this.mCallback != null)
            {
                this.mCallback();
            }
            this.Show(false);
        }
        else
        {
            this.SetTime(this.mCurrentTime);
        }
    }

    private void SetTime(int time)
    {
        this.Text_DeadTime.text = time.ToString();
    }

    public void SetTime(int time, Action callback)
    {
        this.mTime = time;
        this.mCallback = callback;
        this.mCurrentTime = this.mTime;
        this.KillSequence();
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, 1f);
        TweenSettingsExtensions.SetLoops<Sequence>(this.seq, this.mTime);
        TweenSettingsExtensions.OnStepComplete<Sequence>(this.seq, new TweenCallback(this, this.OnUpdateSecond));
        TweenSettingsExtensions.SetUpdate<Sequence>(this.seq, true);
        this.SetTime(this.mCurrentTime);
    }

    public void Show(bool value)
    {
        this.child.SetActive(value);
    }
}

