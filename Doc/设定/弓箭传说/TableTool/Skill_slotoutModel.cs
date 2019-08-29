namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;

    public class Skill_slotoutModel : LocalModel<Skill_slotout, int>
    {
        private const string _Filename = "Skill_slotout";

        public List<string> GetAttributes(LocalSave.CardOne one)
        {
            List<string> list = new List<string>();
            Skill_slotout beanById = base.GetBeanById(one.CardID);
            if (beanById != null)
            {
                if (beanById.BaseAttributes.Length != beanById.AddAttributes.Length)
                {
                    object[] args = new object[] { one.CardID };
                    SdkManager.Bugly_Report("Skill_slotoutModel_Extra", Utils.FormatString("GetAttributes[{0}] attributes is error.", args));
                    return list;
                }
                int index = 0;
                int length = beanById.BaseAttributes.Length;
                while (index < length)
                {
                    string str2;
                    string str = beanById.BaseAttributes[index];
                    float num = beanById.AddAttributes[index];
                    Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
                    if (goodData.percent)
                    {
                        num = goodData.value + (((one.level - 1) * num) * 100f);
                        num /= 100f;
                        str2 = num.ToString();
                    }
                    else
                    {
                        str2 = (goodData.value + ((one.level - 1) * ((long) num))).ToString();
                    }
                    object[] args = new object[] { goodData.goodType, goodData.GetSymbolString(), str2 };
                    str = Utils.FormatString("{0} {1} {2}", args);
                    list.Add(str);
                    index++;
                }
            }
            return list;
        }

        protected override int GetBeanKey(Skill_slotout bean) => 
            bean.GroupID;

        protected override string Filename =>
            "Skill_slotout";
    }
}

