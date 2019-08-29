namespace Com.Google.Android.Gms.Games.Stats
{
    using System;

    public interface PlayerStats
    {
        float getAverageSessionLength();
        float getChurnProbability();
        int getDaysSinceLastPlayed();
        float getHighSpenderProbability();
        int getNumberOfPurchases();
        int getNumberOfSessions();
        float getSessionPercentile();
        float getSpendPercentile();
        float getSpendProbability();
        float getTotalSpendNext28Days();
    }
}

