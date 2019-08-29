namespace Org.BouncyCastle.Math
{
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;

    [Serializable]
    public class BigInteger
    {
        internal static readonly int[][] primeLists = new int[][] { 
            new int[] { 3, 5, 7, 11, 13, 0x11, 0x13, 0x17 }, new int[] { 0x1d, 0x1f, 0x25, 0x29, 0x2b }, new int[] { 0x2f, 0x35, 0x3b, 0x3d, 0x43 }, new int[] { 0x47, 0x49, 0x4f, 0x53 }, new int[] { 0x59, 0x61, 0x65, 0x67 }, new int[] { 0x6b, 0x6d, 0x71, 0x7f }, new int[] { 0x83, 0x89, 0x8b, 0x95 }, new int[] { 0x97, 0x9d, 0xa3, 0xa7 }, new int[] { 0xad, 0xb3, 0xb5, 0xbf }, new int[] { 0xc1, 0xc5, 0xc7, 0xd3 }, new int[] { 0xdf, 0xe3, 0xe5 }, new int[] { 0xe9, 0xef, 0xf1 }, new int[] { 0xfb, 0x101, 0x107 }, new int[] { 0x10d, 0x10f, 0x115 }, new int[] { 0x119, 0x11b, 0x125 }, new int[] { 0x133, 0x137, 0x139 },
            new int[] { 0x13d, 0x14b, 0x151 }, new int[] { 0x15b, 0x15d, 0x161 }, new int[] { 0x167, 0x16f, 0x175 }, new int[] { 0x17b, 0x17f, 0x185 }, new int[] { 0x18d, 0x191, 0x199 }, new int[] { 0x1a3, 0x1a5, 0x1af }, new int[] { 0x1b1, 0x1b7, 0x1bb }, new int[] { 0x1c1, 0x1c9, 0x1cd }, new int[] { 0x1cf, 0x1d3, 0x1df }, new int[] { 0x1e7, 0x1eb, 0x1f3 }, new int[] { 0x1f7, 0x1fd, 0x209 }, new int[] { 0x20b, 0x21d, 0x223 }, new int[] { 0x22d, 0x233, 0x239 }, new int[] { 0x23b, 0x241, 0x24b }, new int[] { 0x251, 0x257, 0x259 }, new int[] { 0x25f, 0x265, 0x269 },
            new int[] { 0x26b, 0x277, 0x281 }, new int[] { 0x283, 0x287, 0x28d }, new int[] { 0x293, 0x295, 0x2a1 }, new int[] { 0x2a5, 0x2ab, 0x2b3 }, new int[] { 0x2bd, 0x2c5, 0x2cf }, new int[] { 0x2d7, 0x2dd, 0x2e3 }, new int[] { 0x2e7, 0x2ef, 0x2f5 }, new int[] { 0x2f9, 0x301, 0x305 }, new int[] { 0x313, 0x31d, 0x329 }, new int[] { 0x32b, 0x335, 0x337 }, new int[] { 0x33b, 0x33d, 0x347 }, new int[] { 0x355, 0x359, 0x35b }, new int[] { 0x35f, 0x36d, 0x371 }, new int[] { 0x373, 0x377, 0x38b }, new int[] { 0x38f, 0x397, 0x3a1 }, new int[] { 0x3a9, 0x3ad, 0x3b3 },
            new int[] { 0x3b9, 0x3c7, 0x3cb }, new int[] { 0x3d1, 0x3d7, 0x3df }, new int[] { 0x3e5, 0x3f1, 0x3f5 }, new int[] { 0x3fb, 0x3fd, 0x407 }, new int[] { 0x409, 0x40f, 0x419 }, new int[] { 0x41b, 0x425, 0x427 }, new int[] { 0x42d, 0x43f, 0x443 }, new int[] { 0x445, 0x449, 0x44f }, new int[] { 0x455, 0x45d, 0x463 }, new int[] { 0x469, 0x47f, 0x481 }, new int[] { 0x48b, 0x493, 0x49d }, new int[] { 0x4a3, 0x4a9, 0x4b1 }, new int[] { 0x4bd, 0x4c1, 0x4c7 }, new int[] { 0x4cd, 0x4cf, 0x4d5 }, new int[] { 0x4e1, 0x4eb, 0x4fd }, new int[] { 0x4ff, 0x503, 0x509 }
        };
        internal static readonly int[] primeProducts;
        private const long IMASK = 0xffffffffL;
        private const ulong UIMASK = 0xffffffffL;
        private static readonly int[] ZeroMagnitude = new int[0];
        private static readonly byte[] ZeroEncoding = new byte[0];
        private static readonly BigInteger[] SMALL_CONSTANTS = new BigInteger[0x11];
        public static readonly BigInteger Zero = new BigInteger(0, ZeroMagnitude, false);
        public static readonly BigInteger One;
        public static readonly BigInteger Two;
        public static readonly BigInteger Three;
        public static readonly BigInteger Ten;
        private static readonly byte[] BitLengthTable = new byte[] { 
            0, 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8
        };
        private const int chunk2 = 1;
        private const int chunk8 = 1;
        private const int chunk10 = 0x13;
        private const int chunk16 = 0x10;
        private static readonly BigInteger radix2;
        private static readonly BigInteger radix2E;
        private static readonly BigInteger radix8;
        private static readonly BigInteger radix8E;
        private static readonly BigInteger radix10;
        private static readonly BigInteger radix10E;
        private static readonly BigInteger radix16;
        private static readonly BigInteger radix16E;
        private static readonly SecureRandom RandomSource = new SecureRandom();
        private static readonly int[] ExpWindowThresholds = new int[] { 7, 0x19, 0x51, 0xf1, 0x2a1, 0x701, 0x1201, 0x7fffffff };
        private const int BitsPerByte = 8;
        private const int BitsPerInt = 0x20;
        private const int BytesPerInt = 4;
        private int[] magnitude;
        private int sign;
        private int nBits;
        private int nBitLength;
        private int mQuote;

        static BigInteger()
        {
            Zero.nBits = 0;
            Zero.nBitLength = 0;
            SMALL_CONSTANTS[0] = Zero;
            for (uint i = 1; i < SMALL_CONSTANTS.Length; i++)
            {
                SMALL_CONSTANTS[i] = CreateUValueOf((ulong) i);
            }
            One = SMALL_CONSTANTS[1];
            Two = SMALL_CONSTANTS[2];
            Three = SMALL_CONSTANTS[3];
            Ten = SMALL_CONSTANTS[10];
            radix2 = ValueOf(2L);
            radix2E = radix2.Pow(1);
            radix8 = ValueOf(8L);
            radix8E = radix8.Pow(1);
            radix10 = ValueOf(10L);
            radix10E = radix10.Pow(0x13);
            radix16 = ValueOf(0x10L);
            radix16E = radix16.Pow(0x10);
            primeProducts = new int[primeLists.Length];
            for (int j = 0; j < primeLists.Length; j++)
            {
                int[] numArray = primeLists[j];
                int num3 = numArray[0];
                for (int k = 1; k < numArray.Length; k++)
                {
                    num3 *= numArray[k];
                }
                primeProducts[j] = num3;
            }
        }

        public BigInteger(string value) : this(value, 10)
        {
        }

        public BigInteger(byte[] bytes) : this(bytes, 0, bytes.Length)
        {
        }

        public BigInteger(int sign, byte[] bytes) : this(sign, bytes, 0, bytes.Length)
        {
        }

        public BigInteger(int sizeInBits, Random random)
        {
            this.nBits = -1;
            this.nBitLength = -1;
            if (sizeInBits < 0)
            {
                throw new ArgumentException("sizeInBits must be non-negative");
            }
            this.nBits = -1;
            this.nBitLength = -1;
            if (sizeInBits == 0)
            {
                this.sign = 0;
                this.magnitude = ZeroMagnitude;
            }
            else
            {
                int byteLength = GetByteLength(sizeInBits);
                byte[] buffer = new byte[byteLength];
                random.NextBytes(buffer);
                int num2 = (8 * byteLength) - sizeInBits;
                buffer[0] = (byte) (buffer[0] & ((byte) (((int) 0xff) >> num2)));
                this.magnitude = MakeMagnitude(buffer, 0, buffer.Length);
                this.sign = (this.magnitude.Length >= 1) ? 1 : 0;
            }
        }

        public BigInteger(string str, int radix)
        {
            NumberStyles allowHexSpecifier;
            int num;
            BigInteger integer;
            BigInteger integer2;
            this.nBits = -1;
            this.nBitLength = -1;
            if (str.Length == 0)
            {
                throw new FormatException("Zero length BigInteger");
            }
            switch (radix)
            {
                case 8:
                    allowHexSpecifier = NumberStyles.Integer;
                    num = 1;
                    integer = radix8;
                    integer2 = radix8E;
                    break;

                case 10:
                    allowHexSpecifier = NumberStyles.Integer;
                    num = 0x13;
                    integer = radix10;
                    integer2 = radix10E;
                    break;

                default:
                    if (radix != 2)
                    {
                        if (radix != 0x10)
                        {
                            throw new FormatException("Only bases 2, 8, 10, or 16 allowed");
                        }
                    }
                    else
                    {
                        allowHexSpecifier = NumberStyles.Integer;
                        num = 1;
                        integer = radix2;
                        integer2 = radix2E;
                        break;
                    }
                    allowHexSpecifier = NumberStyles.AllowHexSpecifier;
                    num = 0x10;
                    integer = radix16;
                    integer2 = radix16E;
                    break;
            }
            int startIndex = 0;
            this.sign = 1;
            if (str[0] == '-')
            {
                if (str.Length == 1)
                {
                    throw new FormatException("Zero length BigInteger");
                }
                this.sign = -1;
                startIndex = 1;
            }
        Label_00FB:
            if (startIndex < str.Length)
            {
                char ch = str[startIndex];
                if (int.Parse(ch.ToString(), allowHexSpecifier) == 0)
                {
                    startIndex++;
                    goto Label_00FB;
                }
            }
            if (startIndex >= str.Length)
            {
                this.sign = 0;
                this.magnitude = ZeroMagnitude;
            }
            else
            {
                BigInteger zero = Zero;
                int num3 = startIndex + num;
                if (num3 <= str.Length)
                {
                    do
                    {
                        string s = str.Substring(startIndex, num);
                        ulong num4 = ulong.Parse(s, allowHexSpecifier);
                        BigInteger integer4 = CreateUValueOf(num4);
                        if (radix != 2)
                        {
                            if (radix == 8)
                            {
                                if (num4 >= 8L)
                                {
                                    throw new FormatException("Bad character in radix 8 string: " + s);
                                }
                                zero = zero.ShiftLeft(3);
                            }
                            else if (radix == 0x10)
                            {
                                zero = zero.ShiftLeft(0x40);
                            }
                            else
                            {
                                zero = zero.Multiply(integer2);
                            }
                        }
                        else
                        {
                            if (num4 >= 2L)
                            {
                                throw new FormatException("Bad character in radix 2 string: " + s);
                            }
                            zero = zero.ShiftLeft(1);
                        }
                        zero = zero.Add(integer4);
                        startIndex = num3;
                        num3 += num;
                    }
                    while (num3 <= str.Length);
                }
                if (startIndex < str.Length)
                {
                    string s = str.Substring(startIndex);
                    BigInteger integer5 = CreateUValueOf(ulong.Parse(s, allowHexSpecifier));
                    if (zero.sign > 0)
                    {
                        if ((radix != 2) && (radix != 8))
                        {
                            if (radix == 0x10)
                            {
                                zero = zero.ShiftLeft(s.Length << 2);
                            }
                            else
                            {
                                zero = zero.Multiply(integer.Pow(s.Length));
                            }
                        }
                        zero = zero.Add(integer5);
                    }
                    else
                    {
                        zero = integer5;
                    }
                }
                this.magnitude = zero.magnitude;
            }
        }

        private BigInteger(int signum, int[] mag, bool checkMag)
        {
            this.nBits = -1;
            this.nBitLength = -1;
            if (checkMag)
            {
                int index = 0;
                while ((index < mag.Length) && (mag[index] == 0))
                {
                    index++;
                }
                if (index == mag.Length)
                {
                    this.sign = 0;
                    this.magnitude = ZeroMagnitude;
                }
                else
                {
                    this.sign = signum;
                    if (index == 0)
                    {
                        this.magnitude = mag;
                    }
                    else
                    {
                        this.magnitude = new int[mag.Length - index];
                        Array.Copy(mag, index, this.magnitude, 0, this.magnitude.Length);
                    }
                }
            }
            else
            {
                this.sign = signum;
                this.magnitude = mag;
            }
        }

        public BigInteger(byte[] bytes, int offset, int length)
        {
            this.nBits = -1;
            this.nBitLength = -1;
            if (length == 0)
            {
                throw new FormatException("Zero length BigInteger");
            }
            if (((sbyte) bytes[offset]) < 0)
            {
                this.sign = -1;
                int num = offset + length;
                int index = offset;
                while ((index < num) && (((sbyte) bytes[index]) == -1))
                {
                    index++;
                }
                if (index >= num)
                {
                    this.magnitude = One.magnitude;
                }
                else
                {
                    int num3 = num - index;
                    byte[] buffer = new byte[num3];
                    int num4 = 0;
                    while (num4 < num3)
                    {
                        buffer[num4++] = ~bytes[index++];
                    }
                    while (buffer[--num4] == 0xff)
                    {
                        buffer[num4] = 0;
                    }
                    buffer[num4] = (byte) (buffer[num4] + 1);
                    this.magnitude = MakeMagnitude(buffer, 0, buffer.Length);
                }
            }
            else
            {
                this.magnitude = MakeMagnitude(bytes, offset, length);
                this.sign = (this.magnitude.Length <= 0) ? 0 : 1;
            }
        }

        public BigInteger(int bitLength, int certainty, Random random)
        {
            this.nBits = -1;
            this.nBitLength = -1;
            if (bitLength < 2)
            {
                throw new ArithmeticException("bitLength < 2");
            }
            this.sign = 1;
            this.nBitLength = bitLength;
            if (bitLength == 2)
            {
                this.magnitude = (random.Next(2) != 0) ? Three.magnitude : Two.magnitude;
            }
            else
            {
                int byteLength = GetByteLength(bitLength);
                byte[] buffer = new byte[byteLength];
                int num2 = (8 * byteLength) - bitLength;
                byte num3 = (byte) (((int) 0xff) >> num2);
                byte num4 = (byte) (((int) 1) << (7 - num2));
                while (true)
                {
                    random.NextBytes(buffer);
                    buffer[0] = (byte) (buffer[0] & num3);
                    buffer[0] = (byte) (buffer[0] | num4);
                    buffer[byteLength - 1] = (byte) (buffer[byteLength - 1] | 1);
                    this.magnitude = MakeMagnitude(buffer, 0, buffer.Length);
                    this.nBits = -1;
                    this.mQuote = 0;
                    if ((certainty < 1) || this.CheckProbablePrime(certainty, random, true))
                    {
                        break;
                    }
                    for (int i = 1; i < (this.magnitude.Length - 1); i++)
                    {
                        this.magnitude[i] ^= random.Next();
                        if (this.CheckProbablePrime(certainty, random, true))
                        {
                            return;
                        }
                    }
                }
            }
        }

        public BigInteger(int sign, byte[] bytes, int offset, int length)
        {
            this.nBits = -1;
            this.nBitLength = -1;
            if ((sign < -1) || (sign > 1))
            {
                throw new FormatException("Invalid sign value");
            }
            if (sign == 0)
            {
                this.sign = 0;
                this.magnitude = ZeroMagnitude;
            }
            else
            {
                this.magnitude = MakeMagnitude(bytes, offset, length);
                this.sign = (this.magnitude.Length >= 1) ? sign : 0;
            }
        }

        public BigInteger Abs() => 
            ((this.sign < 0) ? this.Negate() : this);

        public BigInteger Add(BigInteger value)
        {
            if (this.sign == 0)
            {
                return value;
            }
            if (this.sign == value.sign)
            {
                return this.AddToMagnitude(value.magnitude);
            }
            if (value.sign == 0)
            {
                return this;
            }
            if (value.sign < 0)
            {
                return this.Subtract(value.Negate());
            }
            return value.Subtract(this.Negate());
        }

        private static int[] AddMagnitudes(int[] a, int[] b)
        {
            int index = a.Length - 1;
            int num2 = b.Length - 1;
            long num3 = 0L;
            while (num2 >= 0)
            {
                num3 = (long) (((ulong) num3) + (((ulong) a[index]) + ((ulong) b[num2--])));
                a[index--] = (int) num3;
                num3 = num3 >> 0x20;
            }
            if (num3 != 0L)
            {
                while ((index >= 0) && (++a[index--] == 0))
                {
                }
            }
            return a;
        }

        private BigInteger AddToMagnitude(int[] magToAdd)
        {
            int[] magnitude;
            int[] magnitude;
            int[] numArray3;
            if (this.magnitude.Length < magToAdd.Length)
            {
                magnitude = magToAdd;
                magnitude = this.magnitude;
            }
            else
            {
                magnitude = this.magnitude;
                magnitude = magToAdd;
            }
            uint maxValue = uint.MaxValue;
            if (magnitude.Length == magnitude.Length)
            {
                maxValue -= (uint) magnitude[0];
            }
            bool checkMag = magnitude[0] >= maxValue;
            if (checkMag)
            {
                numArray3 = new int[magnitude.Length + 1];
                magnitude.CopyTo(numArray3, 1);
            }
            else
            {
                numArray3 = (int[]) magnitude.Clone();
            }
            return new BigInteger(this.sign, AddMagnitudes(numArray3, magnitude), checkMag);
        }

        public BigInteger And(BigInteger value)
        {
            if ((this.sign == 0) || (value.sign == 0))
            {
                return Zero;
            }
            int[] numArray = (this.sign <= 0) ? this.Add(One).magnitude : this.magnitude;
            int[] numArray2 = (value.sign <= 0) ? value.Add(One).magnitude : value.magnitude;
            bool flag = (this.sign < 0) && (value.sign < 0);
            int[] mag = new int[Math.Max(numArray.Length, numArray2.Length)];
            int num2 = mag.Length - numArray.Length;
            int num3 = mag.Length - numArray2.Length;
            for (int i = 0; i < mag.Length; i++)
            {
                int num5 = (i < num2) ? 0 : numArray[i - num2];
                int num6 = (i < num3) ? 0 : numArray2[i - num3];
                if (this.sign < 0)
                {
                    num5 = ~num5;
                }
                if (value.sign < 0)
                {
                    num6 = ~num6;
                }
                mag[i] = num5 & num6;
                if (flag)
                {
                    mag[i] = ~mag[i];
                }
            }
            BigInteger integer = new BigInteger(1, mag, true);
            if (flag)
            {
                integer = integer.Not();
            }
            return integer;
        }

        public BigInteger AndNot(BigInteger val) => 
            this.And(val.Not());

        private static void AppendZeroExtendedString(StringBuilder sb, string s, int minLength)
        {
            for (int i = s.Length; i < minLength; i++)
            {
                sb.Append('0');
            }
            sb.Append(s);
        }

        internal static BigInteger Arbitrary(int sizeInBits) => 
            new BigInteger(sizeInBits, RandomSource);

        public static int BitCnt(int i)
        {
            uint num = (uint) i;
            num -= (num >> 1) & 0x55555555;
            num = (num & 0x33333333) + ((num >> 2) & 0x33333333);
            num = (num + (num >> 4)) & 0xf0f0f0f;
            num += num >> 8;
            num += num >> 0x10;
            num &= 0x3f;
            return (int) num;
        }

        internal static int BitLen(int w)
        {
            uint index = (uint) w;
            uint num2 = index >> 0x18;
            if (num2 != 0)
            {
                return (0x18 + BitLengthTable[num2]);
            }
            num2 = index >> 0x10;
            if (num2 != 0)
            {
                return (0x10 + BitLengthTable[num2]);
            }
            num2 = index >> 8;
            if (num2 != 0)
            {
                return (8 + BitLengthTable[num2]);
            }
            return BitLengthTable[index];
        }

        private static int CalcBitLength(int sign, int indx, int[] mag)
        {
            while (true)
            {
                if (indx >= mag.Length)
                {
                    return 0;
                }
                if (mag[indx] != 0)
                {
                    break;
                }
                indx++;
            }
            int num = 0x20 * ((mag.Length - indx) - 1);
            int w = mag[indx];
            num += BitLen(w);
            if ((sign < 0) && ((w & -w) == w))
            {
                do
                {
                    if (++indx >= mag.Length)
                    {
                        num--;
                        return num;
                    }
                }
                while (mag[indx] == 0);
            }
            return num;
        }

        private bool CheckProbablePrime(int certainty, Random random, bool randomlySelected)
        {
            int num = Math.Min(this.BitLength - 1, primeLists.Length);
            for (int i = 0; i < num; i++)
            {
                int num3 = this.Remainder(primeProducts[i]);
                foreach (int num5 in primeLists[i])
                {
                    if ((num3 % num5) == 0)
                    {
                        return ((this.BitLength < 0x10) && (this.IntValue == num5));
                    }
                }
            }
            return this.RabinMillerTest(certainty, random, randomlySelected);
        }

        public BigInteger ClearBit(int n)
        {
            if (n < 0)
            {
                throw new ArithmeticException("Bit address less than zero");
            }
            if (!this.TestBit(n))
            {
                return this;
            }
            if ((this.sign > 0) && (n < (this.BitLength - 1)))
            {
                return this.FlipExistingBit(n);
            }
            return this.AndNot(One.ShiftLeft(n));
        }

        private static int CompareNoLeadingZeroes(int xIndx, int[] x, int yIndx, int[] y)
        {
            int num = (x.Length - y.Length) - (xIndx - yIndx);
            if (num != 0)
            {
                return ((num >= 0) ? 1 : -1);
            }
            while (xIndx < x.Length)
            {
                uint num2 = (uint) x[xIndx++];
                uint num3 = (uint) y[yIndx++];
                if (num2 != num3)
                {
                    return ((num2 >= num3) ? 1 : -1);
                }
            }
            return 0;
        }

        public int CompareTo(BigInteger value) => 
            ((this.sign >= value.sign) ? ((this.sign <= value.sign) ? ((this.sign != 0) ? (this.sign * CompareNoLeadingZeroes(0, this.magnitude, 0, value.magnitude)) : 0) : 1) : -1);

        public int CompareTo(object obj) => 
            this.CompareTo((BigInteger) obj);

        private static int CompareTo(int xIndx, int[] x, int yIndx, int[] y)
        {
            while ((xIndx != x.Length) && (x[xIndx] == 0))
            {
                xIndx++;
            }
            while ((yIndx != y.Length) && (y[yIndx] == 0))
            {
                yIndx++;
            }
            return CompareNoLeadingZeroes(xIndx, x, yIndx, y);
        }

        private static BigInteger CreateUValueOf(ulong value)
        {
            int num = (int) (value >> 0x20);
            int num2 = (int) value;
            if (num != 0)
            {
                return new BigInteger(1, new int[] { num, num2 }, false);
            }
            if (num2 == 0)
            {
                return Zero;
            }
            int[] mag = new int[] { num2 };
            BigInteger integer = new BigInteger(1, mag, false);
            if ((num2 & -num2) == num2)
            {
                integer.nBits = 1;
            }
            return integer;
        }

        private static BigInteger CreateValueOf(long value)
        {
            if (value >= 0L)
            {
                return CreateUValueOf((ulong) value);
            }
            if (value == -9223372036854775808L)
            {
                return CreateValueOf(~value).Not();
            }
            return CreateValueOf(-value).Negate();
        }

        private static int CreateWindowEntry(int mult, int zeroes)
        {
            while ((mult & 1) == 0)
            {
                mult = mult >> 1;
                zeroes++;
            }
            return (mult | (zeroes << 8));
        }

        public BigInteger Divide(BigInteger val)
        {
            if (val.sign == 0)
            {
                throw new ArithmeticException("Division by zero error");
            }
            if (this.sign == 0)
            {
                return Zero;
            }
            if (val.QuickPow2Check())
            {
                BigInteger integer = this.Abs().ShiftRight(val.Abs().BitLength - 1);
                return ((val.sign != this.sign) ? integer.Negate() : integer);
            }
            int[] x = (int[]) this.magnitude.Clone();
            return new BigInteger(this.sign * val.sign, this.Divide(x, val.magnitude), true);
        }

        private int[] Divide(int[] x, int[] y)
        {
            int[] numArray;
            int index = 0;
            while ((index < x.Length) && (x[index] == 0))
            {
                index++;
            }
            int num2 = 0;
            while ((num2 < y.Length) && (y[num2] == 0))
            {
                num2++;
            }
            int num3 = CompareNoLeadingZeroes(index, x, num2, y);
            if (num3 > 0)
            {
                int[] numArray2;
                int[] numArray3;
                int num4 = CalcBitLength(1, num2, y);
                int num5 = CalcBitLength(1, index, x);
                int n = num5 - num4;
                int start = 0;
                int yIndx = 0;
                int num9 = num4;
                if (n > 0)
                {
                    numArray2 = new int[(n >> 5) + 1];
                    numArray2[0] = ((int) 1) << (n % 0x20);
                    numArray3 = ShiftLeft(y, n);
                    num9 += n;
                }
                else
                {
                    numArray2 = new int[] { 1 };
                    int length = y.Length - num2;
                    numArray3 = new int[length];
                    Array.Copy(y, num2, numArray3, 0, length);
                }
                numArray = new int[numArray2.Length];
                while (true)
                {
                    if ((num9 < num5) || (CompareNoLeadingZeroes(index, x, yIndx, numArray3) >= 0))
                    {
                        Subtract(index, x, yIndx, numArray3);
                        AddMagnitudes(numArray, numArray2);
                        while (x[index] == 0)
                        {
                            if (++index == x.Length)
                            {
                                return numArray;
                            }
                        }
                        num5 = (0x20 * ((x.Length - index) - 1)) + BitLen(x[index]);
                        if (num5 <= num4)
                        {
                            if (num5 < num4)
                            {
                                return numArray;
                            }
                            num3 = CompareNoLeadingZeroes(index, x, num2, y);
                            if (num3 <= 0)
                            {
                                goto Label_0211;
                            }
                        }
                    }
                    n = num9 - num5;
                    if (n == 1)
                    {
                        uint num11 = (uint) (numArray3[yIndx] >> 1);
                        uint num12 = (uint) x[index];
                        if (num11 > num12)
                        {
                            n++;
                        }
                    }
                    if (n < 2)
                    {
                        ShiftRightOneInPlace(yIndx, numArray3);
                        num9--;
                        ShiftRightOneInPlace(start, numArray2);
                    }
                    else
                    {
                        ShiftRightInPlace(yIndx, numArray3, n);
                        num9 -= n;
                        ShiftRightInPlace(start, numArray2, n);
                    }
                    while (numArray3[yIndx] == 0)
                    {
                        yIndx++;
                    }
                    while (numArray2[start] == 0)
                    {
                        start++;
                    }
                }
            }
            numArray = new int[1];
        Label_0211:
            if (num3 == 0)
            {
                AddMagnitudes(numArray, One.magnitude);
                Array.Clear(x, index, x.Length - index);
            }
            return numArray;
        }

        public BigInteger[] DivideAndRemainder(BigInteger val)
        {
            if (val.sign == 0)
            {
                throw new ArithmeticException("Division by zero error");
            }
            BigInteger[] integerArray = new BigInteger[2];
            if (this.sign == 0)
            {
                integerArray[0] = Zero;
                integerArray[1] = Zero;
                return integerArray;
            }
            if (val.QuickPow2Check())
            {
                int n = val.Abs().BitLength - 1;
                BigInteger integer = this.Abs().ShiftRight(n);
                int[] numArray = this.LastNBits(n);
                integerArray[0] = (val.sign != this.sign) ? integer.Negate() : integer;
                integerArray[1] = new BigInteger(this.sign, numArray, true);
                return integerArray;
            }
            int[] x = (int[]) this.magnitude.Clone();
            int[] mag = this.Divide(x, val.magnitude);
            integerArray[0] = new BigInteger(this.sign * val.sign, mag, true);
            integerArray[1] = new BigInteger(this.sign, x, true);
            return integerArray;
        }

        private BigInteger DivideWords(int w)
        {
            int length = this.magnitude.Length;
            if (w >= length)
            {
                return Zero;
            }
            int[] destinationArray = new int[length - w];
            Array.Copy(this.magnitude, 0, destinationArray, 0, length - w);
            return new BigInteger(this.sign, destinationArray, false);
        }

        private static int[] doSubBigLil(int[] bigMag, int[] lilMag)
        {
            int[] x = (int[]) bigMag.Clone();
            return Subtract(0, x, 0, lilMag);
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            BigInteger x = obj as BigInteger;
            if (x == null)
            {
                return false;
            }
            return ((this.sign == x.sign) && this.IsEqualMagnitude(x));
        }

        private static BigInteger ExtEuclid(BigInteger a, BigInteger b, out BigInteger u1Out)
        {
            BigInteger one = One;
            BigInteger zero = Zero;
            BigInteger integer3 = a;
            BigInteger val = b;
            if (val.sign > 0)
            {
                while (true)
                {
                    BigInteger[] integerArray = integer3.DivideAndRemainder(val);
                    integer3 = val;
                    val = integerArray[1];
                    BigInteger integer5 = one;
                    one = zero;
                    if (val.sign <= 0)
                    {
                        break;
                    }
                    zero = integer5.Subtract(zero.Multiply(integerArray[0]));
                }
            }
            u1Out = one;
            return integer3;
        }

        public BigInteger FlipBit(int n)
        {
            if (n < 0)
            {
                throw new ArithmeticException("Bit address less than zero");
            }
            if ((this.sign > 0) && (n < (this.BitLength - 1)))
            {
                return this.FlipExistingBit(n);
            }
            return this.Xor(One.ShiftLeft(n));
        }

        private BigInteger FlipExistingBit(int n)
        {
            int[] mag = (int[]) this.magnitude.Clone();
            mag[(mag.Length - 1) - (n >> 5)] ^= ((int) 1) << n;
            return new BigInteger(this.sign, mag, false);
        }

        public BigInteger Gcd(BigInteger value)
        {
            BigInteger integer;
            if (value.sign == 0)
            {
                return this.Abs();
            }
            if (this.sign == 0)
            {
                return value.Abs();
            }
            BigInteger integer2 = this;
            for (BigInteger integer3 = value; integer3.sign != 0; integer3 = integer)
            {
                integer = integer2.Mod(integer3);
                integer2 = integer3;
            }
            return integer2;
        }

        private static int GetByteLength(int nBits) => 
            (((nBits + 8) - 1) / 8);

        public override int GetHashCode()
        {
            int length = this.magnitude.Length;
            if (this.magnitude.Length > 0)
            {
                length ^= this.magnitude[0];
                if (this.magnitude.Length > 1)
                {
                    length ^= this.magnitude[this.magnitude.Length - 1];
                }
            }
            return ((this.sign >= 0) ? length : ~length);
        }

        public int GetLowestSetBit()
        {
            if (this.sign == 0)
            {
                return -1;
            }
            return this.GetLowestSetBitMaskFirst(-1);
        }

        private int GetLowestSetBitMaskFirst(int firstWordMask)
        {
            int length = this.magnitude.Length;
            int num2 = 0;
            uint num3 = (uint) (this.magnitude[--length] & firstWordMask);
            while (num3 == 0)
            {
                num3 = (uint) this.magnitude[--length];
                num2 += 0x20;
            }
            while ((num3 & 0xff) == 0)
            {
                num3 = num3 >> 8;
                num2 += 8;
            }
            while ((num3 & 1) == 0)
            {
                num3 = num3 >> 1;
                num2++;
            }
            return num2;
        }

        private int GetMQuote()
        {
            if (this.mQuote != 0)
            {
                return this.mQuote;
            }
            int d = -this.magnitude[this.magnitude.Length - 1];
            return (this.mQuote = ModInverse32(d));
        }

        private static int[] GetWindowList(int[] mag, int extraBits)
        {
            int w = mag[0];
            int num2 = BitLen(w);
            int num3 = ((((mag.Length - 1) << 5) + num2) / (1 + extraBits)) + 2;
            int[] numArray = new int[num3];
            int index = 0;
            int num5 = 0x21 - num2;
            w = w << num5;
            int mult = 1;
            int num7 = ((int) 1) << extraBits;
            int zeroes = 0;
            int num9 = 0;
            while (true)
            {
                while (num5 < 0x20)
                {
                    if (mult < num7)
                    {
                        mult = (mult << 1) | (w >> 0x1f);
                    }
                    else if (w < 0)
                    {
                        numArray[index++] = CreateWindowEntry(mult, zeroes);
                        mult = 1;
                        zeroes = 0;
                    }
                    else
                    {
                        zeroes++;
                    }
                    w = w << 1;
                    num5++;
                }
                if (++num9 == mag.Length)
                {
                    numArray[index++] = CreateWindowEntry(mult, zeroes);
                    break;
                }
                w = mag[num9];
                num5 = 0;
            }
            numArray[index] = -1;
            return numArray;
        }

        private BigInteger Inc()
        {
            if (this.sign == 0)
            {
                return One;
            }
            if (this.sign < 0)
            {
                return new BigInteger(-1, doSubBigLil(this.magnitude, One.magnitude), true);
            }
            return this.AddToMagnitude(One.magnitude);
        }

        private bool IsEqualMagnitude(BigInteger x)
        {
            if (this.magnitude.Length != x.magnitude.Length)
            {
                return false;
            }
            for (int i = 0; i < this.magnitude.Length; i++)
            {
                if (this.magnitude[i] != x.magnitude[i])
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsProbablePrime(int certainty) => 
            this.IsProbablePrime(certainty, false);

        internal bool IsProbablePrime(int certainty, bool randomlySelected)
        {
            if (certainty <= 0)
            {
                return true;
            }
            BigInteger integer = this.Abs();
            if (!integer.TestBit(0))
            {
                return integer.Equals(Two);
            }
            if (integer.Equals(One))
            {
                return false;
            }
            return integer.CheckProbablePrime(certainty, RandomSource, randomlySelected);
        }

        private int[] LastNBits(int n)
        {
            if (n < 1)
            {
                return ZeroMagnitude;
            }
            int num = ((n + 0x20) - 1) / 0x20;
            num = Math.Min(num, this.magnitude.Length);
            int[] destinationArray = new int[num];
            Array.Copy(this.magnitude, this.magnitude.Length - num, destinationArray, 0, num);
            int num2 = (num << 5) - n;
            if (num2 > 0)
            {
                destinationArray[0] &= ((int) (-1)) >> num2;
            }
            return destinationArray;
        }

        private static int[] MakeMagnitude(byte[] bytes, int offset, int length)
        {
            int num = offset + length;
            int index = offset;
            while ((index < num) && (bytes[index] == 0))
            {
                index++;
            }
            if (index >= num)
            {
                return ZeroMagnitude;
            }
            int num3 = ((num - index) + 3) / 4;
            int num4 = (num - index) % 4;
            if (num4 == 0)
            {
                num4 = 4;
            }
            if (num3 < 1)
            {
                return ZeroMagnitude;
            }
            int[] numArray = new int[num3];
            int num5 = 0;
            int num6 = 0;
            for (int i = index; i < num; i++)
            {
                num5 = num5 << 8;
                num5 |= bytes[i] & 0xff;
                num4--;
                if (num4 <= 0)
                {
                    numArray[num6] = num5;
                    num6++;
                    num4 = 4;
                    num5 = 0;
                }
            }
            if (num6 < numArray.Length)
            {
                numArray[num6] = num5;
            }
            return numArray;
        }

        public BigInteger Max(BigInteger value) => 
            ((this.CompareTo(value) <= 0) ? value : this);

        public BigInteger Min(BigInteger value) => 
            ((this.CompareTo(value) >= 0) ? value : this);

        public BigInteger Mod(BigInteger m)
        {
            if (m.sign < 1)
            {
                throw new ArithmeticException("Modulus must be positive");
            }
            BigInteger integer = this.Remainder(m);
            return ((integer.sign < 0) ? integer.Add(m) : integer);
        }

        public BigInteger ModInverse(BigInteger m)
        {
            if (m.sign < 1)
            {
                throw new ArithmeticException("Modulus must be positive");
            }
            if (m.QuickPow2Check())
            {
                return this.ModInversePow2(m);
            }
            if (!ExtEuclid(this.Remainder(m), m, out BigInteger integer2).Equals(One))
            {
                throw new ArithmeticException("Numbers not relatively prime.");
            }
            if (integer2.sign < 0)
            {
                integer2 = integer2.Add(m);
            }
            return integer2;
        }

        private static int ModInverse32(int d)
        {
            int num = d + (((d + 1) & 4) << 1);
            num *= 2 - (d * num);
            num *= 2 - (d * num);
            return (num * (2 - (d * num)));
        }

        private static long ModInverse64(long d)
        {
            long num = d + (((d + 1L) & 4L) << 1);
            num *= 2L - (d * num);
            num *= 2L - (d * num);
            num *= 2L - (d * num);
            return (num * (2L - (d * num)));
        }

        private BigInteger ModInversePow2(BigInteger m)
        {
            if (!this.TestBit(0))
            {
                throw new ArithmeticException("Numbers not relatively prime.");
            }
            int num = m.BitLength - 1;
            long num2 = ModInverse64(this.LongValue);
            if (num < 0x40)
            {
                num2 &= (((long) 1L) << num) - 1L;
            }
            BigInteger integer = ValueOf(num2);
            if (num > 0x40)
            {
                BigInteger val = this.Remainder(m);
                int num3 = 0x40;
                do
                {
                    BigInteger n = integer.Multiply(val).Remainder(m);
                    integer = integer.Multiply(Two.Subtract(n)).Remainder(m);
                    num3 = num3 << 1;
                }
                while (num3 < num);
            }
            if (integer.sign < 0)
            {
                integer = integer.Add(m);
            }
            return integer;
        }

        public BigInteger ModPow(BigInteger e, BigInteger m)
        {
            if (m.sign < 1)
            {
                throw new ArithmeticException("Modulus must be positive");
            }
            if (m.Equals(One))
            {
                return Zero;
            }
            if (e.sign == 0)
            {
                return One;
            }
            if (this.sign == 0)
            {
                return Zero;
            }
            bool flag = e.sign < 0;
            if (flag)
            {
                e = e.Negate();
            }
            BigInteger b = this.Mod(m);
            if (!e.Equals(One))
            {
                if ((m.magnitude[m.magnitude.Length - 1] & 1) == 0)
                {
                    b = ModPowBarrett(b, e, m);
                }
                else
                {
                    b = ModPowMonty(b, e, m, true);
                }
            }
            if (flag)
            {
                b = b.ModInverse(m);
            }
            return b;
        }

        private static BigInteger ModPowBarrett(BigInteger b, BigInteger e, BigInteger m)
        {
            BigInteger integer4;
            int length = m.magnitude.Length;
            BigInteger mr = One.ShiftLeft((length + 1) << 5);
            BigInteger yu = One.ShiftLeft(length << 6).Divide(m);
            int index = 0;
            int bitLength = e.BitLength;
            while (bitLength > ExpWindowThresholds[index])
            {
                index++;
            }
            int num4 = ((int) 1) << index;
            BigInteger[] integerArray = new BigInteger[num4];
            integerArray[0] = b;
            BigInteger val = ReduceBarrett(b.Square(), m, mr, yu);
            for (int i = 1; i < num4; i++)
            {
                integerArray[i] = ReduceBarrett(integerArray[i - 1].Multiply(val), m, mr, yu);
            }
            int[] windowList = GetWindowList(e.magnitude, index);
            int num6 = windowList[0];
            int num7 = num6 & 0xff;
            int num8 = num6 >> 8;
            if (num7 == 1)
            {
                integer4 = val;
                num8--;
            }
            else
            {
                integer4 = integerArray[num7 >> 1];
            }
            int num9 = 1;
            while ((num6 = windowList[num9++]) != -1)
            {
                num7 = num6 & 0xff;
                int num10 = num8 + BitLengthTable[num7];
                for (int k = 0; k < num10; k++)
                {
                    integer4 = ReduceBarrett(integer4.Square(), m, mr, yu);
                }
                integer4 = ReduceBarrett(integer4.Multiply(integerArray[num7 >> 1]), m, mr, yu);
                num8 = num6 >> 8;
            }
            for (int j = 0; j < num8; j++)
            {
                integer4 = ReduceBarrett(integer4.Square(), m, mr, yu);
            }
            return integer4;
        }

        private static BigInteger ModPowMonty(BigInteger b, BigInteger e, BigInteger m, bool convert)
        {
            int[] numArray7;
            int length = m.magnitude.Length;
            int n = 0x20 * length;
            bool smallMontyModulus = (m.BitLength + 2) <= n;
            uint mQuote = (uint) m.GetMQuote();
            if (convert)
            {
                b = b.ShiftLeft(n).Remainder(m);
            }
            int[] a = new int[length + 1];
            int[] magnitude = b.magnitude;
            if (magnitude.Length < length)
            {
                int[] array = new int[length];
                magnitude.CopyTo(array, (int) (length - magnitude.Length));
                magnitude = array;
            }
            int index = 0;
            if ((e.magnitude.Length > 1) || (e.BitCount > 2))
            {
                int bitLength = e.BitLength;
                while (bitLength > ExpWindowThresholds[index])
                {
                    index++;
                }
            }
            int num6 = ((int) 1) << index;
            int[][] numArray4 = new int[num6][];
            numArray4[0] = magnitude;
            int[] x = Arrays.Clone(magnitude);
            SquareMonty(a, x, m.magnitude, mQuote, smallMontyModulus);
            for (int i = 1; i < num6; i++)
            {
                numArray4[i] = Arrays.Clone(numArray4[i - 1]);
                MultiplyMonty(a, numArray4[i], x, m.magnitude, mQuote, smallMontyModulus);
            }
            int[] windowList = GetWindowList(e.magnitude, index);
            int num8 = windowList[0];
            int num9 = num8 & 0xff;
            int num10 = num8 >> 8;
            if (num9 == 1)
            {
                numArray7 = x;
                num10--;
            }
            else
            {
                numArray7 = Arrays.Clone(numArray4[num9 >> 1]);
            }
            int num11 = 1;
            while ((num8 = windowList[num11++]) != -1)
            {
                num9 = num8 & 0xff;
                int num12 = num10 + BitLengthTable[num9];
                for (int k = 0; k < num12; k++)
                {
                    SquareMonty(a, numArray7, m.magnitude, mQuote, smallMontyModulus);
                }
                MultiplyMonty(a, numArray7, numArray4[num9 >> 1], m.magnitude, mQuote, smallMontyModulus);
                num10 = num8 >> 8;
            }
            for (int j = 0; j < num10; j++)
            {
                SquareMonty(a, numArray7, m.magnitude, mQuote, smallMontyModulus);
            }
            if (convert)
            {
                MontgomeryReduce(numArray7, m.magnitude, mQuote);
            }
            else if (smallMontyModulus && (CompareTo(0, numArray7, 0, m.magnitude) >= 0))
            {
                Subtract(0, numArray7, 0, m.magnitude);
            }
            return new BigInteger(1, numArray7, true);
        }

        private static void MontgomeryReduce(int[] x, int[] m, uint mDash)
        {
            int length = m.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                uint num3 = (uint) x[length - 1];
                ulong num4 = num3 * mDash;
                ulong num5 = (num4 * ((ulong) m[length - 1])) + num3;
                num5 = num5 >> 0x20;
                for (int j = length - 2; j >= 0; j--)
                {
                    num5 += (num4 * ((ulong) m[j])) + ((ulong) x[j]);
                    x[j + 1] = (int) num5;
                    num5 = num5 >> 0x20;
                }
                x[0] = (int) num5;
            }
            if (CompareTo(0, x, 0, m) >= 0)
            {
                Subtract(0, x, 0, m);
            }
        }

        public BigInteger Multiply(BigInteger val)
        {
            if (val == this)
            {
                return this.Square();
            }
            if ((this.sign & val.sign) == 0)
            {
                return Zero;
            }
            if (val.QuickPow2Check())
            {
                BigInteger integer = this.ShiftLeft(val.Abs().BitLength - 1);
                return ((val.sign <= 0) ? integer.Negate() : integer);
            }
            if (this.QuickPow2Check())
            {
                BigInteger integer2 = val.ShiftLeft(this.Abs().BitLength - 1);
                return ((this.sign <= 0) ? integer2.Negate() : integer2);
            }
            int num = this.magnitude.Length + val.magnitude.Length;
            int[] x = new int[num];
            Multiply(x, this.magnitude, val.magnitude);
            return new BigInteger((this.sign ^ val.sign) ^ 1, x, true);
        }

        private static int[] Multiply(int[] x, int[] y, int[] z)
        {
            int length = z.Length;
            if (length >= 1)
            {
                int index = x.Length - y.Length;
                do
                {
                    long num3 = z[--length] & 0xffffffffL;
                    long num4 = 0L;
                    if (num3 != 0L)
                    {
                        for (int i = y.Length - 1; i >= 0; i--)
                        {
                            num4 += (num3 * (y[i] & 0xffffffffL)) + (x[index + i] & 0xffffffffL);
                            x[index + i] = (int) num4;
                            num4 = num4 >> 0x20;
                        }
                    }
                    index--;
                    if (index >= 0)
                    {
                        x[index] = (int) num4;
                    }
                }
                while (length > 0);
            }
            return x;
        }

        private static void MultiplyMonty(int[] a, int[] x, int[] y, int[] m, uint mDash, bool smallMontyModulus)
        {
            int length = m.Length;
            if (length == 1)
            {
                x[0] = (int) MultiplyMontyNIsOne((uint) x[0], (uint) y[0], (uint) m[0], mDash);
            }
            else
            {
                uint num2 = (uint) y[length - 1];
                ulong num4 = (ulong) x[length - 1];
                ulong num5 = num4 * num2;
                ulong num6 = ((uint) num5) * mDash;
                ulong num7 = num6 * ((ulong) m[length - 1]);
                num5 += (uint) num7;
                num5 = (num5 >> 0x20) + (num7 >> 0x20);
                for (int i = length - 2; i >= 0; i--)
                {
                    ulong num9 = num4 * ((ulong) y[i]);
                    num7 = num6 * ((ulong) m[i]);
                    num5 += (num9 & 0xffffffffL) + ((uint) num7);
                    a[i + 2] = (int) num5;
                    num5 = ((num5 >> 0x20) + (num9 >> 0x20)) + (num7 >> 0x20);
                }
                a[1] = (int) num5;
                int num3 = (int) (num5 >> 0x20);
                for (int j = length - 2; j >= 0; j--)
                {
                    uint num11 = (uint) a[length];
                    ulong num12 = (ulong) x[j];
                    ulong num13 = num12 * num2;
                    ulong num14 = (num13 & 0xffffffffL) + num11;
                    ulong num15 = ((uint) num14) * mDash;
                    ulong num16 = num15 * ((ulong) m[length - 1]);
                    num14 += (uint) num16;
                    num14 = ((num14 >> 0x20) + (num13 >> 0x20)) + (num16 >> 0x20);
                    for (int k = length - 2; k >= 0; k--)
                    {
                        num13 = num12 * ((ulong) y[k]);
                        num16 = num15 * ((ulong) m[k]);
                        num14 += ((num13 & 0xffffffffL) + ((uint) num16)) + ((ulong) a[k + 1]);
                        a[k + 2] = (int) num14;
                        num14 = ((num14 >> 0x20) + (num13 >> 0x20)) + (num16 >> 0x20);
                    }
                    num14 += (ulong) num3;
                    a[1] = (int) num14;
                    num3 = (int) (num14 >> 0x20);
                }
                a[0] = num3;
                if (!smallMontyModulus && (CompareTo(0, a, 0, m) >= 0))
                {
                    Subtract(0, a, 0, m);
                }
                Array.Copy(a, 1, x, 0, length);
            }
        }

        private static uint MultiplyMontyNIsOne(uint x, uint y, uint m, uint mDash)
        {
            ulong num = x * y;
            uint num2 = ((uint) num) * mDash;
            ulong num3 = m;
            ulong num4 = num3 * num2;
            num += (uint) num4;
            num = (num >> 0x20) + (num4 >> 0x20);
            if (num > num3)
            {
                num -= num3;
            }
            return (uint) num;
        }

        public BigInteger Negate()
        {
            if (this.sign == 0)
            {
                return this;
            }
            return new BigInteger(-this.sign, this.magnitude, false);
        }

        public BigInteger NextProbablePrime()
        {
            if (this.sign < 0)
            {
                throw new ArithmeticException("Cannot be called on value < 0");
            }
            if (this.CompareTo(Two) < 0)
            {
                return Two;
            }
            BigInteger integer = this.Inc().SetBit(0);
            while (!integer.CheckProbablePrime(100, RandomSource, false))
            {
                integer = integer.Add(Two);
            }
            return integer;
        }

        public BigInteger Not() => 
            this.Inc().Negate();

        public BigInteger Or(BigInteger value)
        {
            if (this.sign == 0)
            {
                return value;
            }
            if (value.sign == 0)
            {
                return this;
            }
            int[] numArray = (this.sign <= 0) ? this.Add(One).magnitude : this.magnitude;
            int[] numArray2 = (value.sign <= 0) ? value.Add(One).magnitude : value.magnitude;
            bool flag = (this.sign < 0) || (value.sign < 0);
            int[] mag = new int[Math.Max(numArray.Length, numArray2.Length)];
            int num2 = mag.Length - numArray.Length;
            int num3 = mag.Length - numArray2.Length;
            for (int i = 0; i < mag.Length; i++)
            {
                int num5 = (i < num2) ? 0 : numArray[i - num2];
                int num6 = (i < num3) ? 0 : numArray2[i - num3];
                if (this.sign < 0)
                {
                    num5 = ~num5;
                }
                if (value.sign < 0)
                {
                    num6 = ~num6;
                }
                mag[i] = num5 | num6;
                if (flag)
                {
                    mag[i] = ~mag[i];
                }
            }
            BigInteger integer = new BigInteger(1, mag, true);
            if (flag)
            {
                integer = integer.Not();
            }
            return integer;
        }

        public BigInteger Pow(int exp)
        {
            if (exp <= 0)
            {
                if (exp < 0)
                {
                    throw new ArithmeticException("Negative exponent");
                }
                return One;
            }
            if (this.sign == 0)
            {
                return this;
            }
            if (this.QuickPow2Check())
            {
                long num = exp * (this.BitLength - 1);
                if (num > 0x7fffffffL)
                {
                    throw new ArithmeticException("Result too large");
                }
                return One.ShiftLeft((int) num);
            }
            BigInteger one = One;
            BigInteger val = this;
            while (true)
            {
                if ((exp & 1) == 1)
                {
                    one = one.Multiply(val);
                }
                exp = exp >> 1;
                if (exp == 0)
                {
                    return one;
                }
                val = val.Multiply(val);
            }
        }

        public static BigInteger ProbablePrime(int bitLength, Random random) => 
            new BigInteger(bitLength, 100, random);

        private bool QuickPow2Check() => 
            ((this.sign > 0) && (this.nBits == 1));

        public bool RabinMillerTest(int certainty, Random random) => 
            this.RabinMillerTest(certainty, random, false);

        internal bool RabinMillerTest(int certainty, Random random, bool randomlySelected)
        {
            BigInteger integer5;
            int bitLength = this.BitLength;
            int num2 = ((certainty - 1) / 2) + 1;
            if (randomlySelected)
            {
                int num3 = (bitLength < 0x400) ? ((bitLength < 0x200) ? ((bitLength < 0x100) ? 50 : 0x10) : 8) : 4;
                if (certainty < 100)
                {
                    num2 = Math.Min(num3, num2);
                }
                else
                {
                    num2 -= 50;
                    num2 += num3;
                }
            }
            BigInteger n = this;
            int lowestSetBitMaskFirst = n.GetLowestSetBitMaskFirst(-2);
            BigInteger e = n.ShiftRight(lowestSetBitMaskFirst);
            BigInteger integer3 = One.ShiftLeft(0x20 * n.magnitude.Length).Remainder(n);
            BigInteger x = n.Subtract(integer3);
        Label_00A7:
            integer5 = new BigInteger(n.BitLength, random);
            if (((integer5.sign == 0) || (integer5.CompareTo(n) >= 0)) || (integer5.IsEqualMagnitude(integer3) || integer5.IsEqualMagnitude(x)))
            {
                goto Label_00A7;
            }
            BigInteger b = ModPowMonty(integer5, e, n, false);
            if (!b.Equals(integer3))
            {
                int num5 = 0;
                while (!b.Equals(x))
                {
                    if (++num5 == lowestSetBitMaskFirst)
                    {
                        return false;
                    }
                    b = ModPowMonty(b, Two, n, false);
                    if (b.Equals(integer3))
                    {
                        return false;
                    }
                }
            }
            if (--num2 > 0)
            {
                goto Label_00A7;
            }
            return true;
        }

        private static BigInteger ReduceBarrett(BigInteger x, BigInteger m, BigInteger mr, BigInteger yu)
        {
            int bitLength = x.BitLength;
            int num2 = m.BitLength;
            if (bitLength >= num2)
            {
                if ((bitLength - num2) > 1)
                {
                    int length = m.magnitude.Length;
                    BigInteger integer3 = x.DivideWords(length - 1).Multiply(yu).DivideWords(length + 1);
                    BigInteger integer4 = x.RemainderWords(length + 1);
                    BigInteger n = integer3.Multiply(m).RemainderWords(length + 1);
                    x = integer4.Subtract(n);
                    if (x.sign < 0)
                    {
                        x = x.Add(mr);
                    }
                }
                while (x.CompareTo(m) >= 0)
                {
                    x = x.Subtract(m);
                }
            }
            return x;
        }

        public BigInteger Remainder(BigInteger n)
        {
            int[] numArray;
            if (n.sign == 0)
            {
                throw new ArithmeticException("Division by zero error");
            }
            if (this.sign == 0)
            {
                return Zero;
            }
            if (n.magnitude.Length == 1)
            {
                int m = n.magnitude[0];
                if (m > 0)
                {
                    if (m == 1)
                    {
                        return Zero;
                    }
                    int num2 = this.Remainder(m);
                    return ((num2 != 0) ? new BigInteger(this.sign, new int[] { num2 }, false) : Zero);
                }
            }
            if (CompareNoLeadingZeroes(0, this.magnitude, 0, n.magnitude) < 0)
            {
                return this;
            }
            if (n.QuickPow2Check())
            {
                numArray = this.LastNBits(n.Abs().BitLength - 1);
            }
            else
            {
                numArray = (int[]) this.magnitude.Clone();
                numArray = Remainder(numArray, n.magnitude);
            }
            return new BigInteger(this.sign, numArray, true);
        }

        private int Remainder(int m)
        {
            long num = 0L;
            for (int i = 0; i < this.magnitude.Length; i++)
            {
                long num3 = (long) ((ulong) this.magnitude[i]);
                num = ((num << 0x20) | num3) % ((long) m);
            }
            return (int) num;
        }

        private static int[] Remainder(int[] x, int[] y)
        {
            int index = 0;
            while ((index < x.Length) && (x[index] == 0))
            {
                index++;
            }
            int num2 = 0;
            while ((num2 < y.Length) && (y[num2] == 0))
            {
                num2++;
            }
            int num3 = CompareNoLeadingZeroes(index, x, num2, y);
            if (num3 > 0)
            {
                int[] numArray;
                int num4 = CalcBitLength(1, num2, y);
                int num5 = CalcBitLength(1, index, x);
                int n = num5 - num4;
                int yIndx = 0;
                int num8 = num4;
                if (n > 0)
                {
                    numArray = ShiftLeft(y, n);
                    num8 += n;
                }
                else
                {
                    int length = y.Length - num2;
                    numArray = new int[length];
                    Array.Copy(y, num2, numArray, 0, length);
                }
                while (true)
                {
                    if ((num8 < num5) || (CompareNoLeadingZeroes(index, x, yIndx, numArray) >= 0))
                    {
                        Subtract(index, x, yIndx, numArray);
                        while (x[index] == 0)
                        {
                            if (++index == x.Length)
                            {
                                return x;
                            }
                        }
                        num5 = (0x20 * ((x.Length - index) - 1)) + BitLen(x[index]);
                        if (num5 <= num4)
                        {
                            if (num5 < num4)
                            {
                                return x;
                            }
                            num3 = CompareNoLeadingZeroes(index, x, num2, y);
                            if (num3 <= 0)
                            {
                                break;
                            }
                        }
                    }
                    n = num8 - num5;
                    if (n == 1)
                    {
                        uint num10 = (uint) (numArray[yIndx] >> 1);
                        uint num11 = (uint) x[index];
                        if (num10 > num11)
                        {
                            n++;
                        }
                    }
                    if (n < 2)
                    {
                        ShiftRightOneInPlace(yIndx, numArray);
                        num8--;
                    }
                    else
                    {
                        ShiftRightInPlace(yIndx, numArray, n);
                        num8 -= n;
                    }
                    while (numArray[yIndx] == 0)
                    {
                        yIndx++;
                    }
                }
            }
            if (num3 == 0)
            {
                Array.Clear(x, index, x.Length - index);
            }
            return x;
        }

        private BigInteger RemainderWords(int w)
        {
            int length = this.magnitude.Length;
            if (w >= length)
            {
                return this;
            }
            int[] destinationArray = new int[w];
            Array.Copy(this.magnitude, length - w, destinationArray, 0, w);
            return new BigInteger(this.sign, destinationArray, false);
        }

        public BigInteger SetBit(int n)
        {
            if (n < 0)
            {
                throw new ArithmeticException("Bit address less than zero");
            }
            if (this.TestBit(n))
            {
                return this;
            }
            if ((this.sign > 0) && (n < (this.BitLength - 1)))
            {
                return this.FlipExistingBit(n);
            }
            return this.Or(One.ShiftLeft(n));
        }

        public BigInteger ShiftLeft(int n)
        {
            if ((this.sign == 0) || (this.magnitude.Length == 0))
            {
                return Zero;
            }
            if (n == 0)
            {
                return this;
            }
            if (n < 0)
            {
                return this.ShiftRight(-n);
            }
            BigInteger integer = new BigInteger(this.sign, ShiftLeft(this.magnitude, n), true);
            if (this.nBits != -1)
            {
                integer.nBits = (this.sign <= 0) ? (this.nBits + n) : this.nBits;
            }
            if (this.nBitLength != -1)
            {
                integer.nBitLength = this.nBitLength + n;
            }
            return integer;
        }

        private static int[] ShiftLeft(int[] mag, int n)
        {
            int[] numArray;
            int num = n >> 5;
            int num2 = n & 0x1f;
            int length = mag.Length;
            if (num2 == 0)
            {
                numArray = new int[length + num];
                mag.CopyTo(numArray, 0);
                return numArray;
            }
            int index = 0;
            int num5 = 0x20 - num2;
            int num6 = mag[0] >> num5;
            if (num6 != 0)
            {
                numArray = new int[(length + num) + 1];
                numArray[index++] = num6;
            }
            else
            {
                numArray = new int[length + num];
            }
            int num7 = mag[0];
            for (int i = 0; i < (length - 1); i++)
            {
                int num9 = mag[i + 1];
                numArray[index++] = (num7 << num2) | (num9 >> num5);
                num7 = num9;
            }
            numArray[index] = mag[length - 1] << num2;
            return numArray;
        }

        private static int ShiftLeftOneInPlace(int[] x, int carry)
        {
            int length = x.Length;
            while (--length >= 0)
            {
                uint num2 = (uint) x[length];
                x[length] = ((int) (num2 << 1)) | carry;
                carry = (int) (num2 >> 0x1f);
            }
            return carry;
        }

        public BigInteger ShiftRight(int n)
        {
            if (n == 0)
            {
                return this;
            }
            if (n < 0)
            {
                return this.ShiftLeft(-n);
            }
            if (n >= this.BitLength)
            {
                return ((this.sign >= 0) ? Zero : One.Negate());
            }
            int num = ((this.BitLength - n) + 0x1f) >> 5;
            int[] destinationArray = new int[num];
            int num2 = n >> 5;
            int num3 = n & 0x1f;
            if (num3 == 0)
            {
                Array.Copy(this.magnitude, 0, destinationArray, 0, destinationArray.Length);
            }
            else
            {
                int num4 = 0x20 - num3;
                int index = (this.magnitude.Length - 1) - num2;
                for (int i = num - 1; i >= 0; i--)
                {
                    destinationArray[i] = this.magnitude[index--] >> num3;
                    if (index >= 0)
                    {
                        destinationArray[i] |= this.magnitude[index] << num4;
                    }
                }
            }
            return new BigInteger(this.sign, destinationArray, false);
        }

        private static void ShiftRightInPlace(int start, int[] mag, int n)
        {
            int index = (n >> 5) + start;
            int num2 = n & 0x1f;
            int num3 = mag.Length - 1;
            if (index != start)
            {
                int num4 = index - start;
                for (int i = num3; i >= index; i--)
                {
                    mag[i] = mag[i - num4];
                }
                for (int j = index - 1; j >= start; j--)
                {
                    mag[j] = 0;
                }
            }
            if (num2 != 0)
            {
                int num7 = 0x20 - num2;
                int num8 = mag[num3];
                for (int i = num3; i > index; i--)
                {
                    int num10 = mag[i - 1];
                    mag[i] = (num8 >> num2) | (num10 << num7);
                    num8 = num10;
                }
                mag[index] = mag[index] >> num2;
            }
        }

        private static void ShiftRightOneInPlace(int start, int[] mag)
        {
            int num3;
            int length = mag.Length;
            for (int i = mag[length - 1]; --length > start; i = num3)
            {
                num3 = mag[length - 1];
                mag[length] = (i >> 1) | (num3 << 0x1f);
            }
            mag[start] = mag[start] >> 1;
        }

        public BigInteger Square()
        {
            if (this.sign == 0)
            {
                return Zero;
            }
            if (this.QuickPow2Check())
            {
                return this.ShiftLeft(this.Abs().BitLength - 1);
            }
            int num = this.magnitude.Length << 1;
            if ((this.magnitude[0] >> 0x10) == 0)
            {
                num--;
            }
            int[] w = new int[num];
            Square(w, this.magnitude);
            return new BigInteger(1, w, false);
        }

        private static int[] Square(int[] w, int[] x)
        {
            ulong num;
            int index = w.Length - 1;
            for (int i = x.Length - 1; i > 0; i--)
            {
                ulong num4 = (ulong) x[i];
                num = (num4 * num4) + ((ulong) w[index]);
                w[index] = (int) num;
                num = num >> 0x20;
                for (int j = i - 1; j >= 0; j--)
                {
                    ulong num6 = num4 * ((ulong) x[j]);
                    num += (((ulong) w[--index]) & 0xffffffffL) + (((uint) num6) << 1);
                    w[index] = (int) num;
                    num = (num >> 0x20) + (num6 >> 0x1f);
                }
                num += (ulong) w[--index];
                w[index] = (int) num;
                if (--index >= 0)
                {
                    w[index] = (int) (num >> 0x20);
                }
                index += i;
            }
            num = (ulong) x[0];
            num = (num * num) + ((ulong) w[index]);
            w[index] = (int) num;
            if (--index >= 0)
            {
                w[index] += (int) (num >> 0x20);
            }
            return w;
        }

        private static void SquareMonty(int[] a, int[] x, int[] m, uint mDash, bool smallMontyModulus)
        {
            int length = m.Length;
            if (length == 1)
            {
                uint num2 = (uint) x[0];
                x[0] = (int) MultiplyMontyNIsOne(num2, num2, (uint) m[0], mDash);
            }
            else
            {
                ulong num3 = (ulong) x[length - 1];
                ulong num5 = num3 * num3;
                ulong num6 = ((uint) num5) * mDash;
                ulong num7 = num6 * ((ulong) m[length - 1]);
                num5 += (uint) num7;
                num5 = (num5 >> 0x20) + (num7 >> 0x20);
                for (int i = length - 2; i >= 0; i--)
                {
                    ulong num9 = num3 * ((ulong) x[i]);
                    num7 = num6 * ((ulong) m[i]);
                    num5 += (num7 & 0xffffffffL) + (((uint) num9) << 1);
                    a[i + 2] = (int) num5;
                    num5 = ((num5 >> 0x20) + (num9 >> 0x1f)) + (num7 >> 0x20);
                }
                a[1] = (int) num5;
                int num4 = (int) (num5 >> 0x20);
                for (int j = length - 2; j >= 0; j--)
                {
                    uint num11 = (uint) a[length];
                    ulong num12 = num11 * mDash;
                    ulong num13 = (num12 * ((ulong) m[length - 1])) + num11;
                    num13 = num13 >> 0x20;
                    for (int k = length - 2; k > j; k--)
                    {
                        num13 += (num12 * ((ulong) m[k])) + ((ulong) a[k + 1]);
                        a[k + 2] = (int) num13;
                        num13 = num13 >> 0x20;
                    }
                    ulong num15 = (ulong) x[j];
                    ulong num16 = num15 * num15;
                    ulong num17 = num12 * ((ulong) m[j]);
                    num13 += ((num16 & 0xffffffffL) + ((uint) num17)) + ((ulong) a[j + 1]);
                    a[j + 2] = (int) num13;
                    num13 = ((num13 >> 0x20) + (num16 >> 0x20)) + (num17 >> 0x20);
                    for (int n = j - 1; n >= 0; n--)
                    {
                        ulong num19 = num15 * ((ulong) x[n]);
                        ulong num20 = num12 * ((ulong) m[n]);
                        num13 += ((num20 & 0xffffffffL) + (((uint) num19) << 1)) + ((ulong) a[n + 1]);
                        a[n + 2] = (int) num13;
                        num13 = ((num13 >> 0x20) + (num19 >> 0x1f)) + (num20 >> 0x20);
                    }
                    num13 += (ulong) num4;
                    a[1] = (int) num13;
                    num4 = (int) (num13 >> 0x20);
                }
                a[0] = num4;
                if (!smallMontyModulus && (CompareTo(0, a, 0, m) >= 0))
                {
                    Subtract(0, a, 0, m);
                }
                Array.Copy(a, 1, x, 0, length);
            }
        }

        public BigInteger Subtract(BigInteger n)
        {
            BigInteger integer;
            BigInteger integer2;
            if (n.sign == 0)
            {
                return this;
            }
            if (this.sign == 0)
            {
                return n.Negate();
            }
            if (this.sign != n.sign)
            {
                return this.Add(n.Negate());
            }
            int num = CompareNoLeadingZeroes(0, this.magnitude, 0, n.magnitude);
            if (num == 0)
            {
                return Zero;
            }
            if (num < 0)
            {
                integer = n;
                integer2 = this;
            }
            else
            {
                integer = this;
                integer2 = n;
            }
            return new BigInteger(this.sign * num, doSubBigLil(integer.magnitude, integer2.magnitude), true);
        }

        private static int[] Subtract(int xStart, int[] x, int yStart, int[] y)
        {
            int length = x.Length;
            int num2 = y.Length;
            int num4 = 0;
            do
            {
                long num3 = ((x[--length] & 0xffffffffL) - (y[--num2] & 0xffffffffL)) + num4;
                x[length] = (int) num3;
                num4 = (int) (num3 >> 0x3f);
            }
            while (num2 > yStart);
            if (num4 != 0)
            {
                while (--x[--length] == -1)
                {
                }
            }
            return x;
        }

        public bool TestBit(int n)
        {
            if (n < 0)
            {
                throw new ArithmeticException("Bit position must not be negative");
            }
            if (this.sign < 0)
            {
                return !this.Not().TestBit(n);
            }
            int num = n / 0x20;
            if (num >= this.magnitude.Length)
            {
                return false;
            }
            int num2 = this.magnitude[(this.magnitude.Length - 1) - num];
            return (((num2 >> (n % 0x20)) & 1) > 0);
        }

        public byte[] ToByteArray() => 
            this.ToByteArray(false);

        private byte[] ToByteArray(bool unsigned)
        {
            if (this.sign == 0)
            {
                return (!unsigned ? new byte[1] : ZeroEncoding);
            }
            int nBits = (!unsigned || (this.sign <= 0)) ? (this.BitLength + 1) : this.BitLength;
            byte[] buffer = new byte[GetByteLength(nBits)];
            int length = this.magnitude.Length;
            int num4 = buffer.Length;
            if (this.sign > 0)
            {
                while (length > 1)
                {
                    uint num5 = (uint) this.magnitude[--length];
                    buffer[--num4] = (byte) num5;
                    buffer[--num4] = (byte) (num5 >> 8);
                    buffer[--num4] = (byte) (num5 >> 0x10);
                    buffer[--num4] = (byte) (num5 >> 0x18);
                }
                uint num6 = (uint) this.magnitude[0];
                while (num6 > 0xff)
                {
                    buffer[--num4] = (byte) num6;
                    num6 = num6 >> 8;
                }
                buffer[--num4] = (byte) num6;
                return buffer;
            }
            bool flag = true;
            while (length > 1)
            {
                uint num7 = (uint) ~this.magnitude[--length];
                if (flag)
                {
                    flag = ++num7 == 0;
                }
                buffer[--num4] = (byte) num7;
                buffer[--num4] = (byte) (num7 >> 8);
                buffer[--num4] = (byte) (num7 >> 0x10);
                buffer[--num4] = (byte) (num7 >> 0x18);
            }
            uint num8 = (uint) this.magnitude[0];
            if (flag)
            {
                num8--;
            }
            while (num8 > 0xff)
            {
                buffer[--num4] = (byte) ~num8;
                num8 = num8 >> 8;
            }
            buffer[--num4] = (byte) ~num8;
            if (num4 > 0)
            {
                buffer[--num4] = 0xff;
            }
            return buffer;
        }

        public byte[] ToByteArrayUnsigned() => 
            this.ToByteArray(true);

        public override string ToString() => 
            this.ToString(10);

        public string ToString(int radix)
        {
            switch (radix)
            {
                case 8:
                case 10:
                    break;

                default:
                    if ((radix != 2) && (radix != 0x10))
                    {
                        throw new FormatException("Only bases 2, 8, 10, 16 are allowed");
                    }
                    break;
            }
            if (this.magnitude == null)
            {
                return "null";
            }
            if (this.sign == 0)
            {
                return "0";
            }
            int index = 0;
            while (index < this.magnitude.Length)
            {
                if (this.magnitude[index] != 0)
                {
                    break;
                }
                index++;
            }
            if (index == this.magnitude.Length)
            {
                return "0";
            }
            StringBuilder sb = new StringBuilder();
            if (this.sign == -1)
            {
                sb.Append('-');
            }
            switch (radix)
            {
                case 8:
                {
                    int num3 = 0x3fffffff;
                    BigInteger integer = this.Abs();
                    int bitLength = integer.BitLength;
                    IList list = Platform.CreateArrayList();
                    while (bitLength > 30)
                    {
                        list.Add(Convert.ToString((int) (integer.IntValue & num3), 8));
                        integer = integer.ShiftRight(30);
                        bitLength -= 30;
                    }
                    sb.Append(Convert.ToString(integer.IntValue, 8));
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        AppendZeroExtendedString(sb, (string) list[i], 10);
                    }
                    break;
                }
                case 10:
                {
                    BigInteger integer2 = this.Abs();
                    if (integer2.BitLength >= 0x40)
                    {
                        long num7 = 0x7fffffffffffffffL / ((long) radix);
                        long num8 = radix;
                        int minLength = 1;
                        while (num8 <= num7)
                        {
                            num8 *= radix;
                            minLength++;
                        }
                        BigInteger integer3 = ValueOf(num8);
                        IList list2 = Platform.CreateArrayList();
                        while (integer2.CompareTo(integer3) >= 0)
                        {
                            BigInteger[] integerArray = integer2.DivideAndRemainder(integer3);
                            list2.Add(Convert.ToString(integerArray[1].LongValue, radix));
                            integer2 = integerArray[0];
                        }
                        sb.Append(Convert.ToString(integer2.LongValue, radix));
                        for (int i = list2.Count - 1; i >= 0; i--)
                        {
                            AppendZeroExtendedString(sb, (string) list2[i], minLength);
                        }
                    }
                    else
                    {
                        sb.Append(Convert.ToString(integer2.LongValue, radix));
                    }
                    break;
                }
                default:
                    if (radix != 2)
                    {
                        if (radix == 0x10)
                        {
                            int num6 = index;
                            sb.Append(Convert.ToString(this.magnitude[num6], 0x10));
                            while (++num6 < this.magnitude.Length)
                            {
                                AppendZeroExtendedString(sb, Convert.ToString(this.magnitude[num6], 0x10), 8);
                            }
                        }
                    }
                    else
                    {
                        int num2 = index;
                        sb.Append(Convert.ToString(this.magnitude[num2], 2));
                        while (++num2 < this.magnitude.Length)
                        {
                            AppendZeroExtendedString(sb, Convert.ToString(this.magnitude[num2], 2), 0x20);
                        }
                    }
                    break;
            }
            return sb.ToString();
        }

        public static BigInteger ValueOf(long value)
        {
            if ((value >= 0L) && (value < SMALL_CONSTANTS.Length))
            {
                return SMALL_CONSTANTS[(int) ((IntPtr) value)];
            }
            return CreateValueOf(value);
        }

        public BigInteger Xor(BigInteger value)
        {
            if (this.sign == 0)
            {
                return value;
            }
            if (value.sign == 0)
            {
                return this;
            }
            int[] numArray = (this.sign <= 0) ? this.Add(One).magnitude : this.magnitude;
            int[] numArray2 = (value.sign <= 0) ? value.Add(One).magnitude : value.magnitude;
            bool flag = ((this.sign < 0) && (value.sign >= 0)) || ((this.sign >= 0) && (value.sign < 0));
            int[] mag = new int[Math.Max(numArray.Length, numArray2.Length)];
            int num2 = mag.Length - numArray.Length;
            int num3 = mag.Length - numArray2.Length;
            for (int i = 0; i < mag.Length; i++)
            {
                int num5 = (i < num2) ? 0 : numArray[i - num2];
                int num6 = (i < num3) ? 0 : numArray2[i - num3];
                if (this.sign < 0)
                {
                    num5 = ~num5;
                }
                if (value.sign < 0)
                {
                    num6 = ~num6;
                }
                mag[i] = num5 ^ num6;
                if (flag)
                {
                    mag[i] = ~mag[i];
                }
            }
            BigInteger integer = new BigInteger(1, mag, true);
            if (flag)
            {
                integer = integer.Not();
            }
            return integer;
        }

        private static void ZeroOut(int[] x)
        {
            Array.Clear(x, 0, x.Length);
        }

        public int BitCount
        {
            get
            {
                if (this.nBits == -1)
                {
                    if (this.sign < 0)
                    {
                        this.nBits = this.Not().BitCount;
                    }
                    else
                    {
                        int num = 0;
                        for (int i = 0; i < this.magnitude.Length; i++)
                        {
                            num += BitCnt(this.magnitude[i]);
                        }
                        this.nBits = num;
                    }
                }
                return this.nBits;
            }
        }

        public int BitLength
        {
            get
            {
                if (this.nBitLength == -1)
                {
                    this.nBitLength = (this.sign != 0) ? CalcBitLength(this.sign, 0, this.magnitude) : 0;
                }
                return this.nBitLength;
            }
        }

        public int IntValue
        {
            get
            {
                if (this.sign == 0)
                {
                    return 0;
                }
                int length = this.magnitude.Length;
                int num2 = this.magnitude[length - 1];
                return ((this.sign >= 0) ? num2 : -num2);
            }
        }

        public long LongValue
        {
            get
            {
                if (this.sign == 0)
                {
                    return 0L;
                }
                int length = this.magnitude.Length;
                long num2 = this.magnitude[length - 1] & 0xffffffffL;
                if (length > 1)
                {
                    num2 |= (this.magnitude[length - 2] & 0xffffffffL) << 0x20;
                }
                return ((this.sign >= 0) ? num2 : -num2);
            }
        }

        public int SignValue =>
            this.sign;
    }
}

