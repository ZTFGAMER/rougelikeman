namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math.EC;
    using System;

    public class WTauNafPreCompInfo : PreCompInfo
    {
        protected AbstractF2mPoint[] m_preComp;

        public virtual AbstractF2mPoint[] PreComp
        {
            get => 
                this.m_preComp;
            set => 
                (this.m_preComp = value);
        }
    }
}

