namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Gost28147Mac : IMac
    {
        private const int blockSize = 8;
        private const int macSize = 4;
        private int bufOff = 0;
        private byte[] buf = new byte[8];
        private byte[] mac = new byte[8];
        private bool firstStep = true;
        private int[] workingKey;
        private byte[] S = new byte[] { 
            9, 6, 3, 2, 8, 11, 1, 7, 10, 4, 14, 15, 12, 0, 13, 5,
            3, 7, 14, 9, 8, 10, 15, 0, 5, 2, 6, 12, 11, 4, 13, 1,
            14, 4, 6, 2, 11, 3, 13, 8, 12, 15, 5, 10, 0, 7, 1, 9,
            14, 7, 10, 12, 13, 1, 3, 9, 0, 2, 11, 4, 15, 8, 5, 6,
            11, 5, 1, 9, 8, 13, 15, 0, 14, 4, 2, 3, 12, 7, 10, 6,
            3, 10, 13, 12, 1, 2, 0, 11, 7, 5, 9, 4, 8, 15, 14, 6,
            1, 13, 2, 9, 7, 10, 6, 0, 8, 12, 4, 5, 15, 3, 11, 14,
            11, 10, 15, 5, 0, 12, 14, 8, 6, 2, 3, 9, 1, 7, 13, 4
        };

        public void BlockUpdate(byte[] input, int inOff, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("Can't have a negative input length!");
            }
            int length = 8 - this.bufOff;
            if (len > length)
            {
                Array.Copy(input, inOff, this.buf, this.bufOff, length);
                byte[] destinationArray = new byte[this.buf.Length];
                Array.Copy(this.buf, 0, destinationArray, 0, this.mac.Length);
                if (this.firstStep)
                {
                    this.firstStep = false;
                }
                else
                {
                    destinationArray = CM5func(this.buf, 0, this.mac);
                }
                this.gost28147MacFunc(this.workingKey, destinationArray, 0, this.mac, 0);
                this.bufOff = 0;
                len -= length;
                inOff += length;
                while (len > 8)
                {
                    destinationArray = CM5func(input, inOff, this.mac);
                    this.gost28147MacFunc(this.workingKey, destinationArray, 0, this.mac, 0);
                    len -= 8;
                    inOff += 8;
                }
            }
            Array.Copy(input, inOff, this.buf, this.bufOff, len);
            this.bufOff += len;
        }

        private static int bytesToint(byte[] input, int inOff) => 
            (((((input[inOff + 3] << 0x18) & 0xff000000L) + ((input[inOff + 2] << 0x10) & 0xff0000)) + ((input[inOff + 1] << 8) & 0xff00)) + (input[inOff] & 0xff));

        private static byte[] CM5func(byte[] buf, int bufOff, byte[] mac)
        {
            byte[] destinationArray = new byte[buf.Length - bufOff];
            Array.Copy(buf, bufOff, destinationArray, 0, mac.Length);
            for (int i = 0; i != mac.Length; i++)
            {
                destinationArray[i] = (byte) (destinationArray[i] ^ mac[i]);
            }
            return destinationArray;
        }

        public int DoFinal(byte[] output, int outOff)
        {
            while (this.bufOff < 8)
            {
                this.buf[this.bufOff++] = 0;
            }
            byte[] destinationArray = new byte[this.buf.Length];
            Array.Copy(this.buf, 0, destinationArray, 0, this.mac.Length);
            if (this.firstStep)
            {
                this.firstStep = false;
            }
            else
            {
                destinationArray = CM5func(this.buf, 0, this.mac);
            }
            this.gost28147MacFunc(this.workingKey, destinationArray, 0, this.mac, 0);
            Array.Copy(this.mac, (this.mac.Length / 2) - 4, output, outOff, 4);
            this.Reset();
            return 4;
        }

        private static int[] generateWorkingKey(byte[] userKey)
        {
            if (userKey.Length != 0x20)
            {
                throw new ArgumentException("Key length invalid. Key needs to be 32 byte - 256 bit!!!");
            }
            int[] numArray = new int[8];
            for (int i = 0; i != 8; i++)
            {
                numArray[i] = bytesToint(userKey, i * 4);
            }
            return numArray;
        }

        public int GetMacSize() => 
            4;

        private int gost28147_mainStep(int n1, int key)
        {
            int num = key + n1;
            int num2 = this.S[(num >> 0) & 15] << 0;
            num2 += this.S[0x10 + ((num >> 4) & 15)] << 4;
            num2 += this.S[0x20 + ((num >> 8) & 15)] << 8;
            num2 += this.S[0x30 + ((num >> 12) & 15)] << 12;
            num2 += this.S[0x40 + ((num >> 0x10) & 15)] << 0x10;
            num2 += this.S[80 + ((num >> 20) & 15)] << 20;
            num2 += this.S[0x60 + ((num >> 0x18) & 15)] << 0x18;
            num2 += this.S[0x70 + ((num >> 0x1c) & 15)] << 0x1c;
            int num3 = num2 << 11;
            int num4 = num2 >> 0x15;
            return (num3 | num4);
        }

        private void gost28147MacFunc(int[] workingKey, byte[] input, int inOff, byte[] output, int outOff)
        {
            int num = bytesToint(input, inOff);
            int num2 = bytesToint(input, inOff + 4);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int num3 = num;
                    num = num2 ^ this.gost28147_mainStep(num, workingKey[j]);
                    num2 = num3;
                }
            }
            intTobytes(num, output, outOff);
            intTobytes(num2, output, outOff + 4);
        }

        public void Init(ICipherParameters parameters)
        {
            this.Reset();
            this.buf = new byte[8];
            if (parameters is ParametersWithSBox)
            {
                ParametersWithSBox box = (ParametersWithSBox) parameters;
                box.GetSBox().CopyTo(this.S, 0);
                if (box.Parameters != null)
                {
                    this.workingKey = generateWorkingKey(((KeyParameter) box.Parameters).GetKey());
                }
            }
            else
            {
                if (!(parameters is KeyParameter))
                {
                    throw new ArgumentException("invalid parameter passed to Gost28147 init - " + Platform.GetTypeName(parameters));
                }
                this.workingKey = generateWorkingKey(((KeyParameter) parameters).GetKey());
            }
        }

        private static void intTobytes(int num, byte[] output, int outOff)
        {
            output[outOff + 3] = (byte) (num >> 0x18);
            output[outOff + 2] = (byte) (num >> 0x10);
            output[outOff + 1] = (byte) (num >> 8);
            output[outOff] = (byte) num;
        }

        public void Reset()
        {
            Array.Clear(this.buf, 0, this.buf.Length);
            this.bufOff = 0;
            this.firstStep = true;
        }

        public void Update(byte input)
        {
            if (this.bufOff == this.buf.Length)
            {
                byte[] destinationArray = new byte[this.buf.Length];
                Array.Copy(this.buf, 0, destinationArray, 0, this.mac.Length);
                if (this.firstStep)
                {
                    this.firstStep = false;
                }
                else
                {
                    destinationArray = CM5func(this.buf, 0, this.mac);
                }
                this.gost28147MacFunc(this.workingKey, destinationArray, 0, this.mac, 0);
                this.bufOff = 0;
            }
            this.buf[this.bufOff++] = input;
        }

        public string AlgorithmName =>
            "Gost28147Mac";
    }
}

