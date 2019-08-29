namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public class Tables1kGcmExponentiator : IGcmExponentiator
    {
        private IList lookupPowX2;

        private void EnsureAvailable(int bit)
        {
            int count = this.lookupPowX2.Count;
            if (count <= bit)
            {
                uint[] data = (uint[]) this.lookupPowX2[count - 1];
                do
                {
                    data = Arrays.Clone(data);
                    GcmUtilities.Multiply(data, data);
                    this.lookupPowX2.Add(data);
                }
                while (++count <= bit);
            }
        }

        public void ExponentiateX(long pow, byte[] output)
        {
            uint[] x = GcmUtilities.OneAsUints();
            int bit = 0;
            while (pow > 0L)
            {
                if ((pow & 1L) != 0L)
                {
                    this.EnsureAvailable(bit);
                    GcmUtilities.Multiply(x, (uint[]) this.lookupPowX2[bit]);
                }
                bit++;
                pow = pow >> 1;
            }
            GcmUtilities.AsBytes(x, output);
        }

        public void Init(byte[] x)
        {
            uint[] a = GcmUtilities.AsUints(x);
            if ((this.lookupPowX2 == null) || !Arrays.AreEqual(a, (uint[]) this.lookupPowX2[0]))
            {
                this.lookupPowX2 = Platform.CreateArrayList(8);
                this.lookupPowX2.Add(a);
            }
        }
    }
}

