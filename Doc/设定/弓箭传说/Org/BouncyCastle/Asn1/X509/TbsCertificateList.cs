namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Collections;
    using System;
    using System.Collections;

    public class TbsCertificateList : Asn1Encodable
    {
        internal Asn1Sequence seq;
        internal DerInteger version;
        internal AlgorithmIdentifier signature;
        internal X509Name issuer;
        internal Time thisUpdate;
        internal Time nextUpdate;
        internal Asn1Sequence revokedCertificates;
        internal X509Extensions crlExtensions;

        internal TbsCertificateList(Asn1Sequence seq)
        {
            if ((seq.Count < 3) || (seq.Count > 7))
            {
                throw new ArgumentException("Bad sequence size: " + seq.Count);
            }
            int num = 0;
            this.seq = seq;
            if (seq[num] is DerInteger)
            {
                this.version = DerInteger.GetInstance(seq[num++]);
            }
            else
            {
                this.version = new DerInteger(0);
            }
            this.signature = AlgorithmIdentifier.GetInstance(seq[num++]);
            this.issuer = X509Name.GetInstance(seq[num++]);
            this.thisUpdate = Time.GetInstance(seq[num++]);
            if ((num < seq.Count) && (((seq[num] is DerUtcTime) || (seq[num] is DerGeneralizedTime)) || (seq[num] is Time)))
            {
                this.nextUpdate = Time.GetInstance(seq[num++]);
            }
            if ((num < seq.Count) && !(seq[num] is DerTaggedObject))
            {
                this.revokedCertificates = Asn1Sequence.GetInstance(seq[num++]);
            }
            if ((num < seq.Count) && (seq[num] is DerTaggedObject))
            {
                this.crlExtensions = X509Extensions.GetInstance(seq[num]);
            }
        }

        public static TbsCertificateList GetInstance(object obj)
        {
            TbsCertificateList list = obj as TbsCertificateList;
            if ((obj == null) || (list != null))
            {
                return list;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new TbsCertificateList((Asn1Sequence) obj);
        }

        public static TbsCertificateList GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public IEnumerable GetRevokedCertificateEnumeration()
        {
            if (this.revokedCertificates == null)
            {
                return EmptyEnumerable.Instance;
            }
            return new RevokedCertificatesEnumeration(this.revokedCertificates);
        }

        public CrlEntry[] GetRevokedCertificates()
        {
            if (this.revokedCertificates == null)
            {
                return new CrlEntry[0];
            }
            CrlEntry[] entryArray = new CrlEntry[this.revokedCertificates.Count];
            for (int i = 0; i < entryArray.Length; i++)
            {
                entryArray[i] = new CrlEntry(Asn1Sequence.GetInstance(this.revokedCertificates[i]));
            }
            return entryArray;
        }

        public override Asn1Object ToAsn1Object() => 
            this.seq;

        public int Version =>
            (this.version.Value.IntValue + 1);

        public DerInteger VersionNumber =>
            this.version;

        public AlgorithmIdentifier Signature =>
            this.signature;

        public X509Name Issuer =>
            this.issuer;

        public Time ThisUpdate =>
            this.thisUpdate;

        public Time NextUpdate =>
            this.nextUpdate;

        public X509Extensions Extensions =>
            this.crlExtensions;

        private class RevokedCertificatesEnumeration : IEnumerable
        {
            private readonly IEnumerable en;

            internal RevokedCertificatesEnumeration(IEnumerable en)
            {
                this.en = en;
            }

            public IEnumerator GetEnumerator() => 
                new RevokedCertificatesEnumerator(this.en.GetEnumerator());

            private class RevokedCertificatesEnumerator : IEnumerator
            {
                private readonly IEnumerator e;

                internal RevokedCertificatesEnumerator(IEnumerator e)
                {
                    this.e = e;
                }

                public bool MoveNext() => 
                    this.e.MoveNext();

                public void Reset()
                {
                    this.e.Reset();
                }

                public object Current =>
                    new CrlEntry(Asn1Sequence.GetInstance(this.e.Current));
            }
        }
    }
}

