namespace Org.BouncyCastle.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Utilities.Collections;
    using System;
    using System.Collections;

    public abstract class X509ExtensionBase : IX509Extension
    {
        protected X509ExtensionBase()
        {
        }

        public virtual ISet GetCriticalExtensionOids() => 
            this.GetExtensionOids(true);

        protected virtual ISet GetExtensionOids(bool critical)
        {
            X509Extensions extensions = this.GetX509Extensions();
            if (extensions == null)
            {
                return null;
            }
            HashSet set = new HashSet();
            IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    if (extensions.GetExtension(current).IsCritical == critical)
                    {
                        set.Add(current.Id);
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
            return set;
        }

        public virtual Asn1OctetString GetExtensionValue(DerObjectIdentifier oid)
        {
            X509Extensions extensions = this.GetX509Extensions();
            if (extensions != null)
            {
                X509Extension extension = extensions.GetExtension(oid);
                if (extension != null)
                {
                    return extension.Value;
                }
            }
            return null;
        }

        [Obsolete("Use version taking a DerObjectIdentifier instead")]
        public Asn1OctetString GetExtensionValue(string oid) => 
            this.GetExtensionValue(new DerObjectIdentifier(oid));

        public virtual ISet GetNonCriticalExtensionOids() => 
            this.GetExtensionOids(false);

        protected abstract X509Extensions GetX509Extensions();
    }
}

