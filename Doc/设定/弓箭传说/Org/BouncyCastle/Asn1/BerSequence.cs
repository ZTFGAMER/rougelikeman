namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.Collections;

    public class BerSequence : DerSequence
    {
        public static readonly BerSequence Empty = new BerSequence();

        public BerSequence()
        {
        }

        public BerSequence(Asn1Encodable obj) : base(obj)
        {
        }

        public BerSequence(params Asn1Encodable[] v) : base(v)
        {
        }

        public BerSequence(Asn1EncodableVector v) : base(v)
        {
        }

        internal override void Encode(DerOutputStream derOut)
        {
            if ((derOut is Asn1OutputStream) || (derOut is BerOutputStream))
            {
                derOut.WriteByte(0x30);
                derOut.WriteByte(0x80);
                IEnumerator enumerator = this.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Asn1Encodable current = (Asn1Encodable) enumerator.Current;
                        derOut.WriteObject(current);
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
                derOut.WriteByte(0);
                derOut.WriteByte(0);
            }
            else
            {
                base.Encode(derOut);
            }
        }

        public static BerSequence FromVector(Asn1EncodableVector v) => 
            ((v.Count >= 1) ? new BerSequence(v) : Empty);
    }
}

