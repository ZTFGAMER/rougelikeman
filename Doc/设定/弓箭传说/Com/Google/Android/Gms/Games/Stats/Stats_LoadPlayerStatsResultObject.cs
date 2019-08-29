namespace Com.Google.Android.Gms.Games.Stats
{
    using Com.Google.Android.Gms.Common.Api;
    using Google.Developers;
    using System;

    public class Stats_LoadPlayerStatsResultObject : JavaObjWrapper, Stats_LoadPlayerStatsResult, Result
    {
        private const string CLASS_NAME = "com/google/android/gms/games/stats/Stats$LoadPlayerStatsResult";

        public Stats_LoadPlayerStatsResultObject(IntPtr ptr) : base(ptr)
        {
        }

        public PlayerStats getPlayerStats() => 
            new PlayerStatsObject(base.InvokeCall<IntPtr>("getPlayerStats", "()Lcom/google/android/gms/games/stats/PlayerStats;", Array.Empty<object>()));

        public Status getStatus() => 
            new Status(base.InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", Array.Empty<object>()));
    }
}

