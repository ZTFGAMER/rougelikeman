namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;

    public class Box_ChapterBoxModel : LocalModel<Box_ChapterBox, int>
    {
        private const string _Filename = "Box_ChapterBox";
        private int mMaxID;

        protected override int GetBeanKey(Box_ChapterBox bean) => 
            bean.ID;

        public List<Box_ChapterBox> GetCurrentList()
        {
            List<Box_ChapterBox> list = new List<Box_ChapterBox>();
            for (int i = 0; i <= this.mMaxID; i++)
            {
                Box_ChapterBox beanById = base.GetBeanById(i);
                if (beanById != null)
                {
                    list.Add(beanById);
                }
            }
            return list;
        }

        public List<Drop_DropModel.DropData> GetDrops(int id)
        {
            List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
            Box_ChapterBox beanById = base.GetBeanById(id);
            if (beanById == null)
            {
                object[] args = new object[] { id };
                SdkManager.Bugly_Report("Box_ChapterBoxModel_Extra", Utils.FormatString("GetDrops id[{0}] is invalid.", args));
                return list;
            }
            return Drop_DropModel.GetDropDatas(beanById.Reward);
        }

        public Box_ChapterBox GetNext(int id) => 
            base.GetBeanById(id + 1);

        public int GetNextLevel(int id)
        {
            Box_ChapterBox beanById = base.GetBeanById(id);
            if (beanById != null)
            {
                return beanById.Chapter;
            }
            return 0x7fffffff;
        }

        public int GetOpenCount(int currentlayer, int openedcount)
        {
            this.InitMaxID();
            if (currentlayer >= base.GetBeanById(1).Chapter)
            {
                if (currentlayer >= base.GetBeanById(this.mMaxID).Chapter)
                {
                    return (this.mMaxID - openedcount);
                }
                for (int i = 1; i < (this.mMaxID - 1); i++)
                {
                    if ((currentlayer >= base.GetBeanById(i).Chapter) && (currentlayer < base.GetBeanById(i + 1).Chapter))
                    {
                        return (i - openedcount);
                    }
                }
            }
            return 0;
        }

        private void InitMaxID()
        {
            if (this.mMaxID <= 0)
            {
                int key = 1;
                while (base.GetBeanById(key) != null)
                {
                    key++;
                }
                this.mMaxID = key - 1;
            }
        }

        protected override string Filename =>
            "Box_ChapterBox";
    }
}

