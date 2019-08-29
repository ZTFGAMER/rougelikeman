namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IVerifierFactory
    {
        IStreamCalculator CreateCalculator();

        object AlgorithmDetails { get; }
    }
}

