namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class DerApplicationSpecific : Asn1Object
    {
        private readonly bool isConstructed;
        private readonly int tag;
        private readonly byte[] octets;

        public DerApplicationSpecific(int tag, byte[] octets) : this(false, tag, octets)
        {
        }

        public DerApplicationSpecific(int tag, Asn1Encodable obj) : this(true, tag, obj)
        {
        }

        public DerApplicationSpecific(int tagNo, Asn1EncodableVector vec)
        {
            this.tag = tagNo;
            this.isConstructed = true;
            MemoryStream stream = new MemoryStream();
            for (int i = 0; i != vec.Count; i++)
            {
                try
                {
                    byte[] derEncoded = vec[i].GetDerEncoded();
                    stream.Write(derEncoded, 0, derEncoded.Length);
                }
                catch (IOException exception)
                {
                    throw new InvalidOperationException("malformed object", exception);
                }
            }
            this.octets = stream.ToArray();
        }

        internal DerApplicationSpecific(bool isConstructed, int tag, byte[] octets)
        {
            this.isConstructed = isConstructed;
            this.tag = tag;
            this.octets = octets;
        }

        public DerApplicationSpecific(bool isExplicit, int tag, Asn1Encodable obj)
        {
            Asn1Object obj2 = obj.ToAsn1Object();
            byte[] derEncoded = obj2.GetDerEncoded();
            this.isConstructed = Asn1TaggedObject.IsConstructed(isExplicit, obj2);
            this.tag = tag;
            if (isExplicit)
            {
                this.octets = derEncoded;
            }
            else
            {
                int lengthOfHeader = this.GetLengthOfHeader(derEncoded);
                byte[] destinationArray = new byte[derEncoded.Length - lengthOfHeader];
                Array.Copy(derEncoded, lengthOfHeader, destinationArray, 0, destinationArray.Length);
                this.octets = destinationArray;
            }
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerApplicationSpecific specific = asn1Object as DerApplicationSpecific;
            if (specific == null)
            {
                return false;
            }
            return (((this.isConstructed == specific.isConstructed) && (this.tag == specific.tag)) && Arrays.AreEqual(this.octets, specific.octets));
        }

        protected override int Asn1GetHashCode() => 
            ((this.isConstructed.GetHashCode() ^ this.tag.GetHashCode()) ^ Arrays.GetHashCode(this.octets));

        internal override void Encode(DerOutputStream derOut)
        {
            int flags = 0x40;
            if (this.isConstructed)
            {
                flags |= 0x20;
            }
            derOut.WriteEncoded(flags, this.tag, this.octets);
        }

        public byte[] GetContents() => 
            this.octets;

        private int GetLengthOfHeader(byte[] data)
        {
            int num = data[1];
            if (num == 0x80)
            {
                return 2;
            }
            if (num <= 0x7f)
            {
                return 2;
            }
            int num2 = num & 0x7f;
            if (num2 > 4)
            {
                throw new InvalidOperationException("DER length more than 4 bytes: " + num2);
            }
            return (num2 + 2);
        }

        public Asn1Object GetObject() => 
            Asn1Object.FromByteArray(this.GetContents());

        public Asn1Object GetObject(int derTagNo)
        {
            if (derTagNo >= 0x1f)
            {
                throw new IOException("unsupported tag number");
            }
            byte[] encoded = base.GetEncoded();
            byte[] data = this.ReplaceTagNumber(derTagNo, encoded);
            if ((encoded[0] & 0x20) != 0)
            {
                data[0] = (byte) (data[0] | 0x20);
            }
            return Asn1Object.FromByteArray(data);
        }

        public bool IsConstructed() => 
            this.isConstructed;

        private byte[] ReplaceTagNumber(int newTag, byte[] input)
        {
            int num = input[0] & 0x1f;
            int sourceIndex = 1;
            if (num == 0x1f)
            {
                num = 0;
                int num3 = input[sourceIndex++] & 0xff;
                if ((num3 & 0x7f) == 0)
                {
                    throw new InvalidOperationException("corrupted stream - invalid high tag number found");
                }
                while ((num3 >= 0) && ((num3 & 0x80) != 0))
                {
                    num |= num3 & 0x7f;
                    num = num << 7;
                    num3 = input[sourceIndex++] & 0xff;
                }
                num |= num3 & 0x7f;
            }
            byte[] destinationArray = new byte[(input.Length - sourceIndex) + 1];
            Array.Copy(input, sourceIndex, destinationArray, 1, destinationArray.Length - 1);
            destinationArray[0] = (byte) newTag;
            return destinationArray;
        }

        public int ApplicationTag =>
            this.tag;
    }
}

