using Dxx.Util;
using System;

public class XRandom
{
    private long seed;
    private const long multiplier = 0x5deece66dL;
    private const long addend = 11L;
    private const long mask = 0xffffffffffffL;
    private const double DOUBLE_UNIT = 1.1102230246251565E-16;
    private const string BadBound = "bound must be positive";
    private static long _seedUniquifier = 0x1ed8b55fac9decL;
    private double nextNextGaussian;
    private bool haveNextNextGaussian;

    public XRandom() : this(seedUniquifier() ^ nanoTime())
    {
    }

    public XRandom(long seed)
    {
        if (base.GetType() == typeof(XRandom))
        {
            this.seed = initialScramble(seed);
        }
        else
        {
            this.seed = 0L;
            this.setSeed(seed);
        }
    }

    private static long initialScramble(long seed) => 
        ((seed ^ 0x5deece66dL) & 0xffffffffffffL);

    public static long move_fill_0(long value, int bits)
    {
        long num = 0x7fffffffffffffffL;
        for (int i = 0; i < bits; i++)
        {
            value = value >> 1;
            value &= num;
        }
        return value;
    }

    public static long nanoTime() => 
        (DateTime.Now.Ticks * 100L);

    protected int next(int bits)
    {
        long num2;
        bool flag = false;
        do
        {
            long seed = this.seed;
            num2 = ((seed * 0x5deece66dL) + 11L) & 0xffffffffffffL;
            if (this.seed == seed)
            {
                this.seed = num2;
                flag = true;
            }
        }
        while (!flag);
        return (int) move_fill_0(num2, 0x30 - bits);
    }

    public bool nextBoolean() => 
        (this.next(1) != 0);

    public void nextBytes(byte[] bytes)
    {
        int num = 0;
        int length = bytes.Length;
        while (num < length)
        {
            int num3 = this.nextInt();
            int num4 = Math.Min(length - num, 4);
            while (num4-- > 0)
            {
                bytes[num++] = (byte) num3;
                num3 = num3 >> 8;
            }
        }
    }

    public double nextDouble() => 
        (((this.next(0x1a) << 0x1b) + this.next(0x1b)) * 1.1102230246251565E-16);

    public float nextFloat() => 
        (((float) this.next(0x18)) / 1.677722E+07f);

    public double nextGaussian()
    {
        object obj2 = this;
        lock (obj2)
        {
            double num2;
            double num3;
            double num4;
            if (this.haveNextNextGaussian)
            {
                this.haveNextNextGaussian = false;
                return this.nextNextGaussian;
            }
            do
            {
                num2 = (2.0 * this.nextDouble()) - 1.0;
                num3 = (2.0 * this.nextDouble()) - 1.0;
                num4 = (num2 * num2) + (num3 * num3);
            }
            while ((num4 >= 1.0) || (num4 == 0.0));
            double num5 = Math.Sqrt((-2.0 * Math.Log(num4)) / num4);
            this.nextNextGaussian = num3 * num5;
            this.haveNextNextGaussian = true;
            return (num2 * num5);
        }
    }

    public int nextInt() => 
        this.next(0x20);

    public int nextInt(int bound)
    {
        if (bound <= 0)
        {
            throw new ArgumentException("bound must be positive");
        }
        int num = this.next(0x1f);
        int num2 = bound - 1;
        if ((bound & num2) == 0)
        {
            return ((bound * num) >> 0x1f);
        }
        for (int i = num; ((i - (num = i % bound)) + num2) < 0; i = this.next(0x1f))
        {
        }
        return num;
    }

    public int nextInt(int min, int max)
    {
        if (min >= max)
        {
            return min;
        }
        return ((MathDxx.Abs(this.nextInt()) % (max - min)) + min);
    }

    public long nextLong() => 
        ((this.next(0x20) << 0x20) + this.next(0x20));

    private static long seedUniquifier()
    {
        long num;
        long num2;
        do
        {
            num = _seedUniquifier;
            num2 = num * 0x285d320ad33fdb5L;
        }
        while (_seedUniquifier != num);
        _seedUniquifier = num2;
        return num2;
    }

    public void setSeed(long seed)
    {
        object obj2 = this;
        lock (obj2)
        {
            this.seed = initialScramble(seed);
            this.haveNextNextGaussian = false;
        }
    }
}

