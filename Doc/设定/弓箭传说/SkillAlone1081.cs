using Dxx.Util;
using System;
using TableTool;

public class SkillAlone1081 : SkillAloneBase
{
    private float hppercent;
    private string addatt;
    private Goods_goods.GoodData data_add;
    private Goods_goods.GoodData data_remove;
    private bool bAdded;
    private bool bInit;

    private void OnChangeHP(long currentHP, long maxHP, float percent, long change)
    {
        if (percent < this.hppercent)
        {
            if (!this.bAdded)
            {
                base.m_Entity.m_EntityData.ExcuteAttributes(this.data_add);
                this.bAdded = true;
            }
        }
        else if (this.bAdded)
        {
            base.m_Entity.m_EntityData.ExcuteAttributes(this.data_remove);
            this.bAdded = false;
        }
    }

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length != 2)
        {
            object[] args = new object[] { base.m_SkillData.SkillID, base.m_SkillData.Args.Length };
            SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args.length:{1} != 2", args));
        }
        else if (!float.TryParse(base.m_SkillData.Args[0], out this.hppercent))
        {
            object[] args = new object[] { base.m_SkillData.SkillID };
            SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args[0] is not a float type.", args));
        }
        else
        {
            this.hppercent /= 100f;
            this.addatt = base.m_SkillData.Args[1];
            try
            {
                this.data_add = Goods_goods.GetGoodData(this.addatt);
                this.data_remove = Goods_goods.GetGoodData(this.addatt);
                this.data_remove.value *= -1L;
                base.m_Entity.OnChangeHPAction = (Action<long, long, float, long>) Delegate.Combine(base.m_Entity.OnChangeHPAction, new Action<long, long, float, long>(this.OnChangeHP));
                this.bInit = true;
            }
            catch
            {
                object[] args = new object[] { base.m_SkillData.SkillID };
                SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args[1] is invalid.", args));
            }
        }
    }

    protected override void OnUninstall()
    {
        if (this.bInit)
        {
            base.m_Entity.OnChangeHPAction = (Action<long, long, float, long>) Delegate.Remove(base.m_Entity.OnChangeHPAction, new Action<long, long, float, long>(this.OnChangeHP));
        }
    }
}

