namespace GooglePlayGames.BasicApi
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class PlayerStats
    {
        private static float UNSET_VALUE = -1f;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Valid>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <NumberOfPurchases>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <AvgSessonLength>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DaysSinceLastPlayed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <NumberOfSessions>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <SessPercentile>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <SpendPercentile>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <SpendProbability>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <ChurnProbability>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <HighSpenderProbability>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <TotalSpendNext28Days>k__BackingField;

        public PlayerStats()
        {
            this.Valid = false;
        }

        public bool HasAvgSessonLength() => 
            !(this.AvgSessonLength == UNSET_VALUE);

        public bool HasChurnProbability() => 
            !(this.ChurnProbability == UNSET_VALUE);

        public bool HasDaysSinceLastPlayed() => 
            (this.DaysSinceLastPlayed != ((int) UNSET_VALUE));

        public bool HasHighSpenderProbability() => 
            !(this.HighSpenderProbability == UNSET_VALUE);

        public bool HasNumberOfPurchases() => 
            (this.NumberOfPurchases != ((int) UNSET_VALUE));

        public bool HasNumberOfSessions() => 
            (this.NumberOfSessions != ((int) UNSET_VALUE));

        public bool HasSessPercentile() => 
            !(this.SessPercentile == UNSET_VALUE);

        public bool HasSpendPercentile() => 
            !(this.SpendPercentile == UNSET_VALUE);

        public bool HasTotalSpendNext28Days() => 
            !(this.TotalSpendNext28Days == UNSET_VALUE);

        public bool Valid { get; set; }

        public int NumberOfPurchases { get; set; }

        public float AvgSessonLength { get; set; }

        public int DaysSinceLastPlayed { get; set; }

        public int NumberOfSessions { get; set; }

        public float SessPercentile { get; set; }

        public float SpendPercentile { get; set; }

        public float SpendProbability { get; set; }

        public float ChurnProbability { get; set; }

        public float HighSpenderProbability { get; set; }

        public float TotalSpendNext28Days { get; set; }
    }
}

