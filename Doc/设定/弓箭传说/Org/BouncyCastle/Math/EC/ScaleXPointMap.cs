namespace Org.BouncyCastle.Math.EC
{
    using System;

    public class ScaleXPointMap : ECPointMap
    {
        protected readonly ECFieldElement scale;

        public ScaleXPointMap(ECFieldElement scale)
        {
            this.scale = scale;
        }

        public virtual ECPoint Map(ECPoint p) => 
            p.ScaleX(this.scale);
    }
}

