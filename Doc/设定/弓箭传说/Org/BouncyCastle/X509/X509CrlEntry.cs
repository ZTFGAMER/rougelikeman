namespace Org.BouncyCastle.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Utilities;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security.Certificates;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.X509.Extension;
    using System;
    using System.Collections;
    using System.Text;

    public class X509CrlEntry : X509ExtensionBase
    {
        private CrlEntry c;
        private bool isIndirect;
        private X509Name previousCertificateIssuer;
        private X509Name certificateIssuer;

        public X509CrlEntry(CrlEntry c)
        {
            this.c = c;
            this.certificateIssuer = this.loadCertificateIssuer();
        }

        public X509CrlEntry(CrlEntry c, bool isIndirect, X509Name previousCertificateIssuer)
        {
            this.c = c;
            this.isIndirect = isIndirect;
            this.previousCertificateIssuer = previousCertificateIssuer;
            this.certificateIssuer = this.loadCertificateIssuer();
        }

        public X509Name GetCertificateIssuer() => 
            this.certificateIssuer;

        public byte[] GetEncoded()
        {
            byte[] derEncoded;
            try
            {
                derEncoded = this.c.GetDerEncoded();
            }
            catch (Exception exception)
            {
                throw new CrlException(exception.ToString());
            }
            return derEncoded;
        }

        protected override X509Extensions GetX509Extensions() => 
            this.c.Extensions;

        private X509Name loadCertificateIssuer()
        {
            if (this.isIndirect)
            {
                Asn1OctetString extensionValue = this.GetExtensionValue(X509Extensions.CertificateIssuer);
                if (extensionValue == null)
                {
                    return this.previousCertificateIssuer;
                }
                try
                {
                    GeneralName[] names = GeneralNames.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue)).GetNames();
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (names[i].TagNo == 4)
                        {
                            return X509Name.GetInstance(names[i].Name);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            string newLine = Platform.NewLine;
            builder.Append("        userCertificate: ").Append(this.SerialNumber).Append(newLine);
            builder.Append("         revocationDate: ").Append(this.RevocationDate).Append(newLine);
            builder.Append("      certificateIssuer: ").Append(this.GetCertificateIssuer()).Append(newLine);
            X509Extensions extensions = this.c.Extensions;
            if (extensions != null)
            {
                IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    builder.Append("   crlEntryExtensions:").Append(newLine);
                    do
                    {
                        DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                        X509Extension extension = extensions.GetExtension(current);
                        if (extension.Value != null)
                        {
                            Asn1Object obj2 = Asn1Object.FromByteArray(extension.Value.GetOctets());
                            builder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
                            try
                            {
                                if (current.Equals(X509Extensions.ReasonCode))
                                {
                                    builder.Append(new CrlReason(DerEnumerated.GetInstance(obj2)));
                                }
                                else if (current.Equals(X509Extensions.CertificateIssuer))
                                {
                                    builder.Append("Certificate issuer: ").Append(GeneralNames.GetInstance((Asn1Sequence) obj2));
                                }
                                else
                                {
                                    builder.Append(current.Id);
                                    builder.Append(" value = ").Append(Asn1Dump.DumpAsString((Asn1Encodable) obj2));
                                }
                                builder.Append(newLine);
                            }
                            catch (Exception)
                            {
                                builder.Append(current.Id);
                                builder.Append(" value = ").Append("*****").Append(newLine);
                            }
                        }
                        else
                        {
                            builder.Append(newLine);
                        }
                    }
                    while (enumerator.MoveNext());
                }
            }
            return builder.ToString();
        }

        public BigInteger SerialNumber =>
            this.c.UserCertificate.Value;

        public DateTime RevocationDate =>
            this.c.RevocationDate.ToDateTime();

        public bool HasExtensions =>
            (this.c.Extensions != null);
    }
}

