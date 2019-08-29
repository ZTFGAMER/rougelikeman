using Dxx.Util;
using System;

public class SkillMastery101 : SkillMasteryBase
{
    private int wearweapontype;

    protected override void OnInit()
    {
        this.Split();
    }

    private void Split()
    {
        Debugger.Log("武器精通");
        LocalSave.EquipOne one = null;
        if (one != null)
        {
            this.wearweapontype = one.data.Type;
            char[] separator = new char[] { ',' };
            string[] strArray = base.mData.Split(separator);
            if (strArray.Length != 2)
            {
                object[] args = new object[] { base.mData };
                SdkManager.Bugly_Report(base.GetType().ToString(), Utils.FormatString("{0} is invalid.", args));
            }
            else
            {
                int num = int.Parse(strArray[0]);
                string str = strArray[1];
                if (num == this.wearweapontype)
                {
                    Debugger.Log(string.Concat(new object[] { "武器类型相同 ", num, " 增加属性 ", str }));
                    base.m_Entity.m_EntityData.ExcuteAttributes(str);
                }
            }
        }
    }
}

