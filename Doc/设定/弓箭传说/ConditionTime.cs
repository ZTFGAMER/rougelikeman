using Dxx.Util;
using System;

public class ConditionTime : ConditionBase
{
    public float time;

    protected override void Init()
    {
    }

    public override bool IsEnd() => 
        ((Updater.AliveTime - base.starttime) >= this.time);
}

