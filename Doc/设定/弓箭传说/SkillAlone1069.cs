using Dxx.Util;
using System;
using System.Collections.Generic;

public class SkillAlone1069 : SkillAloneBase
{
    private List<SkillMasteryBase> mMasteries = new List<SkillMasteryBase>();

    protected override void OnInstall()
    {
        int index = 0;
        int length = base.m_SkillData.Args.Length;
        while (index < length)
        {
            string str = base.m_SkillData.Args[index];
            if ((str.Length <= 4) || !int.TryParse(str.Substring(0, 3), out int num))
            {
                object[] args = new object[] { base.m_SkillData.SkillID, index, str };
                SdkManager.Bugly_Report("SkillAlone1069", Utils.FormatString("SkillID:{0} args[{1}]:{2} is invalid.", args));
            }
            else
            {
                object[] args = new object[] { "SkillMastery", num };
                object[] objArray3 = new object[] { "SkillMastery", num };
                (Type.GetType(Utils.GetString(args)).Assembly.CreateInstance(Utils.GetString(objArray3)) as SkillMasteryBase).Init(base.m_Entity, str.Substring(4, str.Length - 4));
            }
            index++;
        }
    }

    protected override void OnUninstall()
    {
    }
}

