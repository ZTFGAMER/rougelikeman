namespace Org.BouncyCastle.Utilities.IO.Pem
{
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;
    using System.Collections;
    using System.IO;

    public class PemWriter
    {
        private const int LineLength = 0x40;
        private readonly TextWriter writer;
        private readonly int nlLength;
        private char[] buf = new char[0x40];

        public PemWriter(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            this.writer = writer;
            this.nlLength = Platform.NewLine.Length;
        }

        public int GetOutputSize(PemObject obj)
        {
            int num = ((2 * ((obj.Type.Length + 10) + this.nlLength)) + 6) + 4;
            if (obj.Headers.Count > 0)
            {
                IEnumerator enumerator = obj.Headers.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        PemHeader current = (PemHeader) enumerator.Current;
                        num += ((current.Name.Length + ": ".Length) + current.Value.Length) + this.nlLength;
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
                num += this.nlLength;
            }
            int num2 = ((obj.Content.Length + 2) / 3) * 4;
            return (num + (num2 + ((((num2 + 0x40) - 1) / 0x40) * this.nlLength)));
        }

        private void WriteEncoded(byte[] bytes)
        {
            bytes = Base64.Encode(bytes);
            for (int i = 0; i < bytes.Length; i += this.buf.Length)
            {
                int index = 0;
                while (index != this.buf.Length)
                {
                    if ((i + index) >= bytes.Length)
                    {
                        break;
                    }
                    this.buf[index] = (char) bytes[i + index];
                    index++;
                }
                this.writer.WriteLine(this.buf, 0, index);
            }
        }

        public void WriteObject(PemObjectGenerator objGen)
        {
            PemObject obj2 = objGen.Generate();
            this.WritePreEncapsulationBoundary(obj2.Type);
            if (obj2.Headers.Count > 0)
            {
                IEnumerator enumerator = obj2.Headers.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        PemHeader current = (PemHeader) enumerator.Current;
                        this.writer.Write(current.Name);
                        this.writer.Write(": ");
                        this.writer.WriteLine(current.Value);
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
                this.writer.WriteLine();
            }
            this.WriteEncoded(obj2.Content);
            this.WritePostEncapsulationBoundary(obj2.Type);
        }

        private void WritePostEncapsulationBoundary(string type)
        {
            this.writer.WriteLine("-----END " + type + "-----");
        }

        private void WritePreEncapsulationBoundary(string type)
        {
            this.writer.WriteLine("-----BEGIN " + type + "-----");
        }

        public TextWriter Writer =>
            this.writer;
    }
}

