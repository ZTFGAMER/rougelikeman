namespace Org.BouncyCastle.Asn1.Misc
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class NetscapeRevocationUrl : DerIA5String
    {
        public NetscapeRevocationUrl(DerIA5String str) : base(str.GetString())
        {
        }

        public override string ToString() => 
            ("NetscapeRevocationUrl: " + this.GetString());
    }
}

