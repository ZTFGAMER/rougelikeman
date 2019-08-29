namespace Org.BouncyCastle.Utilities
{
    using System;

    public sealed class Times
    {
        private static long NanosecondsPerTick = 100L;

        public static long NanoTime() => 
            (DateTime.UtcNow.Ticks * NanosecondsPerTick);
    }
}

