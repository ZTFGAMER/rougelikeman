namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class ECBasisType
    {
        public const byte ec_basis_trinomial = 1;
        public const byte ec_basis_pentanomial = 2;

        protected ECBasisType()
        {
        }

        public static bool IsValid(byte ecBasisType) => 
            ((ecBasisType >= 1) && (ecBasisType <= 2));
    }
}

