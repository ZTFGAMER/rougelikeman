namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Collections;
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    public abstract class Asn1Set : Asn1Object, IEnumerable
    {
        private readonly IList _set;

        protected internal Asn1Set(int capacity)
        {
            this._set = Platform.CreateArrayList(capacity);
        }

        protected internal void AddObject(Asn1Encodable obj)
        {
            this._set.Add(obj);
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            Asn1Set set = asn1Object as Asn1Set;
            if (set == null)
            {
                return false;
            }
            if (this.Count != set.Count)
            {
                return false;
            }
            IEnumerator e = this.GetEnumerator();
            IEnumerator enumerator = set.GetEnumerator();
            while (e.MoveNext() && enumerator.MoveNext())
            {
                Asn1Object obj2 = this.GetCurrent(e).ToAsn1Object();
                Asn1Object obj3 = this.GetCurrent(enumerator).ToAsn1Object();
                if (!obj2.Equals(obj3))
                {
                    return false;
                }
            }
            return true;
        }

        protected override int Asn1GetHashCode()
        {
            int count = this.Count;
            IEnumerator enumerator = this.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    count *= 0x11;
                    if (current == null)
                    {
                        count ^= DerNull.Instance.GetHashCode();
                    }
                    else
                    {
                        count ^= current.GetHashCode();
                    }
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
            return count;
        }

        private Asn1Encodable GetCurrent(IEnumerator e)
        {
            Asn1Encodable current = (Asn1Encodable) e.Current;
            if (current == null)
            {
                return DerNull.Instance;
            }
            return current;
        }

        public virtual IEnumerator GetEnumerator() => 
            this._set.GetEnumerator();

        public static Asn1Set GetInstance(object obj)
        {
            switch (obj)
            {
                case (null):
                case (Asn1Set _):
                    return (Asn1Set) obj;
                    break;
            }
            if (obj is Asn1SetParser)
            {
                return GetInstance(((Asn1SetParser) obj).ToAsn1Object());
            }
            if (obj is byte[])
            {
                try
                {
                    return GetInstance(Asn1Object.FromByteArray((byte[]) obj));
                }
                catch (IOException exception)
                {
                    throw new ArgumentException("failed to construct set from byte[]: " + exception.Message);
                }
            }
            if (obj is Asn1Encodable)
            {
                Asn1Object obj2 = ((Asn1Encodable) obj).ToAsn1Object();
                if (obj2 is Asn1Set)
                {
                    return (Asn1Set) obj2;
                }
            }
            throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
        }

        public static Asn1Set GetInstance(Asn1TaggedObject obj, bool explicitly)
        {
            Asn1Object obj2 = obj.GetObject();
            if (explicitly)
            {
                if (!obj.IsExplicit())
                {
                    throw new ArgumentException("object implicit - explicit expected.");
                }
                return (Asn1Set) obj2;
            }
            if (obj.IsExplicit())
            {
                return new DerSet(obj2);
            }
            if (obj2 is Asn1Set)
            {
                return (Asn1Set) obj2;
            }
            if (!(obj2 is Asn1Sequence))
            {
                throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
            }
            Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            IEnumerator enumerator = ((Asn1Sequence) obj2).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Asn1Encodable current = (Asn1Encodable) enumerator.Current;
                    Asn1Encodable[] objs = new Asn1Encodable[] { current };
                    v.Add(objs);
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
            return new DerSet(v, false);
        }

        [Obsolete("Use 'object[index]' syntax instead")]
        public Asn1Encodable GetObjectAt(int index) => 
            this[index];

        [Obsolete("Use GetEnumerator() instead")]
        public IEnumerator GetObjects() => 
            this.GetEnumerator();

        protected internal void Sort()
        {
            if (this._set.Count >= 2)
            {
                Asn1Encodable[] items = new Asn1Encodable[this._set.Count];
                byte[][] keys = new byte[this._set.Count][];
                for (int i = 0; i < this._set.Count; i++)
                {
                    Asn1Encodable encodable = (Asn1Encodable) this._set[i];
                    items[i] = encodable;
                    keys[i] = encodable.GetEncoded("DER");
                }
                Array.Sort(keys, items, new DerComparer());
                for (int j = 0; j < this._set.Count; j++)
                {
                    this._set[j] = items[j];
                }
            }
        }

        public virtual Asn1Encodable[] ToArray()
        {
            Asn1Encodable[] encodableArray = new Asn1Encodable[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                encodableArray[i] = this[i];
            }
            return encodableArray;
        }

        public override string ToString() => 
            CollectionUtilities.ToString(this._set);

        public virtual Asn1Encodable this[int index] =>
            ((Asn1Encodable) this._set[index]);

        [Obsolete("Use 'Count' property instead")]
        public int Size =>
            this.Count;

        public virtual int Count =>
            this._set.Count;

        public Asn1SetParser Parser =>
            new Asn1SetParserImpl(this);

        private class Asn1SetParserImpl : Asn1SetParser, IAsn1Convertible
        {
            private readonly Asn1Set outer;
            private readonly int max;
            private int index;

            public Asn1SetParserImpl(Asn1Set outer)
            {
                this.outer = outer;
                this.max = outer.Count;
            }

            public IAsn1Convertible ReadObject()
            {
                if (this.index == this.max)
                {
                    return null;
                }
                Asn1Encodable encodable = this.outer[this.index++];
                if (encodable is Asn1Sequence)
                {
                    return ((Asn1Sequence) encodable).Parser;
                }
                if (encodable is Asn1Set)
                {
                    return ((Asn1Set) encodable).Parser;
                }
                return encodable;
            }

            public virtual Asn1Object ToAsn1Object() => 
                this.outer;
        }

        private class DerComparer : IComparer
        {
            private bool AllZeroesFrom(byte[] bs, int pos)
            {
                while (pos < bs.Length)
                {
                    if (bs[pos++] != 0)
                    {
                        return false;
                    }
                }
                return true;
            }

            public int Compare(object x, object y)
            {
                byte[] bs = (byte[]) x;
                byte[] buffer2 = (byte[]) y;
                int pos = Math.Min(bs.Length, buffer2.Length);
                for (int i = 0; i != pos; i++)
                {
                    byte num3 = bs[i];
                    byte num4 = buffer2[i];
                    if (num3 != num4)
                    {
                        return ((num3 >= num4) ? 1 : -1);
                    }
                }
                if (bs.Length > buffer2.Length)
                {
                    return (!this.AllZeroesFrom(bs, pos) ? 1 : 0);
                }
                if (bs.Length < buffer2.Length)
                {
                    return (!this.AllZeroesFrom(buffer2, pos) ? -1 : 0);
                }
                return 0;
            }
        }
    }
}

