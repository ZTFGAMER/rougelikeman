namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public class DerBitString : DerStringBase
    {
        private static readonly char[] table = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        protected readonly byte[] mData;
        protected readonly int mPadBits;

        public DerBitString(byte[] data) : this(data, 0)
        {
        }

        public DerBitString(Asn1Encodable obj) : this(obj.GetDerEncoded())
        {
        }

        public DerBitString(int namedBits)
        {
            if (namedBits == 0)
            {
                this.mData = new byte[0];
                this.mPadBits = 0;
            }
            else
            {
                int index = (BigInteger.BitLen(namedBits) + 7) / 8;
                byte[] buffer = new byte[index];
                index--;
                for (int i = 0; i < index; i++)
                {
                    buffer[i] = (byte) namedBits;
                    namedBits = namedBits >> 8;
                }
                buffer[index] = (byte) namedBits;
                int num4 = 0;
                while ((namedBits & (((int) 1) << num4)) == 0)
                {
                    num4++;
                }
                this.mData = buffer;
                this.mPadBits = num4;
            }
        }

        public DerBitString(byte[] data, int padBits)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if ((padBits < 0) || (padBits > 7))
            {
                throw new ArgumentException("must be in the range 0 to 7", "padBits");
            }
            if ((data.Length == 0) && (padBits != 0))
            {
                throw new ArgumentException("if 'data' is empty, 'padBits' must be 0");
            }
            this.mData = Arrays.Clone(data);
            this.mPadBits = padBits;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerBitString str = asn1Object as DerBitString;
            if (str == null)
            {
                return false;
            }
            return ((this.mPadBits == str.mPadBits) && Arrays.AreEqual(this.mData, str.mData));
        }

        protected override int Asn1GetHashCode() => 
            (this.mPadBits.GetHashCode() ^ Arrays.GetHashCode(this.mData));

        internal override void Encode(DerOutputStream derOut)
        {
            if (this.mPadBits > 0)
            {
                int num = this.mData[this.mData.Length - 1];
                int num2 = (((int) 1) << this.mPadBits) - 1;
                int num3 = num & num2;
                if (num3 != 0)
                {
                    byte[] bytes = Arrays.Prepend(this.mData, (byte) this.mPadBits);
                    bytes[bytes.Length - 1] = (byte) (num ^ num3);
                    derOut.WriteEncoded(3, bytes);
                    return;
                }
            }
            derOut.WriteEncoded(3, (byte) this.mPadBits, this.mData);
        }

        internal static DerBitString FromAsn1Octets(byte[] octets)
        {
            if (octets.Length < 1)
            {
                throw new ArgumentException("truncated BIT STRING detected", "octets");
            }
            int padBits = octets[0];
            byte[] data = Arrays.CopyOfRange(octets, 1, octets.Length);
            if (((padBits > 0) && (padBits < 8)) && (data.Length > 0))
            {
                int num2 = data[data.Length - 1];
                int num3 = (((int) 1) << padBits) - 1;
                if ((num2 & num3) != 0)
                {
                    return new BerBitString(data, padBits);
                }
            }
            return new DerBitString(data, padBits);
        }

        public virtual byte[] GetBytes()
        {
            byte[] buffer = Arrays.Clone(this.mData);
            if (this.mPadBits > 0)
            {
                buffer[buffer.Length - 1] = (byte) (buffer[buffer.Length - 1] & ((byte) (((int) 0xff) << this.mPadBits)));
            }
            return buffer;
        }

        public static DerBitString GetInstance(object obj)
        {
            if ((obj == null) || (obj is DerBitString))
            {
                return (DerBitString) obj;
            }
            if (obj is byte[])
            {
                try
                {
                    return (DerBitString) Asn1Object.FromByteArray((byte[]) obj);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("encoding error in GetInstance: " + exception.ToString());
                }
            }
            throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
        }

        public static DerBitString GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerBitString))
            {
                return FromAsn1Octets(((Asn1OctetString) obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public virtual byte[] GetOctets()
        {
            if (this.mPadBits != 0)
            {
                throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
            }
            return Arrays.Clone(this.mData);
        }

        public override string GetString()
        {
            StringBuilder builder = new StringBuilder("#");
            byte[] derEncoded = base.GetDerEncoded();
            for (int i = 0; i != derEncoded.Length; i++)
            {
                uint num2 = derEncoded[i];
                builder.Append(table[(int) ((IntPtr) ((num2 >> 4) & 15))]);
                builder.Append(table[derEncoded[i] & 15]);
            }
            return builder.ToString();
        }

        public virtual int PadBits =>
            this.mPadBits;

        public virtual int IntValue
        {
            get
            {
                int num = 0;
                int num2 = Math.Min(4, this.mData.Length);
                for (int i = 0; i < num2; i++)
                {
                    num |= this.mData[i] << (8 * i);
                }
                if ((this.mPadBits > 0) && (num2 == this.mData.Length))
                {
                    int num4 = (((int) 1) << this.mPadBits) - 1;
                    num &= ~(num4 << (8 * (num2 - 1)));
                }
                return num;
            }
        }
    }
}

