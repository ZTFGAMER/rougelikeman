namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public class CertificateRequest
    {
        protected readonly byte[] mCertificateTypes;
        protected readonly IList mSupportedSignatureAlgorithms;
        protected readonly IList mCertificateAuthorities;

        public CertificateRequest(byte[] certificateTypes, IList supportedSignatureAlgorithms, IList certificateAuthorities)
        {
            this.mCertificateTypes = certificateTypes;
            this.mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
            this.mCertificateAuthorities = certificateAuthorities;
        }

        public virtual void Encode(Stream output)
        {
            if ((this.mCertificateTypes == null) || (this.mCertificateTypes.Length == 0))
            {
                TlsUtilities.WriteUint8(0, output);
            }
            else
            {
                TlsUtilities.WriteUint8ArrayWithUint8Length(this.mCertificateTypes, output);
            }
            if (this.mSupportedSignatureAlgorithms != null)
            {
                TlsUtilities.EncodeSupportedSignatureAlgorithms(this.mSupportedSignatureAlgorithms, false, output);
            }
            if ((this.mCertificateAuthorities == null) || (this.mCertificateAuthorities.Count < 1))
            {
                TlsUtilities.WriteUint16(0, output);
            }
            else
            {
                IList list = Platform.CreateArrayList(this.mCertificateAuthorities.Count);
                int i = 0;
                IEnumerator enumerator = this.mCertificateAuthorities.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        byte[] encoded = ((Asn1Encodable) enumerator.Current).GetEncoded("DER");
                        list.Add(encoded);
                        i += encoded.Length + 2;
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
                TlsUtilities.CheckUint16(i);
                TlsUtilities.WriteUint16(i, output);
                IEnumerator enumerator2 = list.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        byte[] current = (byte[]) enumerator2.Current;
                        TlsUtilities.WriteOpaque16(current, output);
                    }
                }
                finally
                {
                    if (enumerator2 is IDisposable disposable2)
                    {
                        IDisposable disposable2;
                        disposable2.Dispose();
                    }
                }
            }
        }

        public static CertificateRequest Parse(TlsContext context, Stream input)
        {
            int num = TlsUtilities.ReadUint8(input);
            byte[] certificateTypes = new byte[num];
            for (int i = 0; i < num; i++)
            {
                certificateTypes[i] = TlsUtilities.ReadUint8(input);
            }
            IList supportedSignatureAlgorithms = null;
            if (TlsUtilities.IsTlsV12(context))
            {
                supportedSignatureAlgorithms = TlsUtilities.ParseSupportedSignatureAlgorithms(false, input);
            }
            IList certificateAuthorities = Platform.CreateArrayList();
            MemoryStream stream = new MemoryStream(TlsUtilities.ReadOpaque16(input), false);
            while (stream.Position < stream.Length)
            {
                Asn1Object obj2 = TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque16(stream));
                certificateAuthorities.Add(X509Name.GetInstance(obj2));
            }
            return new CertificateRequest(certificateTypes, supportedSignatureAlgorithms, certificateAuthorities);
        }

        public virtual byte[] CertificateTypes =>
            this.mCertificateTypes;

        public virtual IList SupportedSignatureAlgorithms =>
            this.mSupportedSignatureAlgorithms;

        public virtual IList CertificateAuthorities =>
            this.mCertificateAuthorities;
    }
}

