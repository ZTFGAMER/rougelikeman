using Dxx.Util;
using System;

public class SkillAlone1065 : SkillAloneBase
{
    private float delaytime;
    private float range;
    private int debuffid;
    private TimeRepeat mTime;

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length < 3)
        {
            object[] args = new object[] { base.m_SkillData.Args.Length };
            SdkManager.Bugly_Report("SkillAlone1065.cs", Utils.FormatString("SkillAlone1065 m_SkillData.Args.Length = {0}", args));
        }
        else if (((float.TryParse(base.m_SkillData.Args[0], out this.delaytime) && float.TryParse(base.m_SkillData.Args[1], out this.range)) && int.TryParse(base.m_SkillData.Args[2], out this.debuffid)) && (base.m_Entity != null))
        {
            this.mTime = new TimeRepeat("SkillAlone1065", this.delaytime, new Action(this.OnUpdate), false, 0f);
        }
    }

    protected override void OnUninstall()
    {
        if (this.mTime != null)
        {
            this.mTime.UnRegister();
            this.mTime = null;
        }
    }

    private void OnUpdate()
    {
        EntityBase target = GameLogic.Release.Entity.GetNearEntity(base.m_Entity, this.range, false);
        if (target != null)
        {
            GameLogic.Release.MapEffect.Get("Game/SkillPrefab/SkillAlone1065_Effect").transform.position = target.position;
            GameLogic.SendBuff(target, base.m_Entity, this.debuffid, Array.Empty<float>());
        }
    }
}

