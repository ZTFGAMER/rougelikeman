namespace GoogleMobileAds.Api
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Reward : EventArgs
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double <Amount>k__BackingField;

        public string Type { get; set; }

        public double Amount { get; set; }
    }
}

