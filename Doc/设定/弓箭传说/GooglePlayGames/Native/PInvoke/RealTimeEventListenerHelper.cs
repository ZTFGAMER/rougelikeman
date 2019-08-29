namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class RealTimeEventListenerHelper : BaseReferenceHolder
    {
        [CompilerGenerated]
        private static RealTimeEventListenerHelper.OnRoomStatusChangedCallback <>f__mg$cache0;
        [CompilerGenerated]
        private static RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static RealTimeEventListenerHelper.OnP2PConnectedCallback <>f__mg$cache2;
        [CompilerGenerated]
        private static RealTimeEventListenerHelper.OnP2PDisconnectedCallback <>f__mg$cache3;
        [CompilerGenerated]
        private static RealTimeEventListenerHelper.OnParticipantStatusChangedCallback <>f__mg$cache4;
        [CompilerGenerated]
        private static RealTimeEventListenerHelper.OnDataReceivedCallback <>f__mg$cache5;

        internal RealTimeEventListenerHelper(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            RealTimeEventListenerHelper.RealTimeEventListenerHelper_Dispose(selfPointer);
        }

        internal static RealTimeEventListenerHelper Create() => 
            new RealTimeEventListenerHelper(RealTimeEventListenerHelper.RealTimeEventListenerHelper_Construct());

        [MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnDataReceivedCallback))]
        internal static void InternalOnDataReceived(IntPtr room, IntPtr participant, IntPtr data, UIntPtr dataLength, bool isReliable, IntPtr userData)
        {
            Logger.d("Entering InternalOnDataReceived: " + userData.ToInt64());
            Action<NativeRealTimeRoom, MultiplayerParticipant, byte[], bool> action = Callbacks.IntPtrToPermanentCallback<Action<NativeRealTimeRoom, MultiplayerParticipant, byte[], bool>>(userData);
            using (NativeRealTimeRoom room2 = NativeRealTimeRoom.FromPointer(room))
            {
                using (MultiplayerParticipant participant2 = MultiplayerParticipant.FromPointer(participant))
                {
                    if (action != null)
                    {
                        byte[] destination = null;
                        if (dataLength.ToUInt64() != 0L)
                        {
                            destination = new byte[dataLength.ToUInt32()];
                            Marshal.Copy(data, destination, 0, (int) dataLength.ToUInt32());
                        }
                        try
                        {
                            action(room2, participant2, destination, isReliable);
                        }
                        catch (Exception exception)
                        {
                            Logger.e("Error encountered executing InternalOnDataReceived. Smothering to avoid passing exception into Native: " + exception);
                        }
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnP2PConnectedCallback))]
        internal static void InternalOnP2PConnectedCallback(IntPtr room, IntPtr participant, IntPtr data)
        {
            PerformRoomAndParticipantCallback("InternalOnP2PConnectedCallback", room, participant, data);
        }

        [MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnP2PDisconnectedCallback))]
        internal static void InternalOnP2PDisconnectedCallback(IntPtr room, IntPtr participant, IntPtr data)
        {
            PerformRoomAndParticipantCallback("InternalOnP2PDisconnectedCallback", room, participant, data);
        }

        [MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnParticipantStatusChangedCallback))]
        internal static void InternalOnParticipantStatusChangedCallback(IntPtr room, IntPtr participant, IntPtr data)
        {
            PerformRoomAndParticipantCallback("InternalOnParticipantStatusChangedCallback", room, participant, data);
        }

        [MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback))]
        internal static void InternalOnRoomConnectedSetChangedCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("RealTimeEventListenerHelper#InternalOnRoomConnectedSetChangedCallback", Callbacks.Type.Permanent, response, data);
        }

        [MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnRoomStatusChangedCallback))]
        internal static void InternalOnRoomStatusChangedCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("RealTimeEventListenerHelper#InternalOnRoomStatusChangedCallback", Callbacks.Type.Permanent, response, data);
        }

        internal static void PerformRoomAndParticipantCallback(string callbackName, IntPtr room, IntPtr participant, IntPtr data)
        {
            Logger.d("Entering " + callbackName);
            try
            {
                NativeRealTimeRoom room2 = NativeRealTimeRoom.FromPointer(room);
                using (MultiplayerParticipant participant2 = MultiplayerParticipant.FromPointer(participant))
                {
                    Action<NativeRealTimeRoom, MultiplayerParticipant> action = Callbacks.IntPtrToPermanentCallback<Action<NativeRealTimeRoom, MultiplayerParticipant>>(data);
                    if (action != null)
                    {
                        action(room2, participant2);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.e(string.Concat(new object[] { "Error encountered executing ", callbackName, ". Smothering to avoid passing exception into Native: ", exception }));
            }
        }

        internal RealTimeEventListenerHelper SetOnDataReceivedCallback(Action<NativeRealTimeRoom, MultiplayerParticipant, byte[], bool> callback)
        {
            IntPtr ptr = Callbacks.ToIntPtr(callback);
            Logger.d("OnData Callback has addr: " + ptr.ToInt64());
            if (<>f__mg$cache5 == null)
            {
                <>f__mg$cache5 = new RealTimeEventListenerHelper.OnDataReceivedCallback(RealTimeEventListenerHelper.InternalOnDataReceived);
            }
            RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnDataReceivedCallback(base.SelfPtr(), <>f__mg$cache5, ptr);
            return this;
        }

        internal RealTimeEventListenerHelper SetOnP2PConnectedCallback(Action<NativeRealTimeRoom, MultiplayerParticipant> callback)
        {
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new RealTimeEventListenerHelper.OnP2PConnectedCallback(RealTimeEventListenerHelper.InternalOnP2PConnectedCallback);
            }
            RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnP2PConnectedCallback(base.SelfPtr(), <>f__mg$cache2, Callbacks.ToIntPtr(callback));
            return this;
        }

        internal RealTimeEventListenerHelper SetOnP2PDisconnectedCallback(Action<NativeRealTimeRoom, MultiplayerParticipant> callback)
        {
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new RealTimeEventListenerHelper.OnP2PDisconnectedCallback(RealTimeEventListenerHelper.InternalOnP2PDisconnectedCallback);
            }
            RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnP2PDisconnectedCallback(base.SelfPtr(), <>f__mg$cache3, Callbacks.ToIntPtr(callback));
            return this;
        }

        internal RealTimeEventListenerHelper SetOnParticipantStatusChangedCallback(Action<NativeRealTimeRoom, MultiplayerParticipant> callback)
        {
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new RealTimeEventListenerHelper.OnParticipantStatusChangedCallback(RealTimeEventListenerHelper.InternalOnParticipantStatusChangedCallback);
            }
            RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnParticipantStatusChangedCallback(base.SelfPtr(), <>f__mg$cache4, Callbacks.ToIntPtr(callback));
            return this;
        }

        internal RealTimeEventListenerHelper SetOnRoomConnectedSetChangedCallback(Action<NativeRealTimeRoom> callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback(RealTimeEventListenerHelper.InternalOnRoomConnectedSetChangedCallback);
            }
            RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnRoomConnectedSetChangedCallback(base.SelfPtr(), <>f__mg$cache1, ToCallbackPointer(callback));
            return this;
        }

        internal RealTimeEventListenerHelper SetOnRoomStatusChangedCallback(Action<NativeRealTimeRoom> callback)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new RealTimeEventListenerHelper.OnRoomStatusChangedCallback(RealTimeEventListenerHelper.InternalOnRoomStatusChangedCallback);
            }
            RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnRoomStatusChangedCallback(base.SelfPtr(), <>f__mg$cache0, ToCallbackPointer(callback));
            return this;
        }

        private static IntPtr ToCallbackPointer(Action<NativeRealTimeRoom> callback)
        {
            <ToCallbackPointer>c__AnonStorey0 storey = new <ToCallbackPointer>c__AnonStorey0 {
                callback = callback
            };
            Action<IntPtr> action = new Action<IntPtr>(storey.<>m__0);
            return Callbacks.ToIntPtr(action);
        }

        [CompilerGenerated]
        private sealed class <ToCallbackPointer>c__AnonStorey0
        {
            internal Action<NativeRealTimeRoom> callback;

            internal void <>m__0(IntPtr result)
            {
                NativeRealTimeRoom room = NativeRealTimeRoom.FromPointer(result);
                if (this.callback != null)
                {
                    this.callback(room);
                }
                else if (room != null)
                {
                    room.Dispose();
                }
            }
        }
    }
}

