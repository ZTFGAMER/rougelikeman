namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal sealed class AndroidPlatformConfiguration : PlatformConfiguration
    {
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.IntentHandler <>f__mg$cache0;

        private AndroidPlatformConfiguration(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_Dispose(selfPointer);
        }

        internal static GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration Create() => 
            new GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration(GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_Construct());

        [MonoPInvokeCallback(typeof(IntentHandlerInternal))]
        private static void InternalIntentHandler(IntPtr intent, IntPtr userData)
        {
            Callbacks.PerformInternalCallback("AndroidPlatformConfiguration#InternalIntentHandler", Callbacks.Type.Permanent, intent, userData);
        }

        internal void SetActivity(IntPtr activity)
        {
            GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetActivity(base.SelfPtr(), activity);
        }

        internal void SetOptionalIntentHandlerForUI(Action<IntPtr> intentHandler)
        {
            Misc.CheckNotNull<Action<IntPtr>>(intentHandler);
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.IntentHandler(GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration.InternalIntentHandler);
            }
            GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(base.SelfPtr(), <>f__mg$cache0, Callbacks.ToIntPtr(intentHandler));
        }

        internal void SetOptionalViewForPopups(IntPtr view)
        {
            GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalViewForPopups(base.SelfPtr(), view);
        }

        private delegate void IntentHandlerInternal(IntPtr intent, IntPtr userData);
    }
}

