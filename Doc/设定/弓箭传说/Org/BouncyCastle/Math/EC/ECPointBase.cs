namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using System;

    public abstract class ECPointBase : ECPoint
    {
        protected internal ECPointBase(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
        }

        protected internal ECPointBase(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
        {
        }

        public override byte[] GetEncoded(bool compressed)
        {
            if (base.IsInfinity)
            {
                return new byte[1];
            }
            ECPoint point = this.Normalize();
            byte[] encoded = point.XCoord.GetEncoded();
            if (compressed)
            {
                byte[] buffer2 = new byte[encoded.Length + 1];
                buffer2[0] = !point.CompressionYTilde ? ((byte) 2) : ((byte) 3);
                Array.Copy(encoded, 0, buffer2, 1, encoded.Length);
                return buffer2;
            }
            byte[] sourceArray = point.YCoord.GetEncoded();
            byte[] destinationArray = new byte[(encoded.Length + sourceArray.Length) + 1];
            destinationArray[0] = 4;
            Array.Copy(encoded, 0, destinationArray, 1, encoded.Length);
            Array.Copy(sourceArray, 0, destinationArray, encoded.Length + 1, sourceArray.Length);
            return destinationArray;
        }

        public override ECPoint Multiply(BigInteger k) => 
            this.Curve.GetMultiplier().Multiply(this, k);
    }
}

