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

        internal GooglePlayGames.BasicApi.PlayerStats AsPlayerStats()
        {
            GooglePlayGames.BasicApi.PlayerStats stats = new GooglePlayGames.BasicApi.PlayerStats {
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
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_AverageSessionLength(base.SelfPtr());

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_Dispose(selfPointer);
        }

        internal float ChurnProbability() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_ChurnProbability(base.SelfPtr());

        internal int DaysSinceLastPlayed() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_DaysSinceLastPlayed(base.SelfPtr());

        internal bool HasAverageSessionLength() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasAverageSessionLength(base.SelfPtr());

        internal bool HasChurnProbability() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasChurnProbability(base.SelfPtr());

        internal bool HasDaysSinceLastPlayed() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasDaysSinceLastPlayed(base.SelfPtr());

        internal bool HasNumberOfPurchases() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasNumberOfPurchases(base.SelfPtr());

        internal bool HasNumberOfSessions() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasNumberOfSessions(base.SelfPtr());

        internal bool HasSessionPercentile() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasSessionPercentile(base.SelfPtr());

        internal bool HasSpendPercentile() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasSpendPercentile(base.SelfPtr());

        internal int NumberOfPurchases() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_NumberOfPurchases(base.SelfPtr());

        internal int NumberOfSessions() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_NumberOfSessions(base.SelfPtr());

        internal float SessionPercentile() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_SessionPercentile(base.SelfPtr());

        internal float SpendPercentile() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_SpendPercentile(base.SelfPtr());

        internal bool Valid() => 
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_Valid(base.SelfPtr());
    }
}

