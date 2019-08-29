using System;

public class SkillAlone1050 : SkillAloneShieldValueBase
{
    protected override void OnInstall()
    {
        base.OnInstall();
        base.m_Entity.OnMissAngel = (Action) Delegate.Combine(base.m_Entity.OnMissAngel, new Action(this.OnMissAngel));
    }

    private void OnMissAngel()
    {
        base.ResetShieldHitValue();
    }

    protected override void OnUninstall()
    {
        base.OnUninstall();
        base.m_Entity.OnMissAngel = (Action) Delegate.Remove(base.m_Entity.OnMissAngel, new Action(this.OnMissAngel));
    }
}

