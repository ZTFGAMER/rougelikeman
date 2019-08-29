using System;

public class SkillAlone1059 : SkillAloneBase
{
    private float speed;
    private float time;

    protected override void OnInstall()
    {
        this.speed = float.Parse(base.m_Data.Args[0]);
        this.time = float.Parse(base.m_Data.Args[1]);
        base.m_Entity.m_EntityData.Modify_BulletSpeedHitted(1, this.speed, this.time);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_BulletSpeedHitted(-1, -this.speed, -this.time);
    }
}

