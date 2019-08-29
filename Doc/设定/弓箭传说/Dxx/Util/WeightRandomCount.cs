namespace Dxx.Util
{
    using System;
    using System.Collections.Generic;

    public class WeightRandomCount
    {
        private List<WeightRandomCountData> list;
        private int allweight;
        private int maxcontinuecount;
        private int ran;
        private int randomindex;

        public WeightRandomCount(int maxcontinuecount)
        {
            this.list = new List<WeightRandomCountData>();
            this.maxcontinuecount = maxcontinuecount;
        }

        public WeightRandomCount(int maxcontinuecount, int maxcount)
        {
            this.list = new List<WeightRandomCountData>();
            this.maxcontinuecount = maxcontinuecount;
            for (int i = 0; i < maxcount; i++)
            {
                this.Add(i, 1);
            }
        }

        public void Add(int id, int weight)
        {
            WeightRandomCountData item = new WeightRandomCountData(id) {
                weight = weight
            };
            this.list.Add(item);
            this.allweight += weight;
        }

        public int GetRandom()
        {
            this.ran = GameLogic.Random(0, this.allweight);
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                WeightRandomCountData data = this.list[num];
                if (this.ran < data.weight)
                {
                    if (data.GetCanRandom(this.randomindex, this.maxcontinuecount))
                    {
                        data.RandomSelf(++this.randomindex);
                        return data.id;
                    }
                    return this.GetRandom();
                }
                this.ran -= data.weight;
                num++;
            }
            throw new Exception(Utils.FormatString("WeightRandom.GetRandom Weight Random Error!!!", Array.Empty<object>()));
        }
    }
}

