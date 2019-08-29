using System;

public class SkillAlone1001 : SkillAloneBase
{
    private int skillid;

    protected override void OnInstall()
    {
        int.TryParse(base.m_SkillData.Args[0], out this.skillid);
        if (this.skillid > 0)
        {
            base.m_Entity.m_EntityData.AddBabyLearnSkillId(this.skillid);
        }
    }

    protected override void OnUninstall()
    {
    }
}

