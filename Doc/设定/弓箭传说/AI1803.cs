using System;

public class AI1803 : AIBase
{
    private EntityCallBase callentity;
    private int rerandomcount;

    protected override void OnInit()
    {
        this.callentity = base.m_Entity as EntityCallBase;
        if (this.rerandomcount == 0)
        {
            base.AddAction(base.GetActionWait(string.Empty, 0x3e8));
            base.AddActionDelegate(() => base.m_Entity.SetCollider(true));
        }
        base.AddAction(new AIMove1017(base.m_Entity, 600, 0x3e8));
        base.AddAction(new AIMove1012(this.callentity));
        base.bReRandom = this.rerandomcount == 0;
        this.rerandomcount++;
    }
}

