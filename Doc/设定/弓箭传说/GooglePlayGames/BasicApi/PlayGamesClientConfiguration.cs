namespace GooglePlayGames.BasicApi
{
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PlayGamesClientConfiguration
    {
        public static readonly PlayGamesClientConfiguration DefaultConfiguration;
        private readonly bool mEnableSavedGames;
        private readonly string[] mScopes;
        private readonly bool mRequestAuthCode;
        private readonly bool mForceRefresh;
        private readonly bool mHidePopups;
        private readonly bool mRequestEmail;
        private readonly bool mRequestIdToken;
        private readonly string mAccountName;
        private readonly InvitationReceivedDelegate mInvitationDelegate;
        private readonly GooglePlayGames.BasicApi.Multiplayer.MatchDelegate mMatchDelegate;
        private PlayGamesClientConfiguration(Builder builder)
        {
            this.mEnableSavedGames = builder.HasEnableSaveGames();
            this.mInvitationDelegate = builder.GetInvitationDelegate();
            this.mMatchDelegate = builder.GetMatchDelegate();
            this.mScopes = builder.getScopes();
            this.mHidePopups = builder.IsHidingPopups();
            this.mRequestAuthCode = builder.IsRequestingAuthCode();
            this.mForceRefresh = builder.IsForcingRefresh();
            this.mRequestEmail = builder.IsRequestingEmail();
            this.mRequestIdToken = builder.IsRequestingIdToken();
            this.mAccountName = builder.GetAccountName();
        }

        public bool EnableSavedGames =>
            this.mEnableSavedGames;
        public bool IsHidingPopups =>
            this.mHidePopups;
        public bool IsRequestingAuthCode =>
            this.mRequestAuthCode;
        public bool IsForcingRefresh =>
            this.mForceRefresh;
        public bool IsRequestingEmail =>
            this.mRequestEmail;
        public bool IsRequestingIdToken =>
            this.mRequestIdToken;
        public string AccountName =>
            this.mAccountName;
        public string[] Scopes =>
            this.mScopes;
        public InvitationReceivedDelegate InvitationDelegate =>
            this.mInvitationDelegate;
        public GooglePlayGames.BasicApi.Multiplayer.MatchDelegate MatchDelegate =>
            this.mMatchDelegate;
        static PlayGamesClientConfiguration()
        {
            DefaultConfiguration = new Builder().Build();
        }
        public class Builder
        {
            private bool mEnableSaveGames;
            private List<string> mScopes;
            private bool mHidePopups;
            private bool mRequestAuthCode;
            private bool mForceRefresh;
            private bool mRequestEmail;
            private bool mRequestIdToken;
            private string mAccountName;
            private InvitationReceivedDelegate mInvitationDelegate;
            private MatchDelegate mMatchDelegate;
            [CompilerGenerated]
            private static InvitationReceivedDelegate <>f__am$cache0;
            [CompilerGenerated]
            private static MatchDelegate <>f__am$cache1;

            public Builder()
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = new InvitationReceivedDelegate(PlayGamesClientConfiguration.Builder.<mInvitationDelegate>m__0);
                }
                this.mInvitationDelegate = <>f__am$cache0;
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = new MatchDelegate(PlayGamesClientConfiguration.Builder.<mMatchDelegate>m__1);
                }
                this.mMatchDelegate = <>f__am$cache1;
            }

            [CompilerGenerated]
            private static void <mInvitationDelegate>m__0(Invitation, bool)
            {
            }

            [CompilerGenerated]
            private static void <mMatchDelegate>m__1(TurnBasedMatch, bool)
            {
            }

            public PlayGamesClientConfiguration.Builder AddOauthScope(string scope)
            {
                if (this.mScopes == null)
                {
                    this.mScopes = new List<string>();
                }
                this.mScopes.Add(scope);
                return this;
            }

            public PlayGamesClientConfiguration Build() => 
                new PlayGamesClientConfiguration(this);

            public PlayGamesClientConfiguration.Builder EnableHidePopups()
            {
                this.mHidePopups = true;
                return this;
            }

            public PlayGamesClientConfiguration.Builder EnableSavedGames()
            {
                this.mEnableSaveGames = true;
                return this;
            }

            internal string GetAccountName() => 
                this.mAccountName;

            internal InvitationReceivedDelegate GetInvitationDelegate() => 
                this.mInvitationDelegate;

            internal MatchDelegate GetMatchDelegate() => 
                this.mMatchDelegate;

            internal string[] getScopes() => 
                ((this.mScopes != null) ? this.mScopes.ToArray() : new string[0]);

            internal bool HasEnableSaveGames() => 
                this.mEnableSaveGames;

            internal bool IsForcingRefresh() => 
                this.mForceRefresh;

            internal bool IsHidingPopups() => 
                this.mHidePopups;

            internal bool IsRequestingAuthCode() => 
                this.mRequestAuthCode;

            internal bool IsRequestingEmail() => 
                this.mRequestEmail;

            internal bool IsRequestingIdToken() => 
                this.mRequestIdToken;

            public PlayGamesClientConfiguration.Builder RequestEmail()
            {
                this.mRequestEmail = true;
                return this;
            }

            public PlayGamesClientConfiguration.Builder RequestIdToken()
            {
                this.mRequestIdToken = true;
                return this;
            }

            public PlayGamesClientConfiguration.Builder RequestServerAuthCode(bool forceRefresh)
            {
                this.mRequestAuthCode = true;
                this.mForceRefresh = forceRefresh;
                return this;
            }

            public PlayGamesClientConfiguration.Builder SetAccountName(string accountName)
            {
                this.mAccountName = accountName;
                return this;
            }

            public PlayGamesClientConfiguration.Builder WithInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
            {
                this.mInvitationDelegate = Misc.CheckNotNull<InvitationReceivedDelegate>(invitationDelegate);
                return this;
            }

            public PlayGamesClientConfiguration.Builder WithMatchDelegate(MatchDelegate matchDelegate)
            {
                this.mMatchDelegate = Misc.CheckNotNull<MatchDelegate>(matchDelegate);
                return this;
            }
        }
    }
}

