namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class PlayerManager
    {
        private readonly GooglePlayGames.Native.PInvoke.GameServices mGameServices;
        [CompilerGenerated]
        private static Func<IntPtr, FetchSelfResponse> <>f__mg$cache0;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static Func<IntPtr, FetchResponse> <>f__mg$cache2;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback <>f__mg$cache3;
        [CompilerGenerated]
        private static Func<IntPtr, FetchListResponse> <>f__mg$cache4;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback <>f__mg$cache5;

        internal PlayerManager(GooglePlayGames.Native.PInvoke.GameServices services)
        {
            this.mGameServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
        }

        internal void FetchFriends(Action<GooglePlayGames.BasicApi.ResponseStatus, List<Player>> callback)
        {
            <FetchFriends>c__AnonStorey1 storey = new <FetchFriends>c__AnonStorey1 {
                callback = callback,
                $this = this
            };
            if (<>f__mg$cache5 == null)
            {
                <>f__mg$cache5 = new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchConnectedCallback);
            }
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new Func<IntPtr, FetchListResponse>(FetchListResponse.FromPointer);
            }
            GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchConnected(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, <>f__mg$cache5, Callbacks.ToIntPtr<FetchListResponse>(new Action<FetchListResponse>(storey.<>m__0), <>f__mg$cache4));
        }

        internal void FetchList(string[] userIds, Action<NativePlayer[]> callback)
        {
            <FetchList>c__AnonStorey0 storey = new <FetchList>c__AnonStorey0 {
                $this = this,
                coll = new FetchResponseCollector()
            };
            storey.coll.pendingCount = userIds.Length;
            storey.coll.callback = callback;
            foreach (string str in userIds)
            {
                if (<>f__mg$cache3 == null)
                {
                    <>f__mg$cache3 = new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchCallback);
                }
                if (<>f__mg$cache2 == null)
                {
                    <>f__mg$cache2 = new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer);
                }
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_Fetch(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, str, <>f__mg$cache3, Callbacks.ToIntPtr<FetchResponse>(new Action<FetchResponse>(storey.<>m__0), <>f__mg$cache2));
            }
        }

        internal void FetchSelf(Action<FetchSelfResponse> callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchSelfCallback);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<IntPtr, FetchSelfResponse>(FetchSelfResponse.FromPointer);
            }
            GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelf(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, <>f__mg$cache1, Callbacks.ToIntPtr<FetchSelfResponse>(callback, <>f__mg$cache0));
        }

        internal void HandleFetchCollected(FetchListResponse rsp, Action<GooglePlayGames.BasicApi.ResponseStatus, List<Player>> callback)
        {
            List<Player> list = new List<Player>();
            if ((rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) || (rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
            {
                Logger.d("Got " + rsp.Length().ToUInt64() + " players");
                foreach (NativePlayer player in rsp)
                {
                    list.Add(player.AsPlayer());
                }
            }
            callback((GooglePlayGames.BasicApi.ResponseStatus) rsp.Status(), list);
        }

        internal void HandleFetchResponse(FetchResponseCollector collector, FetchResponse resp)
        {
            if ((resp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) || (resp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
            {
                NativePlayer item = resp.GetPlayer();
                collector.results.Add(item);
            }
            collector.pendingCount--;
            if (collector.pendingCount == 0)
            {
                collector.callback(collector.results.ToArray());
            }
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback))]
        private static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("PlayerManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback))]
        private static void InternalFetchConnectedCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("PlayerManager#InternalFetchConnectedCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback))]
        private static void InternalFetchSelfCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("PlayerManager#InternalFetchSelfCallback", Callbacks.Type.Temporary, response, data);
        }

        [CompilerGenerated]
        private sealed class <FetchFriends>c__AnonStorey1
        {
            internal Action<GooglePlayGames.BasicApi.ResponseStatus, List<Player>> callback;
            internal GooglePlayGames.Native.PInvoke.PlayerManager $this;

            internal void <>m__0(GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse rsp)
            {
                this.$this.HandleFetchCollected(rsp, this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <FetchList>c__AnonStorey0
        {
            internal GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponseCollector coll;
            internal GooglePlayGames.Native.PInvoke.PlayerManager $this;

            internal void <>m__0(GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse rsp)
            {
                this.$this.HandleFetchResponse(this.coll, rsp);
            }
        }

        internal class FetchListResponse : BaseReferenceHolder, IEnumerable<NativePlayer>, IEnumerable
        {
            internal FetchListResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_Dispose(base.SelfPtr());
            }

            internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse FromPointer(IntPtr selfPointer)
            {
                if (PInvokeUtilities.IsNull(selfPointer))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse(selfPointer);
            }

            internal NativePlayer GetElement(UIntPtr index)
            {
                if (index.ToUInt64() >= this.Length().ToUInt64())
                {
                    throw new ArgumentOutOfRangeException();
                }
                return new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetData_GetElement(base.SelfPtr(), index));
            }

            public IEnumerator<NativePlayer> GetEnumerator() => 
                PInvokeUtilities.ToEnumerator<NativePlayer>(this.Length(), index => this.GetElement(index));

            internal UIntPtr Length() => 
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetData_Length(base.SelfPtr());

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status() => 
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetStatus(base.SelfPtr());

            IEnumerator IEnumerable.GetEnumerator() => 
                this.GetEnumerator();
        }

        internal class FetchResponse : BaseReferenceHolder
        {
            internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_Dispose(base.SelfPtr());
            }

            internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse FromPointer(IntPtr selfPointer)
            {
                if (PInvokeUtilities.IsNull(selfPointer))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse(selfPointer);
            }

            internal NativePlayer GetPlayer() => 
                new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_GetData(base.SelfPtr()));

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status() => 
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_GetStatus(base.SelfPtr());
        }

        internal class FetchResponseCollector
        {
            internal int pendingCount;
            internal List<NativePlayer> results = new List<NativePlayer>();
            internal Action<NativePlayer[]> callback;
        }

        internal class FetchSelfResponse : BaseReferenceHolder
        {
            internal FetchSelfResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_Dispose(base.SelfPtr());
            }

            internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse FromPointer(IntPtr selfPointer)
            {
                if (PInvokeUtilities.IsNull(selfPointer))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse(selfPointer);
            }

            internal NativePlayer Self() => 
                new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_GetData(base.SelfPtr()));

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status() => 
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_GetStatus(base.SelfPtr());
        }
    }
}

