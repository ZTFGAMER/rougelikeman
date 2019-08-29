namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Sentinels
    {
        [DllImport("gpg")]
        internal static extern IntPtr Sentinels_AutomatchingParticipant();
    }
}

