namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;

    public class Stage_Level_activitylevelModel : LocalModel<Stage_Level_activitylevel, string>
    {
        private const string _Filename = "Stage_Level_activitylevel";
        private Dictionary<int, ActivityData> mList = new Dictionary<int, ActivityData>();

        protected override string GetBeanKey(Stage_Level_activitylevel bean) => 
            bean.RoomID;

        public int GetMaxLayer() => 
            this.GetMaxLayer(GameLogic.Hold.BattleData.ActiveID);

        public int GetMaxLayer(int activityid)
        {
            this.InitActivityData(activityid);
            return this.mList[activityid].maxLayer;
        }

        public void Init()
        {
        }

        private void InitActivityData(int activityid)
        {
            if (!this.mList.ContainsKey(activityid))
            {
                ActivityData data = new ActivityData(activityid);
                this.mList.Add(activityid, data);
            }
        }

        protected override string Filename =>
            "Stage_Level_activitylevel";

        public class ActivityData
        {
            public int activityid;
            public int maxLayer;
            private string stagelevel;

            public ActivityData(int activityid)
            {
                this.stagelevel = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(activityid).StageLevel;
                this.activityid = activityid;
                this.maxLayer = 1;
                for (int i = 0; i < 0x3e7; i++)
                {
                    if (LocalModelManager.Instance.Stage_Level_activitylevel.GetBeanById(this.GetID(this.maxLayer)) == null)
                    {
                        this.maxLayer--;
                        break;
                    }
                    this.maxLayer++;
                }
            }

            private string GetID(int id)
            {
                object[] args = new object[] { this.stagelevel, id };
                return Utils.FormatString("{0}{1:D3}", args);
            }
        }
    }
}

