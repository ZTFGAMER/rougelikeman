using System;

public class SkillAlone1030 : SkillAloneBase
{
    protected override void OnInstall()
    {
        int index = 0;
        int length = base.m_SkillData.Args.Length;
        while (index < length)
        {
            base.m_Entity.m_EntityData.AddBabyAttribute(base.m_SkillData.Args[index]);
            index++;
        }
    }

    protected override void OnUninstall()
    {
        int index = 0;
        int length = base.m_SkillData.Args.Length;
        while (index < length)
        {
            base.m_Entity.m_EntityData.RemoveBabyAttribute(base.m_SkillData.Args[index]);
            index++;
        }
    }
}

