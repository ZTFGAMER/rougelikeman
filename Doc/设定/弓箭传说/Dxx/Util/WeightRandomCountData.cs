namespace Dxx.Util
{
    using System;

    public class WeightRandomCountData : WeightRandomDataBase
    {
        public int randomcount;
        public int lastrandomindex;

        public WeightRandomCountData(int id) : base(id)
        {
        }

        public bool GetCanRandom(int randomindex, int maxcount)
        {
            if (this.lastrandomindex == randomindex)
            {
                if (this.randomcount >= maxcount)
                {
                    return false;
                }
            }
            else
            {
                this.randomcount = 0;
            }
            return true;
        }

        public void RandomSelf(int randomindex)
        {
            this.randomcount++;
            this.lastrandomindex = randomindex;
        }
    }
}

