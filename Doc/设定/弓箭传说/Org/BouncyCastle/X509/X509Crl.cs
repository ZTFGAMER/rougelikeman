namespace Org.BouncyCastle.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Utilities;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Operators;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Security.Certificates;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Collections;
    using Org.BouncyCastle.Utilities.Date;
    using Org.BouncyCastle.Utilities.Encoders;
    using Org.BouncyCastle.X509.Extension;
    using System;
    using System.Collections;
    using System.Text;

    public class X509Crl : X509ExtensionBase
    {
        private readonly CertificateList c;
        private readonly string sigAlgName;
        private readonly byte[] sigAlgParams;
        private readonly bool isIndirect;

        public X509Crl(CertificateList c)
        {
            this.c = c;
            try
            {
                this.sigAlgName = X509SignatureUtilities.GetSignatureName(c.SignatureAlgorithm);
                if (c.SignatureAlgorithm.Parameters != null)
                {
                    this.sigAlgParams = c.SignatureAlgorithm.Parameters.GetDerEncoded();
                }
                else
                {
                    this.sigAlgParams = null;
                }
                this.isIndirect = this.IsIndirectCrl;
            }
            catch (Exception exception)
            {
                throw new CrlException("CRL contents invalid: " + exception);
            }
        }

        protected virtual void CheckSignature(IVerifierFactory verifier)
        {
            if (!this.c.SignatureAlgorithm.Equals(this.c.TbsCertList.Signature))
            {
                throw new CrlException("Signature algorithm on CertificateList does not match TbsCertList.");
            }
            IStreamCalculator calculator = verifier.CreateCalculator();
            byte[] tbsCertList = this.GetTbsCertList();
            calculator.Stream.Write(tbsCertList, 0, tbsCertList.Length);
            Platform.Dispose(calculator.Stream);
            if (!((IVerifier) calculator.GetResult()).IsVerified(this.GetSignature()))
            {
                throw new InvalidKeyException("CRL does not verify with supplied public key.");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            X509Crl crl = obj as X509Crl;
            if (crl == null)
            {
                return false;
            }
            return this.c.Equals(crl.c);
        }

        public virtual byte[] GetEncoded()
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

        public override int GetHashCode() => 
            this.c.GetHashCode();

        public virtual X509CrlEntry GetRevokedCertificate(BigInteger serialNumber)
        {
            IEnumerable revokedCertificateEnumeration = this.c.GetRevokedCertificateEnumeration();
            X509Name issuerDN = this.IssuerDN;
            IEnumerator enumerator = revokedCertificateEnumeration.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    CrlEntry current = (CrlEntry) enumerator.Current;
                    X509CrlEntry entry2 = new X509CrlEntry(current, this.isIndirect, issuerDN);
                    if (serialNumber.Equals(current.UserCertificate.Value))
                    {
                        return entry2;
                    }
                    issuerDN = entry2.GetCertificateIssuer();
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
            return null;
        }

        public virtual ISet GetRevokedCertificates()
        {
            ISet set = this.LoadCrlEntries();
            if (set.Count > 0)
            {
                return set;
            }
            return null;
        }

        public virtual byte[] GetSigAlgParams() => 
            Arrays.Clone(this.sigAlgParams);

        public virtual byte[] GetSignature() => 
            this.c.GetSignatureOctets();

        public virtual byte[] GetTbsCertList()
        {
            byte[] derEncoded;
            try
            {
                derEncoded = this.c.TbsCertList.GetDerEncoded();
            }
            catch (Exception exception)
            {
                throw new CrlException(exception.ToString());
            }
            return derEncoded;
        }

        protected override X509Extensions GetX509Extensions() => 
            ((this.c.Version < 2) ? null : this.c.TbsCertList.Extensions);

        public virtual bool IsRevoked(X509Certificate cert)
        {
            CrlEntry[] revokedCertificates = this.c.GetRevokedCertificates();
            if (revokedCertificates != null)
            {
                BigInteger serialNumber = cert.SerialNumber;
                for (int i = 0; i < revokedCertificates.Length; i++)
                {
                    if (revokedCertificates[i].UserCertificate.Value.Equals(serialNumber))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private ISet LoadCrlEntries()
        {
            ISet set = new HashSet();
            IEnumerable revokedCertificateEnumeration = this.c.GetRevokedCertificateEnumeration();
            X509Name issuerDN = this.IssuerDN;
            IEnumerator enumerator = revokedCertificateEnumeration.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    CrlEntry current = (CrlEntry) enumerator.Current;
                    X509CrlEntry o = new X509CrlEntry(current, this.isIndirect, issuerDN);
                    set.Add(o);
                    issuerDN = o.GetCertificateIssuer();
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
            return set;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            string newLine = Platform.NewLine;
            builder.Append("              Version: ").Append(this.Version).Append(newLine);
            builder.Append("             IssuerDN: ").Append(this.IssuerDN).Append(newLine);
            builder.Append("          This update: ").Append(this.ThisUpdate).Append(newLine);
            builder.Append("          Next update: ").Append(this.NextUpdate).Append(newLine);
            builder.Append("  Signature Algorithm: ").Append(this.SigAlgName).Append(newLine);
            byte[] signature = this.GetSignature();
            builder.Append("            Signature: ");
            builder.Append(Hex.ToHexString(signature, 0, 20)).Append(newLine);
            for (int i = 20; i < signature.Length; i += 20)
            {
                int length = Math.Min(20, signature.Length - i);
                builder.Append("                       ");
                builder.Append(Hex.ToHexString(signature, i, length)).Append(newLine);
            }
            X509Extensions extensions = this.c.TbsCertList.Extensions;
            if (extensions != null)
            {
                IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    builder.Append("           Extensions: ").Append(newLine);
                }
                do
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    X509Extension extension = extensions.GetExtension(current);
                    if (extension.Value != null)
                    {
                        Asn1Object obj2 = X509ExtensionUtilities.FromExtensionValue(extension.Value);
                        builder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
                        try
                        {
                            if (current.Equals(X509Extensions.CrlNumber))
                            {
                                builder.Append(new CrlNumber(DerInteger.GetInstance(obj2).PositiveValue)).Append(newLine);
                            }
                            else if (current.Equals(X509Extensions.DeltaCrlIndicator))
                            {
                                builder.Append("Base CRL: " + new CrlNumber(DerInteger.GetInstance(obj2).PositiveValue)).Append(newLine);
                            }
                            else if (current.Equals(X509Extensions.IssuingDistributionPoint))
                            {
                                builder.Append(IssuingDistributionPoint.GetInstance((Asn1Sequence) obj2)).Append(newLine);
                            }
                            else if (current.Equals(X509Extensions.CrlDistributionPoints))
                            {
                                builder.Append(CrlDistPoint.GetInstance((Asn1Sequence) obj2)).Append(newLine);
                            }
                            else if (current.Equals(X509Extensions.FreshestCrl))
                            {
                                builder.Append(CrlDistPoint.GetInstance((Asn1Sequence) obj2)).Append(newLine);
                            }
                            else
                            {
                                builder.Append(current.Id);
                                builder.Append(" value = ").Append(Asn1Dump.DumpAsString((Asn1Encodable) obj2)).Append(newLine);
                            }
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
            ISet revokedCertificates = this.GetRevokedCertificates();
            if (revokedCertificates != null)
            {
                IEnumerator enumerator = revokedCertificates.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        X509CrlEntry current = (X509CrlEntry) enumerator.Current;
                        builder.Append(current);
                        builder.Append(newLine);
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
            return builder.ToString();
        }

        public virtual void Verify(AsymmetricKeyParameter publicKey)
        {
            this.Verify(new Asn1VerifierFactoryProvider(publicKey));
        }

        public virtual void Verify(IVerifierFactoryProvider verifierProvider)
        {
            this.CheckSignature(verifierProvider.CreateVerifierFactory(this.c.SignatureAlgorithm));
        }

        public virtual int Version =>
            this.c.Version;

        public virtual X509Name IssuerDN =>
            this.c.Issuer;

        public virtual DateTime ThisUpdate =>
            this.c.ThisUpdate.ToDateTime();

        public virtual DateTimeObject NextUpdate =>
            ((this.c.NextUpdate != null) ? new DateTimeObject(this.c.NextUpdate.ToDateTime()) : null);

        public virtual string SigAlgName =>
            this.sigAlgName;

        public virtual string SigAlgOid =>
            this.c.SignatureAlgorithm.Algorithm.Id;

        protected virtual bool IsIndirectCrl
        {
            get
            {
                Asn1OctetString extensionValue = this.GetExtensionValue(X509Extensions.IssuingDistributionPoint);
                bool isIndirectCrl = false;
                try
                {
                    if (extensionValue != null)
                    {
                        isIndirectCrl = IssuingDistributionPoint.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue)).IsIndirectCrl;
                    }
                }
                catch (Exception exception)
                {
                    throw new CrlException("Exception reading IssuingDistributionPoint" + exception);
                }
                return isIndirectCrl;
            }
        }
    }
}

