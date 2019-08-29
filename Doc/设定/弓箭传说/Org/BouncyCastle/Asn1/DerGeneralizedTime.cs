namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Globalization;
    using System.Text;

    public class DerGeneralizedTime : Asn1Object
    {
        private readonly string time;

        public DerGeneralizedTime(DateTime time)
        {
            this.time = time.ToString(@"yyyyMMddHHmmss\Z");
        }

        public DerGeneralizedTime(string time)
        {
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

        internal DerGeneralizedTime(byte[] bytes)
        {
            this.time = Strings.FromAsciiByteArray(bytes);
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerGeneralizedTime time = asn1Object as DerGeneralizedTime;
            if (time == null)
            {
                return false;
            }
            return this.time.Equals(time.time);
        }

        protected override int Asn1GetHashCode() => 
            this.time.GetHashCode();

        private string CalculateGmtOffset()
        {
            char ch = '+';
            DateTime time = this.ToDateTime();
            TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(time);
            if (utcOffset.CompareTo(TimeSpan.Zero) < 0)
            {
                ch = '-';
                utcOffset = utcOffset.Duration();
            }
            int hours = utcOffset.Hours;
            int minutes = utcOffset.Minutes;
            object[] objArray1 = new object[] { "GMT", ch, Convert(hours), ":", Convert(minutes) };
            return string.Concat(objArray1);
        }

        private static string Convert(int time)
        {
            if (time < 10)
            {
                return ("0" + time);
            }
            return time.ToString();
        }

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(0x18, this.GetOctets());
        }

        private string FString(int count)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                builder.Append('f');
            }
            return builder.ToString();
        }

        public static DerGeneralizedTime GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerGeneralizedTime))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
            }
            return (DerGeneralizedTime) obj;
        }

        public static DerGeneralizedTime GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerGeneralizedTime))
            {
                return new DerGeneralizedTime(((Asn1OctetString) obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        private byte[] GetOctets() => 
            Strings.ToAsciiByteArray(this.time);

        public string GetTime()
        {
            if (this.time[this.time.Length - 1] == 'Z')
            {
                return (this.time.Substring(0, this.time.Length - 1) + "GMT+00:00");
            }
            int length = this.time.Length - 5;
            switch (this.time[length])
            {
                case '-':
                case '+':
                {
                    string[] textArray1 = new string[] { this.time.Substring(0, length), "GMT", this.time.Substring(length, 3), ":", this.time.Substring(length + 3) };
                    return string.Concat(textArray1);
                }
            }
            length = this.time.Length - 3;
            char ch = this.time[length];
            if ((ch != '-') && (ch != '+'))
            {
                return (this.time + this.CalculateGmtOffset());
            }
            return (this.time.Substring(0, length) + "GMT" + this.time.Substring(length) + ":00");
        }

        private DateTime ParseDateString(string s, string format, bool makeUniversal)
        {
            DateTimeStyles none = DateTimeStyles.None;
            if (Platform.EndsWith(format, "Z"))
            {
                try
                {
                    none = (DateTimeStyles) Enums.GetEnumValue(typeof(DateTimeStyles), "AssumeUniversal");
                }
                catch (Exception)
                {
                }
                none |= DateTimeStyles.AdjustToUniversal;
            }
            DateTime time = DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, none);
            return (!makeUniversal ? time : time.ToUniversalTime());
        }

        public DateTime ToDateTime()
        {
            string str;
            string time = this.time;
            bool makeUniversal = false;
            if (Platform.EndsWith(time, "Z"))
            {
                if (this.HasFractionalSeconds)
                {
                    int count = (time.Length - time.IndexOf('.')) - 2;
                    str = "yyyyMMddHHmmss." + this.FString(count) + @"\Z";
                }
                else
                {
                    str = @"yyyyMMddHHmmss\Z";
                }
            }
            else if ((this.time.IndexOf('-') > 0) || (this.time.IndexOf('+') > 0))
            {
                time = this.GetTime();
                makeUniversal = true;
                if (this.HasFractionalSeconds)
                {
                    int count = (Platform.IndexOf(time, "GMT") - 1) - time.IndexOf('.');
                    str = "yyyyMMddHHmmss." + this.FString(count) + "'GMT'zzz";
                }
                else
                {
                    str = "yyyyMMddHHmmss'GMT'zzz";
                }
            }
            else if (this.HasFractionalSeconds)
            {
                int count = (time.Length - 1) - time.IndexOf('.');
                str = "yyyyMMddHHmmss." + this.FString(count);
            }
            else
            {
                str = "yyyyMMddHHmmss";
            }
            return this.ParseDateString(time, str, makeUniversal);
        }

        public string TimeString =>
            this.time;

        private bool HasFractionalSeconds =>
            (this.time.IndexOf('.') == 14);
    }
}

