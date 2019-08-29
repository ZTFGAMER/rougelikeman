using System;

public class SkillAlone1021 : SkillAloneBase
{
    private float ratio = 1f;

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length == 1)
        {
            this.ratio = float.Parse(base.m_SkillData.Args[0]);
        }
        base.m_Entity.m_EntityData.Modify_ThroughEnemy(1, this.ratio);
    }

    protected override void OnInstall(object[] args)
    {
        if (args.Length == 1)
        {
            this.ratio = (float) args[0];
        }
        base.m_Entity.m_EntityData.Modify_ThroughEnemy(1, this.ratio);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_ThroughEnemy(-1, -this.ratio);
    }
}

