using System;

public class SkillAlone1016GoodCtrl04 : SkillAlone1016GoodCtrl01
{
    protected override void TriggerEnter(EntityBase entity)
    {
        float num = base.args[0];
        int buffid = (int) base.args[1];
        float[] args = new float[] { num };
        GameLogic.SendBuff(entity, base.m_Entity, buffid, args);
        long num3 = (long) (base.m_Entity.m_EntityData.GetAttackBase() * num);
        GameLogic.SendHit_Skill(entity, -num3);
    }
}

