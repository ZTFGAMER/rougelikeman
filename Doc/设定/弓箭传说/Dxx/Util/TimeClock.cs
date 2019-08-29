namespace Dxx.Util
{
    using System;
    using System.Collections.Generic;

    public class TimeClock
    {
        private static long mClockIndex = 0L;
        private static Dictionary<long, TimeClockOne> mList = new Dictionary<long, TimeClockOne>();

        public static void Clear()
        {
            int num = 0;
            int count = mList.Count;
            while (num < count)
            {
                mList[(long) num].DeInit();
                num++;
            }
            mList.Clear();
            mClockIndex = 0L;
        }

        public static long Register(string name, float delta, Action action)
        {
            TimeClockOne one = new TimeClockOne(name, delta, action);
            mClockIndex += 1L;
            mList.Add(mClockIndex, one);
            return mClockIndex;
        }

        public static void Unregister(long index)
        {
            if (mList.TryGetValue(index, out TimeClockOne one))
            {
                one.DeInit();
                mList.Remove(index);
            }
        }

        public class TimeClockOne
        {
            private float time;
            private float delaytime;
            private Action action;
            private string name;

            public TimeClockOne(string name, float delay, Action action)
            {
                this.name = name;
                this.delaytime = delay;
                this.action = action;
                object[] args = new object[] { name };
                Updater.AddUpdate(Utils.FormatString("TimeClockOne.{0}", args), new Action<float>(this.OnUpdate), false);
            }

            public void DeInit()
            {
                object[] args = new object[] { this.name };
                Updater.RemoveUpdate(Utils.FormatString("TimeClockOne.{0}", args), new Action<float>(this.OnUpdate));
            }

            private void OnUpdate(float delta)
            {
                this.time += delta;
                if (this.time >= this.delaytime)
                {
                    this.time -= this.delaytime;
                    this.action();
                }
            }
        }
    }
}

