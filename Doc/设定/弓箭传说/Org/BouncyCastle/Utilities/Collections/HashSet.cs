namespace Org.BouncyCastle.Utilities.Collections
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public class HashSet : ISet, ICollection, IEnumerable
    {
        private readonly IDictionary impl;

        public HashSet()
        {
            this.impl = Platform.CreateHashtable();
        }

        public HashSet(IEnumerable s)
        {
            this.impl = Platform.CreateHashtable();
            IEnumerator enumerator = s.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    this.Add(current);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
        }

        public virtual void Add(object o)
        {
            this.impl[o] = null;
        }

        public virtual void AddAll(IEnumerable e)
        {
            IEnumerator enumerator = e.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    this.Add(current);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
        }

        public virtual void Clear()
        {
            this.impl.Clear();
        }

        public virtual bool Contains(object o) => 
            this.impl.Contains(o);

        public virtual void CopyTo(Array array, int index)
        {
            this.impl.Keys.CopyTo(array, index);
        }

        public virtual IEnumerator GetEnumerator() => 
            this.impl.Keys.GetEnumerator();

        public virtual void Remove(object o)
        {
            this.impl.Remove(o);
        }

        public virtual void RemoveAll(IEnumerable e)
        {
            IEnumerator enumerator = e.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    this.Remove(current);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
        }

        public virtual int Count =>
            this.impl.Count;

        public virtual bool IsEmpty =>
            (this.impl.Count == 0);

        public virtual bool IsFixedSize =>
            this.impl.IsFixedSize;

        public virtual bool IsReadOnly =>
            this.impl.IsReadOnly;

        public virtual bool IsSynchronized =>
            this.impl.IsSynchronized;

        public virtual object SyncRoot =>
            this.impl.SyncRoot;
    }
}

