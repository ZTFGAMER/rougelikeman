using System;

public class SkillAlone1031 : SkillAloneBase
{
    private float speedratio;
    private float range;

    protected override void OnInstall()
    {
        this.speedratio = float.Parse(base.m_SkillData.Args[0]);
        this.range = float.Parse(base.m_SkillData.Args[1]);
        base.m_Entity.m_EntityData.Modify_BulletSpeedRatio(this.speedratio, this.range);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_BulletSpeedRatio(1f, 0f);
    }
}

