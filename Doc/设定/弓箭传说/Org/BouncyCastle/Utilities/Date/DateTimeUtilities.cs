namespace Org.BouncyCastle.Utilities.Date
{
    using System;

    public class DateTimeUtilities
    {
        public static readonly DateTime UnixEpoch = new DateTime(0x7b2, 1, 1);

        private DateTimeUtilities()
        {
        }

        public static long CurrentUnixMs() => 
            DateTimeToUnixMs(DateTime.UtcNow);

        public static long DateTimeToUnixMs(DateTime dateTime)
        {
            if (dateTime.CompareTo(UnixEpoch) < 0)
            {
                throw new ArgumentException("DateTime value may not be before the epoch", "dateTime");
            }
            return ((dateTime.Ticks - UnixEpoch.Ticks) / 0x2710L);
        }

        public static DateTime UnixMsToDateTime(long unixMs) => 
            new DateTime((unixMs * 0x2710L) + UnixEpoch.Ticks);
    }
}

