namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1.Ocsp;
    using System;
    using System.IO;

    public class CertificateStatus
    {
        protected readonly byte mStatusType;
        protected readonly object mResponse;

        public CertificateStatus(byte statusType, object response)
        {
            if (!IsCorrectType(statusType, response))
            {
                throw new ArgumentException("not an instance of the correct type", "response");
            }
            this.mStatusType = statusType;
            this.mResponse = response;
        }

        public virtual void Encode(Stream output)
        {
            TlsUtilities.WriteUint8(this.mStatusType, output);
            if (this.mStatusType != 1)
            {
                throw new TlsFatalAlert(80);
            }
            TlsUtilities.WriteOpaque24(((OcspResponse) this.mResponse).GetEncoded("DER"), output);
        }

        public virtual OcspResponse GetOcspResponse()
        {
            if (!IsCorrectType(1, this.mResponse))
            {
                throw new InvalidOperationException("'response' is not an OcspResponse");
            }
            return (OcspResponse) this.mResponse;
        }

        protected static bool IsCorrectType(byte statusType, object response)
        {
            if (statusType != 1)
            {
                throw new ArgumentException("unsupported value", "statusType");
            }
            return (response is OcspResponse);
        }

        public static CertificateStatus Parse(Stream input)
        {
            byte statusType = TlsUtilities.ReadUint8(input);
            if (statusType != 1)
            {
                throw new TlsFatalAlert(50);
            }
            return new CertificateStatus(statusType, OcspResponse.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque24(input))));
        }

        public virtual byte StatusType =>
            this.mStatusType;

        public virtual object Response =>
            this.mResponse;
    }
}

