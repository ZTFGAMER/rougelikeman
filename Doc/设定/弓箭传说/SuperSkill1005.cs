using Dxx.Util;
using System;

public class SuperSkill1005 : SuperSkillBase
{
    private float percent;

    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
        this.percent = base.m_Data.Args[0];
    }

    protected override void OnUseSkill()
    {
        object[] args = new object[] { "HPRecover%", " + ", this.percent };
        GameLogic.Self.m_EntityData.ExcuteAttributes(Utils.GetString(args));
    }
}

