namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    public class CertificateStatusRequest
    {
        protected readonly byte mStatusType;
        protected readonly object mRequest;

        public CertificateStatusRequest(byte statusType, object request)
        {
            if (!IsCorrectType(statusType, request))
            {
                throw new ArgumentException("not an instance of the correct type", "request");
            }
            this.mStatusType = statusType;
            this.mRequest = request;
        }

        public virtual void Encode(Stream output)
        {
            TlsUtilities.WriteUint8(this.mStatusType, output);
            if (this.mStatusType != 1)
            {
                throw new TlsFatalAlert(80);
            }
            ((OcspStatusRequest) this.mRequest).Encode(output);
        }

        public virtual OcspStatusRequest GetOcspStatusRequest()
        {
            if (!IsCorrectType(1, this.mRequest))
            {
                throw new InvalidOperationException("'request' is not an OCSPStatusRequest");
            }
            return (OcspStatusRequest) this.mRequest;
        }

        protected static bool IsCorrectType(byte statusType, object request)
        {
            if (statusType != 1)
            {
                throw new ArgumentException("unsupported value", "statusType");
            }
            return (request is OcspStatusRequest);
        }

        public static CertificateStatusRequest Parse(Stream input)
        {
            byte statusType = TlsUtilities.ReadUint8(input);
            if (statusType != 1)
            {
                throw new TlsFatalAlert(50);
            }
            return new CertificateStatusRequest(statusType, OcspStatusRequest.Parse(input));
        }

        public virtual byte StatusType =>
            this.mStatusType;

        public virtual object Request =>
            this.mRequest;
    }
}

