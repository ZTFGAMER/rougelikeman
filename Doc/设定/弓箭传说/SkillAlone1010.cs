using System;

public class SkillAlone1010 : SkillAloneBase
{
    private int value;

    protected override void OnInstall()
    {
        int.TryParse(base.m_Data.Args[0], out this.value);
        base.m_Entity.m_EntityData.attribute.KillVampirePercent.UpdateValuePercent((long) this.value);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.attribute.KillVampirePercent.UpdateValuePercent((long) -this.value);
    }
}

