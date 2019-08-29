using System;

public class AI3105 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1002(base.m_Entity, 500, 0x3e8));
        base.AddAction(base.GetActionAttack(string.Empty, base.m_Entity.m_Data.WeaponID, true));
        base.AddAction(new AIMove1002(base.m_Entity, 500, 0x3e8));
        base.AddAction(base.GetActionWaitRandom(string.Empty, 500, 0x3e8));
    }
}

