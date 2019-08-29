using Dxx.Util;
using System;
using System.Collections.Generic;

public class SkillAlone1067 : SkillAloneBase
{
    private float range;
    private int debuffid;
    private TimeRepeat mTime;

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length < 2)
        {
            object[] args = new object[] { base.m_SkillData.Args.Length };
            SdkManager.Bugly_Report("SkillAlone1067.cs", Utils.FormatString("SkillAlone1067 m_SkillData.Args.Length = {0}", args));
        }
        else if ((float.TryParse(base.m_SkillData.Args[0], out this.range) && int.TryParse(base.m_SkillData.Args[1], out this.debuffid)) && (base.m_Entity != null))
        {
            Updater.AddUpdate("SkillAlone1067", new Action<float>(this.OnUpdate), false);
        }
    }

    protected override void OnUninstall()
    {
        Updater.RemoveUpdate("SkillAlone1067", new Action<float>(this.OnUpdate));
    }

    private void OnUpdate(float delta)
    {
        List<EntityBase> list = GameLogic.Release.Entity.GetRoundEntities(base.m_Entity, this.range, false);
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            GameLogic.SendBuff(list[num], base.m_Entity, this.debuffid, Array.Empty<float>());
            num++;
        }
    }
}

