using System;

public class BossJumpHit5005 : SkillAloneAttrGoodBase
{
    protected override void TriggerEnter(EntityBase entity)
    {
        int num = -((int) (base.m_Entity.m_EntityData.GetAttack(20) * base.args[0]));
        GameLogic.SendHit_Body(entity, base.m_Entity, (long) num, 0x3e8fa1);
    }
}

