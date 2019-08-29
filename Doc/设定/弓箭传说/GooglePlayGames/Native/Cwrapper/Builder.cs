namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class Builder
    {
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_AddOauthScope(HandleRef self, string scope);
        [DllImport("gpg")]
        internal static extern IntPtr GameServices_Builder_Construct();
        [DllImport("gpg")]
        internal static extern IntPtr GameServices_Builder_Create(HandleRef self, IntPtr platform);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_EnableSnapshots(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetDefaultOnLog(HandleRef self, Types.LogLevel min_level);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetLogging(HandleRef self, OnLogCallback callback, IntPtr callback_arg, Types.LogLevel min_level);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetOnAuthActionFinished(HandleRef self, OnAuthActionFinishedCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetOnAuthActionStarted(HandleRef self, OnAuthActionStartedCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetOnLog(HandleRef self, OnLogCallback callback, IntPtr callback_arg, Types.LogLevel min_level);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetOnMultiplayerInvitationEvent(HandleRef self, OnMultiplayerInvitationEventCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetOnQuestCompleted(HandleRef self, OnQuestCompletedCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetOnTurnBasedMatchEvent(HandleRef self, OnTurnBasedMatchEventCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void GameServices_Builder_SetShowConnectingPopup(HandleRef self, bool flag);

        internal delegate void OnAuthActionFinishedCallback(Types.AuthOperation arg0, CommonErrorStatus.AuthStatus arg1, IntPtr arg2);

        internal delegate void OnAuthActionStartedCallback(Types.AuthOperation arg0, IntPtr arg1);

        internal delegate void OnLogCallback(Types.LogLevel arg0, string arg1, IntPtr arg2);

        internal delegate void OnMultiplayerInvitationEventCallback(Types.MultiplayerEvent arg0, string arg1, IntPtr arg2, IntPtr arg3);

        internal delegate void OnQuestCompletedCallback(IntPtr arg0, IntPtr arg1);

        internal delegate void OnTurnBasedMatchEventCallback(Types.MultiplayerEvent arg0, string arg1, IntPtr arg2, IntPtr arg3);
    }
}

