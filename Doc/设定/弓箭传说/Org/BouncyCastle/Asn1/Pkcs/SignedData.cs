namespace Org.BouncyCastle.Asn1.Pkcs
{
    using Org.BouncyCastle.Asn1;
    using System;
    using System.Collections;

    public class SignedData : Asn1Encodable
    {
        private readonly DerInteger version;
        private readonly Asn1Set digestAlgorithms;
        private readonly Org.BouncyCastle.Asn1.Pkcs.ContentInfo contentInfo;
        private readonly Asn1Set certificates;
        private readonly Asn1Set crls;
        private readonly Asn1Set signerInfos;

        private SignedData(Asn1Sequence seq)
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            this.version = (DerInteger) enumerator.Current;
            enumerator.MoveNext();
            this.digestAlgorithms = (Asn1Set) enumerator.Current;
            enumerator.MoveNext();
            this.contentInfo = Org.BouncyCastle.Asn1.Pkcs.ContentInfo.GetInstance(enumerator.Current);
            while (enumerator.MoveNext())
            {
                Asn1Object current = (Asn1Object) enumerator.Current;
                if (current is DerTaggedObject)
                {
                    DerTaggedObject obj3 = (DerTaggedObject) current;
                    switch (obj3.TagNo)
                    {
                        case 0:
                        {
                            this.certificates = Asn1Set.GetInstance(obj3, false);
                            continue;
                        }
                        case 1:
                        {
                            this.crls = Asn1Set.GetInstance(obj3, false);
                            continue;
                        }
                    }
                    throw new ArgumentException("unknown tag value " + obj3.TagNo);
                }
                this.signerInfos = (Asn1Set) current;
            }
        }

        public SignedData(DerInteger _version, Asn1Set _digestAlgorithms, Org.BouncyCastle.Asn1.Pkcs.ContentInfo _contentInfo, Asn1Set _certificates, Asn1Set _crls, Asn1Set _signerInfos)
        {
            this.version = _version;
            this.digestAlgorithms = _digestAlgorithms;
            this.contentInfo = _contentInfo;
            this.certificates = _certificates;
            this.crls = _crls;
            this.signerInfos = _signerInfos;
        }

        public static SignedData GetInstance(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            SignedData data = obj as SignedData;
            if (data != null)
            {
                return data;
            }
            return new SignedData(Asn1Sequence.GetInstance(obj));
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1Encodable[] v = new Asn1Encodable[] { this.version, this.digestAlgorithms, this.contentInfo };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            if (this.certificates != null)
            {
                Asn1Encodable[] encodableArray2 = new Asn1Encodable[] { new DerTaggedObject(false, 0, this.certificates) };
                vector.Add(encodableArray2);
            }
            if (this.crls != null)
            {
                Asn1Encodable[] encodableArray3 = new Asn1Encodable[] { new DerTaggedObject(false, 1, this.crls) };
                vector.Add(encodableArray3);
            }
            Asn1Encodable[] objs = new Asn1Encodable[] { this.signerInfos };
            vector.Add(objs);
            return new BerSequence(vector);
        }

        public DerInteger Version =>
            this.version;

        public Asn1Set DigestAlgorithms =>
            this.digestAlgorithms;

        public Org.BouncyCastle.Asn1.Pkcs.ContentInfo ContentInfo =>
            this.contentInfo;

        public Asn1Set Certificates =>
            this.certificates;

        public Asn1Set Crls =>
            this.crls;

        public Asn1Set SignerInfos =>
            this.signerInfos;
    }
}

