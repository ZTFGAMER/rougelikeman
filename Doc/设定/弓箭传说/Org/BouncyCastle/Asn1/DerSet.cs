namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public class DerSet : Asn1Set
    {
        public static readonly DerSet Empty = new DerSet();

        public DerSet() : base(0)
        {
        }

        public DerSet(Asn1Encodable obj) : base(1)
        {
            base.AddObject(obj);
        }

        public DerSet(params Asn1Encodable[] v) : base(v.Length)
        {
            foreach (Asn1Encodable encodable in v)
            {
                base.AddObject(encodable);
            }
            base.Sort();
        }

        public DerSet(Asn1EncodableVector v) : this(v, true)
        {
        }

        internal DerSet(Asn1EncodableVector v, bool needsSorting) : base(v.Count)
        {
            IEnumerator enumerator = v.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Asn1Encodable current = (Asn1Encodable) enumerator.Current;
                    base.AddObject(current);
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
            if (needsSorting)
            {
                base.Sort();
            }
        }

        internal override void Encode(DerOutputStream derOut)
        {
            MemoryStream os = new MemoryStream();
            DerOutputStream s = new DerOutputStream(os);
            IEnumerator enumerator = this.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Asn1Encodable current = (Asn1Encodable) enumerator.Current;
                    s.WriteObject(current);
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
            Platform.Dispose(s);
            byte[] bytes = os.ToArray();
            derOut.WriteEncoded(0x31, bytes);
        }

        public static DerSet FromVector(Asn1EncodableVector v) => 
            ((v.Count >= 1) ? new DerSet(v) : Empty);

        internal static DerSet FromVector(Asn1EncodableVector v, bool needsSorting) => 
            ((v.Count >= 1) ? new DerSet(v, needsSorting) : Empty);
    }
}

