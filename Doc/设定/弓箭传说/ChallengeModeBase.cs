using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public abstract class ChallengeModeBase
{
    private int _id;
    protected Stage_Level_activity mActivity;
    protected string mData;
    protected Transform mParent;
    private List<ChallengeConditionBase> mConditions = new List<ChallengeConditionBase>();
    private PropType rewardtype;
    private int rewardid;
    private int rewardcount;
    private bool bMonsterHide;
    private float mHideRange = float.MaxValue;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool <RecoverHP>k__BackingField = true;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool <DropExp>k__BackingField = true;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool <AttackEnable>k__BackingField = true;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool <BombermanEnable>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private float <BombermanTime>k__BackingField = 0.5f;

    protected ChallengeModeBase()
    {
    }

    public void CheckCondition()
    {
        bool flag = true;
        if (this.mConditions.Count > 0)
        {
            int num = 0;
            int count = this.mConditions.Count;
            while (num < count)
            {
                if (this.mConditions[num].Result != 1)
                {
                    flag = false;
                    break;
                }
                num++;
            }
            if (flag)
            {
                this.OnSuccess();
            }
            else
            {
                this.OnFailure();
            }
        }
    }

    public void DeInit()
    {
        int num = 0;
        int count = this.mConditions.Count;
        while (num < count)
        {
            this.mConditions[num].DeInit();
            num++;
        }
        this.mConditions.Clear();
        this.OnDeInit();
    }

    public List<string> GetConditions()
    {
        List<string> list = new List<string>();
        int num = 0;
        int count = this.mConditions.Count;
        while (num < count)
        {
            list.Add(this.mConditions[num].GetConditionString());
            num++;
        }
        return list;
    }

    public object GetEvent(string eventname) => 
        this.OnGetEvent(eventname);

    public bool GetMonsterHide() => 
        this.bMonsterHide;

    public float GetMonsterHideRange() => 
        this.mHideRange;

    public void GetRewards()
    {
        this.InitRewards();
        switch (this.rewardtype)
        {
            case PropType.eCurrency:
                if (this.rewardid == 1)
                {
                    LocalSave.Instance.Modify_Gold((long) this.rewardcount, false);
                    CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, (long) this.rewardcount, null, null, true);
                }
                break;
        }
    }

    public string GetSuccessString() => 
        this.OnGetSuccessString();

    public void Init(Stage_Level_activity activity)
    {
        string[] args = activity.Args;
        if (args.Length <= 0)
        {
            object[] objArray1 = new object[] { activity.ID };
            SdkManager.Bugly_Report(base.GetType().ToString(), Utils.FormatString("Stage_Level_activity[{0}] args.length == 0", objArray1));
        }
        char[] separator = new char[] { ':' };
        string[] strArray2 = args[0].Split(separator);
        int.TryParse(strArray2[0], out this._id);
        this.mActivity = activity;
        if (strArray2.Length == 2)
        {
            this.mData = strArray2[1];
        }
        if (args.Length > 1)
        {
            int index = 1;
            int length = args.Length;
            while (index < length)
            {
                int result = 0;
                char[] chArray2 = new char[] { ':' };
                string[] strArray3 = args[index].Split(chArray2);
                if ((strArray3.Length > 0) && int.TryParse(strArray3[0], out result))
                {
                    string arg = string.Empty;
                    if (strArray3.Length > 1)
                    {
                        arg = strArray3[1];
                    }
                    object[] objArray2 = new object[] { "ChallengeCondition", result };
                    object[] objArray3 = new object[] { "ChallengeCondition", result };
                    ChallengeConditionBase item = Type.GetType(Utils.GetString(objArray2)).Assembly.CreateInstance(Utils.GetString(objArray3)) as ChallengeConditionBase;
                    item.Init(result, arg, this);
                    this.mConditions.Add(item);
                }
                index++;
            }
        }
        this.OnInit();
    }

    private void InitRewards()
    {
        int[] reward = this.mActivity.Reward;
        this.rewardtype = (PropType) reward[0];
        this.rewardid = reward[1];
        this.rewardcount = reward[2];
    }

    public void MonsterDead()
    {
        this.OnMonsterDead();
    }

    protected abstract void OnDeInit();
    protected void OnFailure()
    {
        GameLogic.Hold.BattleData.SetWin(false);
        WindowUI.ShowWindow(WindowID.WindowID_GameOver);
    }

    protected virtual object OnGetEvent(string eventname) => 
        null;

    protected abstract string OnGetSuccessString();
    protected abstract void OnInit();
    protected virtual void OnMonsterDead()
    {
    }

    protected virtual void OnSendEvent(string eventname, object body)
    {
    }

    protected abstract void OnStart();
    protected void OnSuccess()
    {
        GameLogic.Hold.BattleData.SetWin(true);
        WindowUI.ShowWindow(WindowID.WindowID_GameOver);
    }

    public void SendEvent(string eventname, object body = null)
    {
        this.OnSendEvent(eventname, body);
    }

    public void SetMonsterHide(float range)
    {
        this.bMonsterHide = true;
        this.mHideRange = range;
    }

    public void SetUIParent(Transform parent)
    {
        this.mParent = parent;
    }

    public void Start()
    {
        this.OnStart();
        int num = 0;
        int count = this.mConditions.Count;
        while (num < count)
        {
            this.mConditions[num].Start();
            num++;
        }
    }

    public int ID =>
        this._id;

    public bool RecoverHP { get; set; }

    public bool DropExp { get; set; }

    public bool AttackEnable { get; set; }

    public bool BombermanEnable { get; set; }

    public float BombermanTime { get; set; }
}

