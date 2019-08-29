namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DesEdeEngine : DesEngine
    {
        private int[] workingKey1;
        private int[] workingKey2;
        private int[] workingKey3;
        private bool forEncryption;

        public override int GetBlockSize() => 
            8;

        public override void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("invalid parameter passed to DESede init - " + Platform.GetTypeName(parameters));
            }
            byte[] key = ((KeyParameter) parameters).GetKey();
            if ((key.Length != 0x18) && (key.Length != 0x10))
            {
                throw new ArgumentException("key size must be 16 or 24 bytes.");
            }
            this.forEncryption = forEncryption;
            byte[] destinationArray = new byte[8];
            Array.Copy(key, 0, destinationArray, 0, destinationArray.Length);
            this.workingKey1 = DesEngine.GenerateWorkingKey(forEncryption, destinationArray);
            byte[] buffer3 = new byte[8];
            Array.Copy(key, 8, buffer3, 0, buffer3.Length);
            this.workingKey2 = DesEngine.GenerateWorkingKey(!forEncryption, buffer3);
            if (key.Length == 0x18)
            {
                byte[] buffer4 = new byte[8];
                Array.Copy(key, 0x10, buffer4, 0, buffer4.Length);
                this.workingKey3 = DesEngine.GenerateWorkingKey(forEncryption, buffer4);
            }
            else
            {
                this.workingKey3 = this.workingKey1;
            }
        }

        public override int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (this.workingKey1 == null)
            {
                throw new InvalidOperationException("DESede engine not initialised");
            }
            Check.DataLength(input, inOff, 8, "input buffer too short");
            Check.OutputLength(output, outOff, 8, "output buffer too short");
            byte[] outBytes = new byte[8];
            if (this.forEncryption)
            {
                DesEngine.DesFunc(this.workingKey1, input, inOff, outBytes, 0);
                DesEngine.DesFunc(this.workingKey2, outBytes, 0, outBytes, 0);
                DesEngine.DesFunc(this.workingKey3, outBytes, 0, output, outOff);
            }
            else
            {
                DesEngine.DesFunc(this.workingKey3, input, inOff, outBytes, 0);
                DesEngine.DesFunc(this.workingKey2, outBytes, 0, outBytes, 0);
                DesEngine.DesFunc(this.workingKey1, outBytes, 0, output, outOff);
            }
            return 8;
        }

        public override void Reset()
        {
        }

        public override string AlgorithmName =>
            "DESede";
    }
}

