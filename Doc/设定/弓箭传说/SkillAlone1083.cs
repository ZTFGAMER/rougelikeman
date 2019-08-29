using Dxx.Util;
using System;

public class SkillAlone1083 : SkillAloneBase
{
    private float ratio;
    private float hppercent;
    private EntityBase mParent;
    private bool bInit;

    private void OnAttack()
    {
        if (((this.mParent != null) && !this.mParent.GetIsDead()) && (GameLogic.Random((float) 0f, (float) 100f) < this.ratio))
        {
            object[] args = new object[] { "HPRecoverFixed%", this.hppercent };
            this.mParent.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + {1}", args));
        }
    }

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length != 2)
        {
            object[] args = new object[] { base.m_SkillData.SkillID, base.m_SkillData.Args.Length };
            SdkManager.Bugly_Report("SkillAlone1083", Utils.FormatString("SkillID:{0} args.length:{1} != 2", args));
        }
        else if (!float.TryParse(base.m_SkillData.Args[0], out this.ratio))
        {
            object[] args = new object[] { base.m_SkillData.SkillID };
            SdkManager.Bugly_Report("SkillAlone1083", Utils.FormatString("SkillID:{0} args[0] is not a float type.", args));
        }
        else if (!float.TryParse(base.m_SkillData.Args[1], out this.hppercent))
        {
            object[] args = new object[] { base.m_SkillData.SkillID };
            SdkManager.Bugly_Report("SkillAlone1083", Utils.FormatString("SkillID:{0} args[1] is not a float type.", args));
        }
        else
        {
            EntityBabyBase entity = base.m_Entity as EntityBabyBase;
            if ((entity == null) || (entity.GetParent() == null))
            {
                object[] args = new object[] { base.m_Entity.m_Data.CharID };
                SdkManager.Bugly_Report("SkillAlone1083", Utils.FormatString("entity : {0} is not a baby.", args));
            }
            else
            {
                this.mParent = entity.GetParent();
                base.m_Entity.Event_OnAttack = (Action) Delegate.Combine(base.m_Entity.Event_OnAttack, new Action(this.OnAttack));
                this.bInit = true;
            }
        }
    }

    protected override void OnUninstall()
    {
        if (this.bInit)
        {
            base.m_Entity.Event_OnAttack = (Action) Delegate.Remove(base.m_Entity.Event_OnAttack, new Action(this.OnAttack));
        }
    }
}

