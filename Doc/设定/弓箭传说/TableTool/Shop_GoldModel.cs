namespace TableTool
{
    using System;
    using System.Collections.Generic;

    public class Shop_GoldModel : LocalModel<Shop_Gold, int>
    {
        private const string _Filename = "Shop_Gold";

        protected override int GetBeanKey(Shop_Gold bean) => 
            bean.ID;

        public int GetDiamond2Gold()
        {
            int maxLevel = LocalSave.Instance.mStage.MaxLevel;
            IList<Shop_Gold> allBeans = base.GetAllBeans();
            return this.GetGold(allBeans, maxLevel, 0, allBeans.Count - 1);
        }

        private int GetGold(IList<Shop_Gold> list, int level, int start, int end)
        {
            if (start == end)
            {
                return list[start].Price;
            }
            if ((end - start) == 1)
            {
                if (level < list[end].Level)
                {
                    return list[start].Price;
                }
                return list[end].Price;
            }
            int num = ((end - start) / 2) + start;
            if (level < list[num].Level)
            {
                return this.GetGold(list, level, start, num - 1);
            }
            return this.GetGold(list, level, num, end);
        }

        protected override string Filename =>
            "Shop_Gold";
    }
}

