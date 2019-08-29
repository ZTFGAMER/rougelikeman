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

    internal class AchievementManager
    {
        private readonly GameServices mServices;
        [CompilerGenerated]
        private static AchievementManager.ShowAllUICallback <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<IntPtr, FetchAllResponse> <>f__mg$cache1;
        [CompilerGenerated]
        private static AchievementManager.FetchAllCallback <>f__mg$cache2;
        [CompilerGenerated]
        private static Func<IntPtr, FetchResponse> <>f__mg$cache3;
        [CompilerGenerated]
        private static AchievementManager.FetchCallback <>f__mg$cache4;

        internal AchievementManager(GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GameServices>(services);
        }

        internal void Fetch(string achId, Action<FetchResponse> callback)
        {
            Misc.CheckNotNull<string>(achId);
            Misc.CheckNotNull<Action<FetchResponse>>(callback);
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new AchievementManager.FetchCallback(AchievementManager.InternalFetchCallback);
            }
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer);
            }
            AchievementManager.AchievementManager_Fetch(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, achId, <>f__mg$cache4, Callbacks.ToIntPtr<FetchResponse>(callback, <>f__mg$cache3));
        }

        internal void FetchAll(Action<FetchAllResponse> callback)
        {
            Misc.CheckNotNull<Action<FetchAllResponse>>(callback);
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new AchievementManager.FetchAllCallback(AchievementManager.InternalFetchAllCallback);
            }
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Func<IntPtr, FetchAllResponse>(FetchAllResponse.FromPointer);
            }
            AchievementManager.AchievementManager_FetchAll(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, <>f__mg$cache2, Callbacks.ToIntPtr<FetchAllResponse>(callback, <>f__mg$cache1));
        }

        internal void Increment(string achievementId, uint numSteps)
        {
            Misc.CheckNotNull<string>(achievementId);
            AchievementManager.AchievementManager_Increment(this.mServices.AsHandle(), achievementId, numSteps);
        }

        [MonoPInvokeCallback(typeof(AchievementManager.FetchAllCallback))]
        private static void InternalFetchAllCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("AchievementManager#InternalFetchAllCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(AchievementManager.FetchCallback))]
        private static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("AchievementManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
        }

        internal void Reveal(string achievementId)
        {
            Misc.CheckNotNull<string>(achievementId);
            AchievementManager.AchievementManager_Reveal(this.mServices.AsHandle(), achievementId);
        }

        internal void SetStepsAtLeast(string achivementId, uint numSteps)
        {
            Misc.CheckNotNull<string>(achivementId);
            AchievementManager.AchievementManager_SetStepsAtLeast(this.mServices.AsHandle(), achivementId, numSteps);
        }

        internal void ShowAllUI(Action<CommonErrorStatus.UIStatus> callback)
        {
            Misc.CheckNotNull<Action<CommonErrorStatus.UIStatus>>(callback);
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new AchievementManager.ShowAllUICallback(Callbacks.InternalShowUICallback);
            }
            AchievementManager.AchievementManager_ShowAllUI(this.mServices.AsHandle(), <>f__mg$cache0, Callbacks.ToIntPtr(callback));
        }

        internal void Unlock(string achievementId)
        {
            Misc.CheckNotNull<string>(achievementId);
            AchievementManager.AchievementManager_Unlock(this.mServices.AsHandle(), achievementId);
        }

        internal class FetchAllResponse : BaseReferenceHolder, IEnumerable<NativeAchievement>, IEnumerable
        {
            internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                AchievementManager.AchievementManager_FetchAllResponse_Dispose(selfPointer);
            }

            internal static AchievementManager.FetchAllResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new AchievementManager.FetchAllResponse(pointer);
            }

            private NativeAchievement GetElement(UIntPtr index)
            {
                if (index.ToUInt64() >= this.Length().ToUInt64())
                {
                    throw new ArgumentOutOfRangeException();
                }
                return new NativeAchievement(AchievementManager.AchievementManager_FetchAllResponse_GetData_GetElement(base.SelfPtr(), index));
            }

            public IEnumerator<NativeAchievement> GetEnumerator() => 
                PInvokeUtilities.ToEnumerator<NativeAchievement>(AchievementManager.AchievementManager_FetchAllResponse_GetData_Length(base.SelfPtr()), index => this.GetElement(index));

            private UIntPtr Length() => 
                AchievementManager.AchievementManager_FetchAllResponse_GetData_Length(base.SelfPtr());

            internal CommonErrorStatus.ResponseStatus Status() => 
                AchievementManager.AchievementManager_FetchAllResponse_GetStatus(base.SelfPtr());

            IEnumerator IEnumerable.GetEnumerator() => 
                this.GetEnumerator();
        }

        internal class FetchResponse : BaseReferenceHolder
        {
            internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            internal NativeAchievement Achievement() => 
                new NativeAchievement(AchievementManager.AchievementManager_FetchResponse_GetData(base.SelfPtr()));

            protected override void CallDispose(HandleRef selfPointer)
            {
                AchievementManager.AchievementManager_FetchResponse_Dispose(selfPointer);
            }

            internal static AchievementManager.FetchResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new AchievementManager.FetchResponse(pointer);
            }

            internal CommonErrorStatus.ResponseStatus Status() => 
                AchievementManager.AchievementManager_FetchResponse_GetStatus(base.SelfPtr());
        }
    }
}

