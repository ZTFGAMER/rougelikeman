namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public class Gost28147Engine : IBlockCipher
    {
        private const int BlockSize = 8;
        private int[] workingKey;
        private bool forEncryption;
        private byte[] S = Sbox_Default;
        private static readonly byte[] Sbox_Default = new byte[] { 
            4, 10, 9, 2, 13, 8, 0, 14, 6, 11, 1, 12, 7, 15, 5, 3,
            14, 11, 4, 12, 6, 13, 15, 10, 2, 3, 8, 1, 0, 7, 5, 9,
            5, 8, 1, 13, 10, 3, 4, 2, 14, 15, 12, 7, 6, 0, 9, 11,
            7, 13, 10, 1, 0, 8, 9, 15, 14, 4, 6, 12, 11, 2, 5, 3,
            6, 12, 7, 1, 5, 15, 13, 8, 4, 10, 9, 14, 0, 3, 11, 2,
            4, 11, 10, 0, 7, 2, 1, 13, 3, 6, 8, 5, 9, 12, 15, 14,
            13, 11, 4, 1, 3, 15, 5, 9, 0, 10, 14, 7, 6, 8, 2, 12,
            1, 15, 13, 0, 5, 7, 10, 4, 9, 2, 3, 14, 6, 11, 8, 12
        };
        private static readonly byte[] ESbox_Test = new byte[] { 
            4, 2, 15, 5, 9, 1, 0, 8, 14, 3, 11, 12, 13, 7, 10, 6,
            12, 9, 15, 14, 8, 1, 3, 10, 2, 7, 4, 13, 6, 0, 11, 5,
            13, 8, 14, 12, 7, 3, 9, 10, 1, 5, 2, 4, 6, 15, 0, 11,
            14, 9, 11, 2, 5, 15, 7, 1, 0, 13, 12, 6, 10, 4, 3, 8,
            3, 14, 5, 9, 6, 8, 0, 13, 10, 11, 7, 12, 2, 1, 15, 4,
            8, 15, 6, 11, 1, 9, 12, 5, 13, 3, 7, 10, 0, 14, 2, 4,
            9, 11, 12, 0, 3, 6, 7, 5, 4, 8, 14, 15, 1, 10, 2, 13,
            12, 6, 5, 2, 11, 0, 9, 13, 3, 14, 7, 10, 15, 4, 1, 8
        };
        private static readonly byte[] ESbox_A = new byte[] { 
            9, 6, 3, 2, 8, 11, 1, 7, 10, 4, 14, 15, 12, 0, 13, 5,
            3, 7, 14, 9, 8, 10, 15, 0, 5, 2, 6, 12, 11, 4, 13, 1,
            14, 4, 6, 2, 11, 3, 13, 8, 12, 15, 5, 10, 0, 7, 1, 9,
            14, 7, 10, 12, 13, 1, 3, 9, 0, 2, 11, 4, 15, 8, 5, 6,
            11, 5, 1, 9, 8, 13, 15, 0, 14, 4, 2, 3, 12, 7, 10, 6,
            3, 10, 13, 12, 1, 2, 0, 11, 7, 5, 9, 4, 8, 15, 14, 6,
            1, 13, 2, 9, 7, 10, 6, 0, 8, 12, 4, 5, 15, 3, 11, 14,
            11, 10, 15, 5, 0, 12, 14, 8, 6, 2, 3, 9, 1, 7, 13, 4
        };
        private static readonly byte[] ESbox_B = new byte[] { 
            8, 4, 11, 1, 3, 5, 0, 9, 2, 14, 10, 12, 13, 6, 7, 15,
            0, 1, 2, 10, 4, 13, 5, 12, 9, 7, 3, 15, 11, 8, 6, 14,
            14, 12, 0, 10, 9, 2, 13, 11, 7, 5, 8, 15, 3, 6, 1, 4,
            7, 5, 0, 13, 11, 6, 1, 2, 3, 10, 12, 15, 4, 14, 9, 8,
            2, 7, 12, 15, 9, 5, 10, 11, 1, 4, 0, 13, 6, 8, 14, 3,
            8, 3, 2, 6, 4, 13, 14, 11, 12, 1, 7, 15, 10, 0, 9, 5,
            5, 2, 10, 11, 9, 1, 12, 3, 7, 4, 13, 0, 6, 15, 8, 14,
            0, 4, 11, 14, 8, 3, 7, 1, 10, 2, 9, 6, 15, 13, 5, 12
        };
        private static readonly byte[] ESbox_C = new byte[] { 
            1, 11, 12, 2, 9, 13, 0, 15, 4, 5, 8, 14, 10, 7, 6, 3,
            0, 1, 7, 13, 11, 4, 5, 2, 8, 14, 15, 12, 9, 10, 6, 3,
            8, 2, 5, 0, 4, 9, 15, 10, 3, 7, 12, 13, 6, 14, 1, 11,
            3, 6, 0, 1, 5, 13, 10, 8, 11, 2, 9, 7, 14, 15, 12, 4,
            8, 13, 11, 0, 4, 5, 1, 2, 9, 3, 12, 14, 6, 15, 10, 7,
            12, 9, 11, 1, 8, 14, 2, 4, 7, 3, 6, 5, 10, 0, 15, 13,
            10, 9, 6, 8, 13, 14, 2, 0, 15, 3, 5, 11, 4, 1, 12, 7,
            7, 4, 0, 5, 10, 2, 15, 14, 12, 6, 1, 11, 13, 9, 3, 8
        };
        private static readonly byte[] ESbox_D = new byte[] { 
            15, 12, 2, 10, 6, 4, 5, 0, 7, 9, 14, 13, 1, 11, 8, 3,
            11, 6, 3, 4, 12, 15, 14, 2, 7, 13, 8, 0, 5, 10, 9, 1,
            1, 12, 11, 0, 15, 14, 6, 5, 10, 13, 4, 8, 9, 3, 7, 2,
            1, 5, 14, 12, 10, 7, 0, 13, 6, 2, 11, 4, 9, 3, 15, 8,
            0, 12, 8, 9, 13, 2, 10, 11, 7, 3, 6, 5, 4, 14, 15, 1,
            8, 0, 15, 3, 2, 5, 14, 11, 1, 10, 4, 7, 12, 9, 13, 6,
            3, 0, 6, 15, 1, 14, 9, 2, 13, 8, 12, 4, 11, 10, 5, 7,
            1, 10, 6, 8, 15, 11, 0, 4, 12, 3, 5, 9, 7, 13, 2, 14
        };
        private static readonly byte[] DSbox_Test = new byte[] { 
            4, 10, 9, 2, 13, 8, 0, 14, 6, 11, 1, 12, 7, 15, 5, 3,
            14, 11, 4, 12, 6, 13, 15, 10, 2, 3, 8, 1, 0, 7, 5, 9,
            5, 8, 1, 13, 10, 3, 4, 2, 14, 15, 12, 7, 6, 0, 9, 11,
            7, 13, 10, 1, 0, 8, 9, 15, 14, 4, 6, 12, 11, 2, 5, 3,
            6, 12, 7, 1, 5, 15, 13, 8, 4, 10, 9, 14, 0, 3, 11, 2,
            4, 11, 10, 0, 7, 2, 1, 13, 3, 6, 8, 5, 9, 12, 15, 14,
            13, 11, 4, 1, 3, 15, 5, 9, 0, 10, 14, 7, 6, 8, 2, 12,
            1, 15, 13, 0, 5, 7, 10, 4, 9, 2, 3, 14, 6, 11, 8, 12
        };
        private static readonly byte[] DSbox_A = new byte[] { 
            10, 4, 5, 6, 8, 1, 3, 7, 13, 12, 14, 0, 9, 2, 11, 15,
            5, 15, 4, 0, 2, 13, 11, 9, 1, 7, 6, 3, 12, 14, 10, 8,
            7, 15, 12, 14, 9, 4, 1, 0, 3, 11, 5, 2, 6, 10, 8, 13,
            4, 10, 7, 12, 0, 15, 2, 8, 14, 1, 6, 5, 13, 11, 9, 3,
            7, 6, 4, 11, 9, 12, 2, 10, 1, 8, 0, 14, 15, 13, 3, 5,
            7, 6, 2, 4, 13, 9, 15, 0, 10, 1, 5, 11, 8, 14, 12, 3,
            13, 14, 4, 1, 7, 0, 5, 10, 3, 12, 8, 15, 6, 2, 9, 11,
            1, 3, 10, 9, 5, 11, 4, 15, 8, 6, 7, 14, 13, 0, 2, 12
        };
        private static readonly IDictionary sBoxes = Platform.CreateHashtable();

        static Gost28147Engine()
        {
            AddSBox("Default", Sbox_Default);
            AddSBox("E-TEST", ESbox_Test);
            AddSBox("E-A", ESbox_A);
            AddSBox("E-B", ESbox_B);
            AddSBox("E-C", ESbox_C);
            AddSBox("E-D", ESbox_D);
            AddSBox("D-TEST", DSbox_Test);
            AddSBox("D-A", DSbox_A);
        }

        private static void AddSBox(string sBoxName, byte[] sBox)
        {
            sBoxes.Add(Platform.ToUpperInvariant(sBoxName), sBox);
        }

        private static int bytesToint(byte[] inBytes, int inOff) => 
            (((((inBytes[inOff + 3] << 0x18) & 0xff000000L) + ((inBytes[inOff + 2] << 0x10) & 0xff0000)) + ((inBytes[inOff + 1] << 8) & 0xff00)) + (inBytes[inOff] & 0xff));

        private int[] generateWorkingKey(bool forEncryption, byte[] userKey)
        {
            this.forEncryption = forEncryption;
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

        public virtual int GetBlockSize() => 
            8;

        public static byte[] GetSBox(string sBoxName)
        {
            byte[] data = (byte[]) sBoxes[Platform.ToUpperInvariant(sBoxName)];
            if (data == null)
            {
                throw new ArgumentException("Unknown S-Box - possible types: \"Default\", \"E-Test\", \"E-A\", \"E-B\", \"E-C\", \"E-D\", \"D-Test\", \"D-A\".");
            }
            return Arrays.Clone(data);
        }

        private int Gost28147_mainStep(int n1, int key)
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

        private void Gost28147Func(int[] workingKey, byte[] inBytes, int inOff, byte[] outBytes, int outOff)
        {
            int num3;
            int num = bytesToint(inBytes, inOff);
            int num2 = bytesToint(inBytes, inOff + 4);
            if (this.forEncryption)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        num3 = num;
                        int num6 = this.Gost28147_mainStep(num, workingKey[k]);
                        num = num2 ^ num6;
                        num2 = num3;
                    }
                }
                for (int j = 7; j > 0; j--)
                {
                    num3 = num;
                    num = num2 ^ this.Gost28147_mainStep(num, workingKey[j]);
                    num2 = num3;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    num3 = num;
                    num = num2 ^ this.Gost28147_mainStep(num, workingKey[i]);
                    num2 = num3;
                }
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 7; k >= 0; k--)
                    {
                        if ((j == 2) && (k == 0))
                        {
                            break;
                        }
                        num3 = num;
                        num = num2 ^ this.Gost28147_mainStep(num, workingKey[k]);
                        num2 = num3;
                    }
                }
            }
            num2 ^= this.Gost28147_mainStep(num, workingKey[0]);
            intTobytes(num, outBytes, outOff);
            intTobytes(num2, outBytes, outOff + 4);
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (parameters is ParametersWithSBox)
            {
                ParametersWithSBox box = (ParametersWithSBox) parameters;
                byte[] sBox = box.GetSBox();
                if (sBox.Length != Sbox_Default.Length)
                {
                    throw new ArgumentException("invalid S-box passed to GOST28147 init");
                }
                this.S = Arrays.Clone(sBox);
                if (box.Parameters != null)
                {
                    this.workingKey = this.generateWorkingKey(forEncryption, ((KeyParameter) box.Parameters).GetKey());
                }
            }
            else if (parameters is KeyParameter)
            {
                this.workingKey = this.generateWorkingKey(forEncryption, ((KeyParameter) parameters).GetKey());
            }
            else if (parameters != null)
            {
                throw new ArgumentException("invalid parameter passed to Gost28147 init - " + Platform.GetTypeName(parameters));
            }
        }

        private static void intTobytes(int num, byte[] outBytes, int outOff)
        {
            outBytes[outOff + 3] = (byte) (num >> 0x18);
            outBytes[outOff + 2] = (byte) (num >> 0x10);
            outBytes[outOff + 1] = (byte) (num >> 8);
            outBytes[outOff] = (byte) num;
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (this.workingKey == null)
            {
                throw new InvalidOperationException("Gost28147 engine not initialised");
            }
            Check.DataLength(input, inOff, 8, "input buffer too short");
            Check.OutputLength(output, outOff, 8, "output buffer too short");
            this.Gost28147Func(this.workingKey, input, inOff, output, outOff);
            return 8;
        }

        public virtual void Reset()
        {
        }

        public virtual string AlgorithmName =>
            "Gost28147";

        public virtual bool IsPartialBlockOkay =>
            false;
    }
}

