namespace TableTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Stage_Level_activityModel : LocalModel<Stage_Level_activity, int>
    {
        private const string _Filename = "Stage_Level_activity";
        private Dictionary<int, ActivityTypeData> mList = new Dictionary<int, ActivityTypeData>();
        private List<ActivityTypeData> mList2 = new List<ActivityTypeData>();
        private List<Stage_Level_activity> mChallengeList = new List<Stage_Level_activity>();
        [CompilerGenerated]
        private static Comparison<ActivityTypeData> <>f__am$cache0;

        protected override int GetBeanKey(Stage_Level_activity bean) => 
            bean.ID;

        public List<Stage_Level_activity> GetChallengeList() => 
            this.mChallengeList;

        public List<ActivityTypeData> GetDifficults() => 
            this.mList2;

        public void Init()
        {
            this.InitActive();
            this.InitChallenge();
        }

        private void InitActive()
        {
            IList<Stage_Level_activity> allBeans = base.GetAllBeans();
            for (int i = 0; i < allBeans.Count; i++)
            {
                Stage_Level_activity _activity = allBeans[i];
                if (_activity.ID < 0x7d0)
                {
                    if (!this.mList.ContainsKey(_activity.Type))
                    {
                        ActivityTypeData data = new ActivityTypeData(_activity.Type);
                        data.Add(_activity);
                        this.mList.Add(_activity.Type, data);
                        this.mList2.Add(data);
                    }
                    else
                    {
                        this.mList[_activity.Type].Add(_activity);
                    }
                }
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (ActivityTypeData a, ActivityTypeData b) {
                    if (a.type < b.type)
                    {
                        return -1;
                    }
                    return 1;
                };
            }
            this.mList2.Sort(<>f__am$cache0);
            for (int j = 0; j < this.mList2.Count; j++)
            {
                this.mList2[j].index = j;
            }
        }

        private void InitChallenge()
        {
            int key = 0x835;
            for (Stage_Level_activity _activity = base.GetBeanById(key); _activity != null; _activity = base.GetBeanById(key))
            {
                this.mChallengeList.Add(_activity);
                key++;
            }
        }

        protected override string Filename =>
            "Stage_Level_activity";

        public class ActivityTypeData
        {
            public int index;
            public int type;
            public Dictionary<int, int> list = new Dictionary<int, int>();
            public List<int> mIds = new List<int>();
            [CompilerGenerated]
            private static Comparison<int> <>f__am$cache0;

            public ActivityTypeData(int type)
            {
                this.type = type;
            }

            public void Add(Stage_Level_activity value)
            {
                this.list.Add(value.Difficult - 1, value.ID);
                this.mIds.Add(value.ID);
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate (int a, int b) {
                        if (a < b)
                        {
                            return -1;
                        }
                        return 1;
                    };
                }
                this.mIds.Sort(<>f__am$cache0);
            }

            public int GetCount(int index) => 
                LocalModelManager.Instance.Stage_Level_activity.GetBeanById(this.mIds[index]).Number;

            public Stage_Level_activity GetData(int index) => 
                LocalModelManager.Instance.Stage_Level_activity.GetBeanById(this.mIds[index]);
        }
    }
}

