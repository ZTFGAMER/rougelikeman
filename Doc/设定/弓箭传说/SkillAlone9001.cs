using System;

public class SkillAlone9001 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.AddElement(EElementType.eFire);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.RemoveElement(EElementType.eFire);
    }
}

