using System;

public class BulletHitOnceBase : BulletBase
{
    private ConditionBase mCondition1;
    private ConditionBase mCondition2;
    public int DelayTimeEnable = 100;

    protected override void OnInit()
    {
        base.OnInit();
        this.BoxEnable(false);
        this.mCondition1 = AIMoveBase.GetConditionTime(this.DelayTimeEnable);
        this.mCondition2 = AIMoveBase.GetConditionTime(this.DelayTimeEnable + 100);
    }

    protected override void OnUpdate()
    {
        if ((this.mCondition1 != null) && this.mCondition1.IsEnd())
        {
            this.BoxEnable(true);
            this.mCondition1 = null;
        }
        if ((this.mCondition2 != null) && this.mCondition2.IsEnd())
        {
            this.BoxEnable(false);
            this.mCondition2 = null;
        }
    }
}

