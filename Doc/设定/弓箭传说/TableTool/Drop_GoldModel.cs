namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;

    public class Drop_GoldModel : LocalModel<Drop_Gold, int>
    {
        private const string _Filename = "Drop_Gold";
        private Dictionary<int, DropWeight> mList = new Dictionary<int, DropWeight>();

        protected override int GetBeanKey(Drop_Gold bean) => 
            bean.ID;

        public List<DropGold> GetDropList(int dropid) => 
            this.GetDropWeight(dropid).GetDrops();

        private DropWeight GetDropWeight(int dropid)
        {
            if (!this.mList.TryGetValue(dropid, out DropWeight weight))
            {
                weight = new DropWeight();
                weight.Init(base.GetBeanById(dropid).GoldDropLevel);
                this.mList.Add(dropid, weight);
            }
            return weight;
        }

        protected override string Filename =>
            "Drop_Gold";

        public class DropGold
        {
            public int Gold;
        }

        private class DropWeight
        {
            private Dictionary<int, Drop_GoldModel.DropWeightOne> mList = new Dictionary<int, Drop_GoldModel.DropWeightOne>();
            private WeightRandom mRandom = new WeightRandom();

            private void Add(int id, Drop_GoldModel.DropWeightOne one)
            {
                this.mList.Add(id, one);
                this.mRandom.Add(id, one.Weight);
            }

            public List<Drop_GoldModel.DropGold> GetDrops()
            {
                List<Drop_GoldModel.DropGold> list = new List<Drop_GoldModel.DropGold>();
                int random = this.mRandom.GetRandom();
                Drop_GoldModel.DropWeightOne one = this.mList[random];
                for (int i = 0; i < one.Count; i++)
                {
                    Drop_GoldModel.DropGold item = new Drop_GoldModel.DropGold {
                        Gold = GameLogic.Random(one.Min, one.Max + 1)
                    };
                    list.Add(item);
                }
                return list;
            }

            public void Init(string[] strs)
            {
                int index = 0;
                int length = strs.Length;
                while (index < length)
                {
                    char[] separator = new char[] { ',' };
                    string[] strArray = strs[index].Split(separator);
                    Drop_GoldModel.DropWeightOne one = new Drop_GoldModel.DropWeightOne {
                        Count = int.Parse(strArray[0]),
                        Weight = int.Parse(strArray[1]),
                        Min = int.Parse(strArray[2]),
                        Max = int.Parse(strArray[3])
                    };
                    this.Add(index, one);
                    index++;
                }
            }
        }

        private class DropWeightOne
        {
            public int Count;
            public int Weight;
            public int Min;
            public int Max;
        }
    }
}

