namespace Com.Google.Android.Gms.Games.Stats
{
    using Google.Developers;
    using System;

    public class PlayerStatsObject : JavaObjWrapper, PlayerStats
    {
        private const string CLASS_NAME = "com/google/android/gms/games/stats/PlayerStats";

        public PlayerStatsObject(IntPtr ptr) : base(ptr)
        {
        }

        public float getAverageSessionLength() => 
            base.InvokeCall<float>("getAverageSessionLength", "()F", Array.Empty<object>());

        public float getChurnProbability() => 
            base.InvokeCall<float>("getChurnProbability", "()F", Array.Empty<object>());

        public int getDaysSinceLastPlayed() => 
            base.InvokeCall<int>("getDaysSinceLastPlayed", "()I", Array.Empty<object>());

        public float getHighSpenderProbability() => 
            base.InvokeCall<float>("getHighSpenderProbability", "()F", Array.Empty<object>());

        public int getNumberOfPurchases() => 
            base.InvokeCall<int>("getNumberOfPurchases", "()I", Array.Empty<object>());

        public int getNumberOfSessions() => 
            base.InvokeCall<int>("getNumberOfSessions", "()I", Array.Empty<object>());

        public float getSessionPercentile() => 
            base.InvokeCall<float>("getSessionPercentile", "()F", Array.Empty<object>());

        public float getSpendPercentile() => 
            base.InvokeCall<float>("getSpendPercentile", "()F", Array.Empty<object>());

        public float getSpendProbability() => 
            base.InvokeCall<float>("getSpendProbability", "()F", Array.Empty<object>());

        public float getTotalSpendNext28Days() => 
            base.InvokeCall<float>("getTotalSpendNext28Days", "()F", Array.Empty<object>());

        public static float UNSET_VALUE =>
            JavaObjWrapper.GetStaticFloatField("com/google/android/gms/games/stats/PlayerStats", "UNSET_VALUE");

        public static int CONTENTS_FILE_DESCRIPTOR =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/games/stats/PlayerStats", "CONTENTS_FILE_DESCRIPTOR");

        public static int PARCELABLE_WRITE_RETURN_VALUE =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/games/stats/PlayerStats", "PARCELABLE_WRITE_RETURN_VALUE");
    }
}

