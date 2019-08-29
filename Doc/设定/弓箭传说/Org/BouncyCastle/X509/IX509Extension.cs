namespace Org.BouncyCastle.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities.Collections;
    using System;

    public interface IX509Extension
    {
        ISet GetCriticalExtensionOids();
        Asn1OctetString GetExtensionValue(DerObjectIdentifier oid);
        [Obsolete("Use version taking a DerObjectIdentifier instead")]
        Asn1OctetString GetExtensionValue(string oid);
        ISet GetNonCriticalExtensionOids();
    }
}

