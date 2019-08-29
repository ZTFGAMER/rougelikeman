using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeMode101 : ChallengeModeBase
{
    private Text Text_Value;
    private int currenttime;
    private int alltime;
    private Sequence seq;

    protected override void OnDeInit()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
        }
    }

    protected override string OnGetSuccessString()
    {
        object[] args = new object[] { this.currenttime / 60 };
        return Utils.FormatString("{0}分钟内通关", args);
    }

    protected override void OnInit()
    {
        if (!int.TryParse(base.mData, out this.alltime))
        {
            object[] args = new object[] { base.mData };
            SdkManager.Bugly_Report("ChallengeCondition101", Utils.FormatString("[{0}] is not a int value.", args));
        }
        this.currenttime = this.alltime;
    }

    protected override void OnStart()
    {
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, 1f);
        TweenSettingsExtensions.SetLoops<Sequence>(this.seq, this.alltime);
        TweenSettingsExtensions.OnStepComplete<Sequence>(this.seq, new TweenCallback(this, this.OnUpdate));
        Transform transform = base.mParent.Find("Text_Value");
        if (transform == null)
        {
            SdkManager.Bugly_Report("ChallengeCondition101", "Text_Value is not found.");
        }
        this.Text_Value = transform.GetComponent<Text>();
        this.UpdateText();
    }

    private void OnUpdate()
    {
        this.currenttime--;
        if (this.currenttime <= 0)
        {
            if (this.seq != null)
            {
                TweenExtensions.Kill(this.seq, false);
                this.seq = null;
            }
            base.OnFailure();
        }
        else
        {
            this.UpdateText();
        }
    }

    private void UpdateText()
    {
        if (this.Text_Value != null)
        {
            object[] args = new object[] { Utils.GetSecond2String(this.currenttime) };
            this.Text_Value.text = Utils.FormatString("{0}", args);
        }
    }
}

