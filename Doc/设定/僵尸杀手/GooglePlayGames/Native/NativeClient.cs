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
        private volatile Dictionary<string, GooglePlayGames.BasicApi.Achievement> mAchievements;
        private volatile GooglePlayGames.BasicApi.Multiplayer.Player mUser;
        private volatile List<GooglePlayGames.BasicApi.Multiplayer.Player> mFriends;
        private volatile Action<bool, string> mPendingAuthCallbacks;
        private volatile Action<bool, string> mSilentAuthCallbacks;
        private volatile AuthState mAuthState;
        private volatile uint mAuthGeneration;
        private volatile bool mSilentAuthFailed;
        private volatile bool friendsLoading;
        [CompilerGenerated]
        private static Predicate<GooglePlayGames.BasicApi.Achievement> <>f__am$cache0;
        [CompilerGenerated]
        private static Predicate<GooglePlayGames.BasicApi.Achievement> <>f__am$cache1;

        internal NativeClient(PlayGamesClientConfiguration configuration, IClientImpl clientImpl)
        {
            PlayGamesHelperObject.CreateObject();
            this.mConfiguration = Misc.CheckNotNull<PlayGamesClientConfiguration>(configuration);
            this.clientImpl = clientImpl;
        }

        [CompilerGenerated]
        private static void <AsOnGameThreadCallback`1>m__1<T>(T)
        {
        }

        private static Action<T> AsOnGameThreadCallback<T>(Action<T> callback)
        {
            <AsOnGameThreadCallback>c__AnonStorey0<T> storey = new <AsOnGameThreadCallback>c__AnonStorey0<T> {
                callback = callback
            };
            if (storey.callback == null)
            {
                return new Action<T>(NativeClient.<AsOnGameThreadCallback`1>m__1<T>);
            }
            return new Action<T>(storey.<>m__0);
        }

        public void Authenticate(Action<bool, string> callback, bool silent)
        {
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                if (this.mAuthState == 1)
                {
                    InvokeCallbackOnGameThread<bool, string>(callback, true, null);
                    return;
                }
                if (this.mSilentAuthFailed && silent)
                {
                    InvokeCallbackOnGameThread<bool, string>(callback, false, "silent auth failed");
                    return;
                }
                if (callback != null)
                {
                    if (silent)
                    {
                        this.mSilentAuthCallbacks = (Action<bool, string>) Delegate.Combine(this.mSilentAuthCallbacks, callback);
                    }
                    else
                    {
                        this.mPendingAuthCallbacks = (Action<bool, string>) Delegate.Combine(this.mPendingAuthCallbacks, callback);
                    }
                }
            }
            this.friendsLoading = false;
            this.InitializeTokenClient();
            if (this.mTokenClient.NeedsToRun())
            {
                Debug.Log("Starting Auth with token client.");
                this.mTokenClient.FetchTokens(delegate (int result) {
                    this.InitializeGameServices();
                    if (result == 0)
                    {
                        this.GameServices().StartAuthorizationUI();
                    }
                    else
                    {
                        this.HandleAuthTransition(GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN, (GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus) result);
                    }
                });
            }
            else
            {
                this.InitializeGameServices();
                if (!silent)
                {
                    this.GameServices().StartAuthorizationUI();
                }
            }
        }

        private GooglePlayGames.Native.PInvoke.GameServices GameServices()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mServices;
            }
        }

        public GooglePlayGames.BasicApi.Achievement GetAchievement(string achId)
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
                GooglePlayGames.OurUtils.Logger.w("Getting friends before they are loaded!!!");
                this.friendsLoading = true;
                this.LoadFriends(delegate (bool ok) {
                    GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "loading: ", ok, " mFriends = ", this.mFriends }));
                    if (!ok)
                    {
                        GooglePlayGames.OurUtils.Logger.e("Friends list did not load successfully.  Disabling loading until re-authenticated");
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

        public void GetPlayerStats(Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback)
        {
            <GetPlayerStats>c__AnonStorey7 storey = new <GetPlayerStats>c__AnonStorey7 {
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

        private void HandleAuthTransition(GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation, GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus status)
        {
            GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "Starting Auth Transition. Op: ", operation, " status: ", status }));
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                if (operation != GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN)
                {
                    if (operation == GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_OUT)
                    {
                        goto Label_01A2;
                    }
                    goto Label_01AD;
                }
                if (status == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus.VALID)
                {
                    <HandleAuthTransition>c__AnonStorey5 storey = new <HandleAuthTransition>c__AnonStorey5 {
                        $this = this
                    };
                    if (this.mSilentAuthCallbacks != null)
                    {
                        this.mPendingAuthCallbacks = (Action<bool, string>) Delegate.Combine(this.mPendingAuthCallbacks, this.mSilentAuthCallbacks);
                        this.mSilentAuthCallbacks = null;
                    }
                    storey.currentAuthGeneration = this.mAuthGeneration;
                    this.mServices.AchievementManager().FetchAll(new Action<GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse>(storey.<>m__0));
                    this.mServices.PlayerManager().FetchSelf(new Action<GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse>(storey.<>m__1));
                }
                else if (this.mAuthState == 2)
                {
                    this.mSilentAuthFailed = true;
                    this.mAuthState = 0;
                    Action<bool, string> mSilentAuthCallbacks = this.mSilentAuthCallbacks;
                    this.mSilentAuthCallbacks = null;
                    GooglePlayGames.OurUtils.Logger.d("Invoking callbacks, AuthState changed from silentPending to Unauthenticated.");
                    InvokeCallbackOnGameThread<bool, string>(mSilentAuthCallbacks, false, "silent auth failed");
                    if (this.mPendingAuthCallbacks != null)
                    {
                        GooglePlayGames.OurUtils.Logger.d("there are pending auth callbacks - starting AuthUI");
                        this.GameServices().StartAuthorizationUI();
                    }
                }
                else
                {
                    GooglePlayGames.OurUtils.Logger.d("AuthState == " + ((AuthState) this.mAuthState) + " calling auth callbacks with failure");
                    this.UnpauseUnityPlayer();
                    Action<bool, string> mPendingAuthCallbacks = this.mPendingAuthCallbacks;
                    this.mPendingAuthCallbacks = null;
                    InvokeCallbackOnGameThread<bool, string>(mPendingAuthCallbacks, false, "Authentication failed");
                }
                goto Label_01D3;
            Label_01A2:
                this.ToUnauthenticated();
                goto Label_01D3;
            Label_01AD:
                GooglePlayGames.OurUtils.Logger.e("Unknown AuthOperation " + operation);
            Label_01D3:;
            }
        }

        internal void HandleInvitation(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string invitationId, GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
        {
            <HandleInvitation>c__AnonStorey3 storey = new <HandleInvitation>c__AnonStorey3 {
                currentHandler = this.mInvitationDelegate
            };
            if (storey.currentHandler == null)
            {
                GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "Received ", eventType, " for invitation ", invitationId, " but no handler was registered." }));
            }
            else if (eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.REMOVED)
            {
                GooglePlayGames.OurUtils.Logger.d("Ignoring REMOVED for invitation " + invitationId);
            }
            else
            {
                storey.shouldAutolaunch = eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
                storey.invite = invitation.AsInvitation();
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
        }

        public void IncrementAchievement(string achId, int steps, Action<bool> callback)
        {
            <IncrementAchievement>c__AnonStoreyE ye = new <IncrementAchievement>c__AnonStoreyE {
                achId = achId,
                callback = callback,
                $this = this
            };
            Misc.CheckNotNull<string>(ye.achId);
            ye.callback = AsOnGameThreadCallback<bool>(ye.callback);
            this.InitializeGameServices();
            GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(ye.achId);
            if (achievement == null)
            {
                GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + ye.achId);
                ye.callback(false);
            }
            else if (!achievement.IsIncremental)
            {
                GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + ye.achId + " was not incremental");
                ye.callback(false);
            }
            else if (steps < 0)
            {
                GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
                ye.callback(false);
            }
            else
            {
                this.GameServices().AchievementManager().Increment(ye.achId, Convert.ToUInt32(steps));
                this.GameServices().AchievementManager().Fetch(ye.achId, new Action<GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse>(ye.<>m__0));
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
                            builder.SetOnMultiplayerInvitationEventCallback(new Action<GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation>(this.HandleInvitation));
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
                            this.mAuthState = 2;
                            this.mServices = builder.Build(configuration);
                            this.mEventsClient = new NativeEventClient(new GooglePlayGames.Native.PInvoke.EventManager(this.mServices));
                            this.mVideoClient = new NativeVideoClient(new GooglePlayGames.Native.PInvoke.VideoManager(this.mServices));
                            this.mTurnBasedClient = new NativeTurnBasedMultiplayerClient(this, new TurnBasedManager(this.mServices));
                            this.mTurnBasedClient.RegisterMatchDelegate(this.mConfiguration.MatchDelegate);
                            this.mRealTimeClient = new NativeRealtimeMultiplayerClient(this, new RealtimeManager(this.mServices));
                            if (this.mConfiguration.EnableSavedGames)
                            {
                                this.mSavedGameClient = new NativeSavedGameClient(new GooglePlayGames.Native.PInvoke.SnapshotManager(this.mServices));
                            }
                            else
                            {
                                this.mSavedGameClient = new UnsupportedSavedGamesClient("You must enable saved games before it can be used. See PlayGamesClientConfiguration.Builder.EnableSavedGames.");
                            }
                            this.mAuthState = 2;
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
                    GooglePlayGames.OurUtils.Logger.e("Server Auth Code and ID Token require web clientId to configured.");
                }
                string[] scopes = this.mConfiguration.Scopes;
                this.mTokenClient.SetWebClientId(string.Empty);
                this.mTokenClient.SetRequestAuthCode(this.mConfiguration.IsRequestingAuthCode, this.mConfiguration.IsForcingRefresh);
                this.mTokenClient.SetRequestEmail(this.mConfiguration.IsRequestingEmail);
                this.mTokenClient.SetRequestIdToken(this.mConfiguration.IsRequestingIdToken);
                this.mTokenClient.SetHidePopups(this.mConfiguration.IsHidingPopups);
                this.mTokenClient.AddOauthScopes(scopes);
                this.mTokenClient.SetAccountName(this.mConfiguration.AccountName);
            }
        }

        private static void InvokeCallbackOnGameThread<T>(Action<T> callback, T data)
        {
            <InvokeCallbackOnGameThread>c__AnonStorey2<T> storey = new <InvokeCallbackOnGameThread>c__AnonStorey2<T> {
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
            <InvokeCallbackOnGameThread>c__AnonStorey1<T, S> storey = new <InvokeCallbackOnGameThread>c__AnonStorey1<T, S> {
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

        public void LoadAchievements(Action<GooglePlayGames.BasicApi.Achievement[]> callback)
        {
            <LoadAchievements>c__AnonStoreyA ya = new <LoadAchievements>c__AnonStoreyA {
                callback = callback,
                data = new GooglePlayGames.BasicApi.Achievement[this.mAchievements.Count]
            };
            this.mAchievements.Values.CopyTo(ya.data, 0);
            PlayGamesHelperObject.RunOnGameThread(new Action(ya.<>m__0));
        }

        public void LoadFriends(Action<bool> callback)
        {
            <LoadFriends>c__AnonStorey4 storey = new <LoadFriends>c__AnonStorey4 {
                callback = callback,
                $this = this
            };
            if (!this.IsAuthenticated())
            {
                GooglePlayGames.OurUtils.Logger.d("Cannot loadFriends when not authenticated");
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
            else if (this.mFriends != null)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__1));
            }
            else
            {
                this.mServices.PlayerManager().FetchFriends(new Action<GooglePlayGames.BasicApi.ResponseStatus, List<GooglePlayGames.BasicApi.Multiplayer.Player>>(storey.<>m__2));
            }
        }

        public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
        {
            callback = AsOnGameThreadCallback<LeaderboardScoreData>(callback);
            this.GameServices().LeaderboardManager().LoadScorePage(null, rowCount, token, callback);
        }

        public void LoadScores(string leaderboardId, GooglePlayGames.BasicApi.LeaderboardStart start, int rowCount, GooglePlayGames.BasicApi.LeaderboardCollection collection, GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
        {
            callback = AsOnGameThreadCallback<LeaderboardScoreData>(callback);
            this.GameServices().LeaderboardManager().LoadLeaderboardData(leaderboardId, start, rowCount, collection, timeSpan, this.mUser.id, callback);
        }

        public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
        {
            <LoadUsers>c__AnonStorey8 storey = new <LoadUsers>c__AnonStorey8 {
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
                    GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "Auth not finished. User=", this.mUser, " achievements=", this.mAchievements }));
                    return;
                }
                GooglePlayGames.OurUtils.Logger.d("Auth finished. Proceeding.");
                callback = this.mPendingAuthCallbacks;
                this.mPendingAuthCallbacks = null;
                this.mAuthState = 1;
            }
            if (callback != null)
            {
                GooglePlayGames.OurUtils.Logger.d("Invoking Callbacks: " + callback);
                InvokeCallbackOnGameThread<bool, string>(callback, true, null);
            }
        }

        private void PopulateAchievements(uint authGeneration, GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse response)
        {
            if (authGeneration != this.mAuthGeneration)
            {
                GooglePlayGames.OurUtils.Logger.d("Received achievement callback after signout occurred, ignoring");
            }
            else
            {
                GooglePlayGames.OurUtils.Logger.d("Populating Achievements, status = " + response.Status());
                object authStateLock = this.AuthStateLock;
                lock (authStateLock)
                {
                    if ((response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) && (response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
                    {
                        GooglePlayGames.OurUtils.Logger.e("Error retrieving achievements - check the log for more information. Failing signin.");
                        Action<bool, string> mPendingAuthCallbacks = this.mPendingAuthCallbacks;
                        this.mPendingAuthCallbacks = null;
                        if (mPendingAuthCallbacks != null)
                        {
                            InvokeCallbackOnGameThread<bool, string>(mPendingAuthCallbacks, false, "Cannot load achievements, Authenication failing");
                        }
                        this.SignOut();
                        return;
                    }
                    Dictionary<string, GooglePlayGames.BasicApi.Achievement> dictionary = new Dictionary<string, GooglePlayGames.BasicApi.Achievement>();
                    foreach (NativeAchievement achievement in response)
                    {
                        using (achievement)
                        {
                            dictionary[achievement.Id()] = achievement.AsAchievement();
                        }
                    }
                    GooglePlayGames.OurUtils.Logger.d("Found " + dictionary.Count + " Achievements");
                    this.mAchievements = dictionary;
                }
                GooglePlayGames.OurUtils.Logger.d("Maybe finish for Achievements");
                this.MaybeFinishAuthentication();
            }
        }

        private void PopulateUser(uint authGeneration, GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse response)
        {
            GooglePlayGames.OurUtils.Logger.d("Populating User");
            if (authGeneration != this.mAuthGeneration)
            {
                GooglePlayGames.OurUtils.Logger.d("Received user callback after signout occurred, ignoring");
            }
            else
            {
                object authStateLock = this.AuthStateLock;
                lock (authStateLock)
                {
                    if ((response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) && (response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
                    {
                        GooglePlayGames.OurUtils.Logger.e("Error retrieving user, signing out");
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
                GooglePlayGames.OurUtils.Logger.d("Found User: " + this.mUser);
                GooglePlayGames.OurUtils.Logger.d("Maybe finish for User");
                this.MaybeFinishAuthentication();
            }
        }

        public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
        {
            <RegisterInvitationDelegate>c__AnonStorey12 storey = new <RegisterInvitationDelegate>c__AnonStorey12 {
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
            <RevealAchievement>c__AnonStoreyC yc = new <RevealAchievement>c__AnonStoreyC {
                achId = achId,
                $this = this
            };
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = a => a.IsRevealed;
            }
            this.UpdateAchievement("Reveal", yc.achId, callback, <>f__am$cache1, new Action<GooglePlayGames.BasicApi.Achievement>(yc.<>m__0));
        }

        public void SetGravityForPopups(Gravity gravity)
        {
            <SetGravityForPopups>c__AnonStorey6 storey = new <SetGravityForPopups>c__AnonStorey6 {
                gravity = gravity,
                $this = this
            };
            PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
        }

        public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
        {
            <SetStepsAtLeast>c__AnonStoreyF yf = new <SetStepsAtLeast>c__AnonStoreyF {
                achId = achId,
                callback = callback,
                $this = this
            };
            Misc.CheckNotNull<string>(yf.achId);
            yf.callback = AsOnGameThreadCallback<bool>(yf.callback);
            this.InitializeGameServices();
            GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(yf.achId);
            if (achievement == null)
            {
                GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + yf.achId);
                yf.callback(false);
            }
            else if (!achievement.IsIncremental)
            {
                GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + yf.achId + " is not incremental");
                yf.callback(false);
            }
            else if (steps < 0)
            {
                GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
                yf.callback(false);
            }
            else
            {
                this.GameServices().AchievementManager().SetStepsAtLeast(yf.achId, Convert.ToUInt32(steps));
                this.GameServices().AchievementManager().Fetch(yf.achId, new Action<GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse>(yf.<>m__0));
            }
        }

        public void ShowAchievementsUI(Action<GooglePlayGames.BasicApi.UIStatus> cb)
        {
            <ShowAchievementsUI>c__AnonStorey10 storey = new <ShowAchievementsUI>c__AnonStorey10 {
                cb = cb
            };
            if (this.IsAuthenticated())
            {
                Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> noopUICallback = Callbacks.NoopUICallback;
                if (storey.cb != null)
                {
                    noopUICallback = new Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>(storey.<>m__0);
                }
                noopUICallback = AsOnGameThreadCallback<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>(noopUICallback);
                this.GameServices().AchievementManager().ShowAllUI(noopUICallback);
            }
        }

        public void ShowLeaderboardUI(string leaderboardId, GooglePlayGames.BasicApi.LeaderboardTimeSpan span, Action<GooglePlayGames.BasicApi.UIStatus> cb)
        {
            <ShowLeaderboardUI>c__AnonStorey11 storey = new <ShowLeaderboardUI>c__AnonStorey11 {
                cb = cb
            };
            if (this.IsAuthenticated())
            {
                Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> noopUICallback = Callbacks.NoopUICallback;
                if (storey.cb != null)
                {
                    noopUICallback = new Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>(storey.<>m__0);
                }
                noopUICallback = AsOnGameThreadCallback<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>(noopUICallback);
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
            <UnlockAchievement>c__AnonStoreyB yb = new <UnlockAchievement>c__AnonStoreyB {
                achId = achId,
                $this = this
            };
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = a => a.IsUnlocked;
            }
            this.UpdateAchievement("Unlock", yb.achId, callback, <>f__am$cache0, new Action<GooglePlayGames.BasicApi.Achievement>(yb.<>m__0));
        }

        private void UnpauseUnityPlayer()
        {
        }

        private void UpdateAchievement(string updateType, string achId, Action<bool> callback, Predicate<GooglePlayGames.BasicApi.Achievement> alreadyDone, Action<GooglePlayGames.BasicApi.Achievement> updateAchievment)
        {
            <UpdateAchievement>c__AnonStoreyD yd = new <UpdateAchievement>c__AnonStoreyD {
                achId = achId,
                callback = callback,
                $this = this
            };
            yd.callback = AsOnGameThreadCallback<bool>(yd.callback);
            Misc.CheckNotNull<string>(yd.achId);
            this.InitializeGameServices();
            GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(yd.achId);
            if (achievement == null)
            {
                GooglePlayGames.OurUtils.Logger.d("Could not " + updateType + ", no achievement with ID " + yd.achId);
                yd.callback(false);
            }
            else if (alreadyDone(achievement))
            {
                GooglePlayGames.OurUtils.Logger.d("Did not need to perform " + updateType + ": on achievement " + yd.achId);
                yd.callback(true);
            }
            else
            {
                GooglePlayGames.OurUtils.Logger.d("Performing " + updateType + " on " + yd.achId);
                updateAchievment(achievement);
                this.GameServices().AchievementManager().Fetch(yd.achId, new Action<GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse>(yd.<>m__0));
            }
        }

        [CompilerGenerated]
        private sealed class <AsOnGameThreadCallback>c__AnonStorey0<T>
        {
            internal Action<T> callback;

            internal void <>m__0(T result)
            {
                NativeClient.InvokeCallbackOnGameThread<T>(this.callback, result);
            }
        }

        [CompilerGenerated]
        private sealed class <GetPlayerStats>c__AnonStorey7
        {
            internal Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback;
            internal NativeClient $this;

            internal void <>m__0()
            {
                this.$this.clientImpl.GetPlayerStats(this.$this.GetApiClient(), this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <HandleAuthTransition>c__AnonStorey5
        {
            internal uint currentAuthGeneration;
            internal NativeClient $this;

            internal void <>m__0(GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse results)
            {
                this.$this.PopulateAchievements(this.currentAuthGeneration, results);
            }

            internal void <>m__1(GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse results)
            {
                this.$this.PopulateUser(this.currentAuthGeneration, results);
            }
        }

        [CompilerGenerated]
        private sealed class <HandleInvitation>c__AnonStorey3
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
        private sealed class <IncrementAchievement>c__AnonStoreyE
        {
            internal string achId;
            internal Action<bool> callback;
            internal NativeClient $this;

            internal void <>m__0(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.$this.mAchievements.Remove(this.achId);
                    this.$this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <InvokeCallbackOnGameThread>c__AnonStorey1<T, S>
        {
            internal Action<T, S> callback;
            internal T data;
            internal S msg;

            internal void <>m__0()
            {
                GooglePlayGames.OurUtils.Logger.d("Invoking user callback on game thread");
                this.callback(this.data, this.msg);
            }
        }

        [CompilerGenerated]
        private sealed class <InvokeCallbackOnGameThread>c__AnonStorey2<T>
        {
            internal Action<T> callback;
            internal T data;

            internal void <>m__0()
            {
                GooglePlayGames.OurUtils.Logger.d("Invoking user callback on game thread");
                this.callback(this.data);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAchievements>c__AnonStoreyA
        {
            internal Action<GooglePlayGames.BasicApi.Achievement[]> callback;
            internal GooglePlayGames.BasicApi.Achievement[] data;

            internal void <>m__0()
            {
                this.callback(this.data);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadFriends>c__AnonStorey4
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

            internal void <>m__2(GooglePlayGames.BasicApi.ResponseStatus status, List<GooglePlayGames.BasicApi.Multiplayer.Player> players)
            {
                if ((status == GooglePlayGames.BasicApi.ResponseStatus.Success) || (status == GooglePlayGames.BasicApi.ResponseStatus.SuccessWithStale))
                {
                    this.$this.mFriends = players;
                    PlayGamesHelperObject.RunOnGameThread(() => this.callback(true));
                }
                else
                {
                    this.$this.mFriends = new List<GooglePlayGames.BasicApi.Multiplayer.Player>();
                    GooglePlayGames.OurUtils.Logger.e("Got " + status + " loading friends");
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
        private sealed class <LoadUsers>c__AnonStorey8
        {
            internal Action<IUserProfile[]> callback;

            internal void <>m__0(NativePlayer[] nativeUsers)
            {
                <LoadUsers>c__AnonStorey9 storey = new <LoadUsers>c__AnonStorey9 {
                    <>f__ref$8 = this,
                    users = new IUserProfile[nativeUsers.Length]
                };
                for (int i = 0; i < storey.users.Length; i++)
                {
                    storey.users[i] = nativeUsers[i].AsPlayer();
                }
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            private sealed class <LoadUsers>c__AnonStorey9
            {
                internal IUserProfile[] users;
                internal NativeClient.<LoadUsers>c__AnonStorey8 <>f__ref$8;

                internal void <>m__0()
                {
                    this.<>f__ref$8.callback(this.users);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterInvitationDelegate>c__AnonStorey12
        {
            internal InvitationReceivedDelegate invitationDelegate;

            internal void <>m__0(Invitation invitation, bool autoAccept)
            {
                this.invitationDelegate(invitation, autoAccept);
            }
        }

        [CompilerGenerated]
        private sealed class <RevealAchievement>c__AnonStoreyC
        {
            internal string achId;
            internal NativeClient $this;

            internal void <>m__0(GooglePlayGames.BasicApi.Achievement a)
            {
                a.IsRevealed = true;
                this.$this.GameServices().AchievementManager().Reveal(this.achId);
            }
        }

        [CompilerGenerated]
        private sealed class <SetGravityForPopups>c__AnonStorey6
        {
            internal Gravity gravity;
            internal NativeClient $this;

            internal void <>m__0()
            {
                this.$this.clientImpl.SetGravityForPopups(this.$this.GetApiClient(), this.gravity);
            }
        }

        [CompilerGenerated]
        private sealed class <SetStepsAtLeast>c__AnonStoreyF
        {
            internal string achId;
            internal Action<bool> callback;
            internal NativeClient $this;

            internal void <>m__0(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.$this.mAchievements.Remove(this.achId);
                    this.$this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ShowAchievementsUI>c__AnonStorey10
        {
            internal Action<GooglePlayGames.BasicApi.UIStatus> cb;

            internal void <>m__0(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus result)
            {
                this.cb((GooglePlayGames.BasicApi.UIStatus) result);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowLeaderboardUI>c__AnonStorey11
        {
            internal Action<GooglePlayGames.BasicApi.UIStatus> cb;

            internal void <>m__0(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus result)
            {
                this.cb((GooglePlayGames.BasicApi.UIStatus) result);
            }
        }

        [CompilerGenerated]
        private sealed class <UnlockAchievement>c__AnonStoreyB
        {
            internal string achId;
            internal NativeClient $this;

            internal void <>m__0(GooglePlayGames.BasicApi.Achievement a)
            {
                a.IsUnlocked = true;
                this.$this.GameServices().AchievementManager().Unlock(this.achId);
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAchievement>c__AnonStoreyD
        {
            internal string achId;
            internal Action<bool> callback;
            internal NativeClient $this;

            internal void <>m__0(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.$this.mAchievements.Remove(this.achId);
                    this.$this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        private enum AuthState
        {
            Unauthenticated,
            Authenticated,
            SilentPending
        }
    }
}

