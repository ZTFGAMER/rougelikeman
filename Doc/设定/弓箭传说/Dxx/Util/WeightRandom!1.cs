namespace Dxx.Util
{
    using System;
    using System.Collections.Generic;

    public class WeightRandom<T> where T: WeightRandomDataBase
    {
        private List<T> list;
        private int allweight;
        private int ran;

        public WeightRandom()
        {
            this.list = new List<T>();
        }

        public void Add(T t, int weight)
        {
            t.weight = weight;
            this.list.Add(t);
            this.allweight += weight;
        }

        public int GetAllWeight() => 
            this.allweight;

        public T GetRandom()
        {
            this.ran = GameLogic.Random(0, this.allweight);
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                T local = this.list[num];
                if (this.ran < local.weight)
                {
                    return local;
                }
                this.ran -= local.weight;
                num++;
            }
            object[] args = new object[] { base.GetType().ToString() };
            throw new Exception(Utils.FormatString("WeightRandom<{0}>.GetRandom Weight Random Error!!!", args));
        }
    }
}

