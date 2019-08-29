namespace TableTool
{
    using System;
    using System.Collections.Generic;

    public class Achieve_AchieveModel : LocalModel<Achieve_Achieve, int>
    {
        private const string _Filename = "Achieve_Achieve";
        public Dictionary<int, List<int>> mList = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> mLocalList = new Dictionary<int, List<int>>();

        protected override int GetBeanKey(Achieve_Achieve bean) => 
            bean.ID;

        public int GetStage(int id)
        {
            Achieve_Achieve beanById = base.GetBeanById(id);
            return beanById?.Stage;
        }

        public List<int> GetStageList(int stageid, bool haveglobal)
        {
            List<int> list = null;
            if (haveglobal && this.mList.TryGetValue(stageid, out list))
            {
                return list;
            }
            if (!haveglobal && this.mLocalList.TryGetValue(stageid, out list))
            {
                return list;
            }
            return new List<int>();
        }

        public void Init()
        {
            IList<Achieve_Achieve> allBeans = base.GetAllBeans();
            int num = 0;
            int count = allBeans.Count;
            while (num < count)
            {
                List<int> list2 = null;
                List<int> list3 = null;
                Achieve_Achieve achieve = allBeans[num];
                if (!this.mList.TryGetValue(achieve.Stage, out list2))
                {
                    list2 = new List<int>();
                    this.mList.Add(achieve.Stage, list2);
                }
                if (!this.mLocalList.TryGetValue(achieve.Stage, out list3))
                {
                    list3 = new List<int>();
                    this.mLocalList.Add(achieve.Stage, list3);
                }
                if (!achieve.IsGlobal)
                {
                    list3.Add(achieve.ID);
                }
                list2.Add(achieve.ID);
                num++;
            }
        }

        protected override string Filename =>
            "Achieve_Achieve";
    }
}

