namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Collections;
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    public abstract class Asn1Sequence : Asn1Object, IEnumerable
    {
        private readonly IList seq;

        protected internal Asn1Sequence(int capacity)
        {
            this.seq = Platform.CreateArrayList(capacity);
        }

        protected internal void AddObject(Asn1Encodable obj)
        {
            this.seq.Add(obj);
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            Asn1Sequence sequence = asn1Object as Asn1Sequence;
            if (sequence == null)
            {
                return false;
            }
            if (this.Count != sequence.Count)
            {
                return false;
            }
            IEnumerator e = this.GetEnumerator();
            IEnumerator enumerator = sequence.GetEnumerator();
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
            this.seq.GetEnumerator();

        public static Asn1Sequence GetInstance(object obj)
        {
            switch (obj)
            {
                case (null):
                case (Asn1Sequence _):
                    return (Asn1Sequence) obj;
                    break;
            }
            if (obj is Asn1SequenceParser)
            {
                return GetInstance(((Asn1SequenceParser) obj).ToAsn1Object());
            }
            if (obj is byte[])
            {
                try
                {
                    return GetInstance(Asn1Object.FromByteArray((byte[]) obj));
                }
                catch (IOException exception)
                {
                    throw new ArgumentException("failed to construct sequence from byte[]: " + exception.Message);
                }
            }
            if (obj is Asn1Encodable)
            {
                Asn1Object obj2 = ((Asn1Encodable) obj).ToAsn1Object();
                if (obj2 is Asn1Sequence)
                {
                    return (Asn1Sequence) obj2;
                }
            }
            throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
        }

        public static Asn1Sequence GetInstance(Asn1TaggedObject obj, bool explicitly)
        {
            Asn1Object obj2 = obj.GetObject();
            if (explicitly)
            {
                if (!obj.IsExplicit())
                {
                    throw new ArgumentException("object implicit - explicit expected.");
                }
                return (Asn1Sequence) obj2;
            }
            if (obj.IsExplicit())
            {
                if (obj is BerTaggedObject)
                {
                    return new BerSequence(obj2);
                }
                return new DerSequence(obj2);
            }
            if (!(obj2 is Asn1Sequence))
            {
                throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
            }
            return (Asn1Sequence) obj2;
        }

        [Obsolete("Use 'object[index]' syntax instead")]
        public Asn1Encodable GetObjectAt(int index) => 
            this[index];

        [Obsolete("Use GetEnumerator() instead")]
        public IEnumerator GetObjects() => 
            this.GetEnumerator();

        public override string ToString() => 
            CollectionUtilities.ToString(this.seq);

        public virtual Asn1SequenceParser Parser =>
            new Asn1SequenceParserImpl(this);

        public virtual Asn1Encodable this[int index] =>
            ((Asn1Encodable) this.seq[index]);

        [Obsolete("Use 'Count' property instead")]
        public int Size =>
            this.Count;

        public virtual int Count =>
            this.seq.Count;

        private class Asn1SequenceParserImpl : Asn1SequenceParser, IAsn1Convertible
        {
            private readonly Asn1Sequence outer;
            private readonly int max;
            private int index;

            public Asn1SequenceParserImpl(Asn1Sequence outer)
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

            public Asn1Object ToAsn1Object() => 
                this.outer;
        }
    }
}

