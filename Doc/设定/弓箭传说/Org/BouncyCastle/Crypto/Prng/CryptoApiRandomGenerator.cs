﻿namespace Org.BouncyCastle.Crypto.Prng
{
    using System;
    using System.Security.Cryptography;

    public class CryptoApiRandomGenerator : IRandomGenerator
    {
        private readonly RandomNumberGenerator rndProv;

        public CryptoApiRandomGenerator() : this(new RNGCryptoServiceProvider())
        {
        }

        public CryptoApiRandomGenerator(RandomNumberGenerator rng)
        {
            this.rndProv = rng;
        }

        public virtual void AddSeedMaterial(byte[] seed)
        {
        }

        public virtual void AddSeedMaterial(long seed)
        {
        }

        public virtual void NextBytes(byte[] bytes)
        {
            this.rndProv.GetBytes(bytes);
        }

        public virtual void NextBytes(byte[] bytes, int start, int len)
        {
            if (start < 0)
            {
                throw new ArgumentException("Start offset cannot be negative", "start");
            }
            if (bytes.Length < (start + len))
            {
                throw new ArgumentException("Byte array too small for requested offset and length");
            }
            if ((bytes.Length == len) && (start == 0))
            {
                this.NextBytes(bytes);
            }
            else
            {
                byte[] buffer = new byte[len];
                this.NextBytes(buffer);
                Array.Copy(buffer, 0, bytes, start, len);
            }
        }
    }
}

