namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface ISignatureFactory
    {
        IStreamCalculator CreateCalculator();

        object AlgorithmDetails { get; }
    }
}

