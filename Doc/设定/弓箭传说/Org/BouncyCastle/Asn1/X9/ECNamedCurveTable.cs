namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Anssi;
    using Org.BouncyCastle.Asn1.Nist;
    using Org.BouncyCastle.Asn1.Sec;
    using Org.BouncyCastle.Asn1.TeleTrust;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Collections;
    using System;
    using System.Collections;

    public class ECNamedCurveTable
    {
        public static X9ECParameters GetByName(string name)
        {
            X9ECParameters byName = X962NamedCurves.GetByName(name);
            if (byName == null)
            {
                byName = SecNamedCurves.GetByName(name);
            }
            if (byName == null)
            {
                byName = NistNamedCurves.GetByName(name);
            }
            if (byName == null)
            {
                byName = TeleTrusTNamedCurves.GetByName(name);
            }
            if (byName == null)
            {
                byName = AnssiNamedCurves.GetByName(name);
            }
            return byName;
        }

        public static X9ECParameters GetByOid(DerObjectIdentifier oid)
        {
            X9ECParameters byOid = X962NamedCurves.GetByOid(oid);
            if (byOid == null)
            {
                byOid = SecNamedCurves.GetByOid(oid);
            }
            if (byOid == null)
            {
                byOid = TeleTrusTNamedCurves.GetByOid(oid);
            }
            if (byOid == null)
            {
                byOid = AnssiNamedCurves.GetByOid(oid);
            }
            return byOid;
        }

        public static string GetName(DerObjectIdentifier oid)
        {
            string name = X962NamedCurves.GetName(oid);
            if (name == null)
            {
                name = SecNamedCurves.GetName(oid);
            }
            if (name == null)
            {
                name = NistNamedCurves.GetName(oid);
            }
            if (name == null)
            {
                name = TeleTrusTNamedCurves.GetName(oid);
            }
            if (name == null)
            {
                name = AnssiNamedCurves.GetName(oid);
            }
            return name;
        }

        public static DerObjectIdentifier GetOid(string name)
        {
            DerObjectIdentifier oid = X962NamedCurves.GetOid(name);
            if (oid == null)
            {
                oid = SecNamedCurves.GetOid(name);
            }
            if (oid == null)
            {
                oid = NistNamedCurves.GetOid(name);
            }
            if (oid == null)
            {
                oid = TeleTrusTNamedCurves.GetOid(name);
            }
            if (oid == null)
            {
                oid = AnssiNamedCurves.GetOid(name);
            }
            return oid;
        }

        public static IEnumerable Names
        {
            get
            {
                IList to = Platform.CreateArrayList();
                CollectionUtilities.AddRange(to, X962NamedCurves.Names);
                CollectionUtilities.AddRange(to, SecNamedCurves.Names);
                CollectionUtilities.AddRange(to, NistNamedCurves.Names);
                CollectionUtilities.AddRange(to, TeleTrusTNamedCurves.Names);
                CollectionUtilities.AddRange(to, AnssiNamedCurves.Names);
                return to;
            }
        }
    }
}

