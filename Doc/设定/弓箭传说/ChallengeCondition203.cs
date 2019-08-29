using System;

public class ChallengeCondition203 : ChallengeConditionBase
{
    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
        base.mChallenge.DropExp = false;
    }
}

