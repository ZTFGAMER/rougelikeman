namespace Org.BouncyCastle.Utilities.Collections
{
    using System;
    using System.Collections;

    public interface ISet : ICollection, IEnumerable
    {
        void Add(object o);
        void AddAll(IEnumerable e);
        void Clear();
        bool Contains(object o);
        void Remove(object o);
        void RemoveAll(IEnumerable e);

        bool IsEmpty { get; }

        bool IsFixedSize { get; }

        bool IsReadOnly { get; }
    }
}

