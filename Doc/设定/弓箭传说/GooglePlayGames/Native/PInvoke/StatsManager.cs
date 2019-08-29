namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class StatsManager
    {
        private readonly GameServices mServices;
        [CompilerGenerated]
        private static Func<IntPtr, FetchForPlayerResponse> <>f__mg$cache0;
        [CompilerGenerated]
        private static StatsManager.FetchForPlayerCallback <>f__mg$cache1;

        internal StatsManager(GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GameServices>(services);
        }

        internal void FetchForPlayer(Action<FetchForPlayerResponse> callback)
        {
            Misc.CheckNotNull<Action<FetchForPlayerResponse>>(callback);
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new StatsManager.FetchForPlayerCallback(StatsManager.InternalFetchForPlayerCallback);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<IntPtr, FetchForPlayerResponse>(FetchForPlayerResponse.FromPointer);
            }
            StatsManager.StatsManager_FetchForPlayer(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, <>f__mg$cache1, Callbacks.ToIntPtr<FetchForPlayerResponse>(callback, <>f__mg$cache0));
        }

        [MonoPInvokeCallback(typeof(StatsManager.FetchForPlayerCallback))]
        private static void InternalFetchForPlayerCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("StatsManager#InternalFetchForPlayerCallback", Callbacks.Type.Temporary, response, data);
        }

        internal class FetchForPlayerResponse : BaseReferenceHolder
        {
            internal FetchForPlayerResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                StatsManager.StatsManager_FetchForPlayerResponse_Dispose(selfPointer);
            }

            internal static StatsManager.FetchForPlayerResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new StatsManager.FetchForPlayerResponse(pointer);
            }

            internal NativePlayerStats PlayerStats() => 
                new NativePlayerStats(StatsManager.StatsManager_FetchForPlayerResponse_GetData(base.SelfPtr()));

            internal CommonErrorStatus.ResponseStatus Status() => 
                StatsManager.StatsManager_FetchForPlayerResponse_GetStatus(base.SelfPtr());
        }
    }
}

