namespace Org.BouncyCastle.Utilities
{
    using System;

    public abstract class Integers
    {
        protected Integers()
        {
        }

        public static int RotateLeft(int i, int distance) => 
            ((i << distance) ^ (i >> -distance));

        public static int RotateRight(int i, int distance) => 
            ((i >> distance) ^ (i << -distance));
    }
}

