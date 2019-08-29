using Dxx.Util;
using System;
using System.Collections.Generic;

public class SkillAlone1079 : SkillAloneBase
{
    private long clockindex;
    private int debuffid;

    private void OnAttack()
    {
        List<EntityBase> entities = GameLogic.Release.Entity.GetEntities();
        int num = 0;
        int count = entities.Count;
        while (num < count)
        {
            GameLogic.SendBuff(entities[num], base.m_Entity, this.debuffid, Array.Empty<float>());
            num++;
        }
    }

    protected override void OnInstall()
    {
        this.debuffid = int.Parse(base.m_SkillData.Args[0]);
        this.clockindex = TimeClock.Register("SkillAlone1079", 1f, new Action(this.OnAttack));
    }

    protected override void OnUninstall()
    {
        TimeClock.Unregister(this.clockindex);
    }
}

