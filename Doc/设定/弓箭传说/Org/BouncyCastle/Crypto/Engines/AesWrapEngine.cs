namespace Org.BouncyCastle.Crypto.Engines
{
    using System;

    public class AesWrapEngine : Rfc3394WrapEngine
    {
        public AesWrapEngine() : base(new AesEngine())
        {
        }
    }
}

