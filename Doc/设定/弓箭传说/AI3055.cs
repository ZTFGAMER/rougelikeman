using System;

public class AI3055 : AIBase
{
    private int waittime;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1022(base.m_Entity, 6f));
        base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, this.waittime, this.waittime));
        base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, this.waittime, this.waittime));
        base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, this.waittime, this.waittime));
    }
}

