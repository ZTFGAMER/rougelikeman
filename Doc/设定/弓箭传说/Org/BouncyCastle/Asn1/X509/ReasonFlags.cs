namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class ReasonFlags : DerBitString
    {
        public const int Unused = 0x80;
        public const int KeyCompromise = 0x40;
        public const int CACompromise = 0x20;
        public const int AffiliationChanged = 0x10;
        public const int Superseded = 8;
        public const int CessationOfOperation = 4;
        public const int CertificateHold = 2;
        public const int PrivilegeWithdrawn = 1;
        public const int AACompromise = 0x8000;

        public ReasonFlags(DerBitString reasons) : base(reasons.GetBytes(), reasons.PadBits)
        {
        }

        public ReasonFlags(int reasons) : base(reasons)
        {
        }
    }
}

