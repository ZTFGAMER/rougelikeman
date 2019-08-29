namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities;
    using System;

    public class KeccakDigest : IDigest, IMemoable
    {
        private static readonly ulong[] KeccakRoundConstants = KeccakInitializeRoundConstants();
        private static readonly int[] KeccakRhoOffsets = KeccakInitializeRhoOffsets();
        protected byte[] state;
        protected byte[] dataQueue;
        protected int rate;
        protected int bitsInQueue;
        protected int fixedOutputLength;
        protected bool squeezing;
        protected int bitsAvailableForSqueezing;
        protected byte[] chunk;
        protected byte[] oneByte;
        private ulong[] C;
        private ulong[] tempA;
        private ulong[] chiC;

        public KeccakDigest() : this(0x120)
        {
        }

        public KeccakDigest(KeccakDigest source)
        {
            this.state = new byte[200];
            this.dataQueue = new byte[0xc0];
            this.C = new ulong[5];
            this.tempA = new ulong[0x19];
            this.chiC = new ulong[5];
            this.CopyIn(source);
        }

        public KeccakDigest(int bitLength)
        {
            this.state = new byte[200];
            this.dataQueue = new byte[0xc0];
            this.C = new ulong[5];
            this.tempA = new ulong[0x19];
            this.chiC = new ulong[5];
            this.Init(bitLength);
        }

        protected virtual void Absorb(byte[] data, int off, long databitlen)
        {
            if ((this.bitsInQueue % 8) != 0)
            {
                throw new InvalidOperationException("attempt to absorb with odd length queue");
            }
            if (this.squeezing)
            {
                throw new InvalidOperationException("attempt to absorb while squeezing");
            }
            long num = 0L;
            while (num < databitlen)
            {
                if (((this.bitsInQueue == 0) && (databitlen >= this.rate)) && (num <= (databitlen - this.rate)))
                {
                    long num3 = (databitlen - num) / ((long) this.rate);
                    for (long i = 0L; i < num3; i += 1L)
                    {
                        Array.Copy(data, (int) ((off + (num / 8L)) + (i * this.chunk.Length)), this.chunk, 0, this.chunk.Length);
                        this.KeccakAbsorb(this.state, this.chunk, this.chunk.Length);
                    }
                    num += num3 * this.rate;
                }
                else
                {
                    int num4 = (int) (databitlen - num);
                    if ((num4 + this.bitsInQueue) > this.rate)
                    {
                        num4 = this.rate - this.bitsInQueue;
                    }
                    int num5 = num4 % 8;
                    num4 -= num5;
                    Array.Copy(data, (int) (off + ((int) (num / 8L))), this.dataQueue, (int) (this.bitsInQueue / 8), (int) (num4 / 8));
                    this.bitsInQueue += num4;
                    num += num4;
                    if (this.bitsInQueue == this.rate)
                    {
                        this.AbsorbQueue();
                    }
                    if (num5 > 0)
                    {
                        int num6 = (((int) 1) << num5) - 1;
                        this.dataQueue[this.bitsInQueue / 8] = (byte) (data[off + ((int) (num / 8L))] & num6);
                        this.bitsInQueue += num5;
                        num += num5;
                    }
                }
            }
        }

        private void AbsorbQueue()
        {
            this.KeccakAbsorb(this.state, this.dataQueue, this.rate / 8);
            this.bitsInQueue = 0;
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int len)
        {
            this.Absorb(input, inOff, len * 8L);
        }

        private void Chi(ulong[] A)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    this.chiC[j] = A[j + (5 * i)] ^ (~A[((j + 1) % 5) + (5 * i)] & A[((j + 2) % 5) + (5 * i)]);
                }
                for (int k = 0; k < 5; k++)
                {
                    A[k + (5 * i)] = this.chiC[k];
                }
            }
        }

        private void ClearDataQueueSection(int off, int len)
        {
            for (int i = off; i != (off + len); i++)
            {
                this.dataQueue[i] = 0;
            }
        }

        public virtual IMemoable Copy() => 
            new KeccakDigest(this);

        private void CopyIn(KeccakDigest source)
        {
            Array.Copy(source.state, 0, this.state, 0, source.state.Length);
            Array.Copy(source.dataQueue, 0, this.dataQueue, 0, source.dataQueue.Length);
            this.rate = source.rate;
            this.bitsInQueue = source.bitsInQueue;
            this.fixedOutputLength = source.fixedOutputLength;
            this.squeezing = source.squeezing;
            this.bitsAvailableForSqueezing = source.bitsAvailableForSqueezing;
            this.chunk = Arrays.Clone(source.chunk);
            this.oneByte = Arrays.Clone(source.oneByte);
        }

        public virtual int DoFinal(byte[] output, int outOff)
        {
            this.Squeeze(output, outOff, (long) this.fixedOutputLength);
            this.Reset();
            return this.GetDigestSize();
        }

        protected virtual int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
        {
            if (partialBits > 0)
            {
                this.oneByte[0] = partialByte;
                this.Absorb(this.oneByte, 0, (long) partialBits);
            }
            this.Squeeze(output, outOff, (long) this.fixedOutputLength);
            this.Reset();
            return this.GetDigestSize();
        }

        private static void FromBytesToWords(ulong[] stateAsWords, byte[] state)
        {
            for (int i = 0; i < 0x19; i++)
            {
                stateAsWords[i] = 0L;
                int num2 = i * 8;
                for (int j = 0; j < 8; j++)
                {
                    stateAsWords[i] |= (ulong) ((state[num2 + j] & 0xffL) << (8 * j));
                }
            }
        }

        private static void FromWordsToBytes(byte[] state, ulong[] stateAsWords)
        {
            for (int i = 0; i < 0x19; i++)
            {
                int num2 = i * 8;
                for (int j = 0; j < 8; j++)
                {
                    state[num2 + j] = (byte) (stateAsWords[i] >> (8 * j));
                }
            }
        }

        public virtual int GetByteLength() => 
            (this.rate / 8);

        public virtual int GetDigestSize() => 
            (this.fixedOutputLength / 8);

        private void Init(int bitLength)
        {
            if (bitLength != 0x80)
            {
                if (bitLength != 0xe0)
                {
                    if (bitLength != 0x100)
                    {
                        if (bitLength != 0x120)
                        {
                            if (bitLength != 0x180)
                            {
                                if (bitLength != 0x200)
                                {
                                    throw new ArgumentException("must be one of 128, 224, 256, 288, 384, or 512.", "bitLength");
                                }
                                this.InitSponge(0x240, 0x400);
                                return;
                            }
                            this.InitSponge(0x340, 0x300);
                            return;
                        }
                        this.InitSponge(0x400, 0x240);
                        return;
                    }
                    this.InitSponge(0x440, 0x200);
                    return;
                }
            }
            else
            {
                this.InitSponge(0x540, 0x100);
                return;
            }
            this.InitSponge(0x480, 0x1c0);
        }

        private void InitSponge(int rate, int capacity)
        {
            if ((rate + capacity) != 0x640)
            {
                throw new InvalidOperationException("rate + capacity != 1600");
            }
            if (((rate <= 0) || (rate >= 0x640)) || ((rate % 0x40) != 0))
            {
                throw new InvalidOperationException("invalid rate value");
            }
            this.rate = rate;
            this.fixedOutputLength = 0;
            Arrays.Fill(this.state, 0);
            Arrays.Fill(this.dataQueue, 0);
            this.bitsInQueue = 0;
            this.squeezing = false;
            this.bitsAvailableForSqueezing = 0;
            this.fixedOutputLength = capacity / 2;
            this.chunk = new byte[rate / 8];
            this.oneByte = new byte[1];
        }

        private static void Iota(ulong[] A, int indexRound)
        {
            A[0] ^= KeccakRoundConstants[indexRound];
        }

        private void KeccakAbsorb(byte[] byteState, byte[] data, int dataInBytes)
        {
            this.KeccakPermutationAfterXor(byteState, data, dataInBytes);
        }

        private void KeccakExtract(byte[] byteState, byte[] data, int laneCount)
        {
            Array.Copy(byteState, 0, data, 0, laneCount * 8);
        }

        private void KeccakExtract1024bits(byte[] byteState, byte[] data)
        {
            Array.Copy(byteState, 0, data, 0, 0x80);
        }

        private static int[] KeccakInitializeRhoOffsets()
        {
            int[] numArray = new int[0x19];
            int num6 = 0;
            numArray[0] = num6;
            int num = 1;
            int num2 = 0;
            for (int i = 1; i < 0x19; i++)
            {
                num6 = (num6 + i) & 0x3f;
                numArray[(num % 5) + (5 * (num2 % 5))] = num6;
                int num4 = ((0 * num) + num2) % 5;
                int num5 = ((2 * num) + (3 * num2)) % 5;
                num = num4;
                num2 = num5;
            }
            return numArray;
        }

        private static ulong[] KeccakInitializeRoundConstants()
        {
            ulong[] numArray = new ulong[0x18];
            byte num = 1;
            for (int i = 0; i < 0x18; i++)
            {
                numArray[i] = 0L;
                for (int j = 0; j < 7; j++)
                {
                    int num4 = (((int) 1) << j) - 1;
                    if ((num & 1) != 0)
                    {
                        numArray[i] ^= ((ulong) 1L) << num4;
                    }
                    bool flag2 = (num & 0x80) != 0;
                    num = (byte) (num << 1);
                    if (flag2)
                    {
                        num = (byte) (num ^ 0x71);
                    }
                }
            }
            return numArray;
        }

        private void KeccakPermutation(byte[] state)
        {
            ulong[] stateAsWords = new ulong[state.Length / 8];
            FromBytesToWords(stateAsWords, state);
            this.KeccakPermutationOnWords(stateAsWords);
            FromWordsToBytes(state, stateAsWords);
        }

        private void KeccakPermutationAfterXor(byte[] state, byte[] data, int dataLengthInBytes)
        {
            for (int i = 0; i < dataLengthInBytes; i++)
            {
                state[i] = (byte) (state[i] ^ data[i]);
            }
            this.KeccakPermutation(state);
        }

        private void KeccakPermutationOnWords(ulong[] state)
        {
            for (int i = 0; i < 0x18; i++)
            {
                this.Theta(state);
                this.Rho(state);
                this.Pi(state);
                this.Chi(state);
                Iota(state, i);
            }
        }

        private void PadAndSwitchToSqueezingPhase()
        {
            if ((this.bitsInQueue + 1) == this.rate)
            {
                this.dataQueue[this.bitsInQueue / 8] = (byte) (this.dataQueue[this.bitsInQueue / 8] | ((byte) (((int) 1) << (this.bitsInQueue % 8))));
                this.AbsorbQueue();
                this.ClearDataQueueSection(0, this.rate / 8);
            }
            else
            {
                this.ClearDataQueueSection((this.bitsInQueue + 7) / 8, (this.rate / 8) - ((this.bitsInQueue + 7) / 8));
                this.dataQueue[this.bitsInQueue / 8] = (byte) (this.dataQueue[this.bitsInQueue / 8] | ((byte) (((int) 1) << (this.bitsInQueue % 8))));
            }
            this.dataQueue[(this.rate - 1) / 8] = (byte) (this.dataQueue[(this.rate - 1) / 8] | ((byte) (((int) 1) << ((this.rate - 1) % 8))));
            this.AbsorbQueue();
            if (this.rate == 0x400)
            {
                this.KeccakExtract1024bits(this.state, this.dataQueue);
                this.bitsAvailableForSqueezing = 0x400;
            }
            else
            {
                this.KeccakExtract(this.state, this.dataQueue, this.rate / 0x40);
                this.bitsAvailableForSqueezing = this.rate;
            }
            this.squeezing = true;
        }

        private void Pi(ulong[] A)
        {
            Array.Copy(A, 0, this.tempA, 0, this.tempA.Length);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    A[j + (5 * (((2 * i) + (3 * j)) % 5))] = this.tempA[i + (5 * j)];
                }
            }
        }

        public virtual void Reset()
        {
            this.Init(this.fixedOutputLength);
        }

        public virtual void Reset(IMemoable other)
        {
            KeccakDigest source = (KeccakDigest) other;
            this.CopyIn(source);
        }

        private void Rho(ulong[] A)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int index = i + (5 * j);
                    A[index] = (KeccakRhoOffsets[index] == 0) ? A[index] : ((A[index] << KeccakRhoOffsets[index]) ^ (A[index] >> (0x40 - KeccakRhoOffsets[index])));
                }
            }
        }

        protected virtual void Squeeze(byte[] output, int offset, long outputLength)
        {
            int bitsAvailableForSqueezing;
            if (!this.squeezing)
            {
                this.PadAndSwitchToSqueezingPhase();
            }
            if ((outputLength % 8L) != 0L)
            {
                throw new InvalidOperationException("outputLength not a multiple of 8");
            }
            for (long i = 0L; i < outputLength; i += bitsAvailableForSqueezing)
            {
                if (this.bitsAvailableForSqueezing == 0)
                {
                    this.KeccakPermutation(this.state);
                    if (this.rate == 0x400)
                    {
                        this.KeccakExtract1024bits(this.state, this.dataQueue);
                        this.bitsAvailableForSqueezing = 0x400;
                    }
                    else
                    {
                        this.KeccakExtract(this.state, this.dataQueue, this.rate / 0x40);
                        this.bitsAvailableForSqueezing = this.rate;
                    }
                }
                bitsAvailableForSqueezing = this.bitsAvailableForSqueezing;
                if (bitsAvailableForSqueezing > (outputLength - i))
                {
                    bitsAvailableForSqueezing = (int) (outputLength - i);
                }
                Array.Copy(this.dataQueue, (int) ((this.rate - this.bitsAvailableForSqueezing) / 8), output, (int) (offset + ((int) (i / 8L))), (int) (bitsAvailableForSqueezing / 8));
                this.bitsAvailableForSqueezing -= bitsAvailableForSqueezing;
            }
        }

        private void Theta(ulong[] A)
        {
            for (int i = 0; i < 5; i++)
            {
                this.C[i] = 0L;
                for (int k = 0; k < 5; k++)
                {
                    this.C[i] ^= A[i + (5 * k)];
                }
            }
            for (int j = 0; j < 5; j++)
            {
                ulong num4 = ((this.C[(j + 1) % 5] << 1) ^ (this.C[(j + 1) % 5] >> 0x3f)) ^ this.C[(j + 4) % 5];
                for (int k = 0; k < 5; k++)
                {
                    A[j + (5 * k)] ^= num4;
                }
            }
        }

        public virtual void Update(byte input)
        {
            this.oneByte[0] = input;
            this.Absorb(this.oneByte, 0, 8L);
        }

        public virtual string AlgorithmName =>
            ("Keccak-" + this.fixedOutputLength);
    }
}

