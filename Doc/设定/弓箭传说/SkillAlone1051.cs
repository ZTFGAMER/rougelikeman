using System;

public class SkillAlone1051 : SkillAloneShieldCountBase
{
    private void OnInBossRoom()
    {
        base.ResetShieldCount();
    }

    protected override void OnInstall()
    {
        base.OnInstall();
        base.m_Entity.OnInBossRoom = (Action) Delegate.Combine(base.m_Entity.OnInBossRoom, new Action(this.OnInBossRoom));
    }

    protected override void OnUninstall()
    {
        base.OnUninstall();
        base.m_Entity.OnInBossRoom = (Action) Delegate.Remove(base.m_Entity.OnInBossRoom, new Action(this.OnInBossRoom));
    }
}

