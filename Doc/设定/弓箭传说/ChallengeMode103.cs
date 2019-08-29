using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeMode103 : ChallengeModeBase
{
    private Text Text_TimeValue;
    private Text Text_CountValue;
    private int currenttime;
    private int alltime;
    private int currentmonster;
    private int allmonster;
    private Sequence seq;
    private bool bSuccess;

    protected override void OnDeInit()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
        }
    }

    protected override string OnGetSuccessString()
    {
        object[] args = new object[] { this.alltime, this.allmonster };
        return Utils.FormatString("{0}秒内击杀{1}只怪物", args);
    }

    protected override void OnInit()
    {
        char[] separator = new char[] { ',' };
        string[] strArray = base.mData.Split(separator);
        if (strArray.Length < 2)
        {
            object[] args = new object[] { base.mData };
            SdkManager.Bugly_Report("ChallengeCondition106", Utils.FormatString("[{0}] length < 2.", args));
        }
        if (!int.TryParse(strArray[0], out this.alltime))
        {
            object[] args = new object[] { strArray[0] };
            SdkManager.Bugly_Report("ChallengeCondition106", Utils.FormatString("strs[0]:[{0}] is not a int value.", args));
        }
        if (!int.TryParse(strArray[1], out this.allmonster))
        {
            object[] args = new object[] { strArray[1] };
            SdkManager.Bugly_Report("ChallengeCondition106", Utils.FormatString("strs[1]:[{0}] is not a int value.", args));
        }
        this.currenttime = this.alltime;
        this.currentmonster = 0;
    }

    protected override void OnMonsterDead()
    {
        this.currentmonster++;
        this.UpdateMonster();
    }

    protected override void OnStart()
    {
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, 1f);
        TweenSettingsExtensions.SetLoops<Sequence>(this.seq, this.alltime);
        TweenSettingsExtensions.OnStepComplete<Sequence>(this.seq, new TweenCallback(this, this.OnUpdate));
        Transform transform = base.mParent.Find("Condition1/Text_Value");
        if (transform == null)
        {
            SdkManager.Bugly_Report("ChallengeCondition106", "Condition1/Text_Value is not found.");
        }
        this.Text_TimeValue = transform.GetComponent<Text>();
        Transform transform2 = base.mParent.Find("Condition2/Text_Value");
        if (transform2 == null)
        {
            SdkManager.Bugly_Report("ChallengeCondition106", "Condition2/Text_Value is not found.");
        }
        this.Text_CountValue = transform2.GetComponent<Text>();
        this.UpdateText();
        this.UpdateMonster();
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
        this.UpdateText();
    }

    private void UpdateMonster()
    {
        this.UpdateMonsterText();
        if ((this.currentmonster >= this.allmonster) && !this.bSuccess)
        {
            this.bSuccess = true;
            TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.5f), new TweenCallback(this, this.<UpdateMonster>m__0));
        }
    }

    private void UpdateMonsterText()
    {
        if ((this.Text_CountValue != null) && (this.currentmonster <= this.allmonster))
        {
            object[] args = new object[] { this.currentmonster, this.allmonster };
            this.Text_CountValue.text = Utils.FormatString("{0}/{1}", args);
        }
    }

    private void UpdateText()
    {
        if (this.Text_TimeValue != null)
        {
            object[] args = new object[] { this.currenttime };
            this.Text_TimeValue.text = Utils.FormatString("{0}", args);
        }
    }
}

