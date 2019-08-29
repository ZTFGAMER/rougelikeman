namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Net;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class GeneralName : Asn1Encodable, IAsn1Choice
    {
        public const int OtherName = 0;
        public const int Rfc822Name = 1;
        public const int DnsName = 2;
        public const int X400Address = 3;
        public const int DirectoryName = 4;
        public const int EdiPartyName = 5;
        public const int UniformResourceIdentifier = 6;
        public const int IPAddress = 7;
        public const int RegisteredID = 8;
        internal readonly Asn1Encodable obj;
        internal readonly int tag;

        public GeneralName(X509Name directoryName)
        {
            this.obj = directoryName;
            this.tag = 4;
        }

        public GeneralName(Asn1Object name, int tag)
        {
            this.obj = name;
            this.tag = tag;
        }

        public GeneralName(int tag, Asn1Encodable name)
        {
            this.obj = name;
            this.tag = tag;
        }

        public GeneralName(int tag, string name)
        {
            this.tag = tag;
            if (((tag == 1) || (tag == 2)) || (tag == 6))
            {
                this.obj = new DerIA5String(name);
            }
            else if (tag == 8)
            {
                this.obj = new DerObjectIdentifier(name);
            }
            else if (tag == 4)
            {
                this.obj = new X509Name(name);
            }
            else
            {
                if (tag != 7)
                {
                    throw new ArgumentException("can't process string for tag: " + tag, "tag");
                }
                byte[] str = this.toGeneralNameEncoding(name);
                if (str == null)
                {
                    throw new ArgumentException("IP Address is invalid", "name");
                }
                this.obj = new DerOctetString(str);
            }
        }

        private void copyInts(int[] parsedIp, byte[] addr, int offSet)
        {
            for (int i = 0; i != parsedIp.Length; i++)
            {
                addr[(i * 2) + offSet] = (byte) (parsedIp[i] >> 8);
                addr[((i * 2) + 1) + offSet] = (byte) parsedIp[i];
            }
        }

        public static GeneralName GetInstance(object obj)
        {
            if ((obj == null) || (obj is GeneralName))
            {
                return (GeneralName) obj;
            }
            if (obj is Asn1TaggedObject)
            {
                Asn1TaggedObject obj2 = (Asn1TaggedObject) obj;
                int tagNo = obj2.TagNo;
                switch (tagNo)
                {
                    case 0:
                        return new GeneralName(tagNo, Asn1Sequence.GetInstance(obj2, false));

                    case 1:
                        return new GeneralName(tagNo, DerIA5String.GetInstance(obj2, false));

                    case 2:
                        return new GeneralName(tagNo, DerIA5String.GetInstance(obj2, false));

                    case 3:
                        throw new ArgumentException("unknown tag: " + tagNo);

                    case 4:
                        return new GeneralName(tagNo, X509Name.GetInstance(obj2, true));

                    case 5:
                        return new GeneralName(tagNo, Asn1Sequence.GetInstance(obj2, false));

                    case 6:
                        return new GeneralName(tagNo, DerIA5String.GetInstance(obj2, false));

                    case 7:
                        return new GeneralName(tagNo, Asn1OctetString.GetInstance(obj2, false));

                    case 8:
                        return new GeneralName(tagNo, DerObjectIdentifier.GetInstance(obj2, false));
                }
            }
            if (obj is byte[])
            {
                try
                {
                    return GetInstance(Asn1Object.FromByteArray((byte[]) obj));
                }
                catch (IOException)
                {
                    throw new ArgumentException("unable to parse encoded general name");
                }
            }
            throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
        }

        public static GeneralName GetInstance(Asn1TaggedObject tagObj, bool explicitly) => 
            GetInstance(Asn1TaggedObject.GetInstance(tagObj, true));

        private void parseIPv4(string ip, byte[] addr, int offset)
        {
            char[] separator = new char[] { '.', '/' };
            foreach (string str in ip.Split(separator))
            {
                addr[offset++] = (byte) int.Parse(str);
            }
        }

        private void parseIPv4Mask(string mask, byte[] addr, int offset)
        {
            int num = int.Parse(mask);
            for (int i = 0; i != num; i++)
            {
                addr[(i / 8) + offset] = (byte) (addr[(i / 8) + offset] | ((byte) (((int) 1) << (i % 8))));
            }
        }

        private int[] parseIPv6(string ip)
        {
            if (Platform.StartsWith(ip, "::"))
            {
                ip = ip.Substring(1);
            }
            else if (Platform.EndsWith(ip, "::"))
            {
                ip = ip.Substring(0, ip.Length - 1);
            }
            char[] separator = new char[] { ':' };
            IEnumerator enumerator = ip.Split(separator).GetEnumerator();
            int num = 0;
            int[] sourceArray = new int[8];
            int sourceIndex = -1;
            while (enumerator.MoveNext())
            {
                string current = (string) enumerator.Current;
                if (current.Length == 0)
                {
                    sourceIndex = num;
                    sourceArray[num++] = 0;
                }
                else
                {
                    if (current.IndexOf('.') < 0)
                    {
                        sourceArray[num++] = int.Parse(current, NumberStyles.AllowHexSpecifier);
                        continue;
                    }
                    char[] chArray2 = new char[] { '.' };
                    string[] strArray = current.Split(chArray2);
                    sourceArray[num++] = (int.Parse(strArray[0]) << 8) | int.Parse(strArray[1]);
                    sourceArray[num++] = (int.Parse(strArray[2]) << 8) | int.Parse(strArray[3]);
                }
            }
            if (num != sourceArray.Length)
            {
                Array.Copy(sourceArray, sourceIndex, sourceArray, sourceArray.Length - (num - sourceIndex), num - sourceIndex);
                for (int i = sourceIndex; i != (sourceArray.Length - (num - sourceIndex)); i++)
                {
                    sourceArray[i] = 0;
                }
            }
            return sourceArray;
        }

        private int[] parseMask(string mask)
        {
            int[] numArray = new int[8];
            int num = int.Parse(mask);
            for (int i = 0; i != num; i++)
            {
                numArray[i / 0x10] |= ((int) 1) << (i % 0x10);
            }
            return numArray;
        }

        public override Asn1Object ToAsn1Object() => 
            new DerTaggedObject(this.tag == 4, this.tag, this.obj);

        private byte[] toGeneralNameEncoding(string ip)
        {
            if (Org.BouncyCastle.Utilities.Net.IPAddress.IsValidIPv6WithNetmask(ip) || Org.BouncyCastle.Utilities.Net.IPAddress.IsValidIPv6(ip))
            {
                int length = ip.IndexOf('/');
                if (length < 0)
                {
                    byte[] buffer = new byte[0x10];
                    int[] numArray = this.parseIPv6(ip);
                    this.copyInts(numArray, buffer, 0);
                    return buffer;
                }
                byte[] buffer2 = new byte[0x20];
                int[] parsedIp = this.parseIPv6(ip.Substring(0, length));
                this.copyInts(parsedIp, buffer2, 0);
                string str = ip.Substring(length + 1);
                if (str.IndexOf(':') > 0)
                {
                    parsedIp = this.parseIPv6(str);
                }
                else
                {
                    parsedIp = this.parseMask(str);
                }
                this.copyInts(parsedIp, buffer2, 0x10);
                return buffer2;
            }
            if (!Org.BouncyCastle.Utilities.Net.IPAddress.IsValidIPv4WithNetmask(ip) && !Org.BouncyCastle.Utilities.Net.IPAddress.IsValidIPv4(ip))
            {
                return null;
            }
            int index = ip.IndexOf('/');
            if (index < 0)
            {
                byte[] buffer3 = new byte[4];
                this.parseIPv4(ip, buffer3, 0);
                return buffer3;
            }
            byte[] addr = new byte[8];
            this.parseIPv4(ip.Substring(0, index), addr, 0);
            string str2 = ip.Substring(index + 1);
            if (str2.IndexOf('.') > 0)
            {
                this.parseIPv4(str2, addr, 4);
                return addr;
            }
            this.parseIPv4Mask(str2, addr, 4);
            return addr;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.tag);
            builder.Append(": ");
            switch (this.tag)
            {
                case 1:
                case 2:
                case 6:
                    builder.Append(DerIA5String.GetInstance(this.obj).GetString());
                    break;

                case 4:
                    builder.Append(X509Name.GetInstance(this.obj).ToString());
                    break;

                default:
                    builder.Append(this.obj.ToString());
                    break;
            }
            return builder.ToString();
        }

        public int TagNo =>
            this.tag;

        public Asn1Encodable Name =>
            this.obj;
    }
}

