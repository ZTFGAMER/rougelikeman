namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class NativeMessageListenerHelper : BaseReferenceHolder
    {
        [CompilerGenerated]
        private static MessageListenerHelper.OnMessageReceivedCallback <>f__mg$cache0;
        [CompilerGenerated]
        private static MessageListenerHelper.OnDisconnectedCallback <>f__mg$cache1;

        internal NativeMessageListenerHelper() : base(MessageListenerHelper.MessageListenerHelper_Construct())
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            MessageListenerHelper.MessageListenerHelper_Dispose(selfPointer);
        }

        [MonoPInvokeCallback(typeof(MessageListenerHelper.OnDisconnectedCallback))]
        private static void InternalOnDisconnectedCallback(long id, string lostEndpointId, IntPtr userData)
        {
            Action<long, string> action = Callbacks.IntPtrToPermanentCallback<Action<long, string>>(userData);
            if (action != null)
            {
                try
                {
                    action(id, lostEndpointId);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnDisconnectedCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        [MonoPInvokeCallback(typeof(MessageListenerHelper.OnMessageReceivedCallback))]
        private static void InternalOnMessageReceivedCallback(long id, string name, IntPtr data, UIntPtr dataLength, bool isReliable, IntPtr userData)
        {
            OnMessageReceived received = Callbacks.IntPtrToPermanentCallback<OnMessageReceived>(userData);
            if (received != null)
            {
                try
                {
                    received(id, name, Callbacks.IntPtrAndSizeToByteArray(data, dataLength), isReliable);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnMessageReceivedCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        internal void SetOnDisconnectedCallback(Action<long, string> callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new MessageListenerHelper.OnDisconnectedCallback(NativeMessageListenerHelper.InternalOnDisconnectedCallback);
            }
            MessageListenerHelper.MessageListenerHelper_SetOnDisconnectedCallback(base.SelfPtr(), <>f__mg$cache1, Callbacks.ToIntPtr(callback));
        }

        internal void SetOnMessageReceivedCallback(OnMessageReceived callback)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new MessageListenerHelper.OnMessageReceivedCallback(NativeMessageListenerHelper.InternalOnMessageReceivedCallback);
            }
            MessageListenerHelper.MessageListenerHelper_SetOnMessageReceivedCallback(base.SelfPtr(), <>f__mg$cache0, Callbacks.ToIntPtr(callback));
        }

        internal delegate void OnMessageReceived(long localClientId, string remoteEndpointId, byte[] data, bool isReliable);
    }
}

