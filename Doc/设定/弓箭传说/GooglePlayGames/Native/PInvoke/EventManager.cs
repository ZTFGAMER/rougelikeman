namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class EventManager
    {
        private readonly GameServices mServices;
        [CompilerGenerated]
        private static Func<IntPtr, FetchAllResponse> <>f__mg$cache0;
        [CompilerGenerated]
        private static EventManager.FetchAllCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static Func<IntPtr, FetchResponse> <>f__mg$cache2;
        [CompilerGenerated]
        private static EventManager.FetchCallback <>f__mg$cache3;

        internal EventManager(GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GameServices>(services);
        }

        internal void Fetch(Types.DataSource source, string eventId, Action<FetchResponse> callback)
        {
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new EventManager.FetchCallback(EventManager.InternalFetchCallback);
            }
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer);
            }
            EventManager.EventManager_Fetch(this.mServices.AsHandle(), source, eventId, <>f__mg$cache3, Callbacks.ToIntPtr<FetchResponse>(callback, <>f__mg$cache2));
        }

        internal void FetchAll(Types.DataSource source, Action<FetchAllResponse> callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new EventManager.FetchAllCallback(EventManager.InternalFetchAllCallback);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<IntPtr, FetchAllResponse>(FetchAllResponse.FromPointer);
            }
            EventManager.EventManager_FetchAll(this.mServices.AsHandle(), source, <>f__mg$cache1, Callbacks.ToIntPtr<FetchAllResponse>(callback, <>f__mg$cache0));
        }

        internal void Increment(string eventId, uint steps)
        {
            EventManager.EventManager_Increment(this.mServices.AsHandle(), eventId, steps);
        }

        [MonoPInvokeCallback(typeof(EventManager.FetchAllCallback))]
        internal static void InternalFetchAllCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("EventManager#FetchAllCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(EventManager.FetchCallback))]
        internal static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("EventManager#FetchCallback", Callbacks.Type.Temporary, response, data);
        }

        internal class FetchAllResponse : BaseReferenceHolder
        {
            [CompilerGenerated]
            private static Func<IntPtr, NativeEvent> <>f__am$cache0;

            internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                EventManager.EventManager_FetchAllResponse_Dispose(selfPointer);
            }

            internal List<NativeEvent> Data()
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = ptr => new NativeEvent(ptr);
                }
                return PInvokeUtilities.OutParamsToArray<IntPtr>((out_arg, out_size) => EventManager.EventManager_FetchAllResponse_GetData(base.SelfPtr(), out_arg, out_size)).Select<IntPtr, NativeEvent>(<>f__am$cache0).ToList<NativeEvent>();
            }

            internal static EventManager.FetchAllResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new EventManager.FetchAllResponse(pointer);
            }

            internal bool RequestSucceeded() => 
                (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus() => 
                EventManager.EventManager_FetchAllResponse_GetStatus(base.SelfPtr());
        }

        internal class FetchResponse : BaseReferenceHolder
        {
            internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                EventManager.EventManager_FetchResponse_Dispose(selfPointer);
            }

            internal NativeEvent Data()
            {
                if (!this.RequestSucceeded())
                {
                    return null;
                }
                return new NativeEvent(EventManager.EventManager_FetchResponse_GetData(base.SelfPtr()));
            }

            internal static EventManager.FetchResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new EventManager.FetchResponse(pointer);
            }

            internal bool RequestSucceeded() => 
                (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus() => 
                EventManager.EventManager_FetchResponse_GetStatus(base.SelfPtr());
        }
    }
}

