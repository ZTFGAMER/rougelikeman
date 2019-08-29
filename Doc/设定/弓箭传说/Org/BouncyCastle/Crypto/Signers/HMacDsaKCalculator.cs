namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Macs;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class HMacDsaKCalculator : IDsaKCalculator
    {
        private readonly HMac hMac;
        private readonly byte[] K;
        private readonly byte[] V;
        private BigInteger n;

        public HMacDsaKCalculator(IDigest digest)
        {
            this.hMac = new HMac(digest);
            this.V = new byte[this.hMac.GetMacSize()];
            this.K = new byte[this.hMac.GetMacSize()];
        }

        private BigInteger BitsToInt(byte[] t)
        {
            BigInteger integer = new BigInteger(1, t);
            if ((t.Length * 8) > this.n.BitLength)
            {
                integer = integer.ShiftRight((t.Length * 8) - this.n.BitLength);
            }
            return integer;
        }

        public virtual void Init(BigInteger n, SecureRandom random)
        {
            throw new InvalidOperationException("Operation not supported");
        }

        public void Init(BigInteger n, BigInteger d, byte[] message)
        {
            this.n = n;
            Arrays.Fill(this.V, 1);
            Arrays.Fill(this.K, 0);
            byte[] destinationArray = new byte[(n.BitLength + 7) / 8];
            byte[] sourceArray = BigIntegers.AsUnsignedByteArray(d);
            Array.Copy(sourceArray, 0, destinationArray, destinationArray.Length - sourceArray.Length, sourceArray.Length);
            byte[] buffer3 = new byte[(n.BitLength + 7) / 8];
            BigInteger integer = this.BitsToInt(message);
            if (integer.CompareTo(n) >= 0)
            {
                integer = integer.Subtract(n);
            }
            byte[] buffer4 = BigIntegers.AsUnsignedByteArray(integer);
            Array.Copy(buffer4, 0, buffer3, buffer3.Length - buffer4.Length, buffer4.Length);
            this.hMac.Init(new KeyParameter(this.K));
            this.hMac.BlockUpdate(this.V, 0, this.V.Length);
            this.hMac.Update(0);
            this.hMac.BlockUpdate(destinationArray, 0, destinationArray.Length);
            this.hMac.BlockUpdate(buffer3, 0, buffer3.Length);
            this.hMac.DoFinal(this.K, 0);
            this.hMac.Init(new KeyParameter(this.K));
            this.hMac.BlockUpdate(this.V, 0, this.V.Length);
            this.hMac.DoFinal(this.V, 0);
            this.hMac.BlockUpdate(this.V, 0, this.V.Length);
            this.hMac.Update(1);
            this.hMac.BlockUpdate(destinationArray, 0, destinationArray.Length);
            this.hMac.BlockUpdate(buffer3, 0, buffer3.Length);
            this.hMac.DoFinal(this.K, 0);
            this.hMac.Init(new KeyParameter(this.K));
            this.hMac.BlockUpdate(this.V, 0, this.V.Length);
            this.hMac.DoFinal(this.V, 0);
        }

        public virtual BigInteger NextK()
        {
            byte[] destinationArray = new byte[(this.n.BitLength + 7) / 8];
            while (true)
            {
                int num2;
                for (int i = 0; i < destinationArray.Length; i += num2)
                {
                    this.hMac.BlockUpdate(this.V, 0, this.V.Length);
                    this.hMac.DoFinal(this.V, 0);
                    num2 = Math.Min(destinationArray.Length - i, this.V.Length);
                    Array.Copy(this.V, 0, destinationArray, i, num2);
                }
                BigInteger integer = this.BitsToInt(destinationArray);
                if ((integer.SignValue > 0) && (integer.CompareTo(this.n) < 0))
                {
                    return integer;
                }
                this.hMac.BlockUpdate(this.V, 0, this.V.Length);
                this.hMac.Update(0);
                this.hMac.DoFinal(this.K, 0);
                this.hMac.Init(new KeyParameter(this.K));
                this.hMac.BlockUpdate(this.V, 0, this.V.Length);
                this.hMac.DoFinal(this.V, 0);
            }
        }

        public virtual bool IsDeterministic =>
            true;
    }
}

