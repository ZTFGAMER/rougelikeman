namespace Org.BouncyCastle.Asn1
{
    using System;

    public abstract class DerStringBase : Asn1Object, IAsn1String
    {
        protected DerStringBase()
        {
        }

        protected override int Asn1GetHashCode() => 
            this.GetString().GetHashCode();

        public abstract string GetString();
        public override string ToString() => 
            this.GetString();
    }
}

