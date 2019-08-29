namespace Dxx.Util
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class TimeRegister
    {
        private static Dictionary<int, TimeRepeat> mList = new Dictionary<int, TimeRepeat>();
        private static int mIndex = 0;

        public static int Register(string name, float updatetime, Action callback, bool firstdo = false, float delaytime = 0f)
        {
            mIndex++;
            TimeRepeat repeat = new TimeRepeat(name, updatetime, callback, firstdo, delaytime);
            mList.Add(mIndex, repeat);
            return mIndex;
        }

        public static void UnRegister(int index)
        {
            if (mList.TryGetValue(index, out TimeRepeat repeat))
            {
                repeat.UnRegister();
                mList.Remove(index);
            }
        }
    }
}

