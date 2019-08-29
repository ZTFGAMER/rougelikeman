namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class NativeTurnBasedMultiplayerClient : ITurnBasedMultiplayerClient
    {
        private readonly TurnBasedManager mTurnBasedManager;
        private readonly NativeClient mNativeClient;
        private volatile Action<TurnBasedMatch, bool> mMatchDelegate;

        internal NativeTurnBasedMultiplayerClient(NativeClient nativeClient, TurnBasedManager manager)
        {
            this.mTurnBasedManager = manager;
            this.mNativeClient = nativeClient;
        }

        public void AcceptFromInbox(Action<bool, TurnBasedMatch> callback)
        {
            <AcceptFromInbox>c__AnonStorey7 storey = new <AcceptFromInbox>c__AnonStorey7 {
                callback = callback,
                $this = this
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<bool, TurnBasedMatch>(storey.callback);
            this.mTurnBasedManager.ShowInboxUI(new Action<TurnBasedManager.MatchInboxUIResponse>(storey.<>m__0));
        }

        public void AcceptInvitation(string invitationId, Action<bool, TurnBasedMatch> callback)
        {
            <AcceptInvitation>c__AnonStorey8 storey = new <AcceptInvitation>c__AnonStorey8 {
                invitationId = invitationId,
                callback = callback,
                $this = this
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<bool, TurnBasedMatch>(storey.callback);
            this.FindInvitationWithId(storey.invitationId, new Action<MultiplayerInvitation>(storey.<>m__0));
        }

        public void AcknowledgeFinished(TurnBasedMatch match, Action<bool> callback)
        {
            <AcknowledgeFinished>c__AnonStorey10 storey = new <AcknowledgeFinished>c__AnonStorey10 {
                callback = callback,
                $this = this
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<bool>(storey.callback);
            this.FindEqualVersionMatch(match, storey.callback, new Action<NativeTurnBasedMatch>(storey.<>m__0));
        }

        private Action<TurnBasedManager.TurnBasedMatchResponse> BridgeMatchToUserCallback(Action<UIStatus, TurnBasedMatch> userCallback)
        {
            <BridgeMatchToUserCallback>c__AnonStorey6 storey = new <BridgeMatchToUserCallback>c__AnonStorey6 {
                userCallback = userCallback,
                $this = this
            };
            return new Action<TurnBasedManager.TurnBasedMatchResponse>(storey.<>m__0);
        }

        public void Cancel(TurnBasedMatch match, Action<bool> callback)
        {
            <Cancel>c__AnonStorey13 storey = new <Cancel>c__AnonStorey13 {
                callback = callback,
                $this = this
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<bool>(storey.callback);
            this.FindEqualVersionMatch(match, storey.callback, new Action<NativeTurnBasedMatch>(storey.<>m__0));
        }

        public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, Action<bool, TurnBasedMatch> callback)
        {
            this.CreateQuickMatch(minOpponents, maxOpponents, variant, 0L, callback);
        }

        public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, Action<bool, TurnBasedMatch> callback)
        {
            <CreateQuickMatch>c__AnonStorey1 storey = new <CreateQuickMatch>c__AnonStorey1 {
                callback = callback
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<bool, TurnBasedMatch>(storey.callback);
            using (TurnBasedMatchConfigBuilder builder = TurnBasedMatchConfigBuilder.Create())
            {
                builder.SetVariant(variant).SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetExclusiveBitMask(exclusiveBitmask);
                using (TurnBasedMatchConfig config = builder.Build())
                {
                    this.mTurnBasedManager.CreateMatch(config, this.BridgeMatchToUserCallback(new Action<UIStatus, TurnBasedMatch>(storey.<>m__0)));
                }
            }
        }

        public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<UIStatus, TurnBasedMatch> callback)
        {
            <CreateWithInvitationScreen>c__AnonStorey3 storey = new <CreateWithInvitationScreen>c__AnonStorey3 {
                callback = callback,
                variant = variant,
                $this = this
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<UIStatus, TurnBasedMatch>(storey.callback);
            this.mTurnBasedManager.ShowPlayerSelectUI(minOpponents, maxOpponents, true, new Action<PlayerSelectUIResponse>(storey.<>m__0));
        }

        public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<bool, TurnBasedMatch> callback)
        {
            <CreateWithInvitationScreen>c__AnonStorey2 storey = new <CreateWithInvitationScreen>c__AnonStorey2 {
                callback = callback
            };
            this.CreateWithInvitationScreen(minOpponents, maxOpponents, variant, new Action<UIStatus, TurnBasedMatch>(storey.<>m__0));
        }

        public void DeclineInvitation(string invitationId)
        {
            this.FindInvitationWithId(invitationId, delegate (MultiplayerInvitation invitation) {
                if (invitation != null)
                {
                    this.mTurnBasedManager.DeclineInvitation(invitation);
                }
            });
        }

        private void FindEqualVersionMatch(TurnBasedMatch match, Action<bool> onFailure, Action<NativeTurnBasedMatch> onVersionMatch)
        {
            <FindEqualVersionMatch>c__AnonStoreyD yd = new <FindEqualVersionMatch>c__AnonStoreyD {
                match = match,
                onFailure = onFailure,
                onVersionMatch = onVersionMatch
            };
            this.mTurnBasedManager.GetMatch(yd.match.MatchId, new Action<TurnBasedManager.TurnBasedMatchResponse>(yd.<>m__0));
        }

        private void FindEqualVersionMatchWithParticipant(TurnBasedMatch match, string participantId, Action<bool> onFailure, Action<MultiplayerParticipant, NativeTurnBasedMatch> onFoundParticipantAndMatch)
        {
            <FindEqualVersionMatchWithParticipant>c__AnonStoreyE ye = new <FindEqualVersionMatchWithParticipant>c__AnonStoreyE {
                participantId = participantId,
                onFoundParticipantAndMatch = onFoundParticipantAndMatch,
                match = match,
                onFailure = onFailure
            };
            this.FindEqualVersionMatch(ye.match, ye.onFailure, new Action<NativeTurnBasedMatch>(ye.<>m__0));
        }

        private void FindInvitationWithId(string invitationId, Action<MultiplayerInvitation> callback)
        {
            <FindInvitationWithId>c__AnonStorey9 storey = new <FindInvitationWithId>c__AnonStorey9 {
                callback = callback,
                invitationId = invitationId
            };
            this.mTurnBasedManager.GetAllTurnbasedMatches(new Action<TurnBasedManager.TurnBasedMatchesResponse>(storey.<>m__0));
        }

        public void Finish(TurnBasedMatch match, byte[] data, MatchOutcome outcome, Action<bool> callback)
        {
            <Finish>c__AnonStoreyF yf = new <Finish>c__AnonStoreyF {
                outcome = outcome,
                callback = callback,
                data = data,
                $this = this
            };
            yf.callback = Callbacks.AsOnGameThreadCallback<bool>(yf.callback);
            this.FindEqualVersionMatch(match, yf.callback, new Action<NativeTurnBasedMatch>(yf.<>m__0));
        }

        public void GetAllInvitations(Action<Invitation[]> callback)
        {
            <GetAllInvitations>c__AnonStorey4 storey = new <GetAllInvitations>c__AnonStorey4 {
                callback = callback
            };
            this.mTurnBasedManager.GetAllTurnbasedMatches(new Action<TurnBasedManager.TurnBasedMatchesResponse>(storey.<>m__0));
        }

        public void GetAllMatches(Action<TurnBasedMatch[]> callback)
        {
            <GetAllMatches>c__AnonStorey5 storey = new <GetAllMatches>c__AnonStorey5 {
                callback = callback,
                $this = this
            };
            this.mTurnBasedManager.GetAllTurnbasedMatches(new Action<TurnBasedManager.TurnBasedMatchesResponse>(storey.<>m__0));
        }

        public int GetMaxMatchDataSize()
        {
            throw new NotImplementedException();
        }

        internal void HandleMatchEvent(Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match)
        {
            <HandleMatchEvent>c__AnonStoreyB yb = new <HandleMatchEvent>c__AnonStoreyB {
                match = match,
                $this = this,
                currentDelegate = this.mMatchDelegate
            };
            if (yb.currentDelegate != null)
            {
                if (eventType == Types.MultiplayerEvent.REMOVED)
                {
                    Logger.d("Ignoring REMOVE event for match " + matchId);
                }
                else
                {
                    yb.shouldAutolaunch = eventType == Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
                    yb.match.ReferToMe();
                    Callbacks.AsCoroutine(this.WaitForLogin(new Action(yb.<>m__0)));
                }
            }
        }

        public void Leave(TurnBasedMatch match, Action<bool> callback)
        {
            <Leave>c__AnonStorey11 storey = new <Leave>c__AnonStorey11 {
                callback = callback,
                $this = this
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<bool>(storey.callback);
            this.FindEqualVersionMatch(match, storey.callback, new Action<NativeTurnBasedMatch>(storey.<>m__0));
        }

        public void LeaveDuringTurn(TurnBasedMatch match, string pendingParticipantId, Action<bool> callback)
        {
            <LeaveDuringTurn>c__AnonStorey12 storey = new <LeaveDuringTurn>c__AnonStorey12 {
                callback = callback,
                $this = this
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<bool>(storey.callback);
            this.FindEqualVersionMatchWithParticipant(match, pendingParticipantId, storey.callback, new Action<MultiplayerParticipant, NativeTurnBasedMatch>(storey.<>m__0));
        }

        public void RegisterMatchDelegate(MatchDelegate del)
        {
            <RegisterMatchDelegate>c__AnonStoreyA ya = new <RegisterMatchDelegate>c__AnonStoreyA {
                del = del
            };
            if (ya.del == null)
            {
                this.mMatchDelegate = null;
            }
            else
            {
                this.mMatchDelegate = Callbacks.AsOnGameThreadCallback<TurnBasedMatch, bool>(new Action<TurnBasedMatch, bool>(ya.<>m__0));
            }
        }

        public void Rematch(TurnBasedMatch match, Action<bool, TurnBasedMatch> callback)
        {
            <Rematch>c__AnonStorey14 storey = new <Rematch>c__AnonStorey14 {
                callback = callback,
                $this = this
            };
            storey.callback = Callbacks.AsOnGameThreadCallback<bool, TurnBasedMatch>(storey.callback);
            this.FindEqualVersionMatch(match, new Action<bool>(storey.<>m__0), new Action<NativeTurnBasedMatch>(storey.<>m__1));
        }

        private static Types.MatchResult ResultToMatchResult(MatchOutcome.ParticipantResult result)
        {
            switch (result)
            {
                case MatchOutcome.ParticipantResult.None:
                    return Types.MatchResult.NONE;

                case MatchOutcome.ParticipantResult.Win:
                    return Types.MatchResult.WIN;

                case MatchOutcome.ParticipantResult.Loss:
                    return Types.MatchResult.LOSS;

                case MatchOutcome.ParticipantResult.Tie:
                    return Types.MatchResult.TIE;
            }
            Logger.e("Received unknown ParticipantResult " + result);
            return Types.MatchResult.NONE;
        }

        public void TakeTurn(TurnBasedMatch match, byte[] data, string pendingParticipantId, Action<bool> callback)
        {
            <TakeTurn>c__AnonStoreyC yc = new <TakeTurn>c__AnonStoreyC {
                data = data,
                callback = callback,
                $this = this
            };
            Logger.describe(yc.data);
            yc.callback = Callbacks.AsOnGameThreadCallback<bool>(yc.callback);
            this.FindEqualVersionMatchWithParticipant(match, pendingParticipantId, yc.callback, new Action<MultiplayerParticipant, NativeTurnBasedMatch>(yc.<>m__0));
        }

        [DebuggerHidden]
        private IEnumerator WaitForLogin(Action method) => 
            new <WaitForLogin>c__Iterator0 { 
                method = method,
                $this = this
            };

        [CompilerGenerated]
        private sealed class <AcceptFromInbox>c__AnonStorey7
        {
            internal Action<bool, TurnBasedMatch> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(TurnBasedManager.MatchInboxUIResponse callbackResult)
            {
                using (NativeTurnBasedMatch match = callbackResult.Match())
                {
                    if (match == null)
                    {
                        this.callback(false, null);
                    }
                    else
                    {
                        TurnBasedMatch match2 = match.AsTurnBasedMatch(this.$this.mNativeClient.GetUserId());
                        Logger.d("Passing converted match to user callback:" + match2);
                        this.callback(true, match2);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AcceptInvitation>c__AnonStorey8
        {
            internal string invitationId;
            internal Action<bool, TurnBasedMatch> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(MultiplayerInvitation invitation)
            {
                if (invitation == null)
                {
                    Logger.e("Could not find invitation with id " + this.invitationId);
                    this.callback(false, null);
                }
                else
                {
                    this.$this.mTurnBasedManager.AcceptInvitation(invitation, this.$this.BridgeMatchToUserCallback((status, match) => this.callback(status == UIStatus.Valid, match)));
                }
            }

            internal void <>m__1(UIStatus status, TurnBasedMatch match)
            {
                this.callback(status == UIStatus.Valid, match);
            }
        }

        [CompilerGenerated]
        private sealed class <AcknowledgeFinished>c__AnonStorey10
        {
            internal Action<bool> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(NativeTurnBasedMatch foundMatch)
            {
                this.$this.mTurnBasedManager.ConfirmPendingCompletion(foundMatch, response => this.callback(response.RequestSucceeded()));
            }

            internal void <>m__1(TurnBasedManager.TurnBasedMatchResponse response)
            {
                this.callback(response.RequestSucceeded());
            }
        }

        [CompilerGenerated]
        private sealed class <BridgeMatchToUserCallback>c__AnonStorey6
        {
            internal Action<UIStatus, TurnBasedMatch> userCallback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(TurnBasedManager.TurnBasedMatchResponse callbackResult)
            {
                using (NativeTurnBasedMatch match = callbackResult.Match())
                {
                    if (match == null)
                    {
                        UIStatus internalError = UIStatus.InternalError;
                        switch ((callbackResult.ResponseStatus() + 5))
                        {
                            case ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED):
                                internalError = UIStatus.Timeout;
                                break;

                            case CommonErrorStatus.MultiplayerStatus.VALID:
                                internalError = UIStatus.VersionUpdateRequired;
                                break;

                            case CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE:
                                internalError = UIStatus.NotAuthorized;
                                break;

                            case ~CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED:
                                internalError = UIStatus.InternalError;
                                break;

                            case ~CommonErrorStatus.MultiplayerStatus.ERROR_MATCH_ALREADY_REMATCHED:
                                internalError = UIStatus.Valid;
                                break;

                            case ~CommonErrorStatus.MultiplayerStatus.ERROR_INACTIVE_MATCH:
                                internalError = UIStatus.Valid;
                                break;
                        }
                        this.userCallback(internalError, null);
                    }
                    else
                    {
                        TurnBasedMatch match2 = match.AsTurnBasedMatch(this.$this.mNativeClient.GetUserId());
                        Logger.d("Passing converted match to user callback:" + match2);
                        this.userCallback(UIStatus.Valid, match2);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Cancel>c__AnonStorey13
        {
            internal Action<bool> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(NativeTurnBasedMatch foundMatch)
            {
                this.$this.mTurnBasedManager.CancelMatch(foundMatch, status => this.callback(status > ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED)));
            }

            internal void <>m__1(CommonErrorStatus.MultiplayerStatus status)
            {
                this.callback(status > ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
            }
        }

        [CompilerGenerated]
        private sealed class <CreateQuickMatch>c__AnonStorey1
        {
            internal Action<bool, TurnBasedMatch> callback;

            internal void <>m__0(UIStatus status, TurnBasedMatch match)
            {
                this.callback(status == UIStatus.Valid, match);
            }
        }

        [CompilerGenerated]
        private sealed class <CreateWithInvitationScreen>c__AnonStorey2
        {
            internal Action<bool, TurnBasedMatch> callback;

            internal void <>m__0(UIStatus status, TurnBasedMatch match)
            {
                this.callback(status == UIStatus.Valid, match);
            }
        }

        [CompilerGenerated]
        private sealed class <CreateWithInvitationScreen>c__AnonStorey3
        {
            internal Action<UIStatus, TurnBasedMatch> callback;
            internal uint variant;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(PlayerSelectUIResponse result)
            {
                if (result.Status() != CommonErrorStatus.UIStatus.VALID)
                {
                    this.callback((UIStatus) result.Status(), null);
                }
                else
                {
                    using (TurnBasedMatchConfigBuilder builder = TurnBasedMatchConfigBuilder.Create())
                    {
                        builder.PopulateFromUIResponse(result).SetVariant(this.variant);
                        using (TurnBasedMatchConfig config = builder.Build())
                        {
                            this.$this.mTurnBasedManager.CreateMatch(config, this.$this.BridgeMatchToUserCallback(this.callback));
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FindEqualVersionMatch>c__AnonStoreyD
        {
            internal TurnBasedMatch match;
            internal Action<bool> onFailure;
            internal Action<NativeTurnBasedMatch> onVersionMatch;

            internal void <>m__0(TurnBasedManager.TurnBasedMatchResponse response)
            {
                using (NativeTurnBasedMatch match = response.Match())
                {
                    if (match == null)
                    {
                        Logger.e($"Could not find match {this.match.MatchId}");
                        this.onFailure(false);
                    }
                    else if (match.Version() != this.match.Version)
                    {
                        Logger.e($"Attempted to update a stale version of the match. Expected version was {this.match.Version} but current version is {match.Version()}.");
                        this.onFailure(false);
                    }
                    else
                    {
                        this.onVersionMatch(match);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FindEqualVersionMatchWithParticipant>c__AnonStoreyE
        {
            internal string participantId;
            internal Action<MultiplayerParticipant, NativeTurnBasedMatch> onFoundParticipantAndMatch;
            internal TurnBasedMatch match;
            internal Action<bool> onFailure;

            internal void <>m__0(NativeTurnBasedMatch foundMatch)
            {
                if (this.participantId == null)
                {
                    using (MultiplayerParticipant participant = MultiplayerParticipant.AutomatchingSentinel())
                    {
                        this.onFoundParticipantAndMatch(participant, foundMatch);
                        return;
                    }
                }
                using (MultiplayerParticipant participant2 = foundMatch.ParticipantWithId(this.participantId))
                {
                    if (participant2 == null)
                    {
                        Logger.e($"Located match {this.match.MatchId} but desired participant with ID {this.participantId} could not be found");
                        this.onFailure(false);
                    }
                    else
                    {
                        this.onFoundParticipantAndMatch(participant2, foundMatch);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FindInvitationWithId>c__AnonStorey9
        {
            internal Action<MultiplayerInvitation> callback;
            internal string invitationId;

            internal void <>m__0(TurnBasedManager.TurnBasedMatchesResponse allMatches)
            {
                if (allMatches.Status() <= ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED))
                {
                    this.callback(null);
                }
                else
                {
                    foreach (MultiplayerInvitation invitation in allMatches.Invitations())
                    {
                        using (invitation)
                        {
                            if (invitation.Id().Equals(this.invitationId))
                            {
                                this.callback(invitation);
                                return;
                            }
                        }
                    }
                    this.callback(null);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Finish>c__AnonStoreyF
        {
            internal MatchOutcome outcome;
            internal Action<bool> callback;
            internal byte[] data;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(NativeTurnBasedMatch foundMatch)
            {
                ParticipantResults results = foundMatch.Results();
                foreach (string str in this.outcome.ParticipantIds)
                {
                    Types.MatchResult result = NativeTurnBasedMultiplayerClient.ResultToMatchResult(this.outcome.GetResultFor(str));
                    uint placementFor = this.outcome.GetPlacementFor(str);
                    if (results.HasResultsForParticipant(str))
                    {
                        Types.MatchResult result2 = results.ResultsForParticipant(str);
                        uint num2 = results.PlacingForParticipant(str);
                        if ((result == result2) && (placementFor == num2))
                        {
                            continue;
                        }
                        Logger.e($"Attempted to override existing results for participant {str}: Placing {num2}, Result {result2}");
                        this.callback(false);
                        return;
                    }
                    ParticipantResults results2 = results;
                    results = results2.WithResult(str, placementFor, result);
                    results2.Dispose();
                }
                this.$this.mTurnBasedManager.FinishMatchDuringMyTurn(foundMatch, this.data, results, response => this.callback(response.RequestSucceeded()));
            }

            internal void <>m__1(TurnBasedManager.TurnBasedMatchResponse response)
            {
                this.callback(response.RequestSucceeded());
            }
        }

        [CompilerGenerated]
        private sealed class <GetAllInvitations>c__AnonStorey4
        {
            internal Action<Invitation[]> callback;

            internal void <>m__0(TurnBasedManager.TurnBasedMatchesResponse allMatches)
            {
                Invitation[] invitationArray = new Invitation[allMatches.InvitationCount()];
                int num = 0;
                foreach (MultiplayerInvitation invitation in allMatches.Invitations())
                {
                    invitationArray[num++] = invitation.AsInvitation();
                }
                this.callback(invitationArray);
            }
        }

        [CompilerGenerated]
        private sealed class <GetAllMatches>c__AnonStorey5
        {
            internal Action<TurnBasedMatch[]> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(TurnBasedManager.TurnBasedMatchesResponse allMatches)
            {
                int num = (allMatches.MyTurnMatchesCount() + allMatches.TheirTurnMatchesCount()) + allMatches.CompletedMatchesCount();
                TurnBasedMatch[] matchArray = new TurnBasedMatch[num];
                int num2 = 0;
                foreach (NativeTurnBasedMatch match in allMatches.MyTurnMatches())
                {
                    matchArray[num2++] = match.AsTurnBasedMatch(this.$this.mNativeClient.GetUserId());
                }
                foreach (NativeTurnBasedMatch match2 in allMatches.TheirTurnMatches())
                {
                    matchArray[num2++] = match2.AsTurnBasedMatch(this.$this.mNativeClient.GetUserId());
                }
                foreach (NativeTurnBasedMatch match3 in allMatches.CompletedMatches())
                {
                    matchArray[num2++] = match3.AsTurnBasedMatch(this.$this.mNativeClient.GetUserId());
                }
                this.callback(matchArray);
            }
        }

        [CompilerGenerated]
        private sealed class <HandleMatchEvent>c__AnonStoreyB
        {
            internal Action<TurnBasedMatch, bool> currentDelegate;
            internal NativeTurnBasedMatch match;
            internal bool shouldAutolaunch;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0()
            {
                this.currentDelegate(this.match.AsTurnBasedMatch(this.$this.mNativeClient.GetUserId()), this.shouldAutolaunch);
                this.match.ForgetMe();
            }
        }

        [CompilerGenerated]
        private sealed class <Leave>c__AnonStorey11
        {
            internal Action<bool> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(NativeTurnBasedMatch foundMatch)
            {
                this.$this.mTurnBasedManager.LeaveMatchDuringTheirTurn(foundMatch, status => this.callback(status > ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED)));
            }

            internal void <>m__1(CommonErrorStatus.MultiplayerStatus status)
            {
                this.callback(status > ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
            }
        }

        [CompilerGenerated]
        private sealed class <LeaveDuringTurn>c__AnonStorey12
        {
            internal Action<bool> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch)
            {
                this.$this.mTurnBasedManager.LeaveDuringMyTurn(foundMatch, pendingParticipant, status => this.callback(status > ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED)));
            }

            internal void <>m__1(CommonErrorStatus.MultiplayerStatus status)
            {
                this.callback(status > ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterMatchDelegate>c__AnonStoreyA
        {
            internal MatchDelegate del;

            internal void <>m__0(TurnBasedMatch match, bool autoLaunch)
            {
                this.del(match, autoLaunch);
            }
        }

        [CompilerGenerated]
        private sealed class <Rematch>c__AnonStorey14
        {
            internal Action<bool, TurnBasedMatch> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(bool failed)
            {
                this.callback(false, null);
            }

            internal void <>m__1(NativeTurnBasedMatch foundMatch)
            {
                this.$this.mTurnBasedManager.Rematch(foundMatch, this.$this.BridgeMatchToUserCallback((status, m) => this.callback(status == UIStatus.Valid, m)));
            }

            internal void <>m__2(UIStatus status, TurnBasedMatch m)
            {
                this.callback(status == UIStatus.Valid, m);
            }
        }

        [CompilerGenerated]
        private sealed class <TakeTurn>c__AnonStoreyC
        {
            internal byte[] data;
            internal Action<bool> callback;
            internal NativeTurnBasedMultiplayerClient $this;

            internal void <>m__0(MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch)
            {
                this.$this.mTurnBasedManager.TakeTurn(foundMatch, this.data, pendingParticipant, delegate (TurnBasedManager.TurnBasedMatchResponse result) {
                    if (result.RequestSucceeded())
                    {
                        this.callback(true);
                    }
                    else
                    {
                        Logger.d("Taking turn failed: " + result.ResponseStatus());
                        this.callback(false);
                    }
                });
            }

            internal void <>m__1(TurnBasedManager.TurnBasedMatchResponse result)
            {
                if (result.RequestSucceeded())
                {
                    this.callback(true);
                }
                else
                {
                    Logger.d("Taking turn failed: " + result.ResponseStatus());
                    this.callback(false);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <WaitForLogin>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Action method;
            internal NativeTurnBasedMultiplayerClient $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(this.$this.mNativeClient.GetUserId()))
                        {
                            break;
                        }
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        break;

                    default:
                        goto Label_0068;
                }
                this.method();
                this.$PC = -1;
            Label_0068:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

