namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;

    internal class LeaderboardManager
    {
        private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.LeaderboardManager.ShowAllUICallback <>f__mg$cache0;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.LeaderboardManager.ShowUICallback <>f__mg$cache1;
        [CompilerGenerated]
        private static Func<IntPtr, GooglePlayGames.Native.PInvoke.FetchResponse> <>f__mg$cache2;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchCallback <>f__mg$cache3;
        [CompilerGenerated]
        private static Func<IntPtr, FetchScoreSummaryResponse> <>f__mg$cache4;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScoreSummaryCallback <>f__mg$cache5;
        [CompilerGenerated]
        private static Func<IntPtr, FetchScorePageResponse> <>f__mg$cache6;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScorePageCallback <>f__mg$cache7;

        internal LeaderboardManager(GooglePlayGames.Native.PInvoke.GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
        }

        internal void HandleFetch(ScorePageToken token, GooglePlayGames.Native.PInvoke.FetchResponse response, string selfPlayerId, int maxResults, Action<LeaderboardScoreData> callback)
        {
            <HandleFetch>c__AnonStorey1 storey = new <HandleFetch>c__AnonStorey1 {
                selfPlayerId = selfPlayerId,
                maxResults = maxResults,
                token = token,
                callback = callback,
                $this = this
            };
            storey.data = new LeaderboardScoreData(storey.token.LeaderboardId, (GooglePlayGames.BasicApi.ResponseStatus) response.GetStatus());
            if ((response.GetStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) && (response.GetStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
            {
                Logger.w("Error returned from fetch: " + response.GetStatus());
                storey.callback(storey.data);
            }
            else
            {
                storey.data.Title = response.Leaderboard().Title();
                storey.data.Id = storey.token.LeaderboardId;
                if (<>f__mg$cache5 == null)
                {
                    <>f__mg$cache5 = new GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScoreSummaryCallback(GooglePlayGames.Native.PInvoke.LeaderboardManager.InternalFetchSummaryCallback);
                }
                if (<>f__mg$cache4 == null)
                {
                    <>f__mg$cache4 = new Func<IntPtr, FetchScoreSummaryResponse>(FetchScoreSummaryResponse.FromPointer);
                }
                GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummary(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, storey.token.LeaderboardId, (Types.LeaderboardTimeSpan) storey.token.TimeSpan, (Types.LeaderboardCollection) storey.token.Collection, <>f__mg$cache5, Callbacks.ToIntPtr<FetchScoreSummaryResponse>(new Action<FetchScoreSummaryResponse>(storey.<>m__0), <>f__mg$cache4));
            }
        }

        internal void HandleFetchScorePage(LeaderboardScoreData data, ScorePageToken token, FetchScorePageResponse rsp, Action<LeaderboardScoreData> callback)
        {
            data.Status = (GooglePlayGames.BasicApi.ResponseStatus) rsp.GetStatus();
            if ((rsp.GetStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) && (rsp.GetStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
            {
                callback(data);
            }
            NativeScorePage scorePage = rsp.GetScorePage();
            if (!scorePage.Valid())
            {
                callback(data);
            }
            if (scorePage.HasNextScorePage())
            {
                data.NextPageToken = new ScorePageToken(scorePage.GetNextScorePageToken(), token.LeaderboardId, token.Collection, token.TimeSpan);
            }
            if (scorePage.HasPrevScorePage())
            {
                data.PrevPageToken = new ScorePageToken(scorePage.GetPreviousScorePageToken(), token.LeaderboardId, token.Collection, token.TimeSpan);
            }
            foreach (NativeScoreEntry entry in scorePage)
            {
                data.AddScore(entry.AsScore(data.Id));
            }
            callback(data);
        }

        internal void HandleFetchScoreSummary(LeaderboardScoreData data, FetchScoreSummaryResponse response, string selfPlayerId, int maxResults, ScorePageToken token, Action<LeaderboardScoreData> callback)
        {
            if ((response.GetStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) && (response.GetStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
            {
                Logger.w("Error returned from fetchScoreSummary: " + response);
                data.Status = (GooglePlayGames.BasicApi.ResponseStatus) response.GetStatus();
                callback(data);
            }
            else
            {
                NativeScoreSummary scoreSummary = response.GetScoreSummary();
                data.ApproximateCount = scoreSummary.ApproximateResults();
                data.PlayerScore = scoreSummary.LocalUserScore().AsScore(data.Id, selfPlayerId);
                if (maxResults <= 0)
                {
                    callback(data);
                }
                else
                {
                    this.LoadScorePage(data, maxResults, token, callback);
                }
            }
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchCallback))]
        private static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScorePageCallback))]
        private static void InternalFetchScorePage(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchScorePage", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScoreSummaryCallback))]
        private static void InternalFetchSummaryCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchSummaryCallback", Callbacks.Type.Temporary, response, data);
        }

        public void LoadLeaderboardData(string leaderboardId, GooglePlayGames.BasicApi.LeaderboardStart start, int rowCount, GooglePlayGames.BasicApi.LeaderboardCollection collection, GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan, string playerId, Action<LeaderboardScoreData> callback)
        {
            <LoadLeaderboardData>c__AnonStorey0 storey = new <LoadLeaderboardData>c__AnonStorey0 {
                playerId = playerId,
                rowCount = rowCount,
                callback = callback,
                $this = this
            };
            NativeScorePageToken internalObject = new NativeScorePageToken(GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ScorePageToken(this.mServices.AsHandle(), leaderboardId, (Types.LeaderboardStart) start, (Types.LeaderboardTimeSpan) timeSpan, (Types.LeaderboardCollection) collection));
            storey.token = new ScorePageToken(internalObject, leaderboardId, collection, timeSpan);
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchCallback(GooglePlayGames.Native.PInvoke.LeaderboardManager.InternalFetchCallback);
            }
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new Func<IntPtr, GooglePlayGames.Native.PInvoke.FetchResponse>(GooglePlayGames.Native.PInvoke.FetchResponse.FromPointer);
            }
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_Fetch(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, leaderboardId, <>f__mg$cache3, Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.FetchResponse>(new Action<GooglePlayGames.Native.PInvoke.FetchResponse>(storey.<>m__0), <>f__mg$cache2));
        }

        public void LoadScorePage(LeaderboardScoreData data, int maxResults, ScorePageToken token, Action<LeaderboardScoreData> callback)
        {
            <LoadScorePage>c__AnonStorey2 storey = new <LoadScorePage>c__AnonStorey2 {
                data = data,
                token = token,
                callback = callback,
                $this = this
            };
            if (storey.data == null)
            {
                storey.data = new LeaderboardScoreData(storey.token.LeaderboardId);
            }
            NativeScorePageToken internalObject = (NativeScorePageToken) storey.token.InternalObject;
            if (<>f__mg$cache7 == null)
            {
                <>f__mg$cache7 = new GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScorePageCallback(GooglePlayGames.Native.PInvoke.LeaderboardManager.InternalFetchScorePage);
            }
            if (<>f__mg$cache6 == null)
            {
                <>f__mg$cache6 = new Func<IntPtr, FetchScorePageResponse>(FetchScorePageResponse.FromPointer);
            }
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScorePage(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, internalObject.AsPointer(), (uint) maxResults, <>f__mg$cache7, Callbacks.ToIntPtr<FetchScorePageResponse>(new Action<FetchScorePageResponse>(storey.<>m__0), <>f__mg$cache6));
        }

        internal void ShowAllUI(Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> callback)
        {
            Misc.CheckNotNull<Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>>(callback);
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new GooglePlayGames.Native.Cwrapper.LeaderboardManager.ShowAllUICallback(Callbacks.InternalShowUICallback);
            }
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ShowAllUI(this.mServices.AsHandle(), <>f__mg$cache0, Callbacks.ToIntPtr(callback));
        }

        internal void ShowUI(string leaderboardId, GooglePlayGames.BasicApi.LeaderboardTimeSpan span, Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> callback)
        {
            Misc.CheckNotNull<Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>>(callback);
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new GooglePlayGames.Native.Cwrapper.LeaderboardManager.ShowUICallback(Callbacks.InternalShowUICallback);
            }
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ShowUI(this.mServices.AsHandle(), leaderboardId, (Types.LeaderboardTimeSpan) span, <>f__mg$cache1, Callbacks.ToIntPtr(callback));
        }

        internal void SubmitScore(string leaderboardId, long score, string metadata)
        {
            Misc.CheckNotNull<string>(leaderboardId, "leaderboardId");
            Logger.d(string.Concat(new object[] { "Native Submitting score: ", score, " for lb ", leaderboardId, " with metadata: ", metadata }));
            if (metadata == null)
            {
            }
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_SubmitScore(this.mServices.AsHandle(), leaderboardId, (ulong) score, string.Empty);
        }

        internal int LeaderboardMaxResults =>
            0x19;

        [CompilerGenerated]
        private sealed class <HandleFetch>c__AnonStorey1
        {
            internal LeaderboardScoreData data;
            internal string selfPlayerId;
            internal int maxResults;
            internal ScorePageToken token;
            internal Action<LeaderboardScoreData> callback;
            internal GooglePlayGames.Native.PInvoke.LeaderboardManager $this;

            internal void <>m__0(FetchScoreSummaryResponse rsp)
            {
                this.$this.HandleFetchScoreSummary(this.data, rsp, this.selfPlayerId, this.maxResults, this.token, this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadLeaderboardData>c__AnonStorey0
        {
            internal ScorePageToken token;
            internal string playerId;
            internal int rowCount;
            internal Action<LeaderboardScoreData> callback;
            internal GooglePlayGames.Native.PInvoke.LeaderboardManager $this;

            internal void <>m__0(GooglePlayGames.Native.PInvoke.FetchResponse rsp)
            {
                this.$this.HandleFetch(this.token, rsp, this.playerId, this.rowCount, this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadScorePage>c__AnonStorey2
        {
            internal LeaderboardScoreData data;
            internal ScorePageToken token;
            internal Action<LeaderboardScoreData> callback;
            internal GooglePlayGames.Native.PInvoke.LeaderboardManager $this;

            internal void <>m__0(FetchScorePageResponse rsp)
            {
                this.$this.HandleFetchScorePage(this.data, this.token, rsp, this.callback);
            }
        }
    }
}

