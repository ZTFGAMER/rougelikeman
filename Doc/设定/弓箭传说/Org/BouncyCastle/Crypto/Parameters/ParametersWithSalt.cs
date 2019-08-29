namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class ParametersWithSalt : ICipherParameters
    {
        private byte[] salt;
        private ICipherParameters parameters;

        public ParametersWithSalt(ICipherParameters parameters, byte[] salt) : this(parameters, salt, 0, salt.Length)
        {
        }

        public ParametersWithSalt(ICipherParameters parameters, byte[] salt, int saltOff, int saltLen)
        {
            this.salt = new byte[saltLen];
            this.parameters = parameters;
            Array.Copy(salt, saltOff, this.salt, 0, saltLen);
        }

        public byte[] GetSalt() => 
            this.salt;

        public ICipherParameters Parameters =>
            this.parameters;
    }
}

