namespace PlatformSupport.Collections.Specialized
{
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class ReadOnlyList : IList, ICollection, IEnumerable
    {
        private readonly IList _list;

        internal ReadOnlyList(IList list)
        {
            this._list = list;
        }

        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object value) => 
            this._list.Contains(value);

        public void CopyTo(Array array, int index)
        {
            this._list.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator() => 
            this._list.GetEnumerator();

        public int IndexOf(object value) => 
            this._list.IndexOf(value);

        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public int Count =>
            this._list.Count;

        public bool IsReadOnly =>
            true;

        public bool IsFixedSize =>
            true;

        public bool IsSynchronized =>
            this._list.IsSynchronized;

        public object this[int index]
        {
            get => 
                this._list[index];
            set
            {
                throw new NotSupportedException();
            }
        }

        public object SyncRoot =>
            this._list.SyncRoot;
    }
}

