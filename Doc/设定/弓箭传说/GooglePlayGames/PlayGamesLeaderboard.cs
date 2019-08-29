namespace GooglePlayGames
{
    using GooglePlayGames.BasicApi;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class PlayGamesLeaderboard : ILeaderboard
    {
        private string mId;
        private UserScope mUserScope;
        private Range mRange;
        private TimeScope mTimeScope;
        private string[] mFilteredUserIds;
        private bool mLoading;
        private IScore mLocalUserScore;
        private uint mMaxRange;
        private List<PlayGamesScore> mScoreList = new List<PlayGamesScore>();
        private string mTitle;

        public PlayGamesLeaderboard(string id)
        {
            this.mId = id;
        }

        internal int AddScore(PlayGamesScore score)
        {
            if ((this.mFilteredUserIds == null) || (this.mFilteredUserIds.Length == 0))
            {
                this.mScoreList.Add(score);
            }
            else
            {
                foreach (string str in this.mFilteredUserIds)
                {
                    if (str.Equals(score.userID))
                    {
                        return this.mScoreList.Count;
                    }
                }
                this.mScoreList.Add(score);
            }
            return this.mScoreList.Count;
        }

        internal bool HasAllScores() => 
            ((this.mScoreList.Count >= this.mRange.count) || (this.mScoreList.Count >= this.maxRange));

        public void LoadScores(Action<bool> callback)
        {
            PlayGamesPlatform.Instance.LoadScores(this, callback);
        }

        internal bool SetFromData(LeaderboardScoreData data)
        {
            if (data.Valid)
            {
                Debug.Log("Setting leaderboard from: " + data);
                this.SetMaxRange(data.ApproximateCount);
                this.SetTitle(data.Title);
                this.SetLocalUserScore((PlayGamesScore) data.PlayerScore);
                foreach (IScore score in data.Scores)
                {
                    this.AddScore((PlayGamesScore) score);
                }
                this.mLoading = (data.Scores.Length == 0) || this.HasAllScores();
            }
            return data.Valid;
        }

        internal void SetLocalUserScore(PlayGamesScore score)
        {
            this.mLocalUserScore = score;
        }

        internal void SetMaxRange(ulong val)
        {
            this.mMaxRange = (uint) val;
        }

        internal void SetTitle(string value)
        {
            this.mTitle = value;
        }

        public void SetUserFilter(string[] userIDs)
        {
            this.mFilteredUserIds = userIDs;
        }

        public bool loading
        {
            get => 
                this.mLoading;
            internal set => 
                (this.mLoading = value);
        }

        public string id
        {
            get => 
                this.mId;
            set => 
                (this.mId = value);
        }

        public UserScope userScope
        {
            get => 
                this.mUserScope;
            set => 
                (this.mUserScope = value);
        }

        public Range range
        {
            get => 
                this.mRange;
            set => 
                (this.mRange = value);
        }

        public TimeScope timeScope
        {
            get => 
                this.mTimeScope;
            set => 
                (this.mTimeScope = value);
        }

        public IScore localUserScore =>
            this.mLocalUserScore;

        public uint maxRange =>
            this.mMaxRange;

        public IScore[] scores
        {
            get
            {
                PlayGamesScore[] array = new PlayGamesScore[this.mScoreList.Count];
                this.mScoreList.CopyTo(array);
                return array;
            }
        }

        public string title =>
            this.mTitle;

        public int ScoreCount =>
            this.mScoreList.Count;
    }
}

