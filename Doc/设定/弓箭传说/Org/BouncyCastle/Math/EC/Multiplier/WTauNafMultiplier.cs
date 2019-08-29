namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Abc;
    using System;

    public class WTauNafMultiplier : AbstractECMultiplier
    {
        internal static readonly string PRECOMP_NAME = "bc_wtnaf";

        private static AbstractF2mPoint MultiplyFromWTnaf(AbstractF2mPoint p, sbyte[] u, PreCompInfo preCompInfo)
        {
            AbstractF2mPoint[] preComp;
            AbstractF2mCurve curve = (AbstractF2mCurve) p.Curve;
            sbyte intValue = (sbyte) curve.A.ToBigInteger().IntValue;
            if ((preCompInfo == null) || !(preCompInfo is WTauNafPreCompInfo))
            {
                preComp = Tnaf.GetPreComp(p, intValue);
                WTauNafPreCompInfo info = new WTauNafPreCompInfo {
                    PreComp = preComp
                };
                curve.SetPreCompInfo(p, PRECOMP_NAME, info);
            }
            else
            {
                preComp = ((WTauNafPreCompInfo) preCompInfo).PreComp;
            }
            AbstractF2mPoint[] pointArray2 = new AbstractF2mPoint[preComp.Length];
            for (int i = 0; i < preComp.Length; i++)
            {
                pointArray2[i] = (AbstractF2mPoint) preComp[i].Negate();
            }
            AbstractF2mPoint infinity = (AbstractF2mPoint) p.Curve.Infinity;
            int pow = 0;
            for (int j = u.Length - 1; j >= 0; j--)
            {
                pow++;
                int num5 = u[j];
                if (num5 != 0)
                {
                    infinity = infinity.TauPow(pow);
                    pow = 0;
                    ECPoint b = (num5 <= 0) ? pointArray2[-num5 >> 1] : preComp[num5 >> 1];
                    infinity = (AbstractF2mPoint) infinity.Add(b);
                }
            }
            if (pow > 0)
            {
                infinity = infinity.TauPow(pow);
            }
            return infinity;
        }

        protected override ECPoint MultiplyPositive(ECPoint point, BigInteger k)
        {
            if (!(point is AbstractF2mPoint))
            {
                throw new ArgumentException("Only AbstractF2mPoint can be used in WTauNafMultiplier");
            }
            AbstractF2mPoint p = (AbstractF2mPoint) point;
            AbstractF2mCurve curve = (AbstractF2mCurve) p.Curve;
            int fieldSize = curve.FieldSize;
            sbyte intValue = (sbyte) curve.A.ToBigInteger().IntValue;
            sbyte mu = Tnaf.GetMu((int) intValue);
            BigInteger[] si = curve.GetSi();
            ZTauElement lambda = Tnaf.PartModReduction(k, fieldSize, intValue, si, mu, 10);
            return this.MultiplyWTnaf(p, lambda, curve.GetPreCompInfo(p, PRECOMP_NAME), intValue, mu);
        }

        private AbstractF2mPoint MultiplyWTnaf(AbstractF2mPoint p, ZTauElement lambda, PreCompInfo preCompInfo, sbyte a, sbyte mu)
        {
            ZTauElement[] alpha = (a != 0) ? Tnaf.Alpha1 : Tnaf.Alpha0;
            BigInteger tw = Tnaf.GetTw(mu, 4);
            sbyte[] u = Tnaf.TauAdicWNaf(mu, lambda, 4, BigInteger.ValueOf(0x10L), tw, alpha);
            return MultiplyFromWTnaf(p, u, preCompInfo);
        }
    }
}

