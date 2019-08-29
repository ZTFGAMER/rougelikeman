namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class MessageListenerHelper
    {
        [DllImport("gpg")]
        internal static extern IntPtr MessageListenerHelper_Construct();
        [DllImport("gpg")]
        internal static extern void MessageListenerHelper_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern void MessageListenerHelper_SetOnDisconnectedCallback(HandleRef self, OnDisconnectedCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void MessageListenerHelper_SetOnMessageReceivedCallback(HandleRef self, OnMessageReceivedCallback callback, IntPtr callback_arg);

        internal delegate void OnDisconnectedCallback(long arg0, string arg1, IntPtr arg2);

        internal delegate void OnMessageReceivedCallback(long arg0, string arg1, IntPtr arg2, UIntPtr arg3, [MarshalAs(UnmanagedType.I1)] bool arg4, IntPtr arg5);
    }
}

