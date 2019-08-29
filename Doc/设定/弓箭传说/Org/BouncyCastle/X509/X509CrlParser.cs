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

    public class X509CrlParser
    {
        private static readonly PemParser PemCrlParser = new PemParser("CRL");
        private readonly bool lazyAsn1;
        private Asn1Set sCrlData;
        private int sCrlDataObjectCount;
        private Stream currentCrlStream;

        public X509CrlParser() : this(false)
        {
        }

        public X509CrlParser(bool lazyAsn1)
        {
            this.lazyAsn1 = lazyAsn1;
        }

        protected virtual X509Crl CreateX509Crl(CertificateList c) => 
            new X509Crl(c);

        private X509Crl GetCrl()
        {
            if ((this.sCrlData != null) && (this.sCrlDataObjectCount < this.sCrlData.Count))
            {
                return this.CreateX509Crl(CertificateList.GetInstance(this.sCrlData[this.sCrlDataObjectCount++]));
            }
            return null;
        }

        public X509Crl ReadCrl(byte[] input) => 
            this.ReadCrl(new MemoryStream(input, false));

        public X509Crl ReadCrl(Stream inStream)
        {
            X509Crl crl;
            if (inStream == null)
            {
                throw new ArgumentNullException("inStream");
            }
            if (!inStream.CanRead)
            {
                throw new ArgumentException("inStream must be read-able", "inStream");
            }
            if (this.currentCrlStream == null)
            {
                this.currentCrlStream = inStream;
                this.sCrlData = null;
                this.sCrlDataObjectCount = 0;
            }
            else if (this.currentCrlStream != inStream)
            {
                this.currentCrlStream = inStream;
                this.sCrlData = null;
                this.sCrlDataObjectCount = 0;
            }
            try
            {
                if (this.sCrlData != null)
                {
                    if (this.sCrlDataObjectCount != this.sCrlData.Count)
                    {
                        return this.GetCrl();
                    }
                    this.sCrlData = null;
                    this.sCrlDataObjectCount = 0;
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
                    return this.ReadPemCrl(stream);
                }
                Asn1InputStream dIn = !this.lazyAsn1 ? new Asn1InputStream(stream) : new LazyAsn1InputStream(stream);
                crl = this.ReadDerCrl(dIn);
            }
            catch (CrlException exception)
            {
                throw exception;
            }
            catch (Exception exception2)
            {
                throw new CrlException(exception2.ToString());
            }
            return crl;
        }

        public ICollection ReadCrls(byte[] input) => 
            this.ReadCrls(new MemoryStream(input, false));

        public ICollection ReadCrls(Stream inStream)
        {
            X509Crl crl;
            IList list = Platform.CreateArrayList();
            while ((crl = this.ReadCrl(inStream)) != null)
            {
                list.Add(crl);
            }
            return list;
        }

        private X509Crl ReadDerCrl(Asn1InputStream dIn)
        {
            Asn1Sequence sequence = (Asn1Sequence) dIn.ReadObject();
            if (((sequence.Count > 1) && (sequence[0] is DerObjectIdentifier)) && sequence[0].Equals(PkcsObjectIdentifiers.SignedData))
            {
                this.sCrlData = SignedData.GetInstance(Asn1Sequence.GetInstance((Asn1TaggedObject) sequence[1], true)).Crls;
                return this.GetCrl();
            }
            return this.CreateX509Crl(CertificateList.GetInstance(sequence));
        }

        private X509Crl ReadPemCrl(Stream inStream)
        {
            Asn1Sequence sequence = PemCrlParser.ReadPemObject(inStream);
            return ((sequence != null) ? this.CreateX509Crl(CertificateList.GetInstance(sequence)) : null);
        }
    }
}

