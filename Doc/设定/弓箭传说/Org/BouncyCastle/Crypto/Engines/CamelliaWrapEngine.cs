namespace Org.BouncyCastle.Crypto.Engines
{
    using System;

    public class CamelliaWrapEngine : Rfc3394WrapEngine
    {
        public CamelliaWrapEngine() : base(new CamelliaEngine())
        {
        }
    }
}

