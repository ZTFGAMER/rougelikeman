namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public abstract class X9IntegerConverter
    {
        protected X9IntegerConverter()
        {
        }

        public static int GetByteLength(ECCurve c) => 
            ((c.FieldSize + 7) / 8);

        public static int GetByteLength(ECFieldElement fe) => 
            ((fe.FieldSize + 7) / 8);

        public static byte[] IntegerToBytes(BigInteger s, int qLength)
        {
            byte[] sourceArray = s.ToByteArrayUnsigned();
            if (qLength < sourceArray.Length)
            {
                byte[] destinationArray = new byte[qLength];
                Array.Copy(sourceArray, sourceArray.Length - destinationArray.Length, destinationArray, 0, destinationArray.Length);
                return destinationArray;
            }
            if (qLength > sourceArray.Length)
            {
                byte[] destinationArray = new byte[qLength];
                Array.Copy(sourceArray, 0, destinationArray, destinationArray.Length - sourceArray.Length, sourceArray.Length);
                return destinationArray;
            }
            return sourceArray;
        }
    }
}

