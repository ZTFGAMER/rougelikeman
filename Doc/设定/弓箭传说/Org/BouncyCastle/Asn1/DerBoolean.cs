namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerBoolean : Asn1Object
    {
        private readonly byte value;
        public static readonly DerBoolean False = new DerBoolean(false);
        public static readonly DerBoolean True = new DerBoolean(true);

        public DerBoolean(byte[] val)
        {
            if (val.Length != 1)
            {
                throw new ArgumentException("byte value should have 1 byte in it", "val");
            }
            this.value = val[0];
        }

        private DerBoolean(bool value)
        {
            this.value = !value ? ((byte) 0) : ((byte) 0xff);
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerBoolean flag = asn1Object as DerBoolean;
            if (flag == null)
            {
                return false;
            }
            return (this.IsTrue == flag.IsTrue);
        }

        protected override int Asn1GetHashCode() => 
            this.IsTrue.GetHashCode();

        internal override void Encode(DerOutputStream derOut)
        {
            byte[] bytes = new byte[] { this.value };
            derOut.WriteEncoded(1, bytes);
        }

        internal static DerBoolean FromOctetString(byte[] value)
        {
            if (value.Length != 1)
            {
                throw new ArgumentException("BOOLEAN value should have 1 byte in it", "value");
            }
            byte num = value[0];
            return ((num != 0) ? ((num != 0xff) ? new DerBoolean(value) : True) : False);
        }

        public static DerBoolean GetInstance(bool value) => 
            (!value ? False : True);

        public static DerBoolean GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerBoolean))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerBoolean) obj;
        }

        public static DerBoolean GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerBoolean))
            {
                return FromOctetString(((Asn1OctetString) obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public override string ToString() => 
            (!this.IsTrue ? "FALSE" : "TRUE");

        public bool IsTrue =>
            (this.value != 0);
    }
}

