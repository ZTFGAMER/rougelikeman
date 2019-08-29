namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RC4Engine : IStreamCipher
    {
        private static readonly int STATE_LENGTH = 0x100;
        private byte[] engineState;
        private int x;
        private int y;
        private byte[] workingKey;

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("invalid parameter passed to RC4 init - " + Platform.GetTypeName(parameters));
            }
            this.workingKey = ((KeyParameter) parameters).GetKey();
            this.SetKey(this.workingKey);
        }

        public virtual void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            Check.DataLength(input, inOff, length, "input buffer too short");
            Check.OutputLength(output, outOff, length, "output buffer too short");
            for (int i = 0; i < length; i++)
            {
                this.x = (this.x + 1) & 0xff;
                this.y = (this.engineState[this.x] + this.y) & 0xff;
                byte num2 = this.engineState[this.x];
                this.engineState[this.x] = this.engineState[this.y];
                this.engineState[this.y] = num2;
                output[i + outOff] = (byte) (input[i + inOff] ^ this.engineState[(this.engineState[this.x] + this.engineState[this.y]) & 0xff]);
            }
        }

        public virtual void Reset()
        {
            this.SetKey(this.workingKey);
        }

        public virtual byte ReturnByte(byte input)
        {
            this.x = (this.x + 1) & 0xff;
            this.y = (this.engineState[this.x] + this.y) & 0xff;
            byte num = this.engineState[this.x];
            this.engineState[this.x] = this.engineState[this.y];
            this.engineState[this.y] = num;
            return (byte) (input ^ this.engineState[(this.engineState[this.x] + this.engineState[this.y]) & 0xff]);
        }

        private void SetKey(byte[] keyBytes)
        {
            this.workingKey = keyBytes;
            this.x = 0;
            this.y = 0;
            if (this.engineState == null)
            {
                this.engineState = new byte[STATE_LENGTH];
            }
            for (int i = 0; i < STATE_LENGTH; i++)
            {
                this.engineState[i] = (byte) i;
            }
            int index = 0;
            int num3 = 0;
            for (int j = 0; j < STATE_LENGTH; j++)
            {
                num3 = (((keyBytes[index] & 0xff) + this.engineState[j]) + num3) & 0xff;
                byte num5 = this.engineState[j];
                this.engineState[j] = this.engineState[num3];
                this.engineState[num3] = num5;
                index = (index + 1) % keyBytes.Length;
            }
        }

        public virtual string AlgorithmName =>
            "RC4";
    }
}

