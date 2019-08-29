namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public class DerSequence : Asn1Sequence
    {
        public static readonly DerSequence Empty = new DerSequence();

        public DerSequence() : base(0)
        {
        }

        public DerSequence(Asn1Encodable obj) : base(1)
        {
            base.AddObject(obj);
        }

        public DerSequence(params Asn1Encodable[] v) : base(v.Length)
        {
            foreach (Asn1Encodable encodable in v)
            {
                base.AddObject(encodable);
            }
        }

        public DerSequence(Asn1EncodableVector v) : base(v.Count)
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
            derOut.WriteEncoded(0x30, bytes);
        }

        public static DerSequence FromVector(Asn1EncodableVector v) => 
            ((v.Count >= 1) ? new DerSequence(v) : Empty);
    }
}

