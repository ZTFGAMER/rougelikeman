namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math.EC;
    using System;

    public class WNafPreCompInfo : PreCompInfo
    {
        protected ECPoint[] m_preComp;
        protected ECPoint[] m_preCompNeg;
        protected ECPoint m_twice;

        public virtual ECPoint[] PreComp
        {
            get => 
                this.m_preComp;
            set => 
                (this.m_preComp = value);
        }

        public virtual ECPoint[] PreCompNeg
        {
            get => 
                this.m_preCompNeg;
            set => 
                (this.m_preCompNeg = value);
        }

        public virtual ECPoint Twice
        {
            get => 
                this.m_twice;
            set => 
                (this.m_twice = value);
        }
    }
}

