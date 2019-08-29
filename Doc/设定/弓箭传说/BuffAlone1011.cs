using System;
using System.Collections.Generic;

public class BuffAlone1011 : BuffAloneBase
{
    private float range;

    protected override void ExcuteBuff(BuffAloneBase.BuffData data)
    {
        List<EntityBase> list = GameLogic.Release.Entity.GetRoundSelfEntities(base.m_Entity, this.range, false);
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            EntityBase target = list[num];
            GameLogic.SendBuff(target, base.m_Entity, 0x3f4, Array.Empty<float>());
            num++;
        }
    }

    protected override void OnRemove()
    {
    }

    protected override void OnStart()
    {
        this.range = base.buff_data.Args[0];
    }
}

