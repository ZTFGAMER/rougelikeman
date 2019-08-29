namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;
    using System.Text;

    public class DerObjectIdentifier : Asn1Object
    {
        private readonly string identifier;
        private byte[] body;
        private const long LONG_LIMIT = 0xffffffffffff80L;
        private static readonly DerObjectIdentifier[] cache = new DerObjectIdentifier[0x400];

        public DerObjectIdentifier(string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier");
            }
            if (!IsValidIdentifier(identifier))
            {
                throw new FormatException("string " + identifier + " not an OID");
            }
            this.identifier = identifier;
        }

        internal DerObjectIdentifier(byte[] bytes)
        {
            this.identifier = MakeOidStringFromBytes(bytes);
            this.body = Arrays.Clone(bytes);
        }

        internal DerObjectIdentifier(DerObjectIdentifier oid, string branchID)
        {
            if (!IsValidBranchID(branchID, 0))
            {
                throw new ArgumentException("string " + branchID + " not a valid OID branch", "branchID");
            }
            this.identifier = oid.Id + "." + branchID;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerObjectIdentifier identifier = asn1Object as DerObjectIdentifier;
            if (identifier == null)
            {
                return false;
            }
            return this.identifier.Equals(identifier.identifier);
        }

        protected override int Asn1GetHashCode() => 
            this.identifier.GetHashCode();

        public virtual DerObjectIdentifier Branch(string branchID) => 
            new DerObjectIdentifier(this, branchID);

        private void DoOutput(MemoryStream bOut)
        {
            OidTokenizer tokenizer = new OidTokenizer(this.identifier);
            int num = int.Parse(tokenizer.NextToken()) * 40;
            string s = tokenizer.NextToken();
            if (s.Length <= 0x12)
            {
                this.WriteField(bOut, (long) (num + long.Parse(s)));
            }
            else
            {
                this.WriteField(bOut, new BigInteger(s).Add(BigInteger.ValueOf((long) num)));
            }
            while (tokenizer.HasMoreTokens)
            {
                s = tokenizer.NextToken();
                if (s.Length <= 0x12)
                {
                    this.WriteField(bOut, long.Parse(s));
                }
                else
                {
                    this.WriteField(bOut, new BigInteger(s));
                }
            }
        }

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(6, this.GetBody());
        }

        internal static DerObjectIdentifier FromOctetString(byte[] enc)
        {
            int index = Arrays.GetHashCode(enc) & 0x3ff;
            object cache = DerObjectIdentifier.cache;
            lock (cache)
            {
                DerObjectIdentifier identifier = DerObjectIdentifier.cache[index];
                if ((identifier != null) && Arrays.AreEqual(enc, identifier.GetBody()))
                {
                    return identifier;
                }
                return (DerObjectIdentifier.cache[index] = new DerObjectIdentifier(enc));
            }
        }

        internal byte[] GetBody()
        {
            object obj2 = this;
            lock (obj2)
            {
                if (this.body == null)
                {
                    MemoryStream bOut = new MemoryStream();
                    this.DoOutput(bOut);
                    this.body = bOut.ToArray();
                }
            }
            return this.body;
        }

        public static DerObjectIdentifier GetInstance(object obj)
        {
            if ((obj == null) || (obj is DerObjectIdentifier))
            {
                return (DerObjectIdentifier) obj;
            }
            if (!(obj is byte[]))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
            }
            return FromOctetString((byte[]) obj);
        }

        public static DerObjectIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(obj.GetObject());

        private static bool IsValidBranchID(string branchID, int start)
        {
            bool flag = false;
            int length = branchID.Length;
            while (--length >= start)
            {
                char ch = branchID[length];
                if (('0' <= ch) && (ch <= '9'))
                {
                    flag = true;
                }
                else
                {
                    if (ch != '.')
                    {
                        return false;
                    }
                    if (!flag)
                    {
                        return false;
                    }
                    flag = false;
                    continue;
                }
            }
            return flag;
        }

        private static bool IsValidIdentifier(string identifier)
        {
            if ((identifier.Length < 3) || (identifier[1] != '.'))
            {
                return false;
            }
            char ch = identifier[0];
            return (((ch >= '0') && (ch <= '2')) && IsValidBranchID(identifier, 2));
        }

        private static string MakeOidStringFromBytes(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            long num = 0L;
            BigInteger integer = null;
            bool flag = true;
            for (int i = 0; i != bytes.Length; i++)
            {
                int num3 = bytes[i];
                if (num <= 0xffffffffffff80L)
                {
                    num += num3 & 0x7f;
                    if ((num3 & 0x80) == 0)
                    {
                        if (flag)
                        {
                            if (num < 40L)
                            {
                                builder.Append('0');
                            }
                            else if (num < 80L)
                            {
                                builder.Append('1');
                                num -= 40L;
                            }
                            else
                            {
                                builder.Append('2');
                                num -= 80L;
                            }
                            flag = false;
                        }
                        builder.Append('.');
                        builder.Append(num);
                        num = 0L;
                    }
                    else
                    {
                        num = num << 7;
                    }
                }
                else
                {
                    if (integer == null)
                    {
                        integer = BigInteger.ValueOf(num);
                    }
                    integer = integer.Or(BigInteger.ValueOf((long) (num3 & 0x7f)));
                    if ((num3 & 0x80) == 0)
                    {
                        if (flag)
                        {
                            builder.Append('2');
                            integer = integer.Subtract(BigInteger.ValueOf(80L));
                            flag = false;
                        }
                        builder.Append('.');
                        builder.Append(integer);
                        integer = null;
                        num = 0L;
                    }
                    else
                    {
                        integer = integer.ShiftLeft(7);
                    }
                }
            }
            return builder.ToString();
        }

        public virtual bool On(DerObjectIdentifier stem)
        {
            string id = this.Id;
            string prefix = stem.Id;
            return (((id.Length > prefix.Length) && (id[prefix.Length] == '.')) && Platform.StartsWith(id, prefix));
        }

        public override string ToString() => 
            this.identifier;

        private void WriteField(Stream outputStream, BigInteger fieldValue)
        {
            int num = (fieldValue.BitLength + 6) / 7;
            if (num == 0)
            {
                outputStream.WriteByte(0);
            }
            else
            {
                BigInteger integer = fieldValue;
                byte[] buffer = new byte[num];
                for (int i = num - 1; i >= 0; i--)
                {
                    buffer[i] = (byte) ((integer.IntValue & 0x7f) | 0x80);
                    integer = integer.ShiftRight(7);
                }
                buffer[num - 1] = (byte) (buffer[num - 1] & 0x7f);
                outputStream.Write(buffer, 0, buffer.Length);
            }
        }

        private void WriteField(Stream outputStream, long fieldValue)
        {
            byte[] buffer = new byte[9];
            int index = 8;
            buffer[index] = (byte) (fieldValue & 0x7fL);
            while (fieldValue >= 0x80L)
            {
                fieldValue = fieldValue >> 7;
                buffer[--index] = (byte) ((fieldValue & 0x7fL) | 0x80L);
            }
            outputStream.Write(buffer, index, 9 - index);
        }

        public string Id =>
            this.identifier;
    }
}

