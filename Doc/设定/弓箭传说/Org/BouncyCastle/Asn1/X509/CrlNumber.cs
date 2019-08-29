namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using System;

    public class CrlNumber : DerInteger
    {
        public CrlNumber(BigInteger number) : base(number)
        {
        }

        public override string ToString() => 
            ("CRLNumber: " + this.Number);

        public BigInteger Number =>
            base.PositiveValue;
    }
}

