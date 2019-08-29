namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class RealtimeManager
    {
        private readonly GameServices mGameServices;
        [CompilerGenerated]
        private static RealTimeMultiplayerManager.RealTimeRoomCallback <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<IntPtr, PlayerSelectUIResponse> <>f__mg$cache1;
        [CompilerGenerated]
        private static RealTimeMultiplayerManager.PlayerSelectUICallback <>f__mg$cache2;
        [CompilerGenerated]
        private static Func<IntPtr, RoomInboxUIResponse> <>f__mg$cache3;
        [CompilerGenerated]
        private static RealTimeMultiplayerManager.RoomInboxUICallback <>f__mg$cache4;
        [CompilerGenerated]
        private static Func<IntPtr, WaitingRoomUIResponse> <>f__mg$cache5;
        [CompilerGenerated]
        private static RealTimeMultiplayerManager.WaitingRoomUICallback <>f__mg$cache6;
        [CompilerGenerated]
        private static Func<IntPtr, FetchInvitationsResponse> <>f__mg$cache7;
        [CompilerGenerated]
        private static RealTimeMultiplayerManager.FetchInvitationsCallback <>f__mg$cache8;
        [CompilerGenerated]
        private static RealTimeMultiplayerManager.LeaveRoomCallback <>f__mg$cache9;
        [CompilerGenerated]
        private static RealTimeMultiplayerManager.RealTimeRoomCallback <>f__mg$cacheA;
        [CompilerGenerated]
        private static RealTimeMultiplayerManager.SendReliableMessageCallback <>f__mg$cacheB;
        [CompilerGenerated]
        private static Func<MultiplayerParticipant, IntPtr> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<IntPtr, RealTimeRoomResponse> <>f__mg$cacheC;

        internal RealtimeManager(GameServices gameServices)
        {
            this.mGameServices = Misc.CheckNotNull<GameServices>(gameServices);
        }

        internal void AcceptInvitation(MultiplayerInvitation invitation, RealTimeEventListenerHelper listener, Action<RealTimeRoomResponse> callback)
        {
            if (<>f__mg$cacheA == null)
            {
                <>f__mg$cacheA = new RealTimeMultiplayerManager.RealTimeRoomCallback(RealtimeManager.InternalRealTimeRoomCallback);
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_AcceptInvitation(this.mGameServices.AsHandle(), invitation.AsPointer(), listener.AsPointer(), <>f__mg$cacheA, ToCallbackPointer(callback));
        }

        internal void CreateRoom(RealtimeRoomConfig config, RealTimeEventListenerHelper helper, Action<RealTimeRoomResponse> callback)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new RealTimeMultiplayerManager.RealTimeRoomCallback(RealtimeManager.InternalRealTimeRoomCallback);
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_CreateRealTimeRoom(this.mGameServices.AsHandle(), config.AsPointer(), helper.AsPointer(), <>f__mg$cache0, ToCallbackPointer(callback));
        }

        internal void DeclineInvitation(MultiplayerInvitation invitation)
        {
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_DeclineInvitation(this.mGameServices.AsHandle(), invitation.AsPointer());
        }

        internal void FetchInvitations(Action<FetchInvitationsResponse> callback)
        {
            if (<>f__mg$cache8 == null)
            {
                <>f__mg$cache8 = new RealTimeMultiplayerManager.FetchInvitationsCallback(RealtimeManager.InternalFetchInvitationsCallback);
            }
            if (<>f__mg$cache7 == null)
            {
                <>f__mg$cache7 = new Func<IntPtr, FetchInvitationsResponse>(FetchInvitationsResponse.FromPointer);
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitations(this.mGameServices.AsHandle(), <>f__mg$cache8, Callbacks.ToIntPtr<FetchInvitationsResponse>(callback, <>f__mg$cache7));
        }

        [MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.FetchInvitationsCallback))]
        internal static void InternalFetchInvitationsCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("RealtimeManager#InternalFetchInvitationsCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.LeaveRoomCallback))]
        internal static void InternalLeaveRoomCallback(CommonErrorStatus.ResponseStatus response, IntPtr data)
        {
            Logger.d("Entering internal callback for InternalLeaveRoomCallback");
            Action<CommonErrorStatus.ResponseStatus> action = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.ResponseStatus>>(data);
            if (action != null)
            {
                try
                {
                    action(response);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing InternalLeaveRoomCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        [MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.PlayerSelectUICallback))]
        internal static void InternalPlayerSelectUIcallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("RealtimeManager#PlayerSelectUICallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.RealTimeRoomCallback))]
        internal static void InternalRealTimeRoomCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("RealtimeManager#InternalRealTimeRoomCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.RoomInboxUICallback))]
        internal static void InternalRoomInboxUICallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("RealtimeManager#InternalRoomInboxUICallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.SendReliableMessageCallback))]
        internal static void InternalSendReliableMessageCallback(CommonErrorStatus.MultiplayerStatus response, IntPtr data)
        {
            Logger.d("Entering internal callback for InternalSendReliableMessageCallback " + response);
            Action<CommonErrorStatus.MultiplayerStatus> action = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.MultiplayerStatus>>(data);
            if (action != null)
            {
                try
                {
                    action(response);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing InternalSendReliableMessageCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        [MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.WaitingRoomUICallback))]
        internal static void InternalWaitingRoomUICallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("RealtimeManager#InternalWaitingRoomUICallback", Callbacks.Type.Temporary, response, data);
        }

        internal void LeaveRoom(NativeRealTimeRoom room, Action<CommonErrorStatus.ResponseStatus> callback)
        {
            if (<>f__mg$cache9 == null)
            {
                <>f__mg$cache9 = new RealTimeMultiplayerManager.LeaveRoomCallback(RealtimeManager.InternalLeaveRoomCallback);
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_LeaveRoom(this.mGameServices.AsHandle(), room.AsPointer(), <>f__mg$cache9, Callbacks.ToIntPtr(callback));
        }

        internal void SendReliableMessage(NativeRealTimeRoom room, MultiplayerParticipant participant, byte[] data, Action<CommonErrorStatus.MultiplayerStatus> callback)
        {
            if (<>f__mg$cacheB == null)
            {
                <>f__mg$cacheB = new RealTimeMultiplayerManager.SendReliableMessageCallback(RealtimeManager.InternalSendReliableMessageCallback);
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendReliableMessage(this.mGameServices.AsHandle(), room.AsPointer(), participant.AsPointer(), data, PInvokeUtilities.ArrayToSizeT<byte>(data), <>f__mg$cacheB, Callbacks.ToIntPtr(callback));
        }

        internal void SendUnreliableMessageToAll(NativeRealTimeRoom room, byte[] data)
        {
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendUnreliableMessageToOthers(this.mGameServices.AsHandle(), room.AsPointer(), data, PInvokeUtilities.ArrayToSizeT<byte>(data));
        }

        internal void SendUnreliableMessageToSpecificParticipants(NativeRealTimeRoom room, List<MultiplayerParticipant> recipients, byte[] data)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = r => r.AsPointer();
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendUnreliableMessage(this.mGameServices.AsHandle(), room.AsPointer(), recipients.Select<MultiplayerParticipant, IntPtr>(<>f__am$cache0).ToArray<IntPtr>(), new UIntPtr((ulong) recipients.LongCount<MultiplayerParticipant>()), data, PInvokeUtilities.ArrayToSizeT<byte>(data));
        }

        internal void ShowPlayerSelectUI(uint minimumPlayers, uint maxiumPlayers, bool allowAutomatching, Action<PlayerSelectUIResponse> callback)
        {
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new RealTimeMultiplayerManager.PlayerSelectUICallback(RealtimeManager.InternalPlayerSelectUIcallback);
            }
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Func<IntPtr, PlayerSelectUIResponse>(PlayerSelectUIResponse.FromPointer);
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowPlayerSelectUI(this.mGameServices.AsHandle(), minimumPlayers, maxiumPlayers, allowAutomatching, <>f__mg$cache2, Callbacks.ToIntPtr<PlayerSelectUIResponse>(callback, <>f__mg$cache1));
        }

        internal void ShowRoomInboxUI(Action<RoomInboxUIResponse> callback)
        {
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new RealTimeMultiplayerManager.RoomInboxUICallback(RealtimeManager.InternalRoomInboxUICallback);
            }
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new Func<IntPtr, RoomInboxUIResponse>(RoomInboxUIResponse.FromPointer);
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowRoomInboxUI(this.mGameServices.AsHandle(), <>f__mg$cache4, Callbacks.ToIntPtr<RoomInboxUIResponse>(callback, <>f__mg$cache3));
        }

        internal void ShowWaitingRoomUI(NativeRealTimeRoom room, uint minimumParticipantsBeforeStarting, Action<WaitingRoomUIResponse> callback)
        {
            Misc.CheckNotNull<NativeRealTimeRoom>(room);
            if (<>f__mg$cache6 == null)
            {
                <>f__mg$cache6 = new RealTimeMultiplayerManager.WaitingRoomUICallback(RealtimeManager.InternalWaitingRoomUICallback);
            }
            if (<>f__mg$cache5 == null)
            {
                <>f__mg$cache5 = new Func<IntPtr, WaitingRoomUIResponse>(WaitingRoomUIResponse.FromPointer);
            }
            RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowWaitingRoomUI(this.mGameServices.AsHandle(), room.AsPointer(), minimumParticipantsBeforeStarting, <>f__mg$cache6, Callbacks.ToIntPtr<WaitingRoomUIResponse>(callback, <>f__mg$cache5));
        }

        private static IntPtr ToCallbackPointer(Action<RealTimeRoomResponse> callback)
        {
            if (<>f__mg$cacheC == null)
            {
                <>f__mg$cacheC = new Func<IntPtr, RealTimeRoomResponse>(RealTimeRoomResponse.FromPointer);
            }
            return Callbacks.ToIntPtr<RealTimeRoomResponse>(callback, <>f__mg$cacheC);
        }

        internal class FetchInvitationsResponse : BaseReferenceHolder
        {
            internal FetchInvitationsResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_Dispose(selfPointer);
            }

            internal static RealtimeManager.FetchInvitationsResponse FromPointer(IntPtr pointer)
            {
                if (PInvokeUtilities.IsNull(pointer))
                {
                    return null;
                }
                return new RealtimeManager.FetchInvitationsResponse(pointer);
            }

            internal IEnumerable<MultiplayerInvitation> Invitations() => 
                PInvokeUtilities.ToEnumerable<MultiplayerInvitation>(RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_Length(base.SelfPtr()), index => new MultiplayerInvitation(RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_GetElement(base.SelfPtr(), index)));

            internal bool RequestSucceeded() => 
                (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus() => 
                RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetStatus(base.SelfPtr());
        }

        internal class RealTimeRoomResponse : BaseReferenceHolder
        {
            internal RealTimeRoomResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_Dispose(selfPointer);
            }

            internal static RealtimeManager.RealTimeRoomResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new RealtimeManager.RealTimeRoomResponse(pointer);
            }

            internal bool RequestSucceeded() => 
                (this.ResponseStatus() > ~(CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE | CommonErrorStatus.MultiplayerStatus.VALID | CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED));

            internal CommonErrorStatus.MultiplayerStatus ResponseStatus() => 
                RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_GetStatus(base.SelfPtr());

            internal NativeRealTimeRoom Room()
            {
                if (!this.RequestSucceeded())
                {
                    return null;
                }
                return new NativeRealTimeRoom(RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_GetRoom(base.SelfPtr()));
            }
        }

        internal class RoomInboxUIResponse : BaseReferenceHolder
        {
            internal RoomInboxUIResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_Dispose(selfPointer);
            }

            internal static RealtimeManager.RoomInboxUIResponse FromPointer(IntPtr pointer)
            {
                if (PInvokeUtilities.IsNull(pointer))
                {
                    return null;
                }
                return new RealtimeManager.RoomInboxUIResponse(pointer);
            }

            internal MultiplayerInvitation Invitation()
            {
                if (this.ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
                {
                    return null;
                }
                return new MultiplayerInvitation(RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_GetInvitation(base.SelfPtr()));
            }

            internal CommonErrorStatus.UIStatus ResponseStatus() => 
                RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_GetStatus(base.SelfPtr());
        }

        internal class WaitingRoomUIResponse : BaseReferenceHolder
        {
            internal WaitingRoomUIResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_Dispose(selfPointer);
            }

            internal static RealtimeManager.WaitingRoomUIResponse FromPointer(IntPtr pointer)
            {
                if (PInvokeUtilities.IsNull(pointer))
                {
                    return null;
                }
                return new RealtimeManager.WaitingRoomUIResponse(pointer);
            }

            internal CommonErrorStatus.UIStatus ResponseStatus() => 
                RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_GetStatus(base.SelfPtr());

            internal NativeRealTimeRoom Room()
            {
                if (this.ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
                {
                    return null;
                }
                return new NativeRealTimeRoom(RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_GetRoom(base.SelfPtr()));
            }
        }
    }
}

