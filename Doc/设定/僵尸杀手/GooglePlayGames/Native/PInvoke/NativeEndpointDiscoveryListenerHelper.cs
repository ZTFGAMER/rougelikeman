namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class NativeEndpointDiscoveryListenerHelper : BaseReferenceHolder
    {
        [CompilerGenerated]
        private static Func<IntPtr, NativeEndpointDetails> <>f__mg$cache0;
        [CompilerGenerated]
        private static EndpointDiscoveryListenerHelper.OnEndpointFoundCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static EndpointDiscoveryListenerHelper.OnEndpointLostCallback <>f__mg$cache2;

        internal NativeEndpointDiscoveryListenerHelper() : base(EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_Construct())
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_Dispose(selfPointer);
        }

        [MonoPInvokeCallback(typeof(EndpointDiscoveryListenerHelper.OnEndpointFoundCallback))]
        private static void InternalOnEndpointFoundCallback(long id, IntPtr data, IntPtr userData)
        {
            Callbacks.PerformInternalCallback<long>("NativeEndpointDiscoveryListenerHelper#InternalOnEndpointFoundCallback", Callbacks.Type.Permanent, id, data, userData);
        }

        [MonoPInvokeCallback(typeof(EndpointDiscoveryListenerHelper.OnEndpointLostCallback))]
        private static void InternalOnEndpointLostCallback(long id, string lostEndpointId, IntPtr userData)
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
                    Logger.e("Error encountered executing NativeEndpointDiscoveryListenerHelper#InternalOnEndpointLostCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        internal void SetOnEndpointFound(Action<long, NativeEndpointDetails> callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new EndpointDiscoveryListenerHelper.OnEndpointFoundCallback(NativeEndpointDiscoveryListenerHelper.InternalOnEndpointFoundCallback);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<IntPtr, NativeEndpointDetails>(NativeEndpointDetails.FromPointer);
            }
            EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_SetOnEndpointFoundCallback(base.SelfPtr(), <>f__mg$cache1, Callbacks.ToIntPtr<long, NativeEndpointDetails>(callback, <>f__mg$cache0));
        }

        internal void SetOnEndpointLostCallback(Action<long, string> callback)
        {
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new EndpointDiscoveryListenerHelper.OnEndpointLostCallback(NativeEndpointDiscoveryListenerHelper.InternalOnEndpointLostCallback);
            }
            EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_SetOnEndpointLostCallback(base.SelfPtr(), <>f__mg$cache2, Callbacks.ToIntPtr(callback));
        }
    }
}

