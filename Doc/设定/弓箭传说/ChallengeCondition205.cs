using System;

public class ChallengeCondition205 : ChallengeConditionBase
{
    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
        base.mChallenge.AttackEnable = false;
    }
}

