namespace Dxx.Util
{
    using DG.Tweening;
    using System;
    using System.Collections.Generic;

    public class SequencePool
    {
        private List<Sequence> mList = new List<Sequence>();

        public void Add(Sequence seq)
        {
            this.mList.Add(seq);
        }

        public void Clear()
        {
            int num = 0;
            int count = this.mList.Count;
            while (num < count)
            {
                Sequence sequence = this.mList[num];
                if (sequence != null)
                {
                    TweenExtensions.Kill(sequence, false);
                    sequence = null;
                }
                num++;
            }
            this.mList.Clear();
        }

        public Sequence Get()
        {
            Sequence item = DOTween.Sequence();
            this.mList.Add(item);
            return item;
        }
    }
}

