using System;

public class SkillAlone1016GoodCtrl01 : SkillAloneAttrGoodBase
{
    protected override void TriggerEnter(EntityBase entity)
    {
        if (((base.m_Entity != null) && (base.m_Entity.m_EntityData != null)) && (entity != null))
        {
            float num = base.args[0];
            int buffid = (int) base.args[1];
            long num3 = (long) (base.m_Entity.m_EntityData.GetAttack(0) * num);
            GameLogic.SendHit_Skill(entity, -num3);
            GameLogic.SendBuff(entity, base.m_Entity, buffid, Array.Empty<float>());
            entity.PlayHittedSound();
        }
    }
}

