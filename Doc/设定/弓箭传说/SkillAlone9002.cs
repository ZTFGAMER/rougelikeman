using System;

public class SkillAlone9002 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.AddElement(EElementType.eThunder);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.RemoveElement(EElementType.eThunder);
    }
}

