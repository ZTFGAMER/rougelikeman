namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1.Ocsp;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public class OcspStatusRequest
    {
        protected readonly IList mResponderIDList;
        protected readonly X509Extensions mRequestExtensions;

        public OcspStatusRequest(IList responderIDList, X509Extensions requestExtensions)
        {
            this.mResponderIDList = responderIDList;
            this.mRequestExtensions = requestExtensions;
        }

        public virtual void Encode(Stream output)
        {
            if ((this.mResponderIDList == null) || (this.mResponderIDList.Count < 1))
            {
                TlsUtilities.WriteUint16(0, output);
            }
            else
            {
                MemoryStream stream = new MemoryStream();
                for (int i = 0; i < this.mResponderIDList.Count; i++)
                {
                    ResponderID rid = (ResponderID) this.mResponderIDList[i];
                    TlsUtilities.WriteOpaque16(rid.GetEncoded("DER"), stream);
                }
                TlsUtilities.CheckUint16(stream.Length);
                TlsUtilities.WriteUint16((int) stream.Length, output);
                stream.WriteTo(output);
            }
            if (this.mRequestExtensions == null)
            {
                TlsUtilities.WriteUint16(0, output);
            }
            else
            {
                byte[] encoded = this.mRequestExtensions.GetEncoded("DER");
                TlsUtilities.CheckUint16(encoded.Length);
                TlsUtilities.WriteUint16(encoded.Length, output);
                output.Write(encoded, 0, encoded.Length);
            }
        }

        public static OcspStatusRequest Parse(Stream input)
        {
            IList responderIDList = Platform.CreateArrayList();
            int length = TlsUtilities.ReadUint16(input);
            if (length > 0)
            {
                MemoryStream stream = new MemoryStream(TlsUtilities.ReadFully(length, input), false);
                do
                {
                    ResponderID instance = ResponderID.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque16(stream)));
                    responderIDList.Add(instance);
                }
                while (stream.Position < stream.Length);
            }
            X509Extensions requestExtensions = null;
            int num2 = TlsUtilities.ReadUint16(input);
            if (num2 > 0)
            {
                requestExtensions = X509Extensions.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadFully(num2, input)));
            }
            return new OcspStatusRequest(responderIDList, requestExtensions);
        }

        public virtual IList ResponderIDList =>
            this.mResponderIDList;

        public virtual X509Extensions RequestExtensions =>
            this.mRequestExtensions;
    }
}

