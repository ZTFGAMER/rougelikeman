using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeMode102 : ChallengeModeBase
{
    private Text Text_Value;
    protected int currenttime;
    protected int alltime;
    private Sequence seq;
    private Sequence seq_update;

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
        }
        if (this.seq_update != null)
        {
            TweenExtensions.Kill(this.seq_update, false);
        }
    }

    protected override void OnDeInit()
    {
        this.KillSequence();
    }

    protected override string OnGetSuccessString()
    {
        object[] args = new object[] { this.currenttime };
        return Utils.FormatString("生存{0}秒", args);
    }

    protected override void OnInit()
    {
        if (!int.TryParse(base.mData, out this.alltime))
        {
            object[] args = new object[] { base.mData };
            SdkManager.Bugly_Report("ChallengeCondition103", Utils.FormatString("[{0}] is not a int value.", args));
        }
        this.currenttime = this.alltime;
    }

    protected override void OnStart()
    {
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, 1f);
        TweenSettingsExtensions.SetLoops<Sequence>(this.seq, this.alltime);
        TweenSettingsExtensions.OnStepComplete<Sequence>(this.seq, new TweenCallback(this, this.OnUpdateSecond));
        Transform transform = base.mParent.Find("Text_Value");
        if (transform == null)
        {
            SdkManager.Bugly_Report("ChallengeCondition102", "Text_Value is not found.");
        }
        this.Text_Value = transform.GetComponent<Text>();
        this.UpdateText();
    }

    protected virtual void OnUpdate()
    {
    }

    private void OnUpdateSecond()
    {
        this.currenttime--;
        if (this.currenttime <= 0)
        {
            this.KillSequence();
            base.OnSuccess();
        }
        else
        {
            this.UpdateText();
            this.OnUpdate();
        }
    }

    private void UpdateText()
    {
        if (this.Text_Value != null)
        {
            object[] args = new object[] { this.currenttime };
            this.Text_Value.text = Utils.FormatString("{0}", args);
        }
    }
}

