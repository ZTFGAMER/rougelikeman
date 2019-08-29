namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Globalization;

    public class DerUtcTime : Asn1Object
    {
        private readonly string time;

        public DerUtcTime(DateTime time)
        {
            this.time = time.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture) + "Z";
        }

        public DerUtcTime(string time)
        {
            if (time == null)
            {
                throw new ArgumentNullException("time");
            }
            this.time = time;
            try
            {
                this.ToDateTime();
            }
            catch (FormatException exception)
            {
                throw new ArgumentException("invalid date string: " + exception.Message);
            }
        }

        internal DerUtcTime(byte[] bytes)
        {
            this.time = Strings.FromAsciiByteArray(bytes);
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerUtcTime time = asn1Object as DerUtcTime;
            if (time == null)
            {
                return false;
            }
            return this.time.Equals(time.time);
        }

        protected override int Asn1GetHashCode() => 
            this.time.GetHashCode();

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(0x17, this.GetOctets());
        }

        public static DerUtcTime GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerUtcTime))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerUtcTime) obj;
        }

        public static DerUtcTime GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerUtcTime))
            {
                return new DerUtcTime(((Asn1OctetString) obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        private byte[] GetOctets() => 
            Strings.ToAsciiByteArray(this.time);

        private DateTime ParseDateString(string dateStr, string formatStr) => 
            DateTime.ParseExact(dateStr, formatStr, DateTimeFormatInfo.InvariantInfo).ToUniversalTime();

        public DateTime ToAdjustedDateTime() => 
            this.ParseDateString(this.AdjustedTimeString, "yyyyMMddHHmmss'GMT'zzz");

        public DateTime ToDateTime() => 
            this.ParseDateString(this.TimeString, "yyMMddHHmmss'GMT'zzz");

        public override string ToString() => 
            this.time;

        public string TimeString
        {
            get
            {
                if ((this.time.IndexOf('-') < 0) && (this.time.IndexOf('+') < 0))
                {
                    if (this.time.Length == 11)
                    {
                        return (this.time.Substring(0, 10) + "00GMT+00:00");
                    }
                    return (this.time.Substring(0, 12) + "GMT+00:00");
                }
                int index = this.time.IndexOf('-');
                if (index < 0)
                {
                    index = this.time.IndexOf('+');
                }
                string time = this.time;
                if (index == (this.time.Length - 3))
                {
                    time = time + "00";
                }
                if (index == 10)
                {
                    string[] textArray1 = new string[] { time.Substring(0, 10), "00GMT", time.Substring(10, 3), ":", time.Substring(13, 2) };
                    return string.Concat(textArray1);
                }
                string[] textArray2 = new string[] { time.Substring(0, 12), "GMT", time.Substring(12, 3), ":", time.Substring(15, 2) };
                return string.Concat(textArray2);
            }
        }

        [Obsolete("Use 'AdjustedTimeString' property instead")]
        public string AdjustedTime =>
            this.AdjustedTimeString;

        public string AdjustedTimeString
        {
            get
            {
                string timeString = this.TimeString;
                string str2 = (timeString[0] >= '5') ? "19" : "20";
                return (str2 + timeString);
            }
        }
    }
}

