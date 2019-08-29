namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class SnapshotManager
    {
        private readonly GameServices mServices;
        [CompilerGenerated]
        private static Func<IntPtr, FetchAllResponse> <>f__mg$cache0;
        [CompilerGenerated]
        private static SnapshotManager.FetchAllCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static Func<IntPtr, SnapshotSelectUIResponse> <>f__mg$cache2;
        [CompilerGenerated]
        private static SnapshotManager.SnapshotSelectUICallback <>f__mg$cache3;
        [CompilerGenerated]
        private static Func<IntPtr, OpenResponse> <>f__mg$cache4;
        [CompilerGenerated]
        private static SnapshotManager.OpenCallback <>f__mg$cache5;
        [CompilerGenerated]
        private static Func<IntPtr, CommitResponse> <>f__mg$cache6;
        [CompilerGenerated]
        private static SnapshotManager.CommitCallback <>f__mg$cache7;
        [CompilerGenerated]
        private static Func<IntPtr, OpenResponse> <>f__mg$cache8;
        [CompilerGenerated]
        private static SnapshotManager.OpenCallback <>f__mg$cache9;
        [CompilerGenerated]
        private static Func<IntPtr, OpenResponse> <>f__mg$cacheA;
        [CompilerGenerated]
        private static SnapshotManager.OpenCallback <>f__mg$cacheB;
        [CompilerGenerated]
        private static Func<IntPtr, ReadResponse> <>f__mg$cacheC;
        [CompilerGenerated]
        private static SnapshotManager.ReadCallback <>f__mg$cacheD;

        internal SnapshotManager(GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GameServices>(services);
        }

        internal void Commit(NativeSnapshotMetadata metadata, NativeSnapshotMetadataChange metadataChange, byte[] updatedData, Action<CommitResponse> callback)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            Misc.CheckNotNull<NativeSnapshotMetadataChange>(metadataChange);
            if (<>f__mg$cache7 == null)
            {
                <>f__mg$cache7 = new SnapshotManager.CommitCallback(SnapshotManager.InternalCommitCallback);
            }
            if (<>f__mg$cache6 == null)
            {
                <>f__mg$cache6 = new Func<IntPtr, CommitResponse>(CommitResponse.FromPointer);
            }
            SnapshotManager.SnapshotManager_Commit(this.mServices.AsHandle(), metadata.AsPointer(), metadataChange.AsPointer(), updatedData, new UIntPtr((ulong) updatedData.Length), <>f__mg$cache7, Callbacks.ToIntPtr<CommitResponse>(callback, <>f__mg$cache6));
        }

        internal void Delete(NativeSnapshotMetadata metadata)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            SnapshotManager.SnapshotManager_Delete(this.mServices.AsHandle(), metadata.AsPointer());
        }

        internal void FetchAll(Types.DataSource source, Action<FetchAllResponse> callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new SnapshotManager.FetchAllCallback(SnapshotManager.InternalFetchAllCallback);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<IntPtr, FetchAllResponse>(FetchAllResponse.FromPointer);
            }
            SnapshotManager.SnapshotManager_FetchAll(this.mServices.AsHandle(), source, <>f__mg$cache1, Callbacks.ToIntPtr<FetchAllResponse>(callback, <>f__mg$cache0));
        }

        [MonoPInvokeCallback(typeof(SnapshotManager.CommitCallback))]
        internal static void InternalCommitCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#CommitCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(SnapshotManager.FetchAllCallback))]
        internal static void InternalFetchAllCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#FetchAllCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(SnapshotManager.OpenCallback))]
        internal static void InternalOpenCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#OpenCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(SnapshotManager.ReadCallback))]
        internal static void InternalReadCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#ReadCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(SnapshotManager.SnapshotSelectUICallback))]
        internal static void InternalSnapshotSelectUICallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#SnapshotSelectUICallback", Callbacks.Type.Temporary, response, data);
        }

        internal void Open(string fileName, Types.DataSource source, Types.SnapshotConflictPolicy conflictPolicy, Action<OpenResponse> callback)
        {
            Misc.CheckNotNull<string>(fileName);
            Misc.CheckNotNull<Action<OpenResponse>>(callback);
            if (<>f__mg$cache5 == null)
            {
                <>f__mg$cache5 = new SnapshotManager.OpenCallback(SnapshotManager.InternalOpenCallback);
            }
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new Func<IntPtr, OpenResponse>(OpenResponse.FromPointer);
            }
            SnapshotManager.SnapshotManager_Open(this.mServices.AsHandle(), source, fileName, conflictPolicy, <>f__mg$cache5, Callbacks.ToIntPtr<OpenResponse>(callback, <>f__mg$cache4));
        }

        internal void Read(NativeSnapshotMetadata metadata, Action<ReadResponse> callback)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            Misc.CheckNotNull<Action<ReadResponse>>(callback);
            if (<>f__mg$cacheD == null)
            {
                <>f__mg$cacheD = new SnapshotManager.ReadCallback(SnapshotManager.InternalReadCallback);
            }
            if (<>f__mg$cacheC == null)
            {
                <>f__mg$cacheC = new Func<IntPtr, ReadResponse>(ReadResponse.FromPointer);
            }
            SnapshotManager.SnapshotManager_Read(this.mServices.AsHandle(), metadata.AsPointer(), <>f__mg$cacheD, Callbacks.ToIntPtr<ReadResponse>(callback, <>f__mg$cacheC));
        }

        internal void Resolve(NativeSnapshotMetadata metadata, NativeSnapshotMetadataChange metadataChange, string conflictId, Action<OpenResponse> callback)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            Misc.CheckNotNull<NativeSnapshotMetadataChange>(metadataChange);
            Misc.CheckNotNull<string>(conflictId);
            if (<>f__mg$cache9 == null)
            {
                <>f__mg$cache9 = new SnapshotManager.OpenCallback(SnapshotManager.InternalOpenCallback);
            }
            if (<>f__mg$cache8 == null)
            {
                <>f__mg$cache8 = new Func<IntPtr, OpenResponse>(OpenResponse.FromPointer);
            }
            SnapshotManager.SnapshotManager_ResolveConflict(this.mServices.AsHandle(), conflictId, metadata.AsPointer(), metadataChange.AsPointer(), <>f__mg$cache9, Callbacks.ToIntPtr<OpenResponse>(callback, <>f__mg$cache8));
        }

        internal void Resolve(NativeSnapshotMetadata metadata, NativeSnapshotMetadataChange metadataChange, string conflictId, byte[] updatedData, Action<OpenResponse> callback)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            Misc.CheckNotNull<NativeSnapshotMetadataChange>(metadataChange);
            Misc.CheckNotNull<string>(conflictId);
            Misc.CheckNotNull<byte[]>(updatedData);
            if (<>f__mg$cacheB == null)
            {
                <>f__mg$cacheB = new SnapshotManager.OpenCallback(SnapshotManager.InternalOpenCallback);
            }
            if (<>f__mg$cacheA == null)
            {
                <>f__mg$cacheA = new Func<IntPtr, OpenResponse>(OpenResponse.FromPointer);
            }
            SnapshotManager.SnapshotManager_ResolveConflict(this.mServices.AsHandle(), conflictId, metadata.AsPointer(), metadataChange.AsPointer(), updatedData, new UIntPtr((ulong) updatedData.Length), <>f__mg$cacheB, Callbacks.ToIntPtr<OpenResponse>(callback, <>f__mg$cacheA));
        }

        internal void SnapshotSelectUI(bool allowCreate, bool allowDelete, uint maxSnapshots, string uiTitle, Action<SnapshotSelectUIResponse> callback)
        {
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new SnapshotManager.SnapshotSelectUICallback(SnapshotManager.InternalSnapshotSelectUICallback);
            }
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new Func<IntPtr, SnapshotSelectUIResponse>(SnapshotSelectUIResponse.FromPointer);
            }
            SnapshotManager.SnapshotManager_ShowSelectUIOperation(this.mServices.AsHandle(), allowCreate, allowDelete, maxSnapshots, uiTitle, <>f__mg$cache3, Callbacks.ToIntPtr<SnapshotSelectUIResponse>(callback, <>f__mg$cache2));
        }

        internal class CommitResponse : BaseReferenceHolder
        {
            internal CommitResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                SnapshotManager.SnapshotManager_CommitResponse_Dispose(selfPointer);
            }

            internal NativeSnapshotMetadata Data()
            {
                if (!this.RequestSucceeded())
                {
                    throw new InvalidOperationException("Request did not succeed");
                }
                return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_CommitResponse_GetData(base.SelfPtr()));
            }

            internal static SnapshotManager.CommitResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new SnapshotManager.CommitResponse(pointer);
            }

            internal bool RequestSucceeded() => 
                (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus() => 
                SnapshotManager.SnapshotManager_CommitResponse_GetStatus(base.SelfPtr());
        }

        internal class FetchAllResponse : BaseReferenceHolder
        {
            internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                SnapshotManager.SnapshotManager_FetchAllResponse_Dispose(selfPointer);
            }

            internal IEnumerable<NativeSnapshotMetadata> Data() => 
                PInvokeUtilities.ToEnumerable<NativeSnapshotMetadata>(SnapshotManager.SnapshotManager_FetchAllResponse_GetData_Length(base.SelfPtr()), index => new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_FetchAllResponse_GetData_GetElement(base.SelfPtr(), index)));

            internal static SnapshotManager.FetchAllResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new SnapshotManager.FetchAllResponse(pointer);
            }

            internal bool RequestSucceeded() => 
                (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus() => 
                SnapshotManager.SnapshotManager_FetchAllResponse_GetStatus(base.SelfPtr());
        }

        internal class OpenResponse : BaseReferenceHolder
        {
            internal OpenResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                SnapshotManager.SnapshotManager_OpenResponse_Dispose(selfPointer);
            }

            internal string ConflictId()
            {
                if (this.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
                {
                    throw new InvalidOperationException("OpenResponse did not have a conflict");
                }
                return PInvokeUtilities.OutParamsToString((out_string, out_size) => SnapshotManager.SnapshotManager_OpenResponse_GetConflictId(base.SelfPtr(), out_string, out_size));
            }

            internal NativeSnapshotMetadata ConflictOriginal()
            {
                if (this.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
                {
                    throw new InvalidOperationException("OpenResponse did not have a conflict");
                }
                return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_OpenResponse_GetConflictOriginal(base.SelfPtr()));
            }

            internal NativeSnapshotMetadata ConflictUnmerged()
            {
                if (this.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
                {
                    throw new InvalidOperationException("OpenResponse did not have a conflict");
                }
                return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_OpenResponse_GetConflictUnmerged(base.SelfPtr()));
            }

            internal NativeSnapshotMetadata Data()
            {
                if (this.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID)
                {
                    throw new InvalidOperationException("OpenResponse had a conflict");
                }
                return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_OpenResponse_GetData(base.SelfPtr()));
            }

            internal static SnapshotManager.OpenResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new SnapshotManager.OpenResponse(pointer);
            }

            internal bool RequestSucceeded() => 
                (this.ResponseStatus() > ((CommonErrorStatus.SnapshotOpenStatus) 0));

            internal CommonErrorStatus.SnapshotOpenStatus ResponseStatus() => 
                SnapshotManager.SnapshotManager_OpenResponse_GetStatus(base.SelfPtr());
        }

        internal class ReadResponse : BaseReferenceHolder
        {
            internal ReadResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                SnapshotManager.SnapshotManager_ReadResponse_Dispose(selfPointer);
            }

            internal byte[] Data()
            {
                if (!this.RequestSucceeded())
                {
                    throw new InvalidOperationException("Request did not succeed");
                }
                return PInvokeUtilities.OutParamsToArray<byte>((out_bytes, out_size) => SnapshotManager.SnapshotManager_ReadResponse_GetData(base.SelfPtr(), out_bytes, out_size));
            }

            internal static SnapshotManager.ReadResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new SnapshotManager.ReadResponse(pointer);
            }

            internal bool RequestSucceeded() => 
                (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus() => 
                SnapshotManager.SnapshotManager_CommitResponse_GetStatus(base.SelfPtr());
        }

        internal class SnapshotSelectUIResponse : BaseReferenceHolder
        {
            internal SnapshotSelectUIResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_Dispose(selfPointer);
            }

            internal NativeSnapshotMetadata Data()
            {
                if (!this.RequestSucceeded())
                {
                    throw new InvalidOperationException("Request did not succeed");
                }
                return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_GetData(base.SelfPtr()));
            }

            internal static SnapshotManager.SnapshotSelectUIResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new SnapshotManager.SnapshotSelectUIResponse(pointer);
            }

            internal CommonErrorStatus.UIStatus RequestStatus() => 
                SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_GetStatus(base.SelfPtr());

            internal bool RequestSucceeded() => 
                (this.RequestStatus() > ~(CommonErrorStatus.UIStatus.VALID | CommonErrorStatus.UIStatus.ERROR_INTERNAL));
        }
    }
}

