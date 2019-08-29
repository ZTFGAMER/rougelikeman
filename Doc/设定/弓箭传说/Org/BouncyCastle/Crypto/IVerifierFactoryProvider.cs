namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IVerifierFactoryProvider
    {
        IVerifierFactory CreateVerifierFactory(object algorithmDetails);
    }
}

