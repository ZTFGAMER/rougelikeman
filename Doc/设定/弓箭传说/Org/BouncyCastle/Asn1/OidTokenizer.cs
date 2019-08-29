namespace Org.BouncyCastle.Asn1
{
    using System;

    public class OidTokenizer
    {
        private string oid;
        private int index;

        public OidTokenizer(string oid)
        {
            this.oid = oid;
        }

        public string NextToken()
        {
            if (this.index == -1)
            {
                return null;
            }
            int index = this.oid.IndexOf('.', this.index);
            if (index == -1)
            {
                string str = this.oid.Substring(this.index);
                this.index = -1;
                return str;
            }
            string str2 = this.oid.Substring(this.index, index - this.index);
            this.index = index + 1;
            return str2;
        }

        public bool HasMoreTokens =>
            (this.index != -1);
    }
}

