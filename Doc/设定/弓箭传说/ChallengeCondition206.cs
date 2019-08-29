using Dxx.Util;
using System;

public class ChallengeCondition206 : ChallengeConditionBase
{
    private float time = 0.3f;

    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
        base.mChallenge.BombermanEnable = true;
        if (!float.TryParse(base.mArg, out this.time))
        {
            object[] args = new object[] { base.mArg };
            SdkManager.Bugly_Report("ChallengeCondition206", Utils.FormatString("[{0}] is not a int value.", args));
        }
        base.mChallenge.BombermanTime = this.time;
    }
}

