namespace Org.BouncyCastle.Utilities.IO.Pem
{
    using System;

    public class PemHeader
    {
        private string name;
        private string val;

        public PemHeader(string name, string val)
        {
            this.name = name;
            this.val = val;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (!(obj is PemHeader))
            {
                return false;
            }
            PemHeader header = (PemHeader) obj;
            return (object.Equals(this.name, header.name) && object.Equals(this.val, header.val));
        }

        public override int GetHashCode() => 
            (this.GetHashCode(this.name) + (0x1f * this.GetHashCode(this.val)));

        private int GetHashCode(string s) => 
            s?.GetHashCode();

        public virtual string Name =>
            this.name;

        public virtual string Value =>
            this.val;
    }
}

