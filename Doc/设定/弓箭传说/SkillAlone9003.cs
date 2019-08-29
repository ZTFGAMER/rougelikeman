using System;

public class SkillAlone9003 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.AddElement(EElementType.eIce);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.RemoveElement(EElementType.eIce);
    }
}

