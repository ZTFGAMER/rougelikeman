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
            base.InvokeCall<float>("getAverageSessionLength", "()F", new object[0]);

        public float getChurnProbability() => 
            base.InvokeCall<float>("getChurnProbability", "()F", new object[0]);

        public int getDaysSinceLastPlayed() => 
            base.InvokeCall<int>("getDaysSinceLastPlayed", "()I", new object[0]);

        public float getHighSpenderProbability() => 
            base.InvokeCall<float>("getHighSpenderProbability", "()F", new object[0]);

        public int getNumberOfPurchases() => 
            base.InvokeCall<int>("getNumberOfPurchases", "()I", new object[0]);

        public int getNumberOfSessions() => 
            base.InvokeCall<int>("getNumberOfSessions", "()I", new object[0]);

        public float getSessionPercentile() => 
            base.InvokeCall<float>("getSessionPercentile", "()F", new object[0]);

        public float getSpendPercentile() => 
            base.InvokeCall<float>("getSpendPercentile", "()F", new object[0]);

        public float getSpendProbability() => 
            base.InvokeCall<float>("getSpendProbability", "()F", new object[0]);

        public float getTotalSpendNext28Days() => 
            base.InvokeCall<float>("getTotalSpendNext28Days", "()F", new object[0]);

        public static float UNSET_VALUE =>
            JavaObjWrapper.GetStaticFloatField("com/google/android/gms/games/stats/PlayerStats", "UNSET_VALUE");

        public static int CONTENTS_FILE_DESCRIPTOR =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/games/stats/PlayerStats", "CONTENTS_FILE_DESCRIPTOR");

        public static int PARCELABLE_WRITE_RETURN_VALUE =>
            JavaObjWrapper.GetStaticIntField("com/google/android/gms/games/stats/PlayerStats", "PARCELABLE_WRITE_RETURN_VALUE");
    }
}

