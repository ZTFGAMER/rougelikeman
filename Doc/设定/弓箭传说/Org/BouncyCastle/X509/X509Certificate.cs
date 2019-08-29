namespace Org.BouncyCastle.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Misc;
    using Org.BouncyCastle.Asn1.Utilities;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Operators;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Security.Certificates;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Encoders;
    using Org.BouncyCastle.X509.Extension;
    using System;
    using System.Collections;
    using System.Text;

    public class X509Certificate : X509ExtensionBase
    {
        private readonly X509CertificateStructure c;
        private readonly BasicConstraints basicConstraints;
        private readonly bool[] keyUsage;
        private bool hashValueSet;
        private int hashValue;

        protected X509Certificate()
        {
        }

        public X509Certificate(X509CertificateStructure c)
        {
            this.c = c;
            try
            {
                Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.19"));
                if (extensionValue != null)
                {
                    this.basicConstraints = BasicConstraints.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
                }
            }
            catch (Exception exception)
            {
                throw new CertificateParsingException("cannot construct BasicConstraints: " + exception);
            }
            try
            {
                Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.15"));
                if (extensionValue != null)
                {
                    DerBitString instance = DerBitString.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
                    byte[] bytes = instance.GetBytes();
                    int num = (bytes.Length * 8) - instance.PadBits;
                    this.keyUsage = new bool[(num >= 9) ? num : 9];
                    for (int i = 0; i != num; i++)
                    {
                        this.keyUsage[i] = (bytes[i / 8] & (((int) 0x80) >> (i % 8))) != 0;
                    }
                }
                else
                {
                    this.keyUsage = null;
                }
            }
            catch (Exception exception2)
            {
                throw new CertificateParsingException("cannot construct KeyUsage: " + exception2);
            }
        }

        protected virtual void CheckSignature(IVerifierFactory verifier)
        {
            if (!IsAlgIDEqual(this.c.SignatureAlgorithm, this.c.TbsCertificate.Signature))
            {
                throw new CertificateException("signature algorithm in TBS cert not same as outer cert");
            }
            IStreamCalculator calculator = verifier.CreateCalculator();
            byte[] tbsCertificate = this.GetTbsCertificate();
            calculator.Stream.Write(tbsCertificate, 0, tbsCertificate.Length);
            Platform.Dispose(calculator.Stream);
            if (!((IVerifier) calculator.GetResult()).IsVerified(this.GetSignature()))
            {
                throw new InvalidKeyException("Public key presented not for certificate signature");
            }
        }

        public virtual void CheckValidity()
        {
            this.CheckValidity(DateTime.UtcNow);
        }

        public virtual void CheckValidity(DateTime time)
        {
            if (time.CompareTo(this.NotAfter) > 0)
            {
                throw new CertificateExpiredException("certificate expired on " + this.c.EndDate.GetTime());
            }
            if (time.CompareTo(this.NotBefore) < 0)
            {
                throw new CertificateNotYetValidException("certificate not valid until " + this.c.StartDate.GetTime());
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            X509Certificate certificate = obj as X509Certificate;
            if (certificate == null)
            {
                return false;
            }
            return this.c.Equals(certificate.c);
        }

        protected virtual ICollection GetAlternativeNames(string oid)
        {
            Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier(oid));
            if (extensionValue == null)
            {
                return null;
            }
            GeneralNames instance = GeneralNames.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
            IList list = Platform.CreateArrayList();
            foreach (GeneralName name in instance.GetNames())
            {
                IList list2 = Platform.CreateArrayList();
                list2.Add(name.TagNo);
                list2.Add(name.Name.ToString());
                list.Add(list2);
            }
            return list;
        }

        public virtual int GetBasicConstraints()
        {
            if ((this.basicConstraints == null) || !this.basicConstraints.IsCA())
            {
                return -1;
            }
            return this.basicConstraints.PathLenConstraint?.IntValue;
        }

        public virtual byte[] GetEncoded() => 
            this.c.GetDerEncoded();

        public virtual IList GetExtendedKeyUsage()
        {
            IList list2;
            Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.37"));
            if (extensionValue == null)
            {
                return null;
            }
            try
            {
                Asn1Sequence instance = Asn1Sequence.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
                IList list = Platform.CreateArrayList();
                IEnumerator enumerator = instance.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                        list.Add(current.Id);
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
                list2 = list;
            }
            catch (Exception exception)
            {
                throw new CertificateParsingException("error processing extended key usage extension", exception);
            }
            return list2;
        }

        public override int GetHashCode()
        {
            object obj2 = this;
            lock (obj2)
            {
                if (!this.hashValueSet)
                {
                    this.hashValue = this.c.GetHashCode();
                    this.hashValueSet = true;
                }
            }
            return this.hashValue;
        }

        public virtual ICollection GetIssuerAlternativeNames() => 
            this.GetAlternativeNames("2.5.29.18");

        public virtual bool[] GetKeyUsage() => 
            ((this.keyUsage != null) ? ((bool[]) this.keyUsage.Clone()) : null);

        public virtual AsymmetricKeyParameter GetPublicKey() => 
            PublicKeyFactory.CreateKey(this.c.SubjectPublicKeyInfo);

        public virtual byte[] GetSigAlgParams()
        {
            if (this.c.SignatureAlgorithm.Parameters != null)
            {
                return this.c.SignatureAlgorithm.Parameters.GetDerEncoded();
            }
            return null;
        }

        public virtual byte[] GetSignature() => 
            this.c.GetSignatureOctets();

        public virtual ICollection GetSubjectAlternativeNames() => 
            this.GetAlternativeNames("2.5.29.17");

        public virtual byte[] GetTbsCertificate() => 
            this.c.TbsCertificate.GetDerEncoded();

        protected override X509Extensions GetX509Extensions() => 
            ((this.c.Version < 3) ? null : this.c.TbsCertificate.Extensions);

        private static bool IsAlgIDEqual(AlgorithmIdentifier id1, AlgorithmIdentifier id2)
        {
            if (!id1.Algorithm.Equals(id2.Algorithm))
            {
                return false;
            }
            Asn1Encodable parameters = id1.Parameters;
            Asn1Encodable objB = id2.Parameters;
            if ((parameters == null) == (objB == null))
            {
                return object.Equals(parameters, objB);
            }
            return ((parameters != null) ? (parameters.ToAsn1Object() is Asn1Null) : (objB.ToAsn1Object() is Asn1Null));
        }

        public virtual bool IsValid(DateTime time) => 
            ((time.CompareTo(this.NotBefore) >= 0) && (time.CompareTo(this.NotAfter) <= 0));

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            string newLine = Platform.NewLine;
            builder.Append("  [0]         Version: ").Append(this.Version).Append(newLine);
            builder.Append("         SerialNumber: ").Append(this.SerialNumber).Append(newLine);
            builder.Append("             IssuerDN: ").Append(this.IssuerDN).Append(newLine);
            builder.Append("           Start Date: ").Append(this.NotBefore).Append(newLine);
            builder.Append("           Final Date: ").Append(this.NotAfter).Append(newLine);
            builder.Append("            SubjectDN: ").Append(this.SubjectDN).Append(newLine);
            builder.Append("           Public Key: ").Append(this.GetPublicKey()).Append(newLine);
            builder.Append("  Signature Algorithm: ").Append(this.SigAlgName).Append(newLine);
            byte[] signature = this.GetSignature();
            builder.Append("            Signature: ").Append(Hex.ToHexString(signature, 0, 20)).Append(newLine);
            for (int i = 20; i < signature.Length; i += 20)
            {
                int length = Math.Min(20, signature.Length - i);
                builder.Append("                       ").Append(Hex.ToHexString(signature, i, length)).Append(newLine);
            }
            X509Extensions extensions = this.c.TbsCertificate.Extensions;
            if (extensions != null)
            {
                IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    builder.Append("       Extensions: \n");
                }
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
                            if (current.Equals(X509Extensions.BasicConstraints))
                            {
                                builder.Append(BasicConstraints.GetInstance(obj2));
                            }
                            else if (current.Equals(X509Extensions.KeyUsage))
                            {
                                builder.Append(KeyUsage.GetInstance(obj2));
                            }
                            else if (current.Equals(MiscObjectIdentifiers.NetscapeCertType))
                            {
                                builder.Append(new NetscapeCertType((DerBitString) obj2));
                            }
                            else if (current.Equals(MiscObjectIdentifiers.NetscapeRevocationUrl))
                            {
                                builder.Append(new NetscapeRevocationUrl((DerIA5String) obj2));
                            }
                            else if (current.Equals(MiscObjectIdentifiers.VerisignCzagExtension))
                            {
                                builder.Append(new VerisignCzagExtension((DerIA5String) obj2));
                            }
                            else
                            {
                                builder.Append(current.Id);
                                builder.Append(" value = ").Append(Asn1Dump.DumpAsString((Asn1Encodable) obj2));
                            }
                        }
                        catch (Exception)
                        {
                            builder.Append(current.Id);
                            builder.Append(" value = ").Append("*****");
                        }
                    }
                    builder.Append(newLine);
                }
                while (enumerator.MoveNext());
            }
            return builder.ToString();
        }

        public virtual void Verify(AsymmetricKeyParameter key)
        {
            this.CheckSignature(new Asn1VerifierFactory(this.c.SignatureAlgorithm, key));
        }

        public virtual void Verify(IVerifierFactoryProvider verifierProvider)
        {
            this.CheckSignature(verifierProvider.CreateVerifierFactory(this.c.SignatureAlgorithm));
        }

        public virtual X509CertificateStructure CertificateStructure =>
            this.c;

        public virtual bool IsValidNow =>
            this.IsValid(DateTime.UtcNow);

        public virtual int Version =>
            this.c.Version;

        public virtual BigInteger SerialNumber =>
            this.c.SerialNumber.Value;

        public virtual X509Name IssuerDN =>
            this.c.Issuer;

        public virtual X509Name SubjectDN =>
            this.c.Subject;

        public virtual DateTime NotBefore =>
            this.c.StartDate.ToDateTime();

        public virtual DateTime NotAfter =>
            this.c.EndDate.ToDateTime();

        public virtual string SigAlgName =>
            SignerUtilities.GetEncodingName(this.c.SignatureAlgorithm.Algorithm);

        public virtual string SigAlgOid =>
            this.c.SignatureAlgorithm.Algorithm.Id;

        public virtual DerBitString IssuerUniqueID =>
            this.c.TbsCertificate.IssuerUniqueID;

        public virtual DerBitString SubjectUniqueID =>
            this.c.TbsCertificate.SubjectUniqueID;
    }
}

