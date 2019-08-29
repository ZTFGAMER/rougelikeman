namespace Spine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    [Serializable, DebuggerDisplay("Count={Count}")]
    public class ExposedList<T> : IEnumerable<T>, IEnumerable
    {
        public T[] Items;
        public int Count;
        private const int DefaultCapacity = 4;
        private static readonly T[] EmptyArray;
        private int version;

        static ExposedList()
        {
            ExposedList<T>.EmptyArray = new T[0];
        }

        public ExposedList()
        {
            this.Items = ExposedList<T>.EmptyArray;
        }

        public ExposedList(IEnumerable<T> collection)
        {
            this.CheckCollection(collection);
            ICollection<T> is2 = collection as ICollection<T>;
            if (is2 == null)
            {
                this.Items = ExposedList<T>.EmptyArray;
                this.AddEnumerable(collection);
            }
            else
            {
                this.Items = new T[is2.Count];
                this.AddCollection(is2);
            }
        }

        public ExposedList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            this.Items = new T[capacity];
        }

        internal ExposedList(T[] data, int size)
        {
            this.Items = data;
            this.Count = size;
        }

        public void Add(T item)
        {
            if (this.Count == this.Items.Length)
            {
                this.GrowIfNeeded(1);
            }
            this.Items[this.Count++] = item;
            this.version++;
        }

        private void AddCollection(ICollection<T> collection)
        {
            int count = collection.Count;
            if (count != 0)
            {
                this.GrowIfNeeded(count);
                collection.CopyTo(this.Items, this.Count);
                this.Count += count;
            }
        }

        private void AddEnumerable(IEnumerable<T> enumerable)
        {
            foreach (T local in enumerable)
            {
                this.Add(local);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            this.CheckCollection(collection);
            ICollection<T> is2 = collection as ICollection<T>;
            if (is2 != null)
            {
                this.AddCollection(is2);
            }
            else
            {
                this.AddEnumerable(collection);
            }
            this.version++;
        }

        public int BinarySearch(T item) => 
            Array.BinarySearch<T>(this.Items, 0, this.Count, item);

        public int BinarySearch(T item, IComparer<T> comparer) => 
            Array.BinarySearch<T>(this.Items, 0, this.Count, item, comparer);

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            this.CheckRange(index, count);
            return Array.BinarySearch<T>(this.Items, index, count, item, comparer);
        }

        private void CheckCollection(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
        }

        private void CheckIndex(int index)
        {
            if ((index < 0) || (index > this.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        private static void CheckMatch(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
        }

        private void CheckRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if ((index + count) > this.Count)
            {
                throw new ArgumentException("index and count exceed length of list");
            }
        }

        public void Clear(bool clearArray = true)
        {
            if (clearArray)
            {
                Array.Clear(this.Items, 0, this.Items.Length);
            }
            this.Count = 0;
            this.version++;
        }

        public bool Contains(T item) => 
            (Array.IndexOf<T>(this.Items, item, 0, this.Count) != -1);

        public ExposedList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            ExposedList<TOutput> list = new ExposedList<TOutput>(this.Count);
            for (int i = 0; i < this.Count; i++)
            {
                list.Items[i] = converter(this.Items[i]);
            }
            list.Count = this.Count;
            return list;
        }

        public void CopyTo(T[] array)
        {
            Array.Copy(this.Items, 0, array, 0, this.Count);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.Items, 0, array, arrayIndex, this.Count);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            this.CheckRange(index, count);
            Array.Copy(this.Items, index, array, arrayIndex, count);
        }

        public void EnsureCapacity(int min)
        {
            if (this.Items.Length < min)
            {
                int num = (this.Items.Length != 0) ? (this.Items.Length * 2) : 4;
                if (num < min)
                {
                    num = min;
                }
                this.Capacity = num;
            }
        }

        public bool Exists(Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            return (this.GetIndex(0, this.Count, match) != -1);
        }

        public T Find(Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            int index = this.GetIndex(0, this.Count, match);
            return ((index == -1) ? default(T) : this.Items[index]);
        }

        public ExposedList<T> FindAll(Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            return this.FindAllList(match);
        }

        private ExposedList<T> FindAllList(Predicate<T> match)
        {
            ExposedList<T> list = new ExposedList<T>();
            for (int i = 0; i < this.Count; i++)
            {
                if (match(this.Items[i]))
                {
                    list.Add(this.Items[i]);
                }
            }
            return list;
        }

        public int FindIndex(Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            return this.GetIndex(0, this.Count, match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            this.CheckIndex(startIndex);
            return this.GetIndex(startIndex, this.Count - startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            this.CheckRange(startIndex, count);
            return this.GetIndex(startIndex, count, match);
        }

        public T FindLast(Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            int index = this.GetLastIndex(0, this.Count, match);
            return ((index != -1) ? this.Items[index] : default(T));
        }

        public int FindLastIndex(Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            return this.GetLastIndex(0, this.Count, match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            this.CheckIndex(startIndex);
            return this.GetLastIndex(0, startIndex + 1, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            int index = (startIndex - count) + 1;
            this.CheckRange(index, count);
            return this.GetLastIndex(index, count, match);
        }

        public void ForEach(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            for (int i = 0; i < this.Count; i++)
            {
                action(this.Items[i]);
            }
        }

        public Enumerator<T> GetEnumerator() => 
            new Enumerator<T>((ExposedList<T>) this);

        private int GetIndex(int startIndex, int count, Predicate<T> match)
        {
            int num = startIndex + count;
            for (int i = startIndex; i < num; i++)
            {
                if (match(this.Items[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetLastIndex(int startIndex, int count, Predicate<T> match)
        {
            int num = startIndex + count;
            while (num != startIndex)
            {
                if (match(this.Items[--num]))
                {
                    return num;
                }
            }
            return -1;
        }

        public ExposedList<T> GetRange(int index, int count)
        {
            this.CheckRange(index, count);
            T[] destinationArray = new T[count];
            Array.Copy(this.Items, index, destinationArray, 0, count);
            return new ExposedList<T>(destinationArray, count);
        }

        public void GrowIfNeeded(int newCount)
        {
            int num = this.Count + newCount;
            if (num > this.Items.Length)
            {
                this.Capacity = Math.Max(Math.Max(this.Capacity * 2, 4), num);
            }
        }

        public int IndexOf(T item) => 
            Array.IndexOf<T>(this.Items, item, 0, this.Count);

        public int IndexOf(T item, int index)
        {
            this.CheckIndex(index);
            return Array.IndexOf<T>(this.Items, item, index, this.Count - index);
        }

        public int IndexOf(T item, int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if ((index + count) > this.Count)
            {
                throw new ArgumentOutOfRangeException("index and count exceed length of list");
            }
            return Array.IndexOf<T>(this.Items, item, index, count);
        }

        public void Insert(int index, T item)
        {
            this.CheckIndex(index);
            if (this.Count == this.Items.Length)
            {
                this.GrowIfNeeded(1);
            }
            this.Shift(index, 1);
            this.Items[index] = item;
            this.version++;
        }

        private void InsertCollection(int index, ICollection<T> collection)
        {
            int count = collection.Count;
            this.GrowIfNeeded(count);
            this.Shift(index, count);
            collection.CopyTo(this.Items, index);
        }

        private void InsertEnumeration(int index, IEnumerable<T> enumerable)
        {
            foreach (T local in enumerable)
            {
                this.Insert(index++, local);
            }
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            this.CheckCollection(collection);
            this.CheckIndex(index);
            if (collection == this)
            {
                T[] array = new T[this.Count];
                this.CopyTo(array, 0);
                this.GrowIfNeeded(this.Count);
                this.Shift(index, array.Length);
                Array.Copy(array, 0, this.Items, index, array.Length);
            }
            else
            {
                ICollection<T> is2 = collection as ICollection<T>;
                if (is2 != null)
                {
                    this.InsertCollection(index, is2);
                }
                else
                {
                    this.InsertEnumeration(index, collection);
                }
            }
            this.version++;
        }

        public int LastIndexOf(T item) => 
            Array.LastIndexOf<T>(this.Items, item, this.Count - 1, this.Count);

        public int LastIndexOf(T item, int index)
        {
            this.CheckIndex(index);
            return Array.LastIndexOf<T>(this.Items, item, index, index + 1);
        }

        public int LastIndexOf(T item, int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, "index is negative");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "count is negative");
            }
            if (((index - count) + 1) < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "count is too large");
            }
            return Array.LastIndexOf<T>(this.Items, item, index, count);
        }

        public T Pop()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("List is empty. Nothing to pop.");
            }
            int index = this.Count - 1;
            T local = this.Items[index];
            this.Items[index] = default(T);
            this.Count--;
            this.version++;
            return local;
        }

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index != -1)
            {
                this.RemoveAt(index);
            }
            return (index != -1);
        }

        public int RemoveAll(Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            int index = 0;
            int num2 = 0;
            index = 0;
            while (index < this.Count)
            {
                if (match(this.Items[index]))
                {
                    break;
                }
                index++;
            }
            if (index == this.Count)
            {
                return 0;
            }
            this.version++;
            num2 = index + 1;
            while (num2 < this.Count)
            {
                if (!match(this.Items[num2]))
                {
                    this.Items[index++] = this.Items[num2];
                }
                num2++;
            }
            if ((num2 - index) > 0)
            {
                Array.Clear(this.Items, index, num2 - index);
            }
            this.Count = index;
            return (num2 - index);
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            this.Shift(index, -1);
            Array.Clear(this.Items, this.Count, 1);
            this.version++;
        }

        public void RemoveRange(int index, int count)
        {
            this.CheckRange(index, count);
            if (count > 0)
            {
                this.Shift(index, -count);
                Array.Clear(this.Items, this.Count, count);
                this.version++;
            }
        }

        public ExposedList<T> Resize(int newSize)
        {
            int length = this.Items.Length;
            T[] items = this.Items;
            if (newSize > length)
            {
                Array.Resize<T>(ref this.Items, newSize);
            }
            else if (newSize < length)
            {
                for (int i = newSize; i < length; i++)
                {
                    items[i] = default(T);
                }
            }
            this.Count = newSize;
            return (ExposedList<T>) this;
        }

        public void Reverse()
        {
            Array.Reverse(this.Items, 0, this.Count);
            this.version++;
        }

        public void Reverse(int index, int count)
        {
            this.CheckRange(index, count);
            Array.Reverse(this.Items, index, count);
            this.version++;
        }

        private void Shift(int start, int delta)
        {
            if (delta < 0)
            {
                start -= delta;
            }
            if (start < this.Count)
            {
                Array.Copy(this.Items, start, this.Items, start + delta, this.Count - start);
            }
            this.Count += delta;
            if (delta < 0)
            {
                Array.Clear(this.Items, this.Count, -delta);
            }
        }

        public void Sort()
        {
            Array.Sort<T>(this.Items, 0, this.Count, Comparer<T>.Default);
            this.version++;
        }

        public void Sort(IComparer<T> comparer)
        {
            Array.Sort<T>(this.Items, 0, this.Count, comparer);
            this.version++;
        }

        public void Sort(Comparison<T> comparison)
        {
            Array.Sort<T>(this.Items, comparison);
            this.version++;
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            this.CheckRange(index, count);
            Array.Sort<T>(this.Items, index, count, comparer);
            this.version++;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => 
            this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public T[] ToArray()
        {
            T[] destinationArray = new T[this.Count];
            Array.Copy(this.Items, destinationArray, this.Count);
            return destinationArray;
        }

        public void TrimExcess()
        {
            this.Capacity = this.Count;
        }

        public bool TrueForAll(Predicate<T> match)
        {
            ExposedList<T>.CheckMatch(match);
            for (int i = 0; i < this.Count; i++)
            {
                if (!match(this.Items[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public int Capacity
        {
            get => 
                this.Items.Length;
            set
            {
                if (value < this.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                Array.Resize<T>(ref this.Items, value);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private ExposedList<T> l;
            private int next;
            private int ver;
            private T current;
            internal Enumerator(ExposedList<T> l)
            {
                this = (ExposedList<>.Enumerator) new ExposedList<T>.Enumerator();
                this.l = l;
                this.ver = l.version;
            }

            public void Dispose()
            {
                this.l = null;
            }

            private void VerifyState()
            {
                if (this.l == null)
                {
                    throw new ObjectDisposedException(base.GetType().FullName);
                }
                if (this.ver != this.l.version)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
            }

            public bool MoveNext()
            {
                this.VerifyState();
                if (this.next >= 0)
                {
                    if (this.next < this.l.Count)
                    {
                        this.current = this.l.Items[this.next++];
                        return true;
                    }
                    this.next = -1;
                }
                return false;
            }

            public T Current =>
                this.current;
            void IEnumerator.Reset()
            {
                this.VerifyState();
                this.next = 0;
            }

            object IEnumerator.Current
            {
                get
                {
                    this.VerifyState();
                    if (this.next <= 0)
                    {
                        throw new InvalidOperationException();
                    }
                    return this.current;
                }
            }
        }
    }
}

