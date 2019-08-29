namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class X509Extension
    {
        internal bool critical;
        internal Asn1OctetString value;

        public X509Extension(DerBoolean critical, Asn1OctetString value)
        {
            if (critical == null)
            {
                throw new ArgumentNullException("critical");
            }
            this.critical = critical.IsTrue;
            this.value = value;
        }

        public X509Extension(bool critical, Asn1OctetString value)
        {
            this.critical = critical;
            this.value = value;
        }

        public static Asn1Object ConvertValueToObject(X509Extension ext)
        {
            Asn1Object obj2;
            try
            {
                obj2 = Asn1Object.FromByteArray(ext.Value.GetOctets());
            }
            catch (Exception exception)
            {
                throw new ArgumentException("can't convert extension", exception);
            }
            return obj2;
        }

        public override bool Equals(object obj)
        {
            X509Extension extension = obj as X509Extension;
            if (extension == null)
            {
                return false;
            }
            return (this.Value.Equals(extension.Value) && (this.IsCritical == extension.IsCritical));
        }

        public override int GetHashCode()
        {
            int hashCode = this.Value.GetHashCode();
            return (!this.IsCritical ? ~hashCode : hashCode);
        }

        public Asn1Encodable GetParsedValue() => 
            ConvertValueToObject(this);

        public bool IsCritical =>
            this.critical;

        public Asn1OctetString Value =>
            this.value;
    }
}

