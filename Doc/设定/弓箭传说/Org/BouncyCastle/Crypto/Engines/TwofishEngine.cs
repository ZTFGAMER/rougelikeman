namespace Org.BouncyCastle.Crypto.Engines
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public sealed class TwofishEngine : IBlockCipher
    {
        private static readonly byte[,] P = new byte[,] { { 
            0xa9, 0x67, 0xb3, 0xe8, 4, 0xfd, 0xa3, 0x76, 0x9a, 0x92, 0x80, 120, 0xe4, 0xdd, 0xd1, 0x38,
            13, 0xc6, 0x35, 0x98, 0x18, 0xf7, 0xec, 0x6c, 0x43, 0x75, 0x37, 0x26, 250, 0x13, 0x94, 0x48,
            0xf2, 0xd0, 0x8b, 0x30, 0x84, 0x54, 0xdf, 0x23, 0x19, 0x5b, 0x3d, 0x59, 0xf3, 0xae, 0xa2, 130,
            0x63, 1, 0x83, 0x2e, 0xd9, 0x51, 0x9b, 0x7c, 0xa6, 0xeb, 0xa5, 190, 0x16, 12, 0xe3, 0x61,
            0xc0, 140, 0x3a, 0xf5, 0x73, 0x2c, 0x25, 11, 0xbb, 0x4e, 0x89, 0x6b, 0x53, 0x6a, 180, 0xf1,
            0xe1, 230, 0xbd, 0x45, 0xe2, 0xf4, 0xb6, 0x66, 0xcc, 0x95, 3, 0x56, 0xd4, 0x1c, 30, 0xd7,
            0xfb, 0xc3, 0x8e, 0xb5, 0xe9, 0xcf, 0xbf, 0xba, 0xea, 0x77, 0x39, 0xaf, 0x33, 0xc9, 0x62, 0x71,
            0x81, 0x79, 9, 0xad, 0x24, 0xcd, 0xf9, 0xd8, 0xe5, 0xc5, 0xb9, 0x4d, 0x44, 8, 0x86, 0xe7,
            0xa1, 0x1d, 170, 0xed, 6, 0x70, 0xb2, 210, 0x41, 0x7b, 160, 0x11, 0x31, 0xc2, 0x27, 0x90,
            0x20, 0xf6, 0x60, 0xff, 150, 0x5c, 0xb1, 0xab, 0x9e, 0x9c, 0x52, 0x1b, 0x5f, 0x93, 10, 0xef,
            0x91, 0x85, 0x49, 0xee, 0x2d, 0x4f, 0x8f, 0x3b, 0x47, 0x87, 0x6d, 70, 0xd6, 0x3e, 0x69, 100,
            0x2a, 0xce, 0xcb, 0x2f, 0xfc, 0x97, 5, 0x7a, 0xac, 0x7f, 0xd5, 0x1a, 0x4b, 14, 0xa7, 90,
            40, 20, 0x3f, 0x29, 0x88, 60, 0x4c, 2, 0xb8, 0xda, 0xb0, 0x17, 0x55, 0x1f, 0x8a, 0x7d,
            0x57, 0xc7, 0x8d, 0x74, 0xb7, 0xc4, 0x9f, 0x72, 0x7e, 0x15, 0x22, 0x12, 0x58, 7, 0x99, 0x34,
            110, 80, 0xde, 0x68, 0x65, 0xbc, 0xdb, 0xf8, 200, 0xa8, 0x2b, 0x40, 220, 0xfe, 50, 0xa4,
            0xca, 0x10, 0x21, 240, 0xd3, 0x5d, 15, 0, 0x6f, 0x9d, 0x36, 0x42, 0x4a, 0x5e, 0xc1, 0xe0
        }, { 
            0x75, 0xf3, 0xc6, 0xf4, 0xdb, 0x7b, 0xfb, 200, 0x4a, 0xd3, 230, 0x6b, 0x45, 0x7d, 0xe8, 0x4b,
            0xd6, 50, 0xd8, 0xfd, 0x37, 0x71, 0xf1, 0xe1, 0x30, 15, 0xf8, 0x1b, 0x87, 250, 6, 0x3f,
            0x5e, 0xba, 0xae, 0x5b, 0x8a, 0, 0xbc, 0x9d, 0x6d, 0xc1, 0xb1, 14, 0x80, 0x5d, 210, 0xd5,
            160, 0x84, 7, 20, 0xb5, 0x90, 0x2c, 0xa3, 0xb2, 0x73, 0x4c, 0x54, 0x92, 0x74, 0x36, 0x51,
            0x38, 0xb0, 0xbd, 90, 0xfc, 0x60, 0x62, 150, 0x6c, 0x42, 0xf7, 0x10, 0x7c, 40, 0x27, 140,
            0x13, 0x95, 0x9c, 0xc7, 0x24, 70, 0x3b, 0x70, 0xca, 0xe3, 0x85, 0xcb, 0x11, 0xd0, 0x93, 0xb8,
            0xa6, 0x83, 0x20, 0xff, 0x9f, 0x77, 0xc3, 0xcc, 3, 0x6f, 8, 0xbf, 0x40, 0xe7, 0x2b, 0xe2,
            0x79, 12, 170, 130, 0x41, 0x3a, 0xea, 0xb9, 0xe4, 0x9a, 0xa4, 0x97, 0x7e, 0xda, 0x7a, 0x17,
            0x66, 0x94, 0xa1, 0x1d, 0x3d, 240, 0xde, 0xb3, 11, 0x72, 0xa7, 0x1c, 0xef, 0xd1, 0x53, 0x3e,
            0x8f, 0x33, 0x26, 0x5f, 0xec, 0x76, 0x2a, 0x49, 0x81, 0x88, 0xee, 0x21, 0xc4, 0x1a, 0xeb, 0xd9,
            0xc5, 0x39, 0x99, 0xcd, 0xad, 0x31, 0x8b, 1, 0x18, 0x23, 0xdd, 0x1f, 0x4e, 0x2d, 0xf9, 0x48,
            0x4f, 0xf2, 0x65, 0x8e, 120, 0x5c, 0x58, 0x19, 0x8d, 0xe5, 0x98, 0x57, 0x67, 0x7f, 5, 100,
            0xaf, 0x63, 0xb6, 0xfe, 0xf5, 0xb7, 60, 0xa5, 0xce, 0xe9, 0x68, 0x44, 0xe0, 0x4d, 0x43, 0x69,
            0x29, 0x2e, 0xac, 0x15, 0x59, 0xa8, 10, 0x9e, 110, 0x47, 0xdf, 0x34, 0x35, 0x6a, 0xcf, 220,
            0x22, 0xc9, 0xc0, 0x9b, 0x89, 0xd4, 0xed, 0xab, 0x12, 0xa2, 13, 0x52, 0xbb, 2, 0x2f, 0xa9,
            0xd7, 0x61, 30, 180, 80, 4, 0xf6, 0xc2, 0x16, 0x25, 0x86, 0x56, 0x55, 9, 190, 0x91
        } };
        private const int P_00 = 1;
        private const int P_01 = 0;
        private const int P_02 = 0;
        private const int P_03 = 1;
        private const int P_04 = 1;
        private const int P_10 = 0;
        private const int P_11 = 0;
        private const int P_12 = 1;
        private const int P_13 = 1;
        private const int P_14 = 0;
        private const int P_20 = 1;
        private const int P_21 = 1;
        private const int P_22 = 0;
        private const int P_23 = 0;
        private const int P_24 = 0;
        private const int P_30 = 0;
        private const int P_31 = 1;
        private const int P_32 = 1;
        private const int P_33 = 0;
        private const int P_34 = 1;
        private const int GF256_FDBK = 0x169;
        private const int GF256_FDBK_2 = 180;
        private const int GF256_FDBK_4 = 90;
        private const int RS_GF_FDBK = 0x14d;
        private const int ROUNDS = 0x10;
        private const int MAX_ROUNDS = 0x10;
        private const int BLOCK_SIZE = 0x10;
        private const int MAX_KEY_BITS = 0x100;
        private const int INPUT_WHITEN = 0;
        private const int OUTPUT_WHITEN = 4;
        private const int ROUND_SUBKEYS = 8;
        private const int TOTAL_SUBKEYS = 40;
        private const int SK_STEP = 0x2020202;
        private const int SK_BUMP = 0x1010101;
        private const int SK_ROTL = 9;
        private bool encrypting;
        private int[] gMDS0 = new int[0x100];
        private int[] gMDS1 = new int[0x100];
        private int[] gMDS2 = new int[0x100];
        private int[] gMDS3 = new int[0x100];
        private int[] gSubKeys;
        private int[] gSBox;
        private int k64Cnt;
        private byte[] workingKey;

        public TwofishEngine()
        {
            int[] numArray = new int[2];
            int[] numArray2 = new int[2];
            int[] numArray3 = new int[2];
            for (int i = 0; i < 0x100; i++)
            {
                int x = P[0, i] & 0xff;
                numArray[0] = x;
                numArray2[0] = this.Mx_X(x) & 0xff;
                numArray3[0] = this.Mx_Y(x) & 0xff;
                x = P[1, i] & 0xff;
                numArray[1] = x;
                numArray2[1] = this.Mx_X(x) & 0xff;
                numArray3[1] = this.Mx_Y(x) & 0xff;
                this.gMDS0[i] = ((numArray[1] | (numArray2[1] << 8)) | (numArray3[1] << 0x10)) | (numArray3[1] << 0x18);
                this.gMDS1[i] = ((numArray3[0] | (numArray3[0] << 8)) | (numArray2[0] << 0x10)) | (numArray[0] << 0x18);
                this.gMDS2[i] = ((numArray2[1] | (numArray3[1] << 8)) | (numArray[1] << 0x10)) | (numArray3[1] << 0x18);
                this.gMDS3[i] = ((numArray2[0] | (numArray[0] << 8)) | (numArray3[0] << 0x10)) | (numArray2[0] << 0x18);
            }
        }

        private void Bits32ToBytes(int inData, byte[] b, int offset)
        {
            b[offset] = (byte) inData;
            b[offset + 1] = (byte) (inData >> 8);
            b[offset + 2] = (byte) (inData >> 0x10);
            b[offset + 3] = (byte) (inData >> 0x18);
        }

        private int BytesTo32Bits(byte[] b, int p) => 
            ((((b[p] & 0xff) | ((b[p + 1] & 0xff) << 8)) | ((b[p + 2] & 0xff) << 0x10)) | ((b[p + 3] & 0xff) << 0x18));

        private void DecryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
        {
            int x = this.BytesTo32Bits(src, srcIndex) ^ this.gSubKeys[4];
            int num2 = this.BytesTo32Bits(src, srcIndex + 4) ^ this.gSubKeys[5];
            int num3 = this.BytesTo32Bits(src, srcIndex + 8) ^ this.gSubKeys[6];
            int num4 = this.BytesTo32Bits(src, srcIndex + 12) ^ this.gSubKeys[7];
            int num5 = 0x27;
            for (int i = 0; i < 0x10; i += 2)
            {
                int num6 = this.Fe32_0(x);
                int num7 = this.Fe32_3(num2);
                num4 ^= (num6 + (2 * num7)) + this.gSubKeys[num5--];
                num3 = ((num3 << 1) | (num3 >> 0x1f)) ^ ((num6 + num7) + this.gSubKeys[num5--]);
                num4 = (num4 >> 1) | (num4 << 0x1f);
                num6 = this.Fe32_0(num3);
                num7 = this.Fe32_3(num4);
                num2 ^= (num6 + (2 * num7)) + this.gSubKeys[num5--];
                x = ((x << 1) | (x >> 0x1f)) ^ ((num6 + num7) + this.gSubKeys[num5--]);
                num2 = (num2 >> 1) | (num2 << 0x1f);
            }
            this.Bits32ToBytes(num3 ^ this.gSubKeys[0], dst, dstIndex);
            this.Bits32ToBytes(num4 ^ this.gSubKeys[1], dst, dstIndex + 4);
            this.Bits32ToBytes(x ^ this.gSubKeys[2], dst, dstIndex + 8);
            this.Bits32ToBytes(num2 ^ this.gSubKeys[3], dst, dstIndex + 12);
        }

        private void EncryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
        {
            int x = this.BytesTo32Bits(src, srcIndex) ^ this.gSubKeys[0];
            int num2 = this.BytesTo32Bits(src, srcIndex + 4) ^ this.gSubKeys[1];
            int num3 = this.BytesTo32Bits(src, srcIndex + 8) ^ this.gSubKeys[2];
            int num4 = this.BytesTo32Bits(src, srcIndex + 12) ^ this.gSubKeys[3];
            int num5 = 8;
            for (int i = 0; i < 0x10; i += 2)
            {
                int num6 = this.Fe32_0(x);
                int num7 = this.Fe32_3(num2);
                num3 ^= (num6 + num7) + this.gSubKeys[num5++];
                num3 = (num3 >> 1) | (num3 << 0x1f);
                num4 = ((num4 << 1) | (num4 >> 0x1f)) ^ ((num6 + (2 * num7)) + this.gSubKeys[num5++]);
                num6 = this.Fe32_0(num3);
                num7 = this.Fe32_3(num4);
                x ^= (num6 + num7) + this.gSubKeys[num5++];
                x = (x >> 1) | (x << 0x1f);
                num2 = ((num2 << 1) | (num2 >> 0x1f)) ^ ((num6 + (2 * num7)) + this.gSubKeys[num5++]);
            }
            this.Bits32ToBytes(num3 ^ this.gSubKeys[4], dst, dstIndex);
            this.Bits32ToBytes(num4 ^ this.gSubKeys[5], dst, dstIndex + 4);
            this.Bits32ToBytes(x ^ this.gSubKeys[6], dst, dstIndex + 8);
            this.Bits32ToBytes(num2 ^ this.gSubKeys[7], dst, dstIndex + 12);
        }

        private int F32(int x, int[] k32)
        {
            int num = this.M_b0(x);
            int num2 = this.M_b1(x);
            int num3 = this.M_b2(x);
            int num4 = this.M_b3(x);
            int num5 = k32[0];
            int num6 = k32[1];
            int num7 = k32[2];
            int num8 = k32[3];
            int num9 = 0;
            switch ((this.k64Cnt & 3))
            {
                case 0:
                    num = (P[1, num] & 0xff) ^ this.M_b0(num8);
                    num2 = (P[0, num2] & 0xff) ^ this.M_b1(num8);
                    num3 = (P[0, num3] & 0xff) ^ this.M_b2(num8);
                    num4 = (P[1, num4] & 0xff) ^ this.M_b3(num8);
                    break;

                case 1:
                    return (((this.gMDS0[(P[0, num] & 0xff) ^ this.M_b0(num5)] ^ this.gMDS1[(P[0, num2] & 0xff) ^ this.M_b1(num5)]) ^ this.gMDS2[(P[1, num3] & 0xff) ^ this.M_b2(num5)]) ^ this.gMDS3[(P[1, num4] & 0xff) ^ this.M_b3(num5)]);

                case 2:
                    goto Label_01D9;

                case 3:
                    break;

                default:
                    return num9;
            }
            num = (P[1, num] & 0xff) ^ this.M_b0(num7);
            num2 = (P[1, num2] & 0xff) ^ this.M_b1(num7);
            num3 = (P[0, num3] & 0xff) ^ this.M_b2(num7);
            num4 = (P[0, num4] & 0xff) ^ this.M_b3(num7);
        Label_01D9:
            return (((this.gMDS0[(P[0, (P[0, num] & 0xff) ^ this.M_b0(num6)] & 0xff) ^ this.M_b0(num5)] ^ this.gMDS1[(P[0, (P[1, num2] & 0xff) ^ this.M_b1(num6)] & 0xff) ^ this.M_b1(num5)]) ^ this.gMDS2[(P[1, (P[0, num3] & 0xff) ^ this.M_b2(num6)] & 0xff) ^ this.M_b2(num5)]) ^ this.gMDS3[(P[1, (P[1, num4] & 0xff) ^ this.M_b3(num6)] & 0xff) ^ this.M_b3(num5)]);
        }

        private int Fe32_0(int x) => 
            (((this.gSBox[2 * (x & 0xff)] ^ this.gSBox[1 + (2 * ((x >> 8) & 0xff))]) ^ this.gSBox[0x200 + (2 * ((x >> 0x10) & 0xff))]) ^ this.gSBox[0x201 + (2 * ((x >> 0x18) & 0xff))]);

        private int Fe32_3(int x) => 
            (((this.gSBox[2 * ((x >> 0x18) & 0xff)] ^ this.gSBox[1 + (2 * (x & 0xff))]) ^ this.gSBox[0x200 + (2 * ((x >> 8) & 0xff))]) ^ this.gSBox[0x201 + (2 * ((x >> 0x10) & 0xff))]);

        public int GetBlockSize() => 
            0x10;

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
            {
                throw new ArgumentException("invalid parameter passed to Twofish init - " + Platform.GetTypeName(parameters));
            }
            this.encrypting = forEncryption;
            this.workingKey = ((KeyParameter) parameters).GetKey();
            this.k64Cnt = this.workingKey.Length / 8;
            this.SetKey(this.workingKey);
        }

        private int LFSR1(int x) => 
            ((x >> 1) ^ (((x & 1) == 0) ? 0 : 180));

        private int LFSR2(int x) => 
            (((x >> 2) ^ (((x & 2) == 0) ? 0 : 180)) ^ (((x & 1) == 0) ? 0 : 90));

        private int M_b0(int x) => 
            (x & 0xff);

        private int M_b1(int x) => 
            ((x >> 8) & 0xff);

        private int M_b2(int x) => 
            ((x >> 0x10) & 0xff);

        private int M_b3(int x) => 
            ((x >> 0x18) & 0xff);

        private int Mx_X(int x) => 
            (x ^ this.LFSR2(x));

        private int Mx_Y(int x) => 
            ((x ^ this.LFSR1(x)) ^ this.LFSR2(x));

        public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if (this.workingKey == null)
            {
                throw new InvalidOperationException("Twofish not initialised");
            }
            Check.DataLength(input, inOff, 0x10, "input buffer too short");
            Check.OutputLength(output, outOff, 0x10, "output buffer too short");
            if (this.encrypting)
            {
                this.EncryptBlock(input, inOff, output, outOff);
            }
            else
            {
                this.DecryptBlock(input, inOff, output, outOff);
            }
            return 0x10;
        }

        public void Reset()
        {
            if (this.workingKey != null)
            {
                this.SetKey(this.workingKey);
            }
        }

        private int RS_MDS_Encode(int k0, int k1)
        {
            int x = k1;
            for (int i = 0; i < 4; i++)
            {
                x = this.RS_rem(x);
            }
            x ^= k0;
            for (int j = 0; j < 4; j++)
            {
                x = this.RS_rem(x);
            }
            return x;
        }

        private int RS_rem(int x)
        {
            int num = (x >> 0x18) & 0xff;
            int num2 = ((num << 1) ^ (((num & 0x80) == 0) ? 0 : 0x14d)) & 0xff;
            int num3 = ((num >> 1) ^ (((num & 1) == 0) ? 0 : 0xa6)) ^ num2;
            return (((((x << 8) ^ (num3 << 0x18)) ^ (num2 << 0x10)) ^ (num3 << 8)) ^ num);
        }

        private void SetKey(byte[] key)
        {
            int[] numArray = new int[4];
            int[] numArray2 = new int[4];
            int[] numArray3 = new int[4];
            this.gSubKeys = new int[40];
            if (this.k64Cnt < 1)
            {
                throw new ArgumentException("Key size less than 64 bits");
            }
            if (this.k64Cnt > 4)
            {
                throw new ArgumentException("Key size larger than 256 bits");
            }
            int index = 0;
            int p = 0;
            while (index < this.k64Cnt)
            {
                p = index * 8;
                numArray[index] = this.BytesTo32Bits(key, p);
                numArray2[index] = this.BytesTo32Bits(key, p + 4);
                numArray3[(this.k64Cnt - 1) - index] = this.RS_MDS_Encode(numArray[index], numArray2[index]);
                index++;
            }
            for (int i = 0; i < 20; i++)
            {
                int num3 = i * 0x2020202;
                int num4 = this.F32(num3, numArray);
                int num5 = this.F32(num3 + 0x1010101, numArray2);
                num5 = (num5 << 8) | (num5 >> 0x18);
                num4 += num5;
                this.gSubKeys[i * 2] = num4;
                num4 += num5;
                this.gSubKeys[(i * 2) + 1] = (num4 << 9) | (num4 >> 0x17);
            }
            int x = numArray3[0];
            int num8 = numArray3[1];
            int num9 = numArray3[2];
            int num10 = numArray3[3];
            this.gSBox = new int[0x400];
            for (int j = 0; j < 0x100; j++)
            {
                int num12;
                int num13;
                int num14;
                int num11 = num12 = num13 = num14 = j;
                switch ((this.k64Cnt & 3))
                {
                    case 0:
                        num11 = (P[1, num11] & 0xff) ^ this.M_b0(num10);
                        num12 = (P[0, num12] & 0xff) ^ this.M_b1(num10);
                        num13 = (P[0, num13] & 0xff) ^ this.M_b2(num10);
                        num14 = (P[1, num14] & 0xff) ^ this.M_b3(num10);
                        break;

                    case 1:
                    {
                        this.gSBox[j * 2] = this.gMDS0[(P[0, num11] & 0xff) ^ this.M_b0(x)];
                        this.gSBox[(j * 2) + 1] = this.gMDS1[(P[0, num12] & 0xff) ^ this.M_b1(x)];
                        this.gSBox[(j * 2) + 0x200] = this.gMDS2[(P[1, num13] & 0xff) ^ this.M_b2(x)];
                        this.gSBox[(j * 2) + 0x201] = this.gMDS3[(P[1, num14] & 0xff) ^ this.M_b3(x)];
                        continue;
                    }
                    case 2:
                        goto Label_0341;

                    case 3:
                        break;

                    default:
                    {
                        continue;
                    }
                }
                num11 = (P[1, num11] & 0xff) ^ this.M_b0(num9);
                num12 = (P[1, num12] & 0xff) ^ this.M_b1(num9);
                num13 = (P[0, num13] & 0xff) ^ this.M_b2(num9);
                num14 = (P[0, num14] & 0xff) ^ this.M_b3(num9);
            Label_0341:
                this.gSBox[j * 2] = this.gMDS0[(P[0, (P[0, num11] & 0xff) ^ this.M_b0(num8)] & 0xff) ^ this.M_b0(x)];
                this.gSBox[(j * 2) + 1] = this.gMDS1[(P[0, (P[1, num12] & 0xff) ^ this.M_b1(num8)] & 0xff) ^ this.M_b1(x)];
                this.gSBox[(j * 2) + 0x200] = this.gMDS2[(P[1, (P[0, num13] & 0xff) ^ this.M_b2(num8)] & 0xff) ^ this.M_b2(x)];
                this.gSBox[(j * 2) + 0x201] = this.gMDS3[(P[1, (P[1, num14] & 0xff) ^ this.M_b3(num8)] & 0xff) ^ this.M_b3(x)];
            }
        }

        public string AlgorithmName =>
            "Twofish";

        public bool IsPartialBlockOkay =>
            false;
    }
}

