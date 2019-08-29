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
        private static AndroidPlatformConfiguration.IntentHandler <>f__mg$cache0;

        private AndroidPlatformConfiguration(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            AndroidPlatformConfiguration.AndroidPlatformConfiguration_Dispose(selfPointer);
        }

        internal static AndroidPlatformConfiguration Create() => 
            new AndroidPlatformConfiguration(AndroidPlatformConfiguration.AndroidPlatformConfiguration_Construct());

        [MonoPInvokeCallback(typeof(IntentHandlerInternal))]
        private static void InternalIntentHandler(IntPtr intent, IntPtr userData)
        {
            Callbacks.PerformInternalCallback("AndroidPlatformConfiguration#InternalIntentHandler", Callbacks.Type.Permanent, intent, userData);
        }

        internal void SetActivity(IntPtr activity)
        {
            AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetActivity(base.SelfPtr(), activity);
        }

        internal void SetOptionalIntentHandlerForUI(Action<IntPtr> intentHandler)
        {
            Misc.CheckNotNull<Action<IntPtr>>(intentHandler);
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new AndroidPlatformConfiguration.IntentHandler(AndroidPlatformConfiguration.InternalIntentHandler);
            }
            AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(base.SelfPtr(), <>f__mg$cache0, Callbacks.ToIntPtr(intentHandler));
        }

        internal void SetOptionalViewForPopups(IntPtr view)
        {
            AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalViewForPopups(base.SelfPtr(), view);
        }

        private delegate void IntentHandlerInternal(IntPtr intent, IntPtr userData);
    }
}

