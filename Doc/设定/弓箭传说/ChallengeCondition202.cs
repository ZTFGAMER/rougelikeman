using System;

public class ChallengeCondition202 : ChallengeConditionBase
{
    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
        base.mChallenge.RecoverHP = false;
    }
}

