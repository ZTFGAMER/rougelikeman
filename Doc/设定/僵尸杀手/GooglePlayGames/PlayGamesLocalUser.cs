namespace GooglePlayGames
{
    using GooglePlayGames.BasicApi;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.SocialPlatforms;

    public class PlayGamesLocalUser : PlayGamesUserProfile, ILocalUser, IUserProfile
    {
        internal PlayGamesPlatform mPlatform;
        private string emailAddress;
        private PlayerStats mStats;

        internal PlayGamesLocalUser(PlayGamesPlatform plaf) : base("localUser", string.Empty, string.Empty)
        {
            this.mPlatform = plaf;
            this.emailAddress = null;
            this.mStats = null;
        }

        public void Authenticate(Action<bool> callback)
        {
            this.mPlatform.Authenticate(callback);
        }

        public void Authenticate(Action<bool, string> callback)
        {
            this.mPlatform.Authenticate(callback);
        }

        public void Authenticate(Action<bool> callback, bool silent)
        {
            this.mPlatform.Authenticate(callback, silent);
        }

        public void Authenticate(Action<bool, string> callback, bool silent)
        {
            this.mPlatform.Authenticate(callback, silent);
        }

        public string GetIdToken() => 
            this.mPlatform.GetIdToken();

        public void GetStats(Action<CommonStatusCodes, PlayerStats> callback)
        {
            <GetStats>c__AnonStorey0 storey = new <GetStats>c__AnonStorey0 {
                callback = callback,
                $this = this
            };
            if ((this.mStats == null) || !this.mStats.Valid)
            {
                this.mPlatform.GetPlayerStats(new Action<CommonStatusCodes, PlayerStats>(storey.<>m__0));
            }
            else
            {
                storey.callback(CommonStatusCodes.Success, this.mStats);
            }
        }

        public void LoadFriends(Action<bool> callback)
        {
            this.mPlatform.LoadFriends(this, callback);
        }

        public IUserProfile[] friends =>
            this.mPlatform.GetFriends();

        public bool authenticated =>
            this.mPlatform.IsAuthenticated();

        public bool underage =>
            true;

        public string userName
        {
            get
            {
                string userDisplayName = string.Empty;
                if (this.authenticated)
                {
                    userDisplayName = this.mPlatform.GetUserDisplayName();
                    if (!base.userName.Equals(userDisplayName))
                    {
                        base.ResetIdentity(userDisplayName, this.mPlatform.GetUserId(), this.mPlatform.GetUserImageUrl());
                    }
                }
                return userDisplayName;
            }
        }

        public string id
        {
            get
            {
                string userId = string.Empty;
                if (this.authenticated)
                {
                    userId = this.mPlatform.GetUserId();
                    if (!base.id.Equals(userId))
                    {
                        base.ResetIdentity(this.mPlatform.GetUserDisplayName(), userId, this.mPlatform.GetUserImageUrl());
                    }
                }
                return userId;
            }
        }

        public bool isFriend =>
            true;

        public UserState state =>
            UserState.Online;

        public string AvatarURL
        {
            get
            {
                string userImageUrl = string.Empty;
                if (this.authenticated)
                {
                    userImageUrl = this.mPlatform.GetUserImageUrl();
                    if (!base.id.Equals(userImageUrl))
                    {
                        base.ResetIdentity(this.mPlatform.GetUserDisplayName(), this.mPlatform.GetUserId(), userImageUrl);
                    }
                }
                return userImageUrl;
            }
        }

        public string Email
        {
            get
            {
                if (this.authenticated && string.IsNullOrEmpty(this.emailAddress))
                {
                    this.emailAddress = this.mPlatform.GetUserEmail();
                    if (this.emailAddress == null)
                    {
                    }
                    this.emailAddress = string.Empty;
                }
                return (!this.authenticated ? string.Empty : this.emailAddress);
            }
        }

        [CompilerGenerated]
        private sealed class <GetStats>c__AnonStorey0
        {
            internal Action<CommonStatusCodes, PlayerStats> callback;
            internal PlayGamesLocalUser $this;

            internal void <>m__0(CommonStatusCodes rc, PlayerStats stats)
            {
                this.$this.mStats = stats;
                this.callback(rc, stats);
            }
        }
    }
}

