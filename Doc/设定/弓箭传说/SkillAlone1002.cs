using System;

public class SkillAlone1002 : SkillAloneBase
{
    private int buffid;

    protected override void OnInstall()
    {
        this.buffid = int.Parse(base.m_SkillData.Args[0]);
        base.m_Entity.OnMiss = (Action) Delegate.Combine(base.m_Entity.OnMiss, new Action(this.OnMiss));
    }

    private void OnMiss()
    {
        GameLogic.SendBuff(base.m_Entity, base.m_Entity, this.buffid, Array.Empty<float>());
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnMiss = (Action) Delegate.Remove(base.m_Entity.OnMiss, new Action(this.OnMiss));
    }
}

