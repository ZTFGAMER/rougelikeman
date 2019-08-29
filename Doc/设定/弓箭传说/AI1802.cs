using System;

public class AI1802 : AIBase
{
    private EntityCallBase callentity;

    protected override void OnInit()
    {
        this.callentity = base.m_Entity as EntityCallBase;
        base.AddAction(new AIMove1003(this.callentity));
        base.AddAction(new AIMove1005(this.callentity));
    }
}

