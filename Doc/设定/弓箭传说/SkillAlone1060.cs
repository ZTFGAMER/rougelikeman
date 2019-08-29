using System;

public class SkillAlone1060 : SkillAloneBase
{
    private float speed;
    private float time;

    protected override void OnInstall()
    {
        base.m_Entity.BabiesClone();
    }

    protected override void OnUninstall()
    {
    }
}

