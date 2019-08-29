namespace Org.BouncyCastle.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Pkcs;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Security.Certificates;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.Collections;
    using System.IO;

    public class X509CertificateParser
    {
        private static readonly PemParser PemCertParser = new PemParser("CERTIFICATE");
        private Asn1Set sData;
        private int sDataObjectCount;
        private Stream currentStream;

        protected virtual X509Certificate CreateX509Certificate(X509CertificateStructure c) => 
            new X509Certificate(c);

        private X509Certificate GetCertificate()
        {
            if (this.sData != null)
            {
                while (this.sDataObjectCount < this.sData.Count)
                {
                    object obj2 = this.sData[this.sDataObjectCount++];
                    if (obj2 is Asn1Sequence)
                    {
                        return this.CreateX509Certificate(X509CertificateStructure.GetInstance(obj2));
                    }
                }
            }
            return null;
        }

        public X509Certificate ReadCertificate(byte[] input) => 
            this.ReadCertificate(new MemoryStream(input, false));

        public X509Certificate ReadCertificate(Stream inStream)
        {
            X509Certificate certificate;
            if (inStream == null)
            {
                throw new ArgumentNullException("inStream");
            }
            if (!inStream.CanRead)
            {
                throw new ArgumentException("inStream must be read-able", "inStream");
            }
            if (this.currentStream == null)
            {
                this.currentStream = inStream;
                this.sData = null;
                this.sDataObjectCount = 0;
            }
            else if (this.currentStream != inStream)
            {
                this.currentStream = inStream;
                this.sData = null;
                this.sDataObjectCount = 0;
            }
            try
            {
                if (this.sData != null)
                {
                    if (this.sDataObjectCount != this.sData.Count)
                    {
                        return this.GetCertificate();
                    }
                    this.sData = null;
                    this.sDataObjectCount = 0;
                    return null;
                }
                PushbackStream stream = new PushbackStream(inStream);
                int b = stream.ReadByte();
                if (b < 0)
                {
                    return null;
                }
                stream.Unread(b);
                if (b != 0x30)
                {
                    return this.ReadPemCertificate(stream);
                }
                certificate = this.ReadDerCertificate(new Asn1InputStream(stream));
            }
            catch (Exception exception)
            {
                throw new CertificateException("Failed to read certificate", exception);
            }
            return certificate;
        }

        public ICollection ReadCertificates(byte[] input) => 
            this.ReadCertificates(new MemoryStream(input, false));

        public ICollection ReadCertificates(Stream inStream)
        {
            X509Certificate certificate;
            IList list = Platform.CreateArrayList();
            while ((certificate = this.ReadCertificate(inStream)) != null)
            {
                list.Add(certificate);
            }
            return list;
        }

        private X509Certificate ReadDerCertificate(Asn1InputStream dIn)
        {
            Asn1Sequence sequence = (Asn1Sequence) dIn.ReadObject();
            if (((sequence.Count > 1) && (sequence[0] is DerObjectIdentifier)) && sequence[0].Equals(PkcsObjectIdentifiers.SignedData))
            {
                this.sData = SignedData.GetInstance(Asn1Sequence.GetInstance((Asn1TaggedObject) sequence[1], true)).Certificates;
                return this.GetCertificate();
            }
            return this.CreateX509Certificate(X509CertificateStructure.GetInstance(sequence));
        }

        private X509Certificate ReadPemCertificate(Stream inStream)
        {
            Asn1Sequence sequence = PemCertParser.ReadPemObject(inStream);
            return ((sequence != null) ? this.CreateX509Certificate(X509CertificateStructure.GetInstance(sequence)) : null);
        }
    }
}

