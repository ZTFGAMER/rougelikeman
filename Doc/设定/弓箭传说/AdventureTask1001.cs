using Dxx.Util;
using System;

public class AdventureTask1001 : AdventureTaskBase
{
    private int mHittedCount;
    private int HittedMaxCount = 3;

    public AdventureTask1001()
    {
        base.Event_OnHitted = new Action<EntityBase>(this.OnHitted);
    }

    protected override bool _IsTaskFinish() => 
        (this.mHittedCount < this.HittedMaxCount);

    public override string GetShowTaskString()
    {
        object[] args = new object[] { this.mHittedCount, this.HittedMaxCount };
        return Utils.FormatString("受击次数 : {0}/{1}", args);
    }

    private void OnHitted(EntityBase source)
    {
        this.mHittedCount++;
        base.UpdateUI();
    }
}

