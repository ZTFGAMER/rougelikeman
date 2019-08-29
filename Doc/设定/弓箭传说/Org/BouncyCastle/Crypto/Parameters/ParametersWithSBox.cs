namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class ParametersWithSBox : ICipherParameters
    {
        private ICipherParameters parameters;
        private byte[] sBox;

        public ParametersWithSBox(ICipherParameters parameters, byte[] sBox)
        {
            this.parameters = parameters;
            this.sBox = sBox;
        }

        public byte[] GetSBox() => 
            this.sBox;

        public ICipherParameters Parameters =>
            this.parameters;
    }
}

