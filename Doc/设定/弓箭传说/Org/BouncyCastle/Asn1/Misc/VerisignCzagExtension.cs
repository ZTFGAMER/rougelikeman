namespace Org.BouncyCastle.Asn1.Misc
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class VerisignCzagExtension : DerIA5String
    {
        public VerisignCzagExtension(DerIA5String str) : base(str.GetString())
        {
        }

        public override string ToString() => 
            ("VerisignCzagExtension: " + this.GetString());
    }
}

