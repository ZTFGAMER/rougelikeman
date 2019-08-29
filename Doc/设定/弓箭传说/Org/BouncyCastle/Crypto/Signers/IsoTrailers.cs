namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Collections;
    using System;
    using System.Collections;

    public class IsoTrailers
    {
        public const int TRAILER_IMPLICIT = 0xbc;
        public const int TRAILER_RIPEMD160 = 0x31cc;
        public const int TRAILER_RIPEMD128 = 0x32cc;
        public const int TRAILER_SHA1 = 0x33cc;
        public const int TRAILER_SHA256 = 0x34cc;
        public const int TRAILER_SHA512 = 0x35cc;
        public const int TRAILER_SHA384 = 0x36cc;
        public const int TRAILER_WHIRLPOOL = 0x37cc;
        public const int TRAILER_SHA224 = 0x38cc;
        public const int TRAILER_SHA512_224 = 0x39cc;
        public const int TRAILER_SHA512_256 = 0x40cc;
        private static readonly IDictionary trailerMap = CreateTrailerMap();

        private static IDictionary CreateTrailerMap()
        {
            IDictionary d = Platform.CreateHashtable();
            d.Add("RIPEMD128", 0x32cc);
            d.Add("RIPEMD160", 0x31cc);
            d.Add("SHA-1", 0x33cc);
            d.Add("SHA-224", 0x38cc);
            d.Add("SHA-256", 0x34cc);
            d.Add("SHA-384", 0x36cc);
            d.Add("SHA-512", 0x35cc);
            d.Add("SHA-512/224", 0x39cc);
            d.Add("SHA-512/256", 0x40cc);
            d.Add("Whirlpool", 0x37cc);
            return CollectionUtilities.ReadOnly(d);
        }

        public static int GetTrailer(IDigest digest) => 
            ((int) trailerMap[digest.AlgorithmName]);

        public static bool NoTrailerAvailable(IDigest digest) => 
            !trailerMap.Contains(digest.AlgorithmName);
    }
}

