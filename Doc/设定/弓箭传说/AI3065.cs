using System;

public class AI3065 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1002(base.m_Entity, 0x3e8, 0x3e8));
        if (base.m_Entity.IsElite)
        {
            AIBase.ActionChooseRandom action = new AIBase.ActionChooseRandom {
                m_Entity = base.m_Entity
            };
            action.AddAction(10, base.GetActionAttack(string.Empty, base.m_Entity.m_Data.WeaponID, true));
            action.AddAction(10, base.GetActionAttack(string.Empty, 0x44d, true));
            base.AddAction(action);
        }
        else
        {
            base.AddAction(base.GetActionAttack(string.Empty, base.m_Entity.m_Data.WeaponID, true));
        }
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
    }
}

