using Dxx.Util;
using System;
using System.Collections.Generic;

public class SkillAlone1064 : SkillAloneBase
{
    private float range;
    private int debuffid;

    private void DeadAction(EntityBase deadentity)
    {
        List<EntityBase> list = GameLogic.Release.Entity.GetRoundEntities(deadentity, this.range, false);
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            GameLogic.SendBuff(list[num], base.m_Entity, this.debuffid, Array.Empty<float>());
            num++;
        }
    }

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length < 2)
        {
            object[] args = new object[] { base.m_SkillData.Args.Length };
            SdkManager.Bugly_Report("SkillAlone1064.cs", Utils.FormatString("SkillAlone1064 m_SkillData.Args.Length = {0}", args));
        }
        else if ((float.TryParse(base.m_SkillData.Args[0], out this.range) && int.TryParse(base.m_SkillData.Args[1], out this.debuffid)) && (base.m_Entity != null))
        {
            base.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Combine(base.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.DeadAction));
        }
    }

    protected override void OnUninstall()
    {
        if (base.m_Entity != null)
        {
            base.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Remove(base.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.DeadAction));
        }
    }
}

