namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.Collections;

    public class BerSet : DerSet
    {
        public static readonly BerSet Empty = new BerSet();

        public BerSet()
        {
        }

        public BerSet(Asn1Encodable obj) : base(obj)
        {
        }

        public BerSet(Asn1EncodableVector v) : base(v, false)
        {
        }

        internal BerSet(Asn1EncodableVector v, bool needsSorting) : base(v, needsSorting)
        {
        }

        internal override void Encode(DerOutputStream derOut)
        {
            if ((derOut is Asn1OutputStream) || (derOut is BerOutputStream))
            {
                derOut.WriteByte(0x31);
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

        public static BerSet FromVector(Asn1EncodableVector v) => 
            ((v.Count >= 1) ? new BerSet(v) : Empty);

        internal static BerSet FromVector(Asn1EncodableVector v, bool needsSorting) => 
            ((v.Count >= 1) ? new BerSet(v, needsSorting) : Empty);
    }
}

