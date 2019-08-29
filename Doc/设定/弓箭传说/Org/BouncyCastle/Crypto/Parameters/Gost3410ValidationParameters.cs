namespace Org.BouncyCastle.Crypto.Parameters
{
    using System;

    public class Gost3410ValidationParameters
    {
        private int x0;
        private int c;
        private long x0L;
        private long cL;

        public Gost3410ValidationParameters(int x0, int c)
        {
            this.x0 = x0;
            this.c = c;
        }

        public Gost3410ValidationParameters(long x0L, long cL)
        {
            this.x0L = x0L;
            this.cL = cL;
        }

        public override bool Equals(object obj)
        {
            Gost3410ValidationParameters parameters = obj as Gost3410ValidationParameters;
            return ((((parameters != null) && (parameters.c == this.c)) && ((parameters.x0 == this.x0) && (parameters.cL == this.cL))) && (parameters.x0L == this.x0L));
        }

        public override int GetHashCode() => 
            (((this.c.GetHashCode() ^ this.x0.GetHashCode()) ^ this.cL.GetHashCode()) ^ this.x0L.GetHashCode());

        public int C =>
            this.c;

        public int X0 =>
            this.x0;

        public long CL =>
            this.cL;

        public long X0L =>
            this.x0L;
    }
}

