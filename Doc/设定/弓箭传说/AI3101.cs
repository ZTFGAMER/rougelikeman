using System;

public class AI3101 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if (base.m_Entity.IsElite)
        {
            base.AddAction(new AIMove1022(base.m_Entity, 4.2f));
            base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 400, 400));
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
            base.m_Entity.m_EntityData.ExcuteAttributes("MoveSpeed%", 0x1b58L);
        }
    }
}

