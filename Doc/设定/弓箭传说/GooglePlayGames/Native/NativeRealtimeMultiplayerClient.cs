namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class NativeRealtimeMultiplayerClient : IRealTimeMultiplayerClient
    {
        private readonly object mSessionLock = new object();
        private readonly NativeClient mNativeClient;
        private readonly RealtimeManager mRealtimeManager;
        private volatile RoomSession mCurrentSession;

        internal NativeRealtimeMultiplayerClient(NativeClient nativeClient, RealtimeManager manager)
        {
            this.mNativeClient = Misc.CheckNotNull<NativeClient>(nativeClient);
            this.mRealtimeManager = Misc.CheckNotNull<RealtimeManager>(manager);
            this.mCurrentSession = this.GetTerminatedSession();
            PlayGamesHelperObject.AddPauseCallback(new Action<bool>(this.HandleAppPausing));
        }

        public void AcceptFromInbox(RealTimeMultiplayerListener listener)
        {
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                <AcceptFromInbox>c__AnonStorey9 storey = new <AcceptFromInbox>c__AnonStorey9 {
                    $this = this,
                    newRoom = new RoomSession(this.mRealtimeManager, listener)
                };
                if (this.mCurrentSession.IsActive())
                {
                    Logger.e("Received attempt to accept invitation without cleaning up active session.");
                    storey.newRoom.LeaveRoom();
                }
                else
                {
                    this.mCurrentSession = storey.newRoom;
                    this.mCurrentSession.ShowingUI = true;
                    this.mRealtimeManager.ShowRoomInboxUI(new Action<RealtimeManager.RoomInboxUIResponse>(storey.<>m__0));
                }
            }
        }

        public void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener)
        {
            <AcceptInvitation>c__AnonStoreyD yd = new <AcceptInvitation>c__AnonStoreyD {
                invitationId = invitationId,
                $this = this
            };
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                <AcceptInvitation>c__AnonStoreyC yc = new <AcceptInvitation>c__AnonStoreyC {
                    <>f__ref$13 = yd,
                    newRoom = new RoomSession(this.mRealtimeManager, listener)
                };
                if (this.mCurrentSession.IsActive())
                {
                    Logger.e("Received attempt to accept invitation without cleaning up active session.");
                    yc.newRoom.LeaveRoom();
                }
                else
                {
                    this.mCurrentSession = yc.newRoom;
                    this.mRealtimeManager.FetchInvitations(new Action<RealtimeManager.FetchInvitationsResponse>(yc.<>m__0));
                }
            }
        }

        public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener)
        {
            this.CreateQuickGame(minOpponents, maxOpponents, variant, 0L, listener);
        }

        public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, RealTimeMultiplayerListener listener)
        {
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                <CreateQuickGame>c__AnonStorey2 storey = new <CreateQuickGame>c__AnonStorey2 {
                    $this = this,
                    newSession = new RoomSession(this.mRealtimeManager, listener)
                };
                if (this.mCurrentSession.IsActive())
                {
                    Logger.e("Received attempt to create a new room without cleaning up the old one.");
                    storey.newSession.LeaveRoom();
                }
                else
                {
                    this.mCurrentSession = storey.newSession;
                    Logger.d("QuickGame: Setting MinPlayersToStart = " + minOpponents);
                    this.mCurrentSession.MinPlayersToStart = minOpponents;
                    using (RealtimeRoomConfigBuilder builder = RealtimeRoomConfigBuilder.Create())
                    {
                        <CreateQuickGame>c__AnonStorey0 storey2 = new <CreateQuickGame>c__AnonStorey0 {
                            config = builder.SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetVariant(variant).SetExclusiveBitMask(exclusiveBitMask).Build()
                        };
                        using (storey2.config)
                        {
                            <CreateQuickGame>c__AnonStorey1 storey3 = new <CreateQuickGame>c__AnonStorey1 {
                                <>f__ref$2 = storey,
                                <>f__ref$0 = storey2,
                                helper = HelperForSession(storey.newSession)
                            };
                            try
                            {
                                storey.newSession.StartRoomCreation(this.mNativeClient.GetUserId(), new Action(storey3.<>m__0));
                            }
                            finally
                            {
                                if (storey3.helper != null)
                                {
                                    storey3.helper.Dispose();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, RealTimeMultiplayerListener listener)
        {
            <CreateWithInvitationScreen>c__AnonStorey5 storey = new <CreateWithInvitationScreen>c__AnonStorey5 {
                variant = variant,
                $this = this
            };
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                <CreateWithInvitationScreen>c__AnonStorey4 storey2 = new <CreateWithInvitationScreen>c__AnonStorey4 {
                    <>f__ref$5 = storey,
                    newRoom = new RoomSession(this.mRealtimeManager, listener)
                };
                if (this.mCurrentSession.IsActive())
                {
                    Logger.e("Received attempt to create a new room without cleaning up the old one.");
                    storey2.newRoom.LeaveRoom();
                }
                else
                {
                    this.mCurrentSession = storey2.newRoom;
                    this.mCurrentSession.ShowingUI = true;
                    this.mRealtimeManager.ShowPlayerSelectUI(minOpponents, maxOppponents, true, new Action<PlayerSelectUIResponse>(storey2.<>m__0));
                }
            }
        }

        public void DeclineInvitation(string invitationId)
        {
            <DeclineInvitation>c__AnonStorey10 storey = new <DeclineInvitation>c__AnonStorey10 {
                invitationId = invitationId,
                $this = this
            };
            this.mRealtimeManager.FetchInvitations(new Action<RealtimeManager.FetchInvitationsResponse>(storey.<>m__0));
        }

        public void GetAllInvitations(Action<Invitation[]> callback)
        {
            <GetAllInvitations>c__AnonStorey8 storey = new <GetAllInvitations>c__AnonStorey8 {
                callback = callback
            };
            this.mRealtimeManager.FetchInvitations(new Action<RealtimeManager.FetchInvitationsResponse>(storey.<>m__0));
        }

        public List<Participant> GetConnectedParticipants() => 
            this.mCurrentSession.GetConnectedParticipants();

        public Invitation GetInvitation() => 
            this.mCurrentSession.GetInvitation();

        public Participant GetParticipant(string participantId) => 
            this.mCurrentSession.GetParticipant(participantId);

        public Participant GetSelf() => 
            this.mCurrentSession.GetSelf();

        private RoomSession GetTerminatedSession()
        {
            RoomSession session = new RoomSession(this.mRealtimeManager, new NoopListener());
            session.EnterState(new ShutdownState(session), false);
            return session;
        }

        private void HandleAppPausing(bool paused)
        {
            if (paused)
            {
                Logger.d("Application is pausing, which disconnects the RTMP  client.  Leaving room.");
                this.LeaveRoom();
            }
        }

        private static RealTimeEventListenerHelper HelperForSession(RoomSession session)
        {
            <HelperForSession>c__AnonStorey3 storey = new <HelperForSession>c__AnonStorey3 {
                session = session
            };
            return RealTimeEventListenerHelper.Create().SetOnDataReceivedCallback(new Action<NativeRealTimeRoom, MultiplayerParticipant, byte[], bool>(storey.<>m__0)).SetOnParticipantStatusChangedCallback(new Action<NativeRealTimeRoom, MultiplayerParticipant>(storey.<>m__1)).SetOnRoomConnectedSetChangedCallback(new Action<NativeRealTimeRoom>(storey.<>m__2)).SetOnRoomStatusChangedCallback(new Action<NativeRealTimeRoom>(storey.<>m__3));
        }

        public bool IsRoomConnected() => 
            this.mCurrentSession.IsRoomConnected();

        public void LeaveRoom()
        {
            this.mCurrentSession.LeaveRoom();
        }

        public void SendMessage(bool reliable, string participantId, byte[] data)
        {
            this.mCurrentSession.SendMessage(reliable, participantId, data);
        }

        public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
        {
            this.mCurrentSession.SendMessage(reliable, participantId, data, offset, length);
        }

        public void SendMessageToAll(bool reliable, byte[] data)
        {
            this.mCurrentSession.SendMessageToAll(reliable, data);
        }

        public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
        {
            this.mCurrentSession.SendMessageToAll(reliable, data, offset, length);
        }

        public void ShowWaitingRoomUI()
        {
            object mSessionLock = this.mSessionLock;
            lock (mSessionLock)
            {
                this.mCurrentSession.ShowWaitingRoomUI();
            }
        }

        private static T WithDefault<T>(T presented, T defaultValue) where T: class => 
            ((presented == null) ? defaultValue : presented);

        [CompilerGenerated]
        private sealed class <AcceptFromInbox>c__AnonStorey9
        {
            internal NativeRealtimeMultiplayerClient.RoomSession newRoom;
            internal NativeRealtimeMultiplayerClient $this;

            internal void <>m__0(RealtimeManager.RoomInboxUIResponse response)
            {
                <AcceptFromInbox>c__AnonStoreyA ya = new <AcceptFromInbox>c__AnonStoreyA {
                    <>f__ref$9 = this
                };
                this.$this.mCurrentSession.ShowingUI = false;
                if (response.ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
                {
                    Logger.d("User did not complete invitation screen.");
                    this.newRoom.LeaveRoom();
                }
                else
                {
                    ya.invitation = response.Invitation();
                    <AcceptFromInbox>c__AnonStoreyB yb = new <AcceptFromInbox>c__AnonStoreyB {
                        <>f__ref$9 = this,
                        <>f__ref$10 = ya,
                        helper = NativeRealtimeMultiplayerClient.HelperForSession(this.newRoom)
                    };
                    try
                    {
                        Logger.d("About to accept invitation " + ya.invitation.Id());
                        this.newRoom.StartRoomCreation(this.$this.mNativeClient.GetUserId(), new Action(yb.<>m__0));
                    }
                    finally
                    {
                        if (yb.helper != null)
                        {
                            yb.helper.Dispose();
                        }
                    }
                }
            }

            private sealed class <AcceptFromInbox>c__AnonStoreyA
            {
                internal MultiplayerInvitation invitation;
                internal NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey9 <>f__ref$9;
            }

            private sealed class <AcceptFromInbox>c__AnonStoreyB
            {
                internal RealTimeEventListenerHelper helper;
                internal NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey9 <>f__ref$9;
                internal NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey9.<AcceptFromInbox>c__AnonStoreyA <>f__ref$10;

                internal void <>m__0()
                {
                    this.<>f__ref$9.$this.mRealtimeManager.AcceptInvitation(this.<>f__ref$10.invitation, this.helper, delegate (RealtimeManager.RealTimeRoomResponse acceptResponse) {
                        using (this.<>f__ref$10.invitation)
                        {
                            this.<>f__ref$9.newRoom.HandleRoomResponse(acceptResponse);
                            this.<>f__ref$9.newRoom.SetInvitation(this.<>f__ref$10.invitation.AsInvitation());
                        }
                    });
                }

                internal void <>m__1(RealtimeManager.RealTimeRoomResponse acceptResponse)
                {
                    using (this.<>f__ref$10.invitation)
                    {
                        this.<>f__ref$9.newRoom.HandleRoomResponse(acceptResponse);
                        this.<>f__ref$9.newRoom.SetInvitation(this.<>f__ref$10.invitation.AsInvitation());
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AcceptInvitation>c__AnonStoreyC
        {
            internal NativeRealtimeMultiplayerClient.RoomSession newRoom;
            internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStoreyD <>f__ref$13;

            internal void <>m__0(RealtimeManager.FetchInvitationsResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    Logger.e("Couldn't load invitations.");
                    this.newRoom.LeaveRoom();
                }
                else
                {
                    using (IEnumerator<MultiplayerInvitation> enumerator = response.Invitations().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            <AcceptInvitation>c__AnonStoreyE ye = new <AcceptInvitation>c__AnonStoreyE {
                                <>f__ref$13 = this.<>f__ref$13,
                                <>f__ref$12 = this,
                                invitation = enumerator.Current
                            };
                            using (ye.invitation)
                            {
                                if (ye.invitation.Id().Equals(this.<>f__ref$13.invitationId))
                                {
                                    this.<>f__ref$13.$this.mCurrentSession.MinPlayersToStart = ye.invitation.AutomatchingSlots() + ye.invitation.ParticipantCount();
                                    Logger.d("Setting MinPlayersToStart with invitation to : " + this.<>f__ref$13.$this.mCurrentSession.MinPlayersToStart);
                                    <AcceptInvitation>c__AnonStoreyF yf = new <AcceptInvitation>c__AnonStoreyF {
                                        <>f__ref$13 = this.<>f__ref$13,
                                        <>f__ref$12 = this,
                                        <>f__ref$14 = ye,
                                        helper = NativeRealtimeMultiplayerClient.HelperForSession(this.newRoom)
                                    };
                                    try
                                    {
                                        this.newRoom.StartRoomCreation(this.<>f__ref$13.$this.mNativeClient.GetUserId(), new Action(yf.<>m__0));
                                        return;
                                    }
                                    finally
                                    {
                                        if (yf.helper != null)
                                        {
                                            yf.helper.Dispose();
                                        }
                                    }
                                }
                                continue;
                            }
                        }
                    }
                    Logger.e("Room creation failed since we could not find invitation with ID " + this.<>f__ref$13.invitationId);
                    this.newRoom.LeaveRoom();
                }
            }

            private sealed class <AcceptInvitation>c__AnonStoreyE
            {
                internal MultiplayerInvitation invitation;
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStoreyD <>f__ref$13;
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStoreyC <>f__ref$12;
            }

            private sealed class <AcceptInvitation>c__AnonStoreyF
            {
                internal RealTimeEventListenerHelper helper;
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStoreyD <>f__ref$13;
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStoreyC <>f__ref$12;
                internal NativeRealtimeMultiplayerClient.<AcceptInvitation>c__AnonStoreyC.<AcceptInvitation>c__AnonStoreyE <>f__ref$14;

                internal void <>m__0()
                {
                    this.<>f__ref$13.$this.mRealtimeManager.AcceptInvitation(this.<>f__ref$14.invitation, this.helper, new Action<RealtimeManager.RealTimeRoomResponse>(this.<>f__ref$12.newRoom.HandleRoomResponse));
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AcceptInvitation>c__AnonStoreyD
        {
            internal string invitationId;
            internal NativeRealtimeMultiplayerClient $this;
        }

        [CompilerGenerated]
        private sealed class <CreateQuickGame>c__AnonStorey0
        {
            internal RealtimeRoomConfig config;
        }

        [CompilerGenerated]
        private sealed class <CreateQuickGame>c__AnonStorey1
        {
            internal RealTimeEventListenerHelper helper;
            internal NativeRealtimeMultiplayerClient.<CreateQuickGame>c__AnonStorey2 <>f__ref$2;
            internal NativeRealtimeMultiplayerClient.<CreateQuickGame>c__AnonStorey0 <>f__ref$0;

            internal void <>m__0()
            {
                this.<>f__ref$2.$this.mRealtimeManager.CreateRoom(this.<>f__ref$0.config, this.helper, new Action<RealtimeManager.RealTimeRoomResponse>(this.<>f__ref$2.newSession.HandleRoomResponse));
            }
        }

        [CompilerGenerated]
        private sealed class <CreateQuickGame>c__AnonStorey2
        {
            internal NativeRealtimeMultiplayerClient.RoomSession newSession;
            internal NativeRealtimeMultiplayerClient $this;
        }

        [CompilerGenerated]
        private sealed class <CreateWithInvitationScreen>c__AnonStorey4
        {
            internal NativeRealtimeMultiplayerClient.RoomSession newRoom;
            internal NativeRealtimeMultiplayerClient.<CreateWithInvitationScreen>c__AnonStorey5 <>f__ref$5;

            internal void <>m__0(PlayerSelectUIResponse response)
            {
                this.<>f__ref$5.$this.mCurrentSession.ShowingUI = false;
                if (response.Status() != CommonErrorStatus.UIStatus.VALID)
                {
                    Logger.d("User did not complete invitation screen.");
                    this.newRoom.LeaveRoom();
                }
                else
                {
                    this.<>f__ref$5.$this.mCurrentSession.MinPlayersToStart = (uint) ((response.MinimumAutomatchingPlayers() + response.Count<string>()) + 1);
                    using (RealtimeRoomConfigBuilder builder = RealtimeRoomConfigBuilder.Create())
                    {
                        builder.SetVariant(this.<>f__ref$5.variant);
                        builder.PopulateFromUIResponse(response);
                        <CreateWithInvitationScreen>c__AnonStorey6 storey = new <CreateWithInvitationScreen>c__AnonStorey6 {
                            config = builder.Build()
                        };
                        try
                        {
                            <CreateWithInvitationScreen>c__AnonStorey7 storey2 = new <CreateWithInvitationScreen>c__AnonStorey7 {
                                <>f__ref$5 = this.<>f__ref$5,
                                <>f__ref$4 = this,
                                <>f__ref$6 = storey,
                                helper = NativeRealtimeMultiplayerClient.HelperForSession(this.newRoom)
                            };
                            try
                            {
                                this.newRoom.StartRoomCreation(this.<>f__ref$5.$this.mNativeClient.GetUserId(), new Action(storey2.<>m__0));
                            }
                            finally
                            {
                                if (storey2.helper != null)
                                {
                                    storey2.helper.Dispose();
                                }
                            }
                        }
                        finally
                        {
                            if (storey.config != null)
                            {
                                storey.config.Dispose();
                            }
                        }
                    }
                }
            }

            private sealed class <CreateWithInvitationScreen>c__AnonStorey6
            {
                internal RealtimeRoomConfig config;
            }

            private sealed class <CreateWithInvitationScreen>c__AnonStorey7
            {
                internal RealTimeEventListenerHelper helper;
                internal NativeRealtimeMultiplayerClient.<CreateWithInvitationScreen>c__AnonStorey5 <>f__ref$5;
                internal NativeRealtimeMultiplayerClient.<CreateWithInvitationScreen>c__AnonStorey4 <>f__ref$4;
                internal NativeRealtimeMultiplayerClient.<CreateWithInvitationScreen>c__AnonStorey4.<CreateWithInvitationScreen>c__AnonStorey6 <>f__ref$6;

                internal void <>m__0()
                {
                    this.<>f__ref$5.$this.mRealtimeManager.CreateRoom(this.<>f__ref$6.config, this.helper, new Action<RealtimeManager.RealTimeRoomResponse>(this.<>f__ref$4.newRoom.HandleRoomResponse));
                }
            }
        }

        [CompilerGenerated]
        private sealed class <CreateWithInvitationScreen>c__AnonStorey5
        {
            internal uint variant;
            internal NativeRealtimeMultiplayerClient $this;
        }

        [CompilerGenerated]
        private sealed class <DeclineInvitation>c__AnonStorey10
        {
            internal string invitationId;
            internal NativeRealtimeMultiplayerClient $this;

            internal void <>m__0(RealtimeManager.FetchInvitationsResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    Logger.e("Couldn't load invitations.");
                }
                else
                {
                    foreach (MultiplayerInvitation invitation in response.Invitations())
                    {
                        using (invitation)
                        {
                            if (invitation.Id().Equals(this.invitationId))
                            {
                                this.$this.mRealtimeManager.DeclineInvitation(invitation);
                            }
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetAllInvitations>c__AnonStorey8
        {
            internal Action<Invitation[]> callback;

            internal void <>m__0(RealtimeManager.FetchInvitationsResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    Logger.e("Couldn't load invitations.");
                    this.callback(new Invitation[0]);
                }
                else
                {
                    List<Invitation> list = new List<Invitation>();
                    foreach (MultiplayerInvitation invitation in response.Invitations())
                    {
                        using (invitation)
                        {
                            list.Add(invitation.AsInvitation());
                        }
                    }
                    this.callback(list.ToArray());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <HelperForSession>c__AnonStorey3
        {
            internal NativeRealtimeMultiplayerClient.RoomSession session;

            internal void <>m__0(NativeRealTimeRoom room, MultiplayerParticipant participant, byte[] data, bool isReliable)
            {
                this.session.OnDataReceived(room, participant, data, isReliable);
            }

            internal void <>m__1(NativeRealTimeRoom room, MultiplayerParticipant participant)
            {
                this.session.OnParticipantStatusChanged(room, participant);
            }

            internal void <>m__2(NativeRealTimeRoom room)
            {
                this.session.OnConnectedSetChanged(room);
            }

            internal void <>m__3(NativeRealTimeRoom room)
            {
                this.session.OnRoomStatusChanged(room);
            }
        }

        private class AbortingRoomCreationState : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

            internal AbortingRoomCreationState(NativeRealtimeMultiplayerClient.RoomSession session)
            {
                this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
            }

            internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
                    this.mSession.OnGameThreadListener().RoomConnected(false);
                }
                else
                {
                    this.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(this.mSession, response.Room(), () => this.mSession.OnGameThreadListener().RoomConnected(false)));
                }
            }

            internal override bool IsActive() => 
                false;
        }

        private class ActiveState : NativeRealtimeMultiplayerClient.MessagingEnabledState
        {
            [CompilerGenerated]
            private static Func<MultiplayerParticipant, string> <>f__am$cache0;
            [CompilerGenerated]
            private static Func<MultiplayerParticipant, Participant> <>f__am$cache1;
            [CompilerGenerated]
            private static Func<Participant, string> <>f__am$cache2;
            [CompilerGenerated]
            private static Func<Participant, string> <>f__am$cache3;

            internal ActiveState(NativeRealTimeRoom room, NativeRealtimeMultiplayerClient.RoomSession session) : base(session, room)
            {
            }

            internal override Participant GetParticipant(string participantId)
            {
                if (!base.mParticipants.ContainsKey(participantId))
                {
                    Logger.e("Attempted to retrieve unknown participant " + participantId);
                    return null;
                }
                return base.mParticipants[participantId];
            }

            internal override Participant GetSelf()
            {
                foreach (Participant participant in base.mParticipants.Values)
                {
                    if ((participant.Player != null) && participant.Player.id.Equals(base.mSession.SelfPlayerId()))
                    {
                        return participant;
                    }
                }
                return null;
            }

            internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
            {
                <HandleConnectedSetChanged>c__AnonStorey0 storey = new <HandleConnectedSetChanged>c__AnonStorey0();
                List<string> source = new List<string>();
                List<string> list2 = new List<string>();
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = p => p.Id();
                }
                Dictionary<string, MultiplayerParticipant> dictionary = room.Participants().ToDictionary<MultiplayerParticipant, string>(<>f__am$cache0);
                foreach (string str in base.mNativeParticipants.Keys)
                {
                    MultiplayerParticipant participant = dictionary[str];
                    MultiplayerParticipant participant2 = base.mNativeParticipants[str];
                    if (!participant.IsConnectedToRoom())
                    {
                        list2.Add(str);
                    }
                    if (!participant2.IsConnectedToRoom() && participant.IsConnectedToRoom())
                    {
                        source.Add(str);
                    }
                }
                foreach (MultiplayerParticipant participant3 in base.mNativeParticipants.Values)
                {
                    participant3.Dispose();
                }
                base.mNativeParticipants = dictionary;
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = p => p.AsParticipant();
                }
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = p => p.ParticipantId;
                }
                base.mParticipants = base.mNativeParticipants.Values.Select<MultiplayerParticipant, Participant>(<>f__am$cache1).ToDictionary<Participant, string>(<>f__am$cache2);
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = p => p.ToString();
                }
                Logger.d("Updated participant statuses: " + string.Join(",", base.mParticipants.Values.Select<Participant, string>(<>f__am$cache3).ToArray<string>()));
                if (list2.Contains(this.GetSelf().ParticipantId))
                {
                    Logger.w("Player was disconnected from the multiplayer session.");
                }
                storey.selfId = this.GetSelf().ParticipantId;
                source = source.Where<string>(new Func<string, bool>(storey.<>m__0)).ToList<string>();
                list2 = list2.Where<string>(new Func<string, bool>(storey.<>m__1)).ToList<string>();
                if (source.Count > 0)
                {
                    source.Sort();
                    base.mSession.OnGameThreadListener().PeersConnected(source.Where<string>(new Func<string, bool>(storey.<>m__2)).ToArray<string>());
                }
                if (list2.Count > 0)
                {
                    list2.Sort();
                    base.mSession.OnGameThreadListener().PeersDisconnected(list2.Where<string>(new Func<string, bool>(storey.<>m__3)).ToArray<string>());
                }
            }

            internal override bool IsRoomConnected() => 
                true;

            internal override void LeaveRoom()
            {
                base.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(base.mSession, base.mRoom, () => base.mSession.OnGameThreadListener().LeftRoom()));
            }

            internal override void OnStateEntered()
            {
                if (this.GetSelf() == null)
                {
                    Logger.e("Room reached active state with unknown participant for the player");
                    this.LeaveRoom();
                }
            }

            [CompilerGenerated]
            private sealed class <HandleConnectedSetChanged>c__AnonStorey0
            {
                internal string selfId;

                internal bool <>m__0(string peerId) => 
                    !peerId.Equals(this.selfId);

                internal bool <>m__1(string peerId) => 
                    !peerId.Equals(this.selfId);

                internal bool <>m__2(string peer) => 
                    !peer.Equals(this.selfId);

                internal bool <>m__3(string peer) => 
                    !peer.Equals(this.selfId);
            }
        }

        private class BeforeRoomCreateStartedState : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mContainingSession;

            internal BeforeRoomCreateStartedState(NativeRealtimeMultiplayerClient.RoomSession session)
            {
                this.mContainingSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
            }

            internal override void LeaveRoom()
            {
                Logger.d("Session was torn down before room was created.");
                this.mContainingSession.OnGameThreadListener().RoomConnected(false);
                this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mContainingSession));
            }
        }

        private class ConnectingState : NativeRealtimeMultiplayerClient.MessagingEnabledState
        {
            private const float InitialPercentComplete = 20f;
            private static readonly HashSet<Types.ParticipantStatus> FailedStatuses;
            private HashSet<string> mConnectedParticipants;
            private float mPercentComplete;
            private float mPercentPerParticipant;

            static ConnectingState()
            {
                HashSet<Types.ParticipantStatus> set = new HashSet<Types.ParticipantStatus> {
                    Types.ParticipantStatus.DECLINED,
                    Types.ParticipantStatus.LEFT
                };
                FailedStatuses = set;
            }

            internal ConnectingState(NativeRealTimeRoom room, NativeRealtimeMultiplayerClient.RoomSession session) : base(session, room)
            {
                this.mConnectedParticipants = new HashSet<string>();
                this.mPercentComplete = 20f;
                this.mPercentPerParticipant = 80f / ((float) session.MinPlayersToStart);
            }

            internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
            {
                HashSet<string> set = new HashSet<string>();
                if (((room.Status() == Types.RealTimeRoomStatus.AUTO_MATCHING) || (room.Status() == Types.RealTimeRoomStatus.CONNECTING)) && (base.mSession.MinPlayersToStart <= room.ParticipantCount()))
                {
                    base.mSession.MinPlayersToStart += room.ParticipantCount();
                    this.mPercentPerParticipant = 80f / ((float) base.mSession.MinPlayersToStart);
                }
                foreach (MultiplayerParticipant participant in room.Participants())
                {
                    using (participant)
                    {
                        if (participant.IsConnectedToRoom())
                        {
                            set.Add(participant.Id());
                        }
                    }
                }
                if (this.mConnectedParticipants.Equals(set))
                {
                    Logger.w("Received connected set callback with unchanged connected set!");
                }
                else
                {
                    IEnumerable<string> source = this.mConnectedParticipants.Except<string>(set);
                    if (room.Status() == Types.RealTimeRoomStatus.DELETED)
                    {
                        Logger.e("Participants disconnected during room setup, failing. Participants were: " + string.Join(",", source.ToArray<string>()));
                        base.mSession.OnGameThreadListener().RoomConnected(false);
                        base.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(base.mSession));
                    }
                    else
                    {
                        IEnumerable<string> enumerable2 = set.Except<string>(this.mConnectedParticipants);
                        Logger.d("New participants connected: " + string.Join(",", enumerable2.ToArray<string>()));
                        if (room.Status() == Types.RealTimeRoomStatus.ACTIVE)
                        {
                            Logger.d("Fully connected! Transitioning to active state.");
                            base.mSession.EnterState(new NativeRealtimeMultiplayerClient.ActiveState(room, base.mSession));
                            base.mSession.OnGameThreadListener().RoomConnected(true);
                        }
                        else
                        {
                            this.mPercentComplete += this.mPercentPerParticipant * enumerable2.Count<string>();
                            this.mConnectedParticipants = set;
                            base.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
                        }
                    }
                }
            }

            internal override void HandleParticipantStatusChanged(NativeRealTimeRoom room, MultiplayerParticipant participant)
            {
                if (FailedStatuses.Contains(participant.Status()))
                {
                    base.mSession.OnGameThreadListener().ParticipantLeft(participant.AsParticipant());
                    if ((room.Status() != Types.RealTimeRoomStatus.CONNECTING) && (room.Status() != Types.RealTimeRoomStatus.AUTO_MATCHING))
                    {
                        this.LeaveRoom();
                    }
                }
            }

            internal override void LeaveRoom()
            {
                base.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(base.mSession, base.mRoom, () => base.mSession.OnGameThreadListener().RoomConnected(false)));
            }

            internal override void OnStateEntered()
            {
                base.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
            }

            internal override void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
            {
                base.mSession.ShowingUI = true;
                base.mSession.Manager().ShowWaitingRoomUI(base.mRoom, minimumParticipantsBeforeStarting, delegate (RealtimeManager.WaitingRoomUIResponse response) {
                    base.mSession.ShowingUI = false;
                    Logger.d("ShowWaitingRoomUI Response: " + response.ResponseStatus());
                    if (response.ResponseStatus() == CommonErrorStatus.UIStatus.VALID)
                    {
                        Logger.d(string.Concat(new object[] { "Connecting state ShowWaitingRoomUI: room pcount:", response.Room().ParticipantCount(), " status: ", response.Room().Status() }));
                        if (response.Room().Status() == Types.RealTimeRoomStatus.ACTIVE)
                        {
                            base.mSession.EnterState(new NativeRealtimeMultiplayerClient.ActiveState(response.Room(), base.mSession));
                        }
                    }
                    else if (response.ResponseStatus() == CommonErrorStatus.UIStatus.ERROR_LEFT_ROOM)
                    {
                        this.LeaveRoom();
                    }
                    else
                    {
                        base.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
                    }
                });
            }
        }

        private class LeavingRoom : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;
            private readonly NativeRealTimeRoom mRoomToLeave;
            private readonly Action mLeavingCompleteCallback;

            internal LeavingRoom(NativeRealtimeMultiplayerClient.RoomSession session, NativeRealTimeRoom room, Action leavingCompleteCallback)
            {
                this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
                this.mRoomToLeave = Misc.CheckNotNull<NativeRealTimeRoom>(room);
                this.mLeavingCompleteCallback = Misc.CheckNotNull<Action>(leavingCompleteCallback);
            }

            internal override bool IsActive() => 
                false;

            internal override void OnStateEntered()
            {
                this.mSession.Manager().LeaveRoom(this.mRoomToLeave, delegate (CommonErrorStatus.ResponseStatus status) {
                    this.mLeavingCompleteCallback();
                    this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
                });
            }
        }

        private abstract class MessagingEnabledState : NativeRealtimeMultiplayerClient.State
        {
            protected readonly NativeRealtimeMultiplayerClient.RoomSession mSession;
            protected NativeRealTimeRoom mRoom;
            protected Dictionary<string, MultiplayerParticipant> mNativeParticipants;
            protected Dictionary<string, Participant> mParticipants;
            [CompilerGenerated]
            private static Func<MultiplayerParticipant, string> <>f__am$cache0;
            [CompilerGenerated]
            private static Func<MultiplayerParticipant, Participant> <>f__am$cache1;
            [CompilerGenerated]
            private static Func<Participant, string> <>f__am$cache2;
            [CompilerGenerated]
            private static Func<Participant, bool> <>f__am$cache3;

            internal MessagingEnabledState(NativeRealtimeMultiplayerClient.RoomSession session, NativeRealTimeRoom room)
            {
                this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
                this.UpdateCurrentRoom(room);
            }

            internal sealed override List<Participant> GetConnectedParticipants()
            {
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = p => p.IsConnectedToRoom;
                }
                List<Participant> list = this.mParticipants.Values.Where<Participant>(<>f__am$cache3).ToList<Participant>();
                list.Sort();
                return list;
            }

            internal virtual void HandleConnectedSetChanged(NativeRealTimeRoom room)
            {
            }

            internal virtual void HandleParticipantStatusChanged(NativeRealTimeRoom room, MultiplayerParticipant participant)
            {
            }

            internal virtual void HandleRoomStatusChanged(NativeRealTimeRoom room)
            {
            }

            internal sealed override void OnConnectedSetChanged(NativeRealTimeRoom room)
            {
                this.HandleConnectedSetChanged(room);
                this.UpdateCurrentRoom(room);
            }

            internal override void OnDataReceived(NativeRealTimeRoom room, MultiplayerParticipant sender, byte[] data, bool isReliable)
            {
                this.mSession.OnGameThreadListener().RealTimeMessageReceived(isReliable, sender.Id(), data);
            }

            internal sealed override void OnParticipantStatusChanged(NativeRealTimeRoom room, MultiplayerParticipant participant)
            {
                this.HandleParticipantStatusChanged(room, participant);
                this.UpdateCurrentRoom(room);
            }

            internal sealed override void OnRoomStatusChanged(NativeRealTimeRoom room)
            {
                this.HandleRoomStatusChanged(room);
                this.UpdateCurrentRoom(room);
            }

            internal override void SendToAll(byte[] data, int offset, int length, bool isReliable)
            {
                byte[] buffer = Misc.GetSubsetBytes(data, offset, length);
                if (isReliable)
                {
                    foreach (string str in this.mNativeParticipants.Keys)
                    {
                        this.SendToSpecificRecipient(str, buffer, 0, buffer.Length, true);
                    }
                }
                else
                {
                    this.mSession.Manager().SendUnreliableMessageToAll(this.mRoom, buffer);
                }
            }

            internal override void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
            {
                if (!this.mNativeParticipants.ContainsKey(recipientId))
                {
                    Logger.e("Attempted to send message to unknown participant " + recipientId);
                }
                else if (isReliable)
                {
                    this.mSession.Manager().SendReliableMessage(this.mRoom, this.mNativeParticipants[recipientId], Misc.GetSubsetBytes(data, offset, length), null);
                }
                else
                {
                    List<MultiplayerParticipant> recipients = new List<MultiplayerParticipant> {
                        this.mNativeParticipants[recipientId]
                    };
                    this.mSession.Manager().SendUnreliableMessageToSpecificParticipants(this.mRoom, recipients, Misc.GetSubsetBytes(data, offset, length));
                }
            }

            internal void UpdateCurrentRoom(NativeRealTimeRoom room)
            {
                if (this.mRoom != null)
                {
                    this.mRoom.Dispose();
                }
                this.mRoom = Misc.CheckNotNull<NativeRealTimeRoom>(room);
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = p => p.Id();
                }
                this.mNativeParticipants = this.mRoom.Participants().ToDictionary<MultiplayerParticipant, string>(<>f__am$cache0);
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = p => p.AsParticipant();
                }
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = p => p.ParticipantId;
                }
                this.mParticipants = this.mNativeParticipants.Values.Select<MultiplayerParticipant, Participant>(<>f__am$cache1).ToDictionary<Participant, string>(<>f__am$cache2);
            }
        }

        private class NoopListener : RealTimeMultiplayerListener
        {
            public void OnLeftRoom()
            {
            }

            public void OnParticipantLeft(Participant participant)
            {
            }

            public void OnPeersConnected(string[] participantIds)
            {
            }

            public void OnPeersDisconnected(string[] participantIds)
            {
            }

            public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
            {
            }

            public void OnRoomConnected(bool success)
            {
            }

            public void OnRoomSetupProgress(float percent)
            {
            }
        }

        private class OnGameThreadForwardingListener
        {
            private readonly RealTimeMultiplayerListener mListener;

            internal OnGameThreadForwardingListener(RealTimeMultiplayerListener listener)
            {
                this.mListener = Misc.CheckNotNull<RealTimeMultiplayerListener>(listener);
            }

            public void LeftRoom()
            {
                PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnLeftRoom());
            }

            public void ParticipantLeft(Participant participant)
            {
                <ParticipantLeft>c__AnonStorey5 storey = new <ParticipantLeft>c__AnonStorey5 {
                    participant = participant,
                    $this = this
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            public void PeersConnected(string[] participantIds)
            {
                <PeersConnected>c__AnonStorey2 storey = new <PeersConnected>c__AnonStorey2 {
                    participantIds = participantIds,
                    $this = this
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            public void PeersDisconnected(string[] participantIds)
            {
                <PeersDisconnected>c__AnonStorey3 storey = new <PeersDisconnected>c__AnonStorey3 {
                    participantIds = participantIds,
                    $this = this
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            public void RealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
            {
                <RealTimeMessageReceived>c__AnonStorey4 storey = new <RealTimeMessageReceived>c__AnonStorey4 {
                    isReliable = isReliable,
                    senderId = senderId,
                    data = data,
                    $this = this
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            public void RoomConnected(bool success)
            {
                <RoomConnected>c__AnonStorey1 storey = new <RoomConnected>c__AnonStorey1 {
                    success = success,
                    $this = this
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            public void RoomSetupProgress(float percent)
            {
                <RoomSetupProgress>c__AnonStorey0 storey = new <RoomSetupProgress>c__AnonStorey0 {
                    percent = percent,
                    $this = this
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            [CompilerGenerated]
            private sealed class <ParticipantLeft>c__AnonStorey5
            {
                internal Participant participant;
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener $this;

                internal void <>m__0()
                {
                    this.$this.mListener.OnParticipantLeft(this.participant);
                }
            }

            [CompilerGenerated]
            private sealed class <PeersConnected>c__AnonStorey2
            {
                internal string[] participantIds;
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener $this;

                internal void <>m__0()
                {
                    this.$this.mListener.OnPeersConnected(this.participantIds);
                }
            }

            [CompilerGenerated]
            private sealed class <PeersDisconnected>c__AnonStorey3
            {
                internal string[] participantIds;
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener $this;

                internal void <>m__0()
                {
                    this.$this.mListener.OnPeersDisconnected(this.participantIds);
                }
            }

            [CompilerGenerated]
            private sealed class <RealTimeMessageReceived>c__AnonStorey4
            {
                internal bool isReliable;
                internal string senderId;
                internal byte[] data;
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener $this;

                internal void <>m__0()
                {
                    this.$this.mListener.OnRealTimeMessageReceived(this.isReliable, this.senderId, this.data);
                }
            }

            [CompilerGenerated]
            private sealed class <RoomConnected>c__AnonStorey1
            {
                internal bool success;
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener $this;

                internal void <>m__0()
                {
                    this.$this.mListener.OnRoomConnected(this.success);
                }
            }

            [CompilerGenerated]
            private sealed class <RoomSetupProgress>c__AnonStorey0
            {
                internal float percent;
                internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener $this;

                internal void <>m__0()
                {
                    this.$this.mListener.OnRoomSetupProgress(this.percent);
                }
            }
        }

        private class RoomCreationPendingState : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mContainingSession;

            internal RoomCreationPendingState(NativeRealtimeMultiplayerClient.RoomSession session)
            {
                this.mContainingSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
            }

            internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mContainingSession));
                    this.mContainingSession.OnGameThreadListener().RoomConnected(false);
                }
                else
                {
                    this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ConnectingState(response.Room(), this.mContainingSession));
                }
            }

            internal override bool IsActive() => 
                true;

            internal override void LeaveRoom()
            {
                Logger.d("Received request to leave room during room creation, aborting creation.");
                this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.AbortingRoomCreationState(this.mContainingSession));
            }
        }

        private class RoomSession
        {
            private readonly object mLifecycleLock = new object();
            private readonly NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener mListener;
            private readonly RealtimeManager mManager;
            private volatile string mCurrentPlayerId;
            private volatile NativeRealtimeMultiplayerClient.State mState;
            private volatile bool mStillPreRoomCreation;
            private Invitation mInvitation;
            private volatile bool mShowingUI;
            private uint mMinPlayersToStart;

            internal RoomSession(RealtimeManager manager, RealTimeMultiplayerListener listener)
            {
                this.mManager = Misc.CheckNotNull<RealtimeManager>(manager);
                this.mListener = new NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener(listener);
                this.EnterState(new NativeRealtimeMultiplayerClient.BeforeRoomCreateStartedState(this), false);
                this.mStillPreRoomCreation = true;
            }

            internal void EnterState(NativeRealtimeMultiplayerClient.State handler)
            {
                this.EnterState(handler, true);
            }

            internal void EnterState(NativeRealtimeMultiplayerClient.State handler, bool fireStateEnteredEvent)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.State>(handler);
                    if (fireStateEnteredEvent)
                    {
                        Logger.d("Entering state: " + handler.GetType().Name);
                        this.mState.OnStateEntered();
                    }
                }
            }

            internal List<Participant> GetConnectedParticipants() => 
                this.mState.GetConnectedParticipants();

            public Invitation GetInvitation() => 
                this.mInvitation;

            internal virtual Participant GetParticipant(string participantId) => 
                this.mState.GetParticipant(participantId);

            internal virtual Participant GetSelf() => 
                this.mState.GetSelf();

            internal void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState.HandleRoomResponse(response);
                }
            }

            internal bool IsActive() => 
                this.mState.IsActive();

            internal virtual bool IsRoomConnected() => 
                this.mState.IsRoomConnected();

            internal void LeaveRoom()
            {
                if (!this.ShowingUI)
                {
                    object mLifecycleLock = this.mLifecycleLock;
                    lock (mLifecycleLock)
                    {
                        this.mState.LeaveRoom();
                    }
                }
                else
                {
                    Logger.d("Not leaving room since showing UI");
                }
            }

            internal RealtimeManager Manager() => 
                this.mManager;

            internal void OnConnectedSetChanged(NativeRealTimeRoom room)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState.OnConnectedSetChanged(room);
                }
            }

            internal void OnDataReceived(NativeRealTimeRoom room, MultiplayerParticipant sender, byte[] data, bool isReliable)
            {
                this.mState.OnDataReceived(room, sender, data, isReliable);
            }

            internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener OnGameThreadListener() => 
                this.mListener;

            internal void OnParticipantStatusChanged(NativeRealTimeRoom room, MultiplayerParticipant participant)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState.OnParticipantStatusChanged(room, participant);
                }
            }

            internal void OnRoomStatusChanged(NativeRealTimeRoom room)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    this.mState.OnRoomStatusChanged(room);
                }
            }

            internal string SelfPlayerId() => 
                this.mCurrentPlayerId;

            internal void SendMessage(bool reliable, string participantId, byte[] data)
            {
                this.SendMessage(reliable, participantId, data, 0, data.Length);
            }

            internal void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
            {
                this.mState.SendToSpecificRecipient(participantId, data, offset, length, reliable);
            }

            internal void SendMessageToAll(bool reliable, byte[] data)
            {
                this.SendMessageToAll(reliable, data, 0, data.Length);
            }

            internal void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
            {
                this.mState.SendToAll(data, offset, length, reliable);
            }

            public void SetInvitation(Invitation invitation)
            {
                this.mInvitation = invitation;
            }

            internal void ShowWaitingRoomUI()
            {
                this.mState.ShowWaitingRoomUI(this.MinPlayersToStart);
            }

            internal void StartRoomCreation(string currentPlayerId, Action createRoom)
            {
                object mLifecycleLock = this.mLifecycleLock;
                lock (mLifecycleLock)
                {
                    if (!this.mStillPreRoomCreation)
                    {
                        Logger.e("Room creation started more than once, this shouldn't happen!");
                    }
                    else if (!this.mState.IsActive())
                    {
                        Logger.w("Received an attempt to create a room after the session was already torn down!");
                    }
                    else
                    {
                        this.mCurrentPlayerId = Misc.CheckNotNull<string>(currentPlayerId);
                        this.mStillPreRoomCreation = false;
                        this.EnterState(new NativeRealtimeMultiplayerClient.RoomCreationPendingState(this));
                        createRoom();
                    }
                }
            }

            internal bool ShowingUI
            {
                get => 
                    this.mShowingUI;
                set => 
                    (this.mShowingUI = value);
            }

            internal uint MinPlayersToStart
            {
                get => 
                    this.mMinPlayersToStart;
                set => 
                    (this.mMinPlayersToStart = value);
            }
        }

        private class ShutdownState : NativeRealtimeMultiplayerClient.State
        {
            private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

            internal ShutdownState(NativeRealtimeMultiplayerClient.RoomSession session)
            {
                this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
            }

            internal override bool IsActive() => 
                false;

            internal override void LeaveRoom()
            {
                this.mSession.OnGameThreadListener().LeftRoom();
            }
        }

        internal abstract class State
        {
            protected State()
            {
            }

            internal virtual List<Participant> GetConnectedParticipants()
            {
                Logger.d(base.GetType().Name + ".GetConnectedParticipants: Returning empty connected participants");
                return new List<Participant>();
            }

            internal virtual Participant GetParticipant(string participantId)
            {
                Logger.d(base.GetType().Name + ".GetSelf: Returning null participant.");
                return null;
            }

            internal virtual Participant GetSelf()
            {
                Logger.d(base.GetType().Name + ".GetSelf: Returning null self.");
                return null;
            }

            internal virtual void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
            {
                Logger.d(base.GetType().Name + ".HandleRoomResponse: Defaulting to no-op.");
            }

            internal virtual bool IsActive()
            {
                Logger.d(base.GetType().Name + ".IsNonPreemptable: Is preemptable by default.");
                return true;
            }

            internal virtual bool IsRoomConnected()
            {
                Logger.d(base.GetType().Name + ".IsRoomConnected: Returning room not connected.");
                return false;
            }

            internal virtual void LeaveRoom()
            {
                Logger.d(base.GetType().Name + ".LeaveRoom: Defaulting to no-op.");
            }

            internal virtual void OnConnectedSetChanged(NativeRealTimeRoom room)
            {
                Logger.d(base.GetType().Name + ".OnConnectedSetChanged: Defaulting to no-op.");
            }

            internal virtual void OnDataReceived(NativeRealTimeRoom room, MultiplayerParticipant sender, byte[] data, bool isReliable)
            {
                Logger.d(base.GetType().Name + ".OnDataReceived: Defaulting to no-op.");
            }

            internal virtual void OnParticipantStatusChanged(NativeRealTimeRoom room, MultiplayerParticipant participant)
            {
                Logger.d(base.GetType().Name + ".OnParticipantStatusChanged: Defaulting to no-op.");
            }

            internal virtual void OnRoomStatusChanged(NativeRealTimeRoom room)
            {
                Logger.d(base.GetType().Name + ".OnRoomStatusChanged: Defaulting to no-op.");
            }

            internal virtual void OnStateEntered()
            {
                Logger.d(base.GetType().Name + ".OnStateEntered: Defaulting to no-op.");
            }

            internal virtual void SendToAll(byte[] data, int offset, int length, bool isReliable)
            {
                Logger.d(base.GetType().Name + ".SendToApp: Defaulting to no-op.");
            }

            internal virtual void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
            {
                Logger.d(base.GetType().Name + ".SendToSpecificRecipient: Defaulting to no-op.");
            }

            internal virtual void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
            {
                Logger.d(base.GetType().Name + ".ShowWaitingRoomUI: Defaulting to no-op.");
            }
        }
    }
}

