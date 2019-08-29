using System;

public class SkillAlone9004 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.AddElement(EElementType.ePoison);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.RemoveElement(EElementType.ePoison);
    }
}

