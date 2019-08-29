using System;

public class AI3027 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if (base.m_Entity.IsElite)
        {
            base.AddAction(new AIMove1022(base.m_Entity, 4.5f));
            base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 300, 300));
        }
        else
        {
            base.AddAction(new AIMove1022(base.m_Entity, 3.3f));
            base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 0x3e8, 0x3e8));
        }
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        if (base.m_Entity.IsElite)
        {
            base.m_Entity.m_EntityData.ExcuteAttributes("MoveSpeed%", 0xdacL);
        }
    }
}

