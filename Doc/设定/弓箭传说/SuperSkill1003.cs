using System;
using System.Collections.Generic;

public class SuperSkill1003 : SuperSkillBase
{
    public void AllEntitiesAddBuff(int buffid)
    {
        List<EntityBase> entities = GameLogic.Release.Entity.GetEntities();
        int num = 0;
        int count = entities.Count;
        while (num < count)
        {
            EntityBase other = entities[num];
            if (((other != null) && other.gameObject.activeInHierarchy) && (!other.GetIsDead() && !GameLogic.IsSameTeam(base.m_Entity, other)))
            {
                GameLogic.SendBuff(other, base.m_Entity, buffid, Array.Empty<float>());
                other.PlayEffect(0x10cccb);
            }
            num++;
        }
    }

    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
    }

    protected override void OnUseSkill()
    {
        this.AllEntitiesAddBuff(0x400);
        GameNode.CameraShake(CameraShakeType.FirstDrop);
    }
}

