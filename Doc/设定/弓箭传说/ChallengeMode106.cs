using DG.Tweening;
using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ChallengeMode106 : ChallengeModeBase
{
    private BattleMatchDefenceTime_ConditionCtrl mCtrl;
    protected int currenttime;
    protected int alltime;
    private Sequence seq;
    private Sequence seq_update;
    private int roomids_index;
    private long mRandomSeed;
    private XRandom mRandom;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map2;

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

    protected override object OnGetEvent(string eventname)
    {
        if (eventname != null)
        {
            if (eventname == "MatchDefenceTime_get_random_roomid_row")
            {
                return this.roomids_index;
            }
            if (eventname != "MatchDefenceTime_get_random_int")
            {
                if (eventname == "MatchDefenceTime_get_xrandom")
                {
                    return this.mRandom;
                }
            }
            else
            {
                if (this.mRandom != null)
                {
                    return this.mRandom.nextInt();
                }
                SdkManager.Bugly_Report("ChallengeMode106", "OnGetEvent get_random_int mRandom is null.");
            }
        }
        return null;
    }

    protected override string OnGetSuccessString()
    {
        object[] args = new object[] { this.currenttime };
        return Utils.FormatString("防守{0}秒", args);
    }

    protected override void OnInit()
    {
        if (!int.TryParse(base.mData, out this.alltime))
        {
            object[] args = new object[] { base.mData };
            SdkManager.Bugly_Report("ChallengeMode106", Utils.FormatString("[{0}] is not a int value.", args));
        }
        this.currenttime = this.alltime;
    }

    protected override void OnSendEvent(string eventname, object body)
    {
        if (eventname != null)
        {
            if (<>f__switch$map2 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(8) {
                    { 
                        "MatchDefenceTime_set_random_seed",
                        0
                    },
                    { 
                        "MatchDefenceTime_me_updatescore",
                        1
                    },
                    { 
                        "MatchDefenceTime_other_updatescore",
                        2
                    },
                    { 
                        "MatchDefenceTime_me_updatename",
                        3
                    },
                    { 
                        "MatchDefenceTime_other_updatename",
                        4
                    },
                    { 
                        "MatchDefenceTime_other_dead",
                        5
                    },
                    { 
                        "MatchDefenceTime_other_reborn",
                        5
                    },
                    { 
                        "MatchDefenceTime_other_learn_skill",
                        5
                    }
                };
                <>f__switch$map2 = dictionary;
            }
            if (<>f__switch$map2.TryGetValue(eventname, out int num))
            {
                switch (num)
                {
                    case 0:
                        this.mRandomSeed = (long) body;
                        this.mRandom = new XRandom(this.mRandomSeed);
                        this.roomids_index = this.mRandom.nextInt(0, 3);
                        this.roomids_index = 0;
                        break;

                    case 1:
                    {
                        int num2 = (int) body;
                        if (this.mCtrl != null)
                        {
                            this.mCtrl.SetMeScore(num2);
                        }
                        break;
                    }
                    case 2:
                    {
                        int num3 = (int) body;
                        if (this.mCtrl != null)
                        {
                            this.mCtrl.SetOtherScore(num3);
                        }
                        break;
                    }
                    case 3:
                    {
                        string name = (string) body;
                        if (this.mCtrl != null)
                        {
                            this.mCtrl.SetMeName(name);
                        }
                        break;
                    }
                    case 4:
                    {
                        string name = (string) body;
                        if (this.mCtrl != null)
                        {
                            this.mCtrl.SetOtherName(name);
                        }
                        break;
                    }
                    case 5:
                        if (this.mCtrl != null)
                        {
                            this.mCtrl.ShowInfo(eventname, body);
                        }
                        break;
                }
            }
        }
    }

    protected override void OnStart()
    {
        this.mCtrl = base.mParent.GetComponent<BattleMatchDefenceTime_ConditionCtrl>();
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, 1f);
        TweenSettingsExtensions.SetLoops<Sequence>(this.seq, this.alltime);
        TweenSettingsExtensions.OnStepComplete<Sequence>(this.seq, new TweenCallback(this, this.OnUpdateSecond));
        TweenSettingsExtensions.SetUpdate<Sequence>(this.seq, true);
        if (this.mCtrl != null)
        {
            this.mCtrl.SetTime(this.currenttime);
            this.mCtrl.SetMeScore(0);
            this.mCtrl.SetOtherScore(0);
        }
        Facade.Instance.SendNotification("MatchDefenceTime_other_updatescore", 50);
    }

    protected virtual void OnUpdate()
    {
    }

    private void OnUpdateSecond()
    {
        this.currenttime--;
        if (this.mCtrl != null)
        {
            this.mCtrl.SetTime(this.currenttime);
        }
        if (this.currenttime <= 0)
        {
            this.KillSequence();
            Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eGameEnd, 0);
            Singleton<MatchDefenceTimeSocketCtrl>.Instance.Close();
            if ((this.mCtrl != null) && this.mCtrl.isWin())
            {
                base.OnSuccess();
            }
            else
            {
                base.OnFailure();
            }
        }
        else
        {
            this.OnUpdate();
        }
    }
}

