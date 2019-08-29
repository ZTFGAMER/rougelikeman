namespace Org.BouncyCastle.Asn1
{
    using System;

    public abstract class Asn1Null : Asn1Object
    {
        internal Asn1Null()
        {
        }

        public override string ToString() => 
            "NULL";
    }
}

