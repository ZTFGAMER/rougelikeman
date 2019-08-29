using System;

public class SkillAlone3002 : SkillAloneBase
{
    private float percent;
    private long speed;
    private bool bUse;

    private void AddSpeed()
    {
        if (!this.bUse)
        {
            this.bUse = true;
            base.m_Entity.m_EntityData.ExcuteAttributes("AttackSpeed%", this.speed);
        }
    }

    private void OnChangeHP(long currentHP, long maxHP, float percent, long change)
    {
        if (percent <= this.percent)
        {
            this.AddSpeed();
        }
        else
        {
            this.RemoveSpeed();
        }
    }

    protected override void OnInstall()
    {
        base.m_Entity.OnChangeHPAction = (Action<long, long, float, long>) Delegate.Combine(base.m_Entity.OnChangeHPAction, new Action<long, long, float, long>(this.OnChangeHP));
        this.percent = float.Parse(base.m_SkillData.Args[0]);
        this.speed = long.Parse(base.m_SkillData.Args[1]);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnChangeHPAction = (Action<long, long, float, long>) Delegate.Remove(base.m_Entity.OnChangeHPAction, new Action<long, long, float, long>(this.OnChangeHP));
        this.RemoveSpeed();
    }

    private void RemoveSpeed()
    {
        if (this.bUse)
        {
            this.bUse = false;
            base.m_Entity.m_EntityData.ExcuteAttributes("AttackSpeed%", -this.speed);
        }
    }
}

