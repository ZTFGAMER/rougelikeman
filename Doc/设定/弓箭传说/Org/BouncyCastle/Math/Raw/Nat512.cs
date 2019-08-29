namespace Org.BouncyCastle.Math.Raw
{
    using System;

    internal abstract class Nat512
    {
        protected Nat512()
        {
        }

        public static void Mul(uint[] x, uint[] y, uint[] zz)
        {
            Nat256.Mul(x, y, zz);
            Nat256.Mul(x, 8, y, 8, zz, 0x10);
            uint num = Nat256.AddToEachOther(zz, 8, zz, 0x10);
            uint cIn = num + Nat256.AddTo(zz, 0, zz, 8, 0);
            num += Nat256.AddTo(zz, 0x18, zz, 0x10, cIn);
            uint[] z = Nat256.Create();
            uint[] numArray2 = Nat256.Create();
            bool flag = Nat256.Diff(x, 8, x, 0, z, 0) != Nat256.Diff(y, 8, y, 0, numArray2, 0);
            uint[] numArray3 = Nat256.CreateExt();
            Nat256.Mul(z, numArray2, numArray3);
            num += !flag ? ((uint) Nat.SubFrom(0x10, numArray3, 0, zz, 8)) : Nat.AddTo(0x10, numArray3, 0, zz, 8);
            Nat.AddWordAt(0x20, num, zz, 0x18);
        }

        public static void Square(uint[] x, uint[] zz)
        {
            Nat256.Square(x, zz);
            Nat256.Square(x, 8, zz, 0x10);
            uint num = Nat256.AddToEachOther(zz, 8, zz, 0x10);
            uint cIn = num + Nat256.AddTo(zz, 0, zz, 8, 0);
            num += Nat256.AddTo(zz, 0x18, zz, 0x10, cIn);
            uint[] z = Nat256.Create();
            Nat256.Diff(x, 8, x, 0, z, 0);
            uint[] numArray2 = Nat256.CreateExt();
            Nat256.Square(z, numArray2);
            num += (uint) Nat.SubFrom(0x10, numArray2, 0, zz, 8);
            Nat.AddWordAt(0x20, num, zz, 0x18);
        }
    }
}

