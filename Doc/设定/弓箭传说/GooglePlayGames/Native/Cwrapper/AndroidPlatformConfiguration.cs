namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class AndroidPlatformConfiguration
    {
        [DllImport("gpg")]
        internal static extern IntPtr AndroidPlatformConfiguration_Construct();
        [DllImport("gpg")]
        internal static extern void AndroidPlatformConfiguration_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern void AndroidPlatformConfiguration_SetActivity(HandleRef self, IntPtr android_app_activity);
        [DllImport("gpg")]
        internal static extern void AndroidPlatformConfiguration_SetOnLaunchedWithQuest(HandleRef self, OnLaunchedWithQuestCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void AndroidPlatformConfiguration_SetOnLaunchedWithSnapshot(HandleRef self, OnLaunchedWithSnapshotCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(HandleRef self, IntentHandler intent_handler, IntPtr intent_handler_arg);
        [DllImport("gpg")]
        internal static extern void AndroidPlatformConfiguration_SetOptionalViewForPopups(HandleRef self, IntPtr android_view);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool AndroidPlatformConfiguration_Valid(HandleRef self);

        internal delegate void IntentHandler(IntPtr arg0, IntPtr arg1);

        internal delegate void OnLaunchedWithQuestCallback(IntPtr arg0, IntPtr arg1);

        internal delegate void OnLaunchedWithSnapshotCallback(IntPtr arg0, IntPtr arg1);
    }
}

