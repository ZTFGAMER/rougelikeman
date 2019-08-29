namespace GooglePlayGames
{
    using GooglePlayGames.BasicApi;
    using System;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    internal class PlayGamesAchievement : IAchievement, IAchievementDescription
    {
        private readonly GooglePlayGames.ReportProgress mProgressCallback;
        private string mId;
        private bool mIsIncremental;
        private int mCurrentSteps;
        private int mTotalSteps;
        private double mPercentComplete;
        private bool mCompleted;
        private bool mHidden;
        private DateTime mLastModifiedTime;
        private string mTitle;
        private string mRevealedImageUrl;
        private string mUnlockedImageUrl;
        private WWW mImageFetcher;
        private Texture2D mImage;
        private string mDescription;
        private ulong mPoints;

        internal PlayGamesAchievement() : this(new GooglePlayGames.ReportProgress(PlayGamesPlatform.Instance.ReportProgress))
        {
        }

        internal PlayGamesAchievement(Achievement ach) : this()
        {
            this.mId = ach.Id;
            this.mIsIncremental = ach.IsIncremental;
            this.mCurrentSteps = ach.CurrentSteps;
            this.mTotalSteps = ach.TotalSteps;
            if (ach.IsIncremental)
            {
                if (ach.TotalSteps > 0)
                {
                    this.mPercentComplete = (((double) ach.CurrentSteps) / ((double) ach.TotalSteps)) * 100.0;
                }
                else
                {
                    this.mPercentComplete = 0.0;
                }
            }
            else
            {
                this.mPercentComplete = !ach.IsUnlocked ? 0.0 : 100.0;
            }
            this.mCompleted = ach.IsUnlocked;
            this.mHidden = !ach.IsRevealed;
            this.mLastModifiedTime = ach.LastModifiedTime;
            this.mTitle = ach.Name;
            this.mDescription = ach.Description;
            this.mPoints = ach.Points;
            this.mRevealedImageUrl = ach.RevealedImageUrl;
            this.mUnlockedImageUrl = ach.UnlockedImageUrl;
        }

        internal PlayGamesAchievement(GooglePlayGames.ReportProgress progressCallback)
        {
            this.mId = string.Empty;
            this.mLastModifiedTime = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
            this.mTitle = string.Empty;
            this.mRevealedImageUrl = string.Empty;
            this.mUnlockedImageUrl = string.Empty;
            this.mDescription = string.Empty;
            this.mProgressCallback = progressCallback;
        }

        private Texture2D LoadImage()
        {
            if (!this.hidden)
            {
                string str = !this.completed ? this.mRevealedImageUrl : this.mUnlockedImageUrl;
                if (!string.IsNullOrEmpty(str))
                {
                    if ((this.mImageFetcher == null) || (this.mImageFetcher.url != str))
                    {
                        this.mImageFetcher = new WWW(str);
                        this.mImage = null;
                    }
                    if (this.mImage != null)
                    {
                        return this.mImage;
                    }
                    if (this.mImageFetcher.isDone)
                    {
                        this.mImage = this.mImageFetcher.texture;
                        return this.mImage;
                    }
                }
            }
            return null;
        }

        public void ReportProgress(Action<bool> callback)
        {
            this.mProgressCallback(this.mId, this.mPercentComplete, callback);
        }

        public string id
        {
            get => 
                this.mId;
            set => 
                (this.mId = value);
        }

        public bool isIncremental =>
            this.mIsIncremental;

        public int currentSteps =>
            this.mCurrentSteps;

        public int totalSteps =>
            this.mTotalSteps;

        public double percentCompleted
        {
            get => 
                this.mPercentComplete;
            set => 
                (this.mPercentComplete = value);
        }

        public bool completed =>
            this.mCompleted;

        public bool hidden =>
            this.mHidden;

        public DateTime lastReportedDate =>
            this.mLastModifiedTime;

        public string title =>
            this.mTitle;

        public Texture2D image =>
            this.LoadImage();

        public string achievedDescription =>
            this.mDescription;

        public string unachievedDescription =>
            this.mDescription;

        public int points =>
            ((int) this.mPoints);
    }
}

