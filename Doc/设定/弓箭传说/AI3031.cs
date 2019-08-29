using System;

public class AI3031 : AIBase
{
    protected override void OnInit()
    {
        base.AddAction(new AIMove1052(base.m_Entity, 3));
        if (base.m_Entity.IsElite)
        {
            AIBase.ActionChooseRandom action = new AIBase.ActionChooseRandom {
                m_Entity = base.m_Entity
            };
            action.AddAction(10, base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 0x3e8, 0x3e8));
            action.AddAction(10, base.GetActionAttackWait(0x44c, 0x3e8, 0x3e8));
            base.AddAction(action);
        }
        else
        {
            base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 0x5dc, 0x5dc));
        }
    }
}

