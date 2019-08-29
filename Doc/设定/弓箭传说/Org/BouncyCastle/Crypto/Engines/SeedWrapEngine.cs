namespace Org.BouncyCastle.Crypto.Engines
{
    using System;

    public class SeedWrapEngine : Rfc3394WrapEngine
    {
        public SeedWrapEngine() : base(new SeedEngine())
        {
        }
    }
}

