namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Collections;
    using System;
    using System.Collections;

    public class X509Extensions : Asn1Encodable
    {
        public static readonly DerObjectIdentifier SubjectDirectoryAttributes = new DerObjectIdentifier("2.5.29.9");
        public static readonly DerObjectIdentifier SubjectKeyIdentifier = new DerObjectIdentifier("2.5.29.14");
        public static readonly DerObjectIdentifier KeyUsage = new DerObjectIdentifier("2.5.29.15");
        public static readonly DerObjectIdentifier PrivateKeyUsagePeriod = new DerObjectIdentifier("2.5.29.16");
        public static readonly DerObjectIdentifier SubjectAlternativeName = new DerObjectIdentifier("2.5.29.17");
        public static readonly DerObjectIdentifier IssuerAlternativeName = new DerObjectIdentifier("2.5.29.18");
        public static readonly DerObjectIdentifier BasicConstraints = new DerObjectIdentifier("2.5.29.19");
        public static readonly DerObjectIdentifier CrlNumber = new DerObjectIdentifier("2.5.29.20");
        public static readonly DerObjectIdentifier ReasonCode = new DerObjectIdentifier("2.5.29.21");
        public static readonly DerObjectIdentifier InstructionCode = new DerObjectIdentifier("2.5.29.23");
        public static readonly DerObjectIdentifier InvalidityDate = new DerObjectIdentifier("2.5.29.24");
        public static readonly DerObjectIdentifier DeltaCrlIndicator = new DerObjectIdentifier("2.5.29.27");
        public static readonly DerObjectIdentifier IssuingDistributionPoint = new DerObjectIdentifier("2.5.29.28");
        public static readonly DerObjectIdentifier CertificateIssuer = new DerObjectIdentifier("2.5.29.29");
        public static readonly DerObjectIdentifier NameConstraints = new DerObjectIdentifier("2.5.29.30");
        public static readonly DerObjectIdentifier CrlDistributionPoints = new DerObjectIdentifier("2.5.29.31");
        public static readonly DerObjectIdentifier CertificatePolicies = new DerObjectIdentifier("2.5.29.32");
        public static readonly DerObjectIdentifier PolicyMappings = new DerObjectIdentifier("2.5.29.33");
        public static readonly DerObjectIdentifier AuthorityKeyIdentifier = new DerObjectIdentifier("2.5.29.35");
        public static readonly DerObjectIdentifier PolicyConstraints = new DerObjectIdentifier("2.5.29.36");
        public static readonly DerObjectIdentifier ExtendedKeyUsage = new DerObjectIdentifier("2.5.29.37");
        public static readonly DerObjectIdentifier FreshestCrl = new DerObjectIdentifier("2.5.29.46");
        public static readonly DerObjectIdentifier InhibitAnyPolicy = new DerObjectIdentifier("2.5.29.54");
        public static readonly DerObjectIdentifier AuthorityInfoAccess = new DerObjectIdentifier("1.3.6.1.5.5.7.1.1");
        public static readonly DerObjectIdentifier SubjectInfoAccess = new DerObjectIdentifier("1.3.6.1.5.5.7.1.11");
        public static readonly DerObjectIdentifier LogoType = new DerObjectIdentifier("1.3.6.1.5.5.7.1.12");
        public static readonly DerObjectIdentifier BiometricInfo = new DerObjectIdentifier("1.3.6.1.5.5.7.1.2");
        public static readonly DerObjectIdentifier QCStatements = new DerObjectIdentifier("1.3.6.1.5.5.7.1.3");
        public static readonly DerObjectIdentifier AuditIdentity = new DerObjectIdentifier("1.3.6.1.5.5.7.1.4");
        public static readonly DerObjectIdentifier NoRevAvail = new DerObjectIdentifier("2.5.29.56");
        public static readonly DerObjectIdentifier TargetInformation = new DerObjectIdentifier("2.5.29.55");
        private readonly IDictionary extensions;
        private readonly IList ordering;

        private X509Extensions(Asn1Sequence seq)
        {
            this.extensions = Platform.CreateHashtable();
            this.ordering = Platform.CreateArrayList();
            IEnumerator enumerator = seq.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Asn1Sequence instance = Asn1Sequence.GetInstance(((Asn1Encodable) enumerator.Current).ToAsn1Object());
                    if ((instance.Count < 2) || (instance.Count > 3))
                    {
                        throw new ArgumentException("Bad sequence size: " + instance.Count);
                    }
                    DerObjectIdentifier key = DerObjectIdentifier.GetInstance(instance[0].ToAsn1Object());
                    bool critical = (instance.Count == 3) && DerBoolean.GetInstance(instance[1].ToAsn1Object()).IsTrue;
                    Asn1OctetString str = Asn1OctetString.GetInstance(instance[instance.Count - 1].ToAsn1Object());
                    this.extensions.Add(key, new X509Extension(critical, str));
                    this.ordering.Add(key);
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

        [Obsolete]
        public X509Extensions(Hashtable extensions) : this(null, extensions)
        {
        }

        public X509Extensions(IDictionary extensions) : this(null, extensions)
        {
        }

        [Obsolete]
        public X509Extensions(ArrayList oids, ArrayList values)
        {
            this.extensions = Platform.CreateHashtable();
            this.ordering = Platform.CreateArrayList((ICollection) oids);
            int num = 0;
            IEnumerator enumerator = this.ordering.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    this.extensions.Add(current, (X509Extension) values[num++]);
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

        [Obsolete]
        public X509Extensions(ArrayList ordering, Hashtable extensions)
        {
            this.extensions = Platform.CreateHashtable();
            if (ordering == null)
            {
                this.ordering = Platform.CreateArrayList(extensions.Keys);
            }
            else
            {
                this.ordering = Platform.CreateArrayList((ICollection) ordering);
            }
            IEnumerator enumerator = this.ordering.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    this.extensions.Add(current, (X509Extension) extensions[current]);
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

        public X509Extensions(IList ordering, IDictionary extensions)
        {
            this.extensions = Platform.CreateHashtable();
            if (ordering == null)
            {
                this.ordering = Platform.CreateArrayList(extensions.Keys);
            }
            else
            {
                this.ordering = Platform.CreateArrayList((ICollection) ordering);
            }
            IEnumerator enumerator = this.ordering.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    this.extensions.Add(current, (X509Extension) extensions[current]);
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

        public X509Extensions(IList oids, IList values)
        {
            this.extensions = Platform.CreateHashtable();
            this.ordering = Platform.CreateArrayList((ICollection) oids);
            int num = 0;
            IEnumerator enumerator = this.ordering.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    this.extensions.Add(current, (X509Extension) values[num++]);
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

        public bool Equivalent(X509Extensions other)
        {
            if (this.extensions.Count != other.extensions.Count)
            {
                return false;
            }
            IEnumerator enumerator = this.extensions.Keys.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    if (!this.extensions[current].Equals(other.extensions[current]))
                    {
                        return false;
                    }
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
            return true;
        }

        public DerObjectIdentifier[] GetCriticalExtensionOids() => 
            this.GetExtensionOids(true);

        public X509Extension GetExtension(DerObjectIdentifier oid) => 
            ((X509Extension) this.extensions[oid]);

        public DerObjectIdentifier[] GetExtensionOids() => 
            ToOidArray(this.ordering);

        private DerObjectIdentifier[] GetExtensionOids(bool isCritical)
        {
            IList oids = Platform.CreateArrayList();
            IEnumerator enumerator = this.ordering.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    X509Extension extension = (X509Extension) this.extensions[current];
                    if (extension.IsCritical == isCritical)
                    {
                        oids.Add(current);
                    }
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
            return ToOidArray(oids);
        }

        public static X509Extensions GetInstance(object obj)
        {
            if ((obj == null) || (obj is X509Extensions))
            {
                return (X509Extensions) obj;
            }
            if (obj is Asn1Sequence)
            {
                return new X509Extensions((Asn1Sequence) obj);
            }
            if (!(obj is Asn1TaggedObject))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return GetInstance(((Asn1TaggedObject) obj).GetObject());
        }

        public static X509Extensions GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public DerObjectIdentifier[] GetNonCriticalExtensionOids() => 
            this.GetExtensionOids(false);

        [Obsolete("Use ExtensionOids IEnumerable property")]
        public IEnumerator Oids() => 
            this.ExtensionOids.GetEnumerator();

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            IEnumerator enumerator = this.ordering.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    X509Extension extension = (X509Extension) this.extensions[current];
                    Asn1Encodable[] encodableArray1 = new Asn1Encodable[] { current };
                    Asn1EncodableVector vector2 = new Asn1EncodableVector(encodableArray1);
                    if (extension.IsCritical)
                    {
                        Asn1Encodable[] encodableArray2 = new Asn1Encodable[] { DerBoolean.True };
                        vector2.Add(encodableArray2);
                    }
                    Asn1Encodable[] objs = new Asn1Encodable[] { extension.Value };
                    vector2.Add(objs);
                    Asn1Encodable[] encodableArray4 = new Asn1Encodable[] { new DerSequence(vector2) };
                    v.Add(encodableArray4);
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
            return new DerSequence(v);
        }

        private static DerObjectIdentifier[] ToOidArray(IList oids)
        {
            DerObjectIdentifier[] array = new DerObjectIdentifier[oids.Count];
            oids.CopyTo(array, 0);
            return array;
        }

        public IEnumerable ExtensionOids =>
            new EnumerableProxy(this.ordering);
    }
}

