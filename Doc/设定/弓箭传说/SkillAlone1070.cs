using Dxx.Util;
using System;

public class SkillAlone1070 : SkillAloneBase
{
    private long clockindex;
    private int bulletid;
    private int createweight;
    private float hitratio;

    private void OnAttack()
    {
    }

    protected override void OnInstall()
    {
        this.bulletid = int.Parse(base.m_SkillData.Args[0]);
        this.createweight = int.Parse(base.m_SkillData.Args[1]);
        this.hitratio = float.Parse(base.m_SkillData.Args[2]);
        base.m_Entity.m_EntityData.AddAttackMeteorite(new AttackCallData(this.bulletid, this.hitratio, this.createweight));
    }

    protected override void OnUninstall()
    {
    }
}

