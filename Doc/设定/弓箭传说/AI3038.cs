using System;

public class AI3038 : AIBase
{
    private float moveMaxDis = 4f;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1033(base.m_Entity, this.moveMaxDis, 1, true));
        base.AddAction(base.GetActionWait(string.Empty, 400));
        base.AddAction(base.GetActionAttack(string.Empty, base.m_Entity.m_Data.WeaponID, true));
        base.AddAction(base.GetActionWait(string.Empty, 100));
    }
}

