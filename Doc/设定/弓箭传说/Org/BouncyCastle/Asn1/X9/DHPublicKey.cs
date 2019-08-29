namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DHPublicKey : Asn1Encodable
    {
        private readonly DerInteger y;

        public DHPublicKey(DerInteger y)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            this.y = y;
        }

        public static DHPublicKey GetInstance(object obj)
        {
            if ((obj == null) || (obj is DHPublicKey))
            {
                return (DHPublicKey) obj;
            }
            if (!(obj is DerInteger))
            {
                throw new ArgumentException("Invalid DHPublicKey: " + Platform.GetTypeName(obj), "obj");
            }
            return new DHPublicKey((DerInteger) obj);
        }

        public static DHPublicKey GetInstance(Asn1TaggedObject obj, bool isExplicit) => 
            GetInstance(DerInteger.GetInstance(obj, isExplicit));

        public override Asn1Object ToAsn1Object() => 
            this.y;

        public DerInteger Y =>
            this.y;
    }
}

