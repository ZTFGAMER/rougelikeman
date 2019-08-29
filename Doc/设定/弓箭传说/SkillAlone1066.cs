using Dxx.Util;
using System;

public class SkillAlone1066 : SkillAloneBase
{
    private int debuffid;

    private void OnHitted(EntityBase source, long hit)
    {
        if ((source != null) && !source.GetIsDead())
        {
            GameLogic.SendBuff(source, this.debuffid, Array.Empty<float>());
        }
    }

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length < 1)
        {
            object[] args = new object[] { base.m_SkillData.Args.Length };
            SdkManager.Bugly_Report("SkillAlone1066.cs", Utils.FormatString("SkillAlone1066 m_SkillData.Args.Length = {0}", args));
        }
        else if (int.TryParse(base.m_SkillData.Args[0], out this.debuffid))
        {
            base.m_Entity.OnHitted = (Action<EntityBase, long>) Delegate.Combine(base.m_Entity.OnHitted, new Action<EntityBase, long>(this.OnHitted));
        }
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnHitted = (Action<EntityBase, long>) Delegate.Remove(base.m_Entity.OnHitted, new Action<EntityBase, long>(this.OnHitted));
    }
}

