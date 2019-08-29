namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public class Certificate
    {
        public static readonly Certificate EmptyChain = new Certificate(new X509CertificateStructure[0]);
        protected readonly X509CertificateStructure[] mCertificateList;

        public Certificate(X509CertificateStructure[] certificateList)
        {
            if (certificateList == null)
            {
                throw new ArgumentNullException("certificateList");
            }
            this.mCertificateList = certificateList;
        }

        protected virtual X509CertificateStructure[] CloneCertificateList() => 
            ((X509CertificateStructure[]) this.mCertificateList.Clone());

        public virtual void Encode(Stream output)
        {
            IList list = Platform.CreateArrayList(this.mCertificateList.Length);
            int i = 0;
            X509CertificateStructure[] mCertificateList = this.mCertificateList;
            for (int j = 0; j < mCertificateList.Length; j++)
            {
                byte[] encoded = mCertificateList[j].GetEncoded("DER");
                list.Add(encoded);
                i += encoded.Length + 3;
            }
            TlsUtilities.CheckUint24(i);
            TlsUtilities.WriteUint24(i, output);
            IEnumerator enumerator = list.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    byte[] current = (byte[]) enumerator.Current;
                    TlsUtilities.WriteOpaque24(current, output);
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

        public virtual X509CertificateStructure GetCertificateAt(int index) => 
            this.mCertificateList[index];

        public virtual X509CertificateStructure[] GetCertificateList() => 
            this.CloneCertificateList();

        public static Certificate Parse(Stream input)
        {
            int length = TlsUtilities.ReadUint24(input);
            if (length == 0)
            {
                return EmptyChain;
            }
            MemoryStream stream = new MemoryStream(TlsUtilities.ReadFully(length, input), false);
            IList list = Platform.CreateArrayList();
            while (stream.Position < stream.Length)
            {
                Asn1Object obj2 = TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque24(stream));
                list.Add(X509CertificateStructure.GetInstance(obj2));
            }
            X509CertificateStructure[] certificateList = new X509CertificateStructure[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                certificateList[i] = (X509CertificateStructure) list[i];
            }
            return new Certificate(certificateList);
        }

        public virtual int Length =>
            this.mCertificateList.Length;

        public virtual bool IsEmpty =>
            (this.mCertificateList.Length == 0);
    }
}

