namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public class BerTaggedObject : DerTaggedObject
    {
        public BerTaggedObject(int tagNo) : base(false, tagNo, BerSequence.Empty)
        {
        }

        public BerTaggedObject(int tagNo, Asn1Encodable obj) : base(tagNo, obj)
        {
        }

        public BerTaggedObject(bool explicitly, int tagNo, Asn1Encodable obj) : base(explicitly, tagNo, obj)
        {
        }

        internal override void Encode(DerOutputStream derOut)
        {
            if ((derOut is Asn1OutputStream) || (derOut is BerOutputStream))
            {
                derOut.WriteTag(160, base.tagNo);
                derOut.WriteByte(0x80);
                if (!base.IsEmpty())
                {
                    if (!base.explicitly)
                    {
                        IEnumerable enumerable;
                        if (base.obj is Asn1OctetString)
                        {
                            if (base.obj is BerOctetString)
                            {
                                enumerable = (BerOctetString) base.obj;
                            }
                            else
                            {
                                Asn1OctetString str = (Asn1OctetString) base.obj;
                                enumerable = new BerOctetString(str.GetOctets());
                            }
                        }
                        else if (base.obj is Asn1Sequence)
                        {
                            enumerable = (Asn1Sequence) base.obj;
                        }
                        else
                        {
                            if (!(base.obj is Asn1Set))
                            {
                                throw Platform.CreateNotImplementedException(Platform.GetTypeName(base.obj));
                            }
                            enumerable = (Asn1Set) base.obj;
                        }
                        IEnumerator enumerator = enumerable.GetEnumerator();
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
                    }
                    else
                    {
                        derOut.WriteObject(base.obj);
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
    }
}

