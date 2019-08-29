namespace Org.BouncyCastle.Utilities.Date
{
    using System;

    public sealed class DateTimeObject
    {
        private readonly DateTime dt;

        public DateTimeObject(DateTime dt)
        {
            this.dt = dt;
        }

        public override string ToString() => 
            this.dt.ToString();

        public DateTime Value =>
            this.dt;
    }
}

