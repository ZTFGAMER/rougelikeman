namespace Org.BouncyCastle.Crypto.Parameters
{
    using System;

    public class RC5Parameters : KeyParameter
    {
        private readonly int rounds;

        public RC5Parameters(byte[] key, int rounds) : base(key)
        {
            if (key.Length > 0xff)
            {
                throw new ArgumentException("RC5 key length can be no greater than 255");
            }
            this.rounds = rounds;
        }

        public int Rounds =>
            this.rounds;
    }
}

