namespace Org.BouncyCastle.Math.Raw
{
    using System;

    internal abstract class Nat384
    {
        protected Nat384()
        {
        }

        public static void Mul(uint[] x, uint[] y, uint[] zz)
        {
            Nat192.Mul(x, y, zz);
            Nat192.Mul(x, 6, y, 6, zz, 12);
            uint num = Nat192.AddToEachOther(zz, 6, zz, 12);
            uint cIn = num + Nat192.AddTo(zz, 0, zz, 6, 0);
            num += Nat192.AddTo(zz, 0x12, zz, 12, cIn);
            uint[] z = Nat192.Create();
            uint[] numArray2 = Nat192.Create();
            bool flag = Nat192.Diff(x, 6, x, 0, z, 0) != Nat192.Diff(y, 6, y, 0, numArray2, 0);
            uint[] numArray3 = Nat192.CreateExt();
            Nat192.Mul(z, numArray2, numArray3);
            num += !flag ? ((uint) Nat.SubFrom(12, numArray3, 0, zz, 6)) : Nat.AddTo(12, numArray3, 0, zz, 6);
            Nat.AddWordAt(0x18, num, zz, 0x12);
        }

        public static void Square(uint[] x, uint[] zz)
        {
            Nat192.Square(x, zz);
            Nat192.Square(x, 6, zz, 12);
            uint num = Nat192.AddToEachOther(zz, 6, zz, 12);
            uint cIn = num + Nat192.AddTo(zz, 0, zz, 6, 0);
            num += Nat192.AddTo(zz, 0x12, zz, 12, cIn);
            uint[] z = Nat192.Create();
            Nat192.Diff(x, 6, x, 0, z, 0);
            uint[] numArray2 = Nat192.CreateExt();
            Nat192.Square(z, numArray2);
            num += (uint) Nat.SubFrom(12, numArray2, 0, zz, 6);
            Nat.AddWordAt(0x18, num, zz, 0x12);
        }
    }
}

