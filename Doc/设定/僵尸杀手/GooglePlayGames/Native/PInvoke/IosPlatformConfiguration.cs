namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    internal sealed class IosPlatformConfiguration : PlatformConfiguration
    {
        private IosPlatformConfiguration(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.IosPlatformConfiguration.IosPlatformConfiguration_Dispose(selfPointer);
        }

        internal static GooglePlayGames.Native.PInvoke.IosPlatformConfiguration Create() => 
            new GooglePlayGames.Native.PInvoke.IosPlatformConfiguration(GooglePlayGames.Native.Cwrapper.IosPlatformConfiguration.IosPlatformConfiguration_Construct());

        internal void SetClientId(string clientId)
        {
            Misc.CheckNotNull<string>(clientId);
            GooglePlayGames.Native.Cwrapper.IosPlatformConfiguration.IosPlatformConfiguration_SetClientID(base.SelfPtr(), clientId);
        }
    }
}

