namespace Org.BouncyCastle.Crypto
{
    using System;
    using System.IO;

    public interface IStreamCalculator
    {
        object GetResult();

        System.IO.Stream Stream { get; }
    }
}

