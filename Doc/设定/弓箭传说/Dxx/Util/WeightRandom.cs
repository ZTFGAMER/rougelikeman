namespace Dxx.Util
{
    using System;
    using System.Collections.Generic;

    public class WeightRandom
    {
        private List<WeightRandomDataBase> list = new List<WeightRandomDataBase>();
        private int allweight;
        private int ran;

        public void Add(int id, int weight)
        {
            WeightRandomDataBase item = new WeightRandomDataBase(id) {
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
                WeightRandomDataBase base2 = this.list[num];
                if (this.ran < base2.weight)
                {
                    return base2.id;
                }
                this.ran -= base2.weight;
                num++;
            }
            throw new Exception(Utils.FormatString("WeightRandom.GetRandom Weight Random Error!!!", Array.Empty<object>()));
        }

        public override string ToString()
        {
            string str = string.Empty;
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                object[] args = new object[] { this.list[num].id, this.list[num].weight };
                str = str + Utils.FormatString("{0}:{1} ", args);
                num++;
            }
            return str;
        }
    }
}

