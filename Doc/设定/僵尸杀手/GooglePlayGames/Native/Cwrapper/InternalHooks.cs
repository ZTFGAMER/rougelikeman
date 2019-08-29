namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class InternalHooks
    {
        [DllImport("gpg")]
        internal static extern void InternalHooks_ConfigureForUnityPlugin(HandleRef builder, string unity_version);
        [DllImport("gpg")]
        internal static extern IntPtr InternalHooks_GetApiClient(HandleRef services);
    }
}

