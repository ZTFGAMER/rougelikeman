namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativePlayerStats : BaseReferenceHolder
    {
        internal NativePlayerStats(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal PlayerStats AsPlayerStats()
        {
            PlayerStats stats = new PlayerStats {
                Valid = this.Valid()
            };
            if (this.Valid())
            {
                stats.AvgSessonLength = this.AverageSessionLength();
                stats.ChurnProbability = this.ChurnProbability();
                stats.DaysSinceLastPlayed = this.DaysSinceLastPlayed();
                stats.NumberOfPurchases = this.NumberOfPurchases();
                stats.NumberOfSessions = this.NumberOfSessions();
                stats.SessPercentile = this.SessionPercentile();
                stats.SpendPercentile = this.SpendPercentile();
                stats.SpendProbability = -1f;
            }
            return stats;
        }

        internal float AverageSessionLength() => 
            PlayerStats.PlayerStats_AverageSessionLength(base.SelfPtr());

        protected override void CallDispose(HandleRef selfPointer)
        {
            PlayerStats.PlayerStats_Dispose(selfPointer);
        }

        internal float ChurnProbability() => 
            PlayerStats.PlayerStats_ChurnProbability(base.SelfPtr());

        internal int DaysSinceLastPlayed() => 
            PlayerStats.PlayerStats_DaysSinceLastPlayed(base.SelfPtr());

        internal bool HasAverageSessionLength() => 
            PlayerStats.PlayerStats_HasAverageSessionLength(base.SelfPtr());

        internal bool HasChurnProbability() => 
            PlayerStats.PlayerStats_HasChurnProbability(base.SelfPtr());

        internal bool HasDaysSinceLastPlayed() => 
            PlayerStats.PlayerStats_HasDaysSinceLastPlayed(base.SelfPtr());

        internal bool HasNumberOfPurchases() => 
            PlayerStats.PlayerStats_HasNumberOfPurchases(base.SelfPtr());

        internal bool HasNumberOfSessions() => 
            PlayerStats.PlayerStats_HasNumberOfSessions(base.SelfPtr());

        internal bool HasSessionPercentile() => 
            PlayerStats.PlayerStats_HasSessionPercentile(base.SelfPtr());

        internal bool HasSpendPercentile() => 
            PlayerStats.PlayerStats_HasSpendPercentile(base.SelfPtr());

        internal int NumberOfPurchases() => 
            PlayerStats.PlayerStats_NumberOfPurchases(base.SelfPtr());

        internal int NumberOfSessions() => 
            PlayerStats.PlayerStats_NumberOfSessions(base.SelfPtr());

        internal float SessionPercentile() => 
            PlayerStats.PlayerStats_SessionPercentile(base.SelfPtr());

        internal float SpendPercentile() => 
            PlayerStats.PlayerStats_SpendPercentile(base.SelfPtr());

        internal bool Valid() => 
            PlayerStats.PlayerStats_Valid(base.SelfPtr());
    }
}

