namespace U2
{
    using System;

    public class NameGenerator
    {
        protected string _szPrefix;
        protected ulong _ulNext;
        protected object _Lock = new object();

        public NameGenerator(string prefix)
        {
            this._szPrefix = prefix;
            this._ulNext = 1L;
        }

        public string Generate()
        {
            object obj2 = this._Lock;
            lock (obj2)
            {
                ulong num;
                this._ulNext = (num = this._ulNext) + ((ulong) 1L);
                return (this._szPrefix + num);
            }
        }

        public ulong GetNext()
        {
            object obj2 = this._Lock;
            lock (obj2)
            {
                return this._ulNext;
            }
        }

        public void Reset()
        {
            object obj2 = this._Lock;
            lock (obj2)
            {
                this._ulNext = 0L;
            }
        }

        public void SetNext(ulong val)
        {
            object obj2 = this._Lock;
            lock (obj2)
            {
                this._ulNext = val;
            }
        }
    }
}

