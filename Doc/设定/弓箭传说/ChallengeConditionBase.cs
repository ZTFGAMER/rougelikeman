using Dxx.Util;
using System;

public abstract class ChallengeConditionBase
{
    private int _id;
    protected int result = 1;
    protected string mArg;
    protected ChallengeModeBase mChallenge;

    protected ChallengeConditionBase()
    {
    }

    public void DeInit()
    {
        this.OnDeInit();
    }

    public string GetConditionString()
    {
        object[] args = new object[] { this.ID };
        return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("Challenge_Condition{0}", args), Array.Empty<object>());
    }

    public void Init(int id, string arg, ChallengeModeBase challengedata)
    {
        this._id = id;
        this.mArg = arg;
        this.mChallenge = challengedata;
        this.OnInit();
    }

    protected abstract void OnDeInit();
    protected void OnFailure()
    {
        this.result = 2;
        this.mChallenge.CheckCondition();
    }

    protected abstract void OnInit();
    protected virtual void OnStart()
    {
    }

    protected void OnSuccess()
    {
        this.result = 1;
        this.mChallenge.CheckCondition();
    }

    public void Start()
    {
        this.OnStart();
    }

    public int ID =>
        this._id;

    public int Result =>
        this.result;
}

