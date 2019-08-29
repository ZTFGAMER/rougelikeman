namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Globalization;

    public class Time : Asn1Encodable, IAsn1Choice
    {
        private readonly Asn1Object time;

        public Time(Asn1Object time)
        {
            if (time == null)
            {
                throw new ArgumentNullException("time");
            }
            if (!(time is DerUtcTime) && !(time is DerGeneralizedTime))
            {
                throw new ArgumentException("unknown object passed to Time");
            }
            this.time = time;
        }

        public Time(DateTime date)
        {
            string time = date.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + "Z";
            int num = int.Parse(time.Substring(0, 4));
            if ((num < 0x79e) || (num > 0x801))
            {
                this.time = new DerGeneralizedTime(time);
            }
            else
            {
                this.time = new DerUtcTime(time.Substring(2));
            }
        }

        public static Time GetInstance(object obj)
        {
            if ((obj == null) || (obj is Time))
            {
                return (Time) obj;
            }
            if (obj is DerUtcTime)
            {
                return new Time((DerUtcTime) obj);
            }
            if (!(obj is DerGeneralizedTime))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new Time((DerGeneralizedTime) obj);
        }

        public static Time GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(obj.GetObject());

        public string GetTime()
        {
            if (this.time is DerUtcTime)
            {
                return ((DerUtcTime) this.time).AdjustedTimeString;
            }
            return ((DerGeneralizedTime) this.time).GetTime();
        }

        public override Asn1Object ToAsn1Object() => 
            this.time;

        public DateTime ToDateTime()
        {
            DateTime time;
            try
            {
                if (this.time is DerUtcTime)
                {
                    return ((DerUtcTime) this.time).ToAdjustedDateTime();
                }
                time = ((DerGeneralizedTime) this.time).ToDateTime();
            }
            catch (FormatException exception)
            {
                throw new InvalidOperationException("invalid date string: " + exception.Message);
            }
            return time;
        }

        public override string ToString() => 
            this.GetTime();
    }
}

