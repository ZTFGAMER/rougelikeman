namespace Org.BouncyCastle.Asn1.X509
{
    using System;
    using System.Text;

    public class X509NameTokenizer
    {
        private string value;
        private int index;
        private char separator;
        private StringBuilder buffer;

        public X509NameTokenizer(string oid) : this(oid, ',')
        {
        }

        public X509NameTokenizer(string oid, char separator)
        {
            this.buffer = new StringBuilder();
            this.value = oid;
            this.index = -1;
            this.separator = separator;
        }

        public bool HasMoreTokens() => 
            (this.index != this.value.Length);

        public string NextToken()
        {
            if (this.index == this.value.Length)
            {
                return null;
            }
            int num = this.index + 1;
            bool flag = false;
            bool flag2 = false;
            this.buffer.Remove(0, this.buffer.Length);
            while (num != this.value.Length)
            {
                char ch = this.value[num];
                if (ch == '"')
                {
                    if (!flag2)
                    {
                        flag = !flag;
                    }
                    else
                    {
                        this.buffer.Append(ch);
                        flag2 = false;
                    }
                }
                else if (flag2 || flag)
                {
                    if ((ch == '#') && (this.buffer[this.buffer.Length - 1] == '='))
                    {
                        this.buffer.Append('\\');
                    }
                    else if ((ch == '+') && (this.separator != '+'))
                    {
                        this.buffer.Append('\\');
                    }
                    this.buffer.Append(ch);
                    flag2 = false;
                }
                else if (ch == '\\')
                {
                    flag2 = true;
                }
                else
                {
                    if (ch == this.separator)
                    {
                        break;
                    }
                    this.buffer.Append(ch);
                }
                num++;
            }
            this.index = num;
            return this.buffer.ToString().Trim();
        }
    }
}

