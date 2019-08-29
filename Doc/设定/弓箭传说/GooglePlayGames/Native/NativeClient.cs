namespace GooglePlayGames.Native
{
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.BasicApi.Video;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class NativeClient : IPlayGamesClient
    {
        private readonly IClientImpl clientImpl;
        private readonly object GameServicesLock = new object();
        private readonly object AuthStateLock = new object();
        private readonly PlayGamesClientConfiguration mConfiguration;
        private GooglePlayGames.Native.PInvoke.GameServices mServices;
        private volatile NativeTurnBasedMultiplayerClient mTurnBasedClient;
        private volatile NativeRealtimeMultiplayerClient mRealTimeClient;
        private volatile ISavedGameClient mSavedGameClient;
        private volatile IEventsClient mEventsClient;
        private volatile IVideoClient mVideoClient;
        private volatile TokenClient mTokenClient;
        private volatile Action<Invitation, bool> mInvitationDelegate;
        private volatile Dictionary<string, Achievement> mAchievements;
        private volatile Player mUser;
        private volatile List<Player> mFriends;
        private volatile Action<bool, string> mPendingAuthCallbacks;
        private volatile AuthState mAuthState;
        private volatile uint mAuthGeneration;
        private volatile bool friendsLoading;
        [CompilerGenerated]
        private static Predicate<Achievement> <>f__am$cache0;
        [CompilerGenerated]
        private static Predicate<Achievement> <>f__am$cache1;

        internal NativeClient(PlayGamesClientConfiguration configuration, IClientImpl clientImpl)
        {
            PlayGamesHelperObject.CreateObject();
            this.mConfiguration = Misc.CheckNotNull<PlayGamesClientConfiguration>(configuration);
            this.clientImpl = clientImpl;
        }

        [CompilerGenerated]
        private static void <AsOnGameThreadCallback`1>m__0<T>(T)
        {
        }

        private static Action<T> AsOnGameThreadCallback<T>(Action<T> callback)
        {
            <AsOnGameThreadCallback>c__AnonStorey1<T> storey = new <AsOnGameThreadCallback>c__AnonStorey1<T> {
                callback = callback
            };
            if (storey.callback == null)
            {
                return new Action<T>(NativeClient.<AsOnGameThreadCallback`1>m__0<T>);
            }
            return new Action<T>(storey.<>m__0);
        }

        public void Authenticate(Action<bool, string> callback, bool silent)
        {
            <Authenticate>c__AnonStorey0 storey = new <Authenticate>c__AnonStorey0 {
                callback = callback,
                $this = this
            };
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                if (this.mAuthState == 1)
                {
                    InvokeCallbackOnGameThread<bool, string>(storey.callback, true, null);
                    return;
                }
            }
            this.friendsLoading = false;
            this.InitializeTokenClient();
            Debug.Log("Starting Auth with token client.");
            this.mTokenClient.FetchTokens(silent, new Action<int>(storey.<>m__0));
        }

        private GooglePlayGames.Native.PInvoke.GameServices GameServices()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mServices;
            }
        }

        public Achievement GetAchievement(string achId)
        {
            if ((this.mAchievements != null) && this.mAchievements.ContainsKey(achId))
            {
                return this.mAchievements[achId];
            }
            return null;
        }

        public void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
        {
            this.mTokenClient.GetAnotherServerAuthCode(reAuthenticateIfNeeded, callback);
        }

        public IntPtr GetApiClient() => 
            InternalHooks.InternalHooks_GetApiClient(this.mServices.AsHandle());

        public IEventsClient GetEventsClient()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mEventsClient;
            }
        }

        public IUserProfile[] GetFriends()
        {
            if ((this.mFriends == null) && !this.friendsLoading)
            {
                Logger.w("Getting friends before they are loaded!!!");
                this.friendsLoading = true;
                this.LoadFriends(delegate (bool ok) {
                    Logger.d(string.Concat(new object[] { "loading: ", ok, " mFriends = ", this.mFriends }));
                    if (!ok)
                    {
                        Logger.e("Friends list did not load successfully.  Disabling loading until re-authenticated");
                    }
                    this.friendsLoading = !ok;
                });
            }
            return ((this.mFriends != null) ? ((IUserProfile[]) this.mFriends.ToArray()) : new IUserProfile[0]);
        }

        public string GetIdToken()
        {
            if (!this.IsAuthenticated())
            {
                Debug.Log("Cannot get API client - not authenticated");
                return null;
            }
            return this.mTokenClient.GetIdToken();
        }

        public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
        {
            <GetPlayerStats>c__AnonStorey8 storey = new <GetPlayerStats>c__AnonStorey8 {
                callback = callback,
                $this = this
            };
            PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
        }

        public IRealTimeMultiplayerClient GetRtmpClient()
        {
            if (!this.IsAuthenticated())
            {
                return null;
            }
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mRealTimeClient;
            }
        }

        public ISavedGameClient GetSavedGameClient()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mSavedGameClient;
            }
        }

        public string GetServerAuthCode()
        {
            if (!this.IsAuthenticated())
            {
                Debug.Log("Cannot get API client - not authenticated");
                return null;
            }
            return this.mTokenClient.GetAuthCode();
        }

        public ITurnBasedMultiplayerClient GetTbmpClient()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mTurnBasedClient;
            }
        }

        public string GetUserDisplayName() => 
            this.mUser?.userName;

        public string GetUserEmail()
        {
            if (!this.IsAuthenticated())
            {
                Debug.Log("Cannot get API client - not authenticated");
                return null;
            }
            return this.mTokenClient.GetEmail();
        }

        public string GetUserId() => 
            this.mUser?.id;

        public string GetUserImageUrl() => 
            this.mUser?.AvatarURL;

        public IVideoClient GetVideoClient()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mVideoClient;
            }
        }

        private void HandleAuthTransition(Types.AuthOperation operation, CommonErrorStatus.AuthStatus status)
        {
            Logger.d(string.Concat(new object[] { "Starting Auth Transition. Op: ", operation, " status: ", status }));
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                if (operation != Types.AuthOperation.SIGN_IN)
                {
                    if (operation == Types.AuthOperation.SIGN_OUT)
                    {
                        goto Label_0102;
                    }
                    goto Label_010D;
                }
                if (status == CommonErrorStatus.AuthStatus.VALID)
                {
                    <HandleAuthTransition>c__AnonStorey6 storey = new <HandleAuthTransition>c__AnonStorey6 {
                        $this = this,
                        currentAuthGeneration = this.mAuthGeneration
                    };
                    this.mServices.AchievementManager().FetchAll(new Action<AchievementManager.FetchAllResponse>(storey.<>m__0));
                    this.mServices.PlayerManager().FetchSelf(new Action<PlayerManager.FetchSelfResponse>(storey.<>m__1));
                }
                else
                {
                    this.mAuthState = 0;
                    Logger.d("AuthState == " + ((AuthState) this.mAuthState) + " calling auth callbacks with failure");
                    Action<bool, string> mPendingAuthCallbacks = this.mPendingAuthCallbacks;
                    this.mPendingAuthCallbacks = null;
                    InvokeCallbackOnGameThread<bool, string>(mPendingAuthCallbacks, false, "Authentication failed");
                }
                goto Label_0136;
            Label_0102:
                this.ToUnauthenticated();
                goto Label_0136;
            Label_010D:
                Logger.e("Unknown AuthOperation " + operation);
            Label_0136:;
            }
        }

        internal void HandleInvitation(Types.MultiplayerEvent eventType, string invitationId, MultiplayerInvitation invitation)
        {
            <HandleInvitation>c__AnonStorey4 storey = new <HandleInvitation>c__AnonStorey4 {
                currentHandler = this.mInvitationDelegate
            };
            if (storey.currentHandler == null)
            {
                Logger.d(string.Concat(new object[] { "Received ", eventType, " for invitation ", invitationId, " but no handler was registered." }));
            }
            else if (eventType == Types.MultiplayerEvent.REMOVED)
            {
                Logger.d("Ignoring REMOVED for invitation " + invitationId);
            }
            else
            {
                storey.shouldAutolaunch = eventType == Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
                storey.invite = invitation.AsInvitation();
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
        }

        public void IncrementAchievement(string achId, int steps, Action<bool> callback)
        {
            <IncrementAchievement>c__AnonStoreyF yf = new <IncrementAchievement>c__AnonStoreyF {
                achId = achId,
                callback = callback,
                $this = this
            };
            Misc.CheckNotNull<string>(yf.achId);
            yf.callback = AsOnGameThreadCallback<bool>(yf.callback);
            this.InitializeGameServices();
            Achievement achievement = this.GetAchievement(yf.achId);
            if (achievement == null)
            {
                Logger.e("Could not increment, no achievement with ID " + yf.achId);
                yf.callback(false);
            }
            else if (!achievement.IsIncremental)
            {
                Logger.e("Could not increment, achievement with ID " + yf.achId + " was not incremental");
                yf.callback(false);
            }
            else if (steps < 0)
            {
                Logger.e("Attempted to increment by negative steps");
                yf.callback(false);
            }
            else
            {
                this.GameServices().AchievementManager().Increment(yf.achId, Convert.ToUInt32(steps));
                this.GameServices().AchievementManager().Fetch(yf.achId, new Action<AchievementManager.FetchResponse>(yf.<>m__0));
            }
        }

        private void InitializeGameServices()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                if (this.mServices == null)
                {
                    using (GameServicesBuilder builder = GameServicesBuilder.Create())
                    {
                        using (PlatformConfiguration configuration = this.clientImpl.CreatePlatformConfiguration(this.mConfiguration))
                        {
                            this.RegisterInvitationDelegate(this.mConfiguration.InvitationDelegate);
                            builder.SetOnAuthFinishedCallback(new GameServicesBuilder.AuthFinishedCallback(this.HandleAuthTransition));
                            builder.SetOnTurnBasedMatchEventCallback((eventType, matchId, match) => this.mTurnBasedClient.HandleMatchEvent(eventType, matchId, match));
                            builder.SetOnMultiplayerInvitationEventCallback(new Action<Types.MultiplayerEvent, string, MultiplayerInvitation>(this.HandleInvitation));
                            if (this.mConfiguration.EnableSavedGames)
                            {
                                builder.EnableSnapshots();
                            }
                            string[] scopes = this.mConfiguration.Scopes;
                            for (int i = 0; i < scopes.Length; i++)
                            {
                                builder.AddOauthScope(scopes[i]);
                            }
                            if (this.mConfiguration.IsHidingPopups)
                            {
                                builder.SetShowConnectingPopup(false);
                            }
                            Debug.Log("Building GPG services, implicitly attempts silent auth");
                            this.mServices = builder.Build(configuration);
                            this.mEventsClient = new NativeEventClient(new EventManager(this.mServices));
                            this.mVideoClient = new NativeVideoClient(new VideoManager(this.mServices));
                            this.mTurnBasedClient = new NativeTurnBasedMultiplayerClient(this, new TurnBasedManager(this.mServices));
                            this.mTurnBasedClient.RegisterMatchDelegate(this.mConfiguration.MatchDelegate);
                            this.mRealTimeClient = new NativeRealtimeMultiplayerClient(this, new RealtimeManager(this.mServices));
                            if (this.mConfiguration.EnableSavedGames)
                            {
                                this.mSavedGameClient = new NativeSavedGameClient(new SnapshotManager(this.mServices));
                            }
                            else
                            {
                                this.mSavedGameClient = new UnsupportedSavedGamesClient("You must enable saved games before it can be used. See PlayGamesClientConfiguration.Builder.EnableSavedGames.");
                            }
                            this.InitializeTokenClient();
                        }
                    }
                }
            }
        }

        private void InitializeTokenClient()
        {
            if (this.mTokenClient == null)
            {
                this.mTokenClient = this.clientImpl.CreateTokenClient(true);
                if (!GameInfo.WebClientIdInitialized() && (this.mConfiguration.IsRequestingIdToken || this.mConfiguration.IsRequestingAuthCode))
                {
                    Logger.e("Server Auth Code and ID Token require web clientId to configured.");
                }
                string[] scopes = this.mConfiguration.Scopes;
                this.mTokenClient.SetWebClientId("689911175141-n6mehbb5hf6srhh55dn9ks3mj7dfi73u.apps.googleusercontent.com");
                this.mTokenClient.SetRequestAuthCode(this.mConfiguration.IsRequestingAuthCode, this.mConfiguration.IsForcingRefresh);
                this.mTokenClient.SetRequestEmail(this.mConfiguration.IsRequestingEmail);
                this.mTokenClient.SetRequestIdToken(this.mConfiguration.IsRequestingIdToken);
                this.mTokenClient.SetHidePopups(this.mConfiguration.IsHidingPopups);
                string[] textArray1 = new string[] { "https://www.googleapis.com/auth/games_lite" };
                this.mTokenClient.AddOauthScopes(textArray1);
                if (this.mConfiguration.EnableSavedGames)
                {
                    string[] textArray2 = new string[] { "https://www.googleapis.com/auth/drive.appdata" };
                    this.mTokenClient.AddOauthScopes(textArray2);
                }
                this.mTokenClient.AddOauthScopes(scopes);
                this.mTokenClient.SetAccountName(this.mConfiguration.AccountName);
            }
        }

        private static void InvokeCallbackOnGameThread<T>(Action<T> callback, T data)
        {
            <InvokeCallbackOnGameThread>c__AnonStorey3<T> storey = new <InvokeCallbackOnGameThread>c__AnonStorey3<T> {
                callback = callback,
                data = data
            };
            if (storey.callback != null)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
        }

        private static void InvokeCallbackOnGameThread<T, S>(Action<T, S> callback, T data, S msg)
        {
            <InvokeCallbackOnGameThread>c__AnonStorey2<T, S> storey = new <InvokeCallbackOnGameThread>c__AnonStorey2<T, S> {
                callback = callback,
                data = data,
                msg = msg
            };
            if (storey.callback != null)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
        }

        public bool IsAuthenticated()
        {
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                return (this.mAuthState == 1);
            }
        }

        public int LeaderboardMaxResults() => 
            this.GameServices().LeaderboardManager().LeaderboardMaxResults;

        public void LoadAchievements(Action<Achievement[]> callback)
        {
            <LoadAchievements>c__AnonStoreyB yb = new <LoadAchievements>c__AnonStoreyB {
                callback = callback,
                data = new Achievement[this.mAchievements.Count]
            };
            this.mAchievements.Values.CopyTo(yb.data, 0);
            PlayGamesHelperObject.RunOnGameThread(new Action(yb.<>m__0));
        }

        public void LoadFriends(Action<bool> callback)
        {
            <LoadFriends>c__AnonStorey5 storey = new <LoadFriends>c__AnonStorey5 {
                callback = callback,
                $this = this
            };
            if (!this.IsAuthenticated())
            {
                Logger.d("Cannot loadFriends when not authenticated");
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
            else if (this.mFriends != null)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__1));
            }
            else
            {
                this.mServices.PlayerManager().FetchFriends(new Action<ResponseStatus, List<Player>>(storey.<>m__2));
            }
        }

        public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
        {
            callback = AsOnGameThreadCallback<LeaderboardScoreData>(callback);
            this.GameServices().LeaderboardManager().LoadScorePage(null, rowCount, token, callback);
        }

        public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
        {
            callback = AsOnGameThreadCallback<LeaderboardScoreData>(callback);
            this.GameServices().LeaderboardManager().LoadLeaderboardData(leaderboardId, start, rowCount, collection, timeSpan, this.mUser.id, callback);
        }

        public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
        {
            <LoadUsers>c__AnonStorey9 storey = new <LoadUsers>c__AnonStorey9 {
                callback = callback
            };
            this.mServices.PlayerManager().FetchList(userIds, new Action<NativePlayer[]>(storey.<>m__0));
        }

        private void MaybeFinishAuthentication()
        {
            Action<bool, string> callback = null;
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                if ((this.mUser == null) || (this.mAchievements == null))
                {
                    Logger.d(string.Concat(new object[] { "Auth not finished. User=", this.mUser, " achievements=", this.mAchievements }));
                    return;
                }
                Logger.d("Auth finished. Proceeding.");
                callback = this.mPendingAuthCallbacks;
                this.mPendingAuthCallbacks = null;
                this.mAuthState = 1;
            }
            if (callback != null)
            {
                Logger.d("Invoking Callbacks: " + callback);
                InvokeCallbackOnGameThread<bool, string>(callback, true, null);
            }
        }

        private void PopulateAchievements(uint authGeneration, AchievementManager.FetchAllResponse response)
        {
            if (authGeneration != this.mAuthGeneration)
            {
                Logger.d("Received achievement callback after signout occurred, ignoring");
            }
            else
            {
                Logger.d("Populating Achievements, status = " + response.Status());
                object authStateLock = this.AuthStateLock;
                lock (authStateLock)
                {
                    if ((response.Status() != CommonErrorStatus.ResponseStatus.VALID) && (response.Status() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
                    {
                        Logger.e("Error retrieving achievements - check the log for more information. Failing signin.");
                        Action<bool, string> mPendingAuthCallbacks = this.mPendingAuthCallbacks;
                        this.mPendingAuthCallbacks = null;
                        if (mPendingAuthCallbacks != null)
                        {
                            InvokeCallbackOnGameThread<bool, string>(mPendingAuthCallbacks, false, "Cannot load achievements, Authenication failing");
                        }
                        this.SignOut();
                        return;
                    }
                    Dictionary<string, Achievement> dictionary = new Dictionary<string, Achievement>();
                    foreach (NativeAchievement achievement in response)
                    {
                        using (achievement)
                        {
                            dictionary[achievement.Id()] = achievement.AsAchievement();
                        }
                    }
                    Logger.d("Found " + dictionary.Count + " Achievements");
                    this.mAchievements = dictionary;
                }
                Logger.d("Maybe finish for Achievements");
                this.MaybeFinishAuthentication();
            }
        }

        private void PopulateUser(uint authGeneration, PlayerManager.FetchSelfResponse response)
        {
            Logger.d("Populating User");
            if (authGeneration != this.mAuthGeneration)
            {
                Logger.d("Received user callback after signout occurred, ignoring");
            }
            else
            {
                object authStateLock = this.AuthStateLock;
                lock (authStateLock)
                {
                    if ((response.Status() != CommonErrorStatus.ResponseStatus.VALID) && (response.Status() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
                    {
                        Logger.e("Error retrieving user, signing out");
                        Action<bool, string> mPendingAuthCallbacks = this.mPendingAuthCallbacks;
                        this.mPendingAuthCallbacks = null;
                        if (mPendingAuthCallbacks != null)
                        {
                            InvokeCallbackOnGameThread<bool, string>(mPendingAuthCallbacks, false, "Cannot load user profile");
                        }
                        this.SignOut();
                        return;
                    }
                    this.mUser = response.Self().AsPlayer();
                    this.mFriends = null;
                }
                Logger.d("Found User: " + this.mUser);
                Logger.d("Maybe finish for User");
                this.MaybeFinishAuthentication();
            }
        }

        public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
        {
            <RegisterInvitationDelegate>c__AnonStorey13 storey = new <RegisterInvitationDelegate>c__AnonStorey13 {
                invitationDelegate = invitationDelegate
            };
            if (storey.invitationDelegate == null)
            {
                this.mInvitationDelegate = null;
            }
            else
            {
                this.mInvitationDelegate = Callbacks.AsOnGameThreadCallback<Invitation, bool>(new Action<Invitation, bool>(storey.<>m__0));
            }
        }

        public void RevealAchievement(string achId, Action<bool> callback)
        {
            <RevealAchievement>c__AnonStoreyD yd = new <RevealAchievement>c__AnonStoreyD {
                achId = achId,
                $this = this
            };
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = a => a.IsRevealed;
            }
            this.UpdateAchievement("Reveal", yd.achId, callback, <>f__am$cache1, new Action<Achievement>(yd.<>m__0));
        }

        public void SetGravityForPopups(Gravity gravity)
        {
            <SetGravityForPopups>c__AnonStorey7 storey = new <SetGravityForPopups>c__AnonStorey7 {
                gravity = gravity,
                $this = this
            };
            PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
        }

        public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
        {
            <SetStepsAtLeast>c__AnonStorey10 storey = new <SetStepsAtLeast>c__AnonStorey10 {
                achId = achId,
                callback = callback,
                $this = this
            };
            Misc.CheckNotNull<string>(storey.achId);
            storey.callback = AsOnGameThreadCallback<bool>(storey.callback);
            this.InitializeGameServices();
            Achievement achievement = this.GetAchievement(storey.achId);
            if (achievement == null)
            {
                Logger.e("Could not increment, no achievement with ID " + storey.achId);
                storey.callback(false);
            }
            else if (!achievement.IsIncremental)
            {
                Logger.e("Could not increment, achievement with ID " + storey.achId + " is not incremental");
                storey.callback(false);
            }
            else if (steps < 0)
            {
                Logger.e("Attempted to increment by negative steps");
                storey.callback(false);
            }
            else
            {
                this.GameServices().AchievementManager().SetStepsAtLeast(storey.achId, Convert.ToUInt32(steps));
                this.GameServices().AchievementManager().Fetch(storey.achId, new Action<AchievementManager.FetchResponse>(storey.<>m__0));
            }
        }

        public void ShowAchievementsUI(Action<UIStatus> cb)
        {
            <ShowAchievementsUI>c__AnonStorey11 storey = new <ShowAchievementsUI>c__AnonStorey11 {
                cb = cb
            };
            if (this.IsAuthenticated())
            {
                Action<CommonErrorStatus.UIStatus> noopUICallback = Callbacks.NoopUICallback;
                if (storey.cb != null)
                {
                    noopUICallback = new Action<CommonErrorStatus.UIStatus>(storey.<>m__0);
                }
                noopUICallback = AsOnGameThreadCallback<CommonErrorStatus.UIStatus>(noopUICallback);
                this.GameServices().AchievementManager().ShowAllUI(noopUICallback);
            }
        }

        public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> cb)
        {
            <ShowLeaderboardUI>c__AnonStorey12 storey = new <ShowLeaderboardUI>c__AnonStorey12 {
                cb = cb
            };
            if (this.IsAuthenticated())
            {
                Action<CommonErrorStatus.UIStatus> noopUICallback = Callbacks.NoopUICallback;
                if (storey.cb != null)
                {
                    noopUICallback = new Action<CommonErrorStatus.UIStatus>(storey.<>m__0);
                }
                noopUICallback = AsOnGameThreadCallback<CommonErrorStatus.UIStatus>(noopUICallback);
                if (leaderboardId == null)
                {
                    this.GameServices().LeaderboardManager().ShowAllUI(noopUICallback);
                }
                else
                {
                    this.GameServices().LeaderboardManager().ShowUI(leaderboardId, span, noopUICallback);
                }
            }
        }

        public void SignOut()
        {
            this.ToUnauthenticated();
            if (this.GameServices() != null)
            {
                this.mTokenClient.Signout();
                this.GameServices().SignOut();
            }
        }

        public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
        {
            callback = AsOnGameThreadCallback<bool>(callback);
            if (!this.IsAuthenticated())
            {
                callback(false);
            }
            this.InitializeGameServices();
            if (leaderboardId == null)
            {
                throw new ArgumentNullException("leaderboardId");
            }
            this.GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, null);
            callback(true);
        }

        public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
        {
            callback = AsOnGameThreadCallback<bool>(callback);
            if (!this.IsAuthenticated())
            {
                callback(false);
            }
            this.InitializeGameServices();
            if (leaderboardId == null)
            {
                throw new ArgumentNullException("leaderboardId");
            }
            this.GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, metadata);
            callback(true);
        }

        private void ToUnauthenticated()
        {
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                this.mUser = null;
                this.mFriends = null;
                this.mAchievements = null;
                this.mAuthState = 0;
                this.mTokenClient = this.clientImpl.CreateTokenClient(true);
                this.mAuthGeneration++;
            }
        }

        public void UnlockAchievement(string achId, Action<bool> callback)
        {
            <UnlockAchievement>c__AnonStoreyC yc = new <UnlockAchievement>c__AnonStoreyC {
                achId = achId,
                $this = this
            };
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = a => a.IsUnlocked;
            }
            this.UpdateAchievement("Unlock", yc.achId, callback, <>f__am$cache0, new Action<Achievement>(yc.<>m__0));
        }

        private void UpdateAchievement(string updateType, string achId, Action<bool> callback, Predicate<Achievement> alreadyDone, Action<Achievement> updateAchievment)
        {
            <UpdateAchievement>c__AnonStoreyE ye = new <UpdateAchievement>c__AnonStoreyE {
                achId = achId,
                callback = callback,
                $this = this
            };
            ye.callback = AsOnGameThreadCallback<bool>(ye.callback);
            Misc.CheckNotNull<string>(ye.achId);
            this.InitializeGameServices();
            Achievement achievement = this.GetAchievement(ye.achId);
            if (achievement == null)
            {
                Logger.d("Could not " + updateType + ", no achievement with ID " + ye.achId);
                ye.callback(false);
            }
            else if (alreadyDone(achievement))
            {
                Logger.d("Did not need to perform " + updateType + ": on achievement " + ye.achId);
                ye.callback(true);
            }
            else
            {
                Logger.d("Performing " + updateType + " on " + ye.achId);
                updateAchievment(achievement);
                this.GameServices().AchievementManager().Fetch(ye.achId, new Action<AchievementManager.FetchResponse>(ye.<>m__0));
            }
        }

        [CompilerGenerated]
        private sealed class <AsOnGameThreadCallback>c__AnonStorey1<T>
        {
            internal Action<T> callback;

            internal void <>m__0(T result)
            {
                NativeClient.InvokeCallbackOnGameThread<T>(this.callback, result);
            }
        }

        [CompilerGenerated]
        private sealed class <Authenticate>c__AnonStorey0
        {
            internal Action<bool, string> callback;
            internal NativeClient $this;

            internal void <>m__0(int result)
            {
                if (result == 0)
                {
                    if (this.callback != null)
                    {
                        this.$this.mPendingAuthCallbacks = (Action<bool, string>) Delegate.Combine(this.$this.mPendingAuthCallbacks, this.callback);
                    }
                    this.$this.InitializeGameServices();
                    this.$this.GameServices().StartAuthorizationUI();
                }
                else
                {
                    Action<bool, string> callback = this.callback;
                    if (result == 0x10)
                    {
                        NativeClient.InvokeCallbackOnGameThread<bool, string>(callback, false, "Authentication canceled");
                    }
                    else if (result == 8)
                    {
                        NativeClient.InvokeCallbackOnGameThread<bool, string>(callback, false, "Authentication failed - developer error");
                    }
                    else
                    {
                        NativeClient.InvokeCallbackOnGameThread<bool, string>(callback, false, "Authentication failed");
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetPlayerStats>c__AnonStorey8
        {
            internal Action<CommonStatusCodes, PlayerStats> callback;
            internal NativeClient $this;

            internal void <>m__0()
            {
                this.$this.clientImpl.GetPlayerStats(this.$this.GetApiClient(), this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <HandleAuthTransition>c__AnonStorey6
        {
            internal uint currentAuthGeneration;
            internal NativeClient $this;

            internal void <>m__0(AchievementManager.FetchAllResponse results)
            {
                this.$this.PopulateAchievements(this.currentAuthGeneration, results);
            }

            internal void <>m__1(PlayerManager.FetchSelfResponse results)
            {
                this.$this.PopulateUser(this.currentAuthGeneration, results);
            }
        }

        [CompilerGenerated]
        private sealed class <HandleInvitation>c__AnonStorey4
        {
            internal Action<Invitation, bool> currentHandler;
            internal Invitation invite;
            internal bool shouldAutolaunch;

            internal void <>m__0()
            {
                this.currentHandler(this.invite, this.shouldAutolaunch);
            }
        }

        [CompilerGenerated]
        private sealed class <IncrementAchievement>c__AnonStoreyF
        {
            internal string achId;
            internal Action<bool> callback;
            internal NativeClient $this;

            internal void <>m__0(AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.$this.mAchievements.Remove(this.achId);
                    this.$this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <InvokeCallbackOnGameThread>c__AnonStorey2<T, S>
        {
            internal Action<T, S> callback;
            internal T data;
            internal S msg;

            internal void <>m__0()
            {
                Logger.d("Invoking user callback on game thread");
                this.callback(this.data, this.msg);
            }
        }

        [CompilerGenerated]
        private sealed class <InvokeCallbackOnGameThread>c__AnonStorey3<T>
        {
            internal Action<T> callback;
            internal T data;

            internal void <>m__0()
            {
                Logger.d("Invoking user callback on game thread");
                this.callback(this.data);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAchievements>c__AnonStoreyB
        {
            internal Action<Achievement[]> callback;
            internal Achievement[] data;

            internal void <>m__0()
            {
                this.callback(this.data);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadFriends>c__AnonStorey5
        {
            internal Action<bool> callback;
            internal NativeClient $this;

            internal void <>m__0()
            {
                this.callback(false);
            }

            internal void <>m__1()
            {
                this.callback(true);
            }

            internal void <>m__2(ResponseStatus status, List<Player> players)
            {
                if ((status == ResponseStatus.Success) || (status == ResponseStatus.SuccessWithStale))
                {
                    this.$this.mFriends = players;
                    PlayGamesHelperObject.RunOnGameThread(() => this.callback(true));
                }
                else
                {
                    this.$this.mFriends = new List<Player>();
                    Logger.e("Got " + status + " loading friends");
                    PlayGamesHelperObject.RunOnGameThread(() => this.callback(false));
                }
            }

            internal void <>m__3()
            {
                this.callback(true);
            }

            internal void <>m__4()
            {
                this.callback(false);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadUsers>c__AnonStorey9
        {
            internal Action<IUserProfile[]> callback;

            internal void <>m__0(NativePlayer[] nativeUsers)
            {
                <LoadUsers>c__AnonStoreyA ya = new <LoadUsers>c__AnonStoreyA {
                    <>f__ref$9 = this,
                    users = new IUserProfile[nativeUsers.Length]
                };
                for (int i = 0; i < ya.users.Length; i++)
                {
                    ya.users[i] = nativeUsers[i].AsPlayer();
                }
                PlayGamesHelperObject.RunOnGameThread(new Action(ya.<>m__0));
            }

            private sealed class <LoadUsers>c__AnonStoreyA
            {
                internal IUserProfile[] users;
                internal NativeClient.<LoadUsers>c__AnonStorey9 <>f__ref$9;

                internal void <>m__0()
                {
                    this.<>f__ref$9.callback(this.users);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterInvitationDelegate>c__AnonStorey13
        {
            internal InvitationReceivedDelegate invitationDelegate;

            internal void <>m__0(Invitation invitation, bool autoAccept)
            {
                this.invitationDelegate(invitation, autoAccept);
            }
        }

        [CompilerGenerated]
        private sealed class <RevealAchievement>c__AnonStoreyD
        {
            internal string achId;
            internal NativeClient $this;

            internal void <>m__0(Achievement a)
            {
                a.IsRevealed = true;
                this.$this.GameServices().AchievementManager().Reveal(this.achId);
            }
        }

        [CompilerGenerated]
        private sealed class <SetGravityForPopups>c__AnonStorey7
        {
            internal Gravity gravity;
            internal NativeClient $this;

            internal void <>m__0()
            {
                this.$this.clientImpl.SetGravityForPopups(this.$this.GetApiClient(), this.gravity);
            }
        }

        [CompilerGenerated]
        private sealed class <SetStepsAtLeast>c__AnonStorey10
        {
            internal string achId;
            internal Action<bool> callback;
            internal NativeClient $this;

            internal void <>m__0(AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.$this.mAchievements.Remove(this.achId);
                    this.$this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ShowAchievementsUI>c__AnonStorey11
        {
            internal Action<UIStatus> cb;

            internal void <>m__0(CommonErrorStatus.UIStatus result)
            {
                this.cb((UIStatus) result);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowLeaderboardUI>c__AnonStorey12
        {
            internal Action<UIStatus> cb;

            internal void <>m__0(CommonErrorStatus.UIStatus result)
            {
                this.cb((UIStatus) result);
            }
        }

        [CompilerGenerated]
        private sealed class <UnlockAchievement>c__AnonStoreyC
        {
            internal string achId;
            internal NativeClient $this;

            internal void <>m__0(Achievement a)
            {
                a.IsUnlocked = true;
                this.$this.GameServices().AchievementManager().Unlock(this.achId);
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAchievement>c__AnonStoreyE
        {
            internal string achId;
            internal Action<bool> callback;
            internal NativeClient $this;

            internal void <>m__0(AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.$this.mAchievements.Remove(this.achId);
                    this.$this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        private enum AuthState
        {
            Unauthenticated,
            Authenticated
        }
    }
}

