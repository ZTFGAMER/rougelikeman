namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;

    public class Beat_beatModel : LocalModel<Beat_beat, int>
    {
        private const string _Filename = "Beat_beat";

        protected override int GetBeanKey(Beat_beat bean) => 
            bean.ID;

        public string GetBeat(int layer)
        {
            IList<Beat_beat> allBeans = base.GetAllBeans();
            int count = base.GetBeanKeyList().Count;
            Beat_beat _beat = this.GetBeatOne((long) layer, 0, count - 1);
            if (_beat == null)
            {
                return (MathDxx.Clamp((float) layer, 0f, 100f) + "%");
            }
            if (_beat.ID == 0)
            {
                return "0.1%";
            }
            Beat_beat beanById = base.GetBeanById(_beat.ID - 1);
            float num2 = ((float) (layer - beanById.Score)) / ((float) (_beat.Score - beanById.Score));
            float num3 = ((_beat.Rate - beanById.Rate) * num2) + beanById.Rate;
            return (Utils.GetFloat2(num3 * 100f) + "%");
        }

        private Beat_beat GetBeatOne(long score, int start, int end)
        {
            int key = ((end - start) / 2) + start;
            Beat_beat beanById = base.GetBeanById(key);
            if (beanById == null)
            {
                return null;
            }
            if (start == end)
            {
                return beanById;
            }
            if (score <= beanById.Score)
            {
                return this.GetBeatOne(score, start, key);
            }
            return this.GetBeatOne(score, key + 1, end);
        }

        protected override string Filename =>
            "Beat_beat";
    }
}

