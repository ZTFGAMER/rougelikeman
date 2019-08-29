using Dxx.Util;
using System;

public class SkillAloneBabyBase : SkillAloneBase
{
    protected EntityBabyBase baby;
    protected int mBabyID;

    protected override void OnInstall()
    {
        if (this.mBabyID == 0)
        {
            object[] args = new object[] { base.ClassID };
            SdkManager.Bugly_Report("SkillAloneBabyBase.cs", Utils.FormatString("OnInstall SkillAlone {0} baby is null", args));
        }
        this.baby = base.CreateBaby(this.mBabyID);
        if (this.baby != null)
        {
            this.baby.SetParent(base.m_Entity);
            this.baby.Init(this.mBabyID);
            base.m_Entity.m_EntityData.AddBaby(this.baby);
            base.m_Entity.AddBabySkillID(base.m_SkillData.SkillID);
        }
    }

    protected override void OnUninstall()
    {
        if ((base.m_Entity != null) && (this.baby != null))
        {
            base.m_Entity.RemoveBabySkillID(base.m_SkillData.SkillID);
            base.m_Entity.m_EntityData.RemoveBaby(this.baby);
            GameLogic.Release.Entity.RemoveBaby(this.baby);
        }
    }
}

