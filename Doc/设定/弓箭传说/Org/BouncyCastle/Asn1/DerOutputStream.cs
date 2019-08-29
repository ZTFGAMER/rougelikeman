namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    public class DerOutputStream : FilterStream
    {
        public DerOutputStream(Stream os) : base(os)
        {
        }

        internal void WriteEncoded(int tag, byte[] bytes)
        {
            this.WriteByte((byte) tag);
            this.WriteLength(bytes.Length);
            this.Write(bytes, 0, bytes.Length);
        }

        internal void WriteEncoded(int tag, byte first, byte[] bytes)
        {
            this.WriteByte((byte) tag);
            this.WriteLength(bytes.Length + 1);
            this.WriteByte(first);
            this.Write(bytes, 0, bytes.Length);
        }

        internal void WriteEncoded(int flags, int tagNo, byte[] bytes)
        {
            this.WriteTag(flags, tagNo);
            this.WriteLength(bytes.Length);
            this.Write(bytes, 0, bytes.Length);
        }

        internal void WriteEncoded(int tag, byte[] bytes, int offset, int length)
        {
            this.WriteByte((byte) tag);
            this.WriteLength(length);
            this.Write(bytes, offset, length);
        }

        private void WriteLength(int length)
        {
            if (length > 0x7f)
            {
                int num = 1;
                uint num2 = (uint) length;
                while ((num2 = num2 >> 8) != 0)
                {
                    num++;
                }
                this.WriteByte((byte) (num | 0x80));
                for (int i = (num - 1) * 8; i >= 0; i -= 8)
                {
                    this.WriteByte((byte) (length >> i));
                }
            }
            else
            {
                this.WriteByte((byte) length);
            }
        }

        protected void WriteNull()
        {
            this.WriteByte(5);
            this.WriteByte(0);
        }

        public virtual void WriteObject(Asn1Encodable obj)
        {
            if (obj == null)
            {
                this.WriteNull();
            }
            else
            {
                obj.ToAsn1Object().Encode(this);
            }
        }

        public virtual void WriteObject(Asn1Object obj)
        {
            if (obj == null)
            {
                this.WriteNull();
            }
            else
            {
                obj.Encode(this);
            }
        }

        [Obsolete("Use version taking an Asn1Encodable arg instead")]
        public virtual void WriteObject(object obj)
        {
            if (obj == null)
            {
                this.WriteNull();
            }
            else if (obj is Asn1Object)
            {
                ((Asn1Object) obj).Encode(this);
            }
            else
            {
                if (!(obj is Asn1Encodable))
                {
                    throw new IOException("object not Asn1Object");
                }
                ((Asn1Encodable) obj).ToAsn1Object().Encode(this);
            }
        }

        internal void WriteTag(int flags, int tagNo)
        {
            if (tagNo < 0x1f)
            {
                this.WriteByte((byte) (flags | tagNo));
            }
            else
            {
                this.WriteByte((byte) (flags | 0x1f));
                if (tagNo < 0x80)
                {
                    this.WriteByte((byte) tagNo);
                }
                else
                {
                    byte[] buffer = new byte[5];
                    int length = buffer.Length;
                    buffer[--length] = (byte) (tagNo & 0x7f);
                    do
                    {
                        tagNo = tagNo >> 7;
                        buffer[--length] = (byte) ((tagNo & 0x7f) | 0x80);
                    }
                    while (tagNo > 0x7f);
                    this.Write(buffer, length, buffer.Length - length);
                }
            }
        }
    }
}

