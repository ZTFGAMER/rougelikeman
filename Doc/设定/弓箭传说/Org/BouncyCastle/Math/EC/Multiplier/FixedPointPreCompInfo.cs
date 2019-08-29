namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math.EC;
    using System;

    public class FixedPointPreCompInfo : PreCompInfo
    {
        protected ECPoint[] m_preComp;
        protected int m_width = -1;

        public virtual ECPoint[] PreComp
        {
            get => 
                this.m_preComp;
            set => 
                (this.m_preComp = value);
        }

        public virtual int Width
        {
            get => 
                this.m_width;
            set => 
                (this.m_width = value);
        }
    }
}

