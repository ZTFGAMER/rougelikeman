namespace GooglePlayGames.BasicApi
{
    using System;

    public class ScorePageToken
    {
        private string mId;
        private object mInternalObject;
        private LeaderboardCollection mCollection;
        private LeaderboardTimeSpan mTimespan;

        internal ScorePageToken(object internalObject, string id, LeaderboardCollection collection, LeaderboardTimeSpan timespan)
        {
            this.mInternalObject = internalObject;
            this.mId = id;
            this.mCollection = collection;
            this.mTimespan = timespan;
        }

        public LeaderboardCollection Collection =>
            this.mCollection;

        public LeaderboardTimeSpan TimeSpan =>
            this.mTimespan;

        public string LeaderboardId =>
            this.mId;

        internal object InternalObject =>
            this.mInternalObject;
    }
}

