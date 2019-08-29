using Dxx.Util;
using System;

public class AdventureTask1002 : AdventureTaskBase
{
    private int mCritCount;
    private int CritMaxCount = 20;

    public AdventureTask1002()
    {
        base.Event_OnCrit = new Action<EntityBase, int>(this.OnCrit);
    }

    protected override bool _IsTaskFinish() => 
        (this.mCritCount >= this.CritMaxCount);

    public override string GetShowTaskString()
    {
        object[] args = new object[] { this.mCritCount, this.CritMaxCount };
        return Utils.FormatString("暴击次数 : {0}/{1}", args);
    }

    private void OnCrit(EntityBase source, int hit)
    {
        this.mCritCount++;
        base.UpdateUI();
    }
}

