namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    internal class NativeSavedGameClient : ISavedGameClient
    {
        private static readonly Regex ValidFilenameRegex = new Regex(@"\A[a-zA-Z0-9-._~]{1,100}\Z");
        private readonly SnapshotManager mSnapshotManager;

        internal NativeSavedGameClient(SnapshotManager manager)
        {
            this.mSnapshotManager = Misc.CheckNotNull<SnapshotManager>(manager);
        }

        private static Types.SnapshotConflictPolicy AsConflictPolicy(ConflictResolutionStrategy strategy)
        {
            switch (strategy)
            {
                case ConflictResolutionStrategy.UseLongestPlaytime:
                    return Types.SnapshotConflictPolicy.LONGEST_PLAYTIME;

                case ConflictResolutionStrategy.UseOriginal:
                    return Types.SnapshotConflictPolicy.LAST_KNOWN_GOOD;

                case ConflictResolutionStrategy.UseUnmerged:
                    return Types.SnapshotConflictPolicy.MOST_RECENTLY_MODIFIED;
            }
            throw new InvalidOperationException("Found unhandled strategy: " + strategy);
        }

        private static Types.DataSource AsDataSource(DataSource source)
        {
            if (source != DataSource.ReadCacheOrNetwork)
            {
                if (source != DataSource.ReadNetworkOnly)
                {
                    throw new InvalidOperationException("Found unhandled DataSource: " + source);
                }
                return Types.DataSource.NETWORK_ONLY;
            }
            return Types.DataSource.CACHE_OR_NETWORK;
        }

        private static NativeSnapshotMetadataChange AsMetadataChange(SavedGameMetadataUpdate update)
        {
            NativeSnapshotMetadataChange.Builder builder = new NativeSnapshotMetadataChange.Builder();
            if (update.IsCoverImageUpdated)
            {
                builder.SetCoverImageFromPngData(update.UpdatedPngCoverImage);
            }
            if (update.IsDescriptionUpdated)
            {
                builder.SetDescription(update.UpdatedDescription);
            }
            if (update.IsPlayedTimeUpdated)
            {
                builder.SetPlayedTime((ulong) update.UpdatedPlayedTime.Value.TotalMilliseconds);
            }
            return builder.Build();
        }

        private static SavedGameRequestStatus AsRequestStatus(CommonErrorStatus.ResponseStatus status)
        {
            switch ((status + 5))
            {
                case ~CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
                    return SavedGameRequestStatus.TimeoutError;

                case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
                    Logger.e("User was not authorized (they were probably not logged in).");
                    return SavedGameRequestStatus.AuthenticationError;

                case ~CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return SavedGameRequestStatus.InternalError;

                case ~CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
                    Logger.e("User attempted to use the game without a valid license.");
                    return SavedGameRequestStatus.AuthenticationError;

                case ((CommonErrorStatus.ResponseStatus) 6):
                case ((CommonErrorStatus.ResponseStatus) 7):
                    return SavedGameRequestStatus.Success;
            }
            Logger.e("Unknown status: " + status);
            return SavedGameRequestStatus.InternalError;
        }

        private static SavedGameRequestStatus AsRequestStatus(CommonErrorStatus.SnapshotOpenStatus status)
        {
            switch ((status + 5))
            {
                case ((CommonErrorStatus.SnapshotOpenStatus) 0):
                    return SavedGameRequestStatus.TimeoutError;

                case ~CommonErrorStatus.SnapshotOpenStatus.ERROR_NOT_AUTHORIZED:
                    return SavedGameRequestStatus.AuthenticationError;
            }
            if (status == CommonErrorStatus.SnapshotOpenStatus.VALID)
            {
                return SavedGameRequestStatus.Success;
            }
            Logger.e("Encountered unknown status: " + status);
            return SavedGameRequestStatus.InternalError;
        }

        private static SelectUIStatus AsUIStatus(CommonErrorStatus.UIStatus uiStatus)
        {
            switch ((uiStatus + 6))
            {
                case ~(CommonErrorStatus.UIStatus.VALID | CommonErrorStatus.UIStatus.ERROR_INTERNAL):
                    return SelectUIStatus.UserClosedUI;

                case CommonErrorStatus.UIStatus.VALID:
                    return SelectUIStatus.TimeoutError;

                case ~CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return SelectUIStatus.AuthenticationError;

                case ~CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
                    return SelectUIStatus.InternalError;

                case ((CommonErrorStatus.UIStatus) 7):
                    return SelectUIStatus.SavedGameSelected;
            }
            Logger.e("Encountered unknown UI Status: " + uiStatus);
            return SelectUIStatus.InternalError;
        }

        public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
        {
            <CommitUpdate>c__AnonStorey7 storey = new <CommitUpdate>c__AnonStorey7 {
                callback = callback
            };
            Misc.CheckNotNull<ISavedGameMetadata>(metadata);
            Misc.CheckNotNull<byte[]>(updatedBinaryData);
            Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(storey.callback);
            storey.callback = ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(storey.callback);
            NativeSnapshotMetadata metadata2 = metadata as NativeSnapshotMetadata;
            if (metadata2 == null)
            {
                Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
                storey.callback(SavedGameRequestStatus.BadInputError, null);
            }
            else if (!metadata2.IsOpen)
            {
                Logger.e("This method requires an open ISavedGameMetadata.");
                storey.callback(SavedGameRequestStatus.BadInputError, null);
            }
            else
            {
                this.mSnapshotManager.Commit(metadata2, AsMetadataChange(updateForMetadata), updatedBinaryData, new Action<SnapshotManager.CommitResponse>(storey.<>m__0));
            }
        }

        public void Delete(ISavedGameMetadata metadata)
        {
            Misc.CheckNotNull<ISavedGameMetadata>(metadata);
            this.mSnapshotManager.Delete((NativeSnapshotMetadata) metadata);
        }

        public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
        {
            <FetchAllSavedGames>c__AnonStorey8 storey = new <FetchAllSavedGames>c__AnonStorey8 {
                callback = callback
            };
            Misc.CheckNotNull<Action<SavedGameRequestStatus, List<ISavedGameMetadata>>>(storey.callback);
            storey.callback = ToOnGameThread<SavedGameRequestStatus, List<ISavedGameMetadata>>(storey.callback);
            this.mSnapshotManager.FetchAll(AsDataSource(source), new Action<SnapshotManager.FetchAllResponse>(storey.<>m__0));
        }

        private void InternalOpen(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
        {
            Types.SnapshotConflictPolicy mANUAL;
            <InternalOpen>c__AnonStorey3 storey = new <InternalOpen>c__AnonStorey3 {
                completedCallback = completedCallback,
                filename = filename,
                source = source,
                resolutionStrategy = resolutionStrategy,
                prefetchDataOnConflict = prefetchDataOnConflict,
                conflictCallback = conflictCallback,
                $this = this
            };
            switch (storey.resolutionStrategy)
            {
                case ConflictResolutionStrategy.UseLongestPlaytime:
                    mANUAL = Types.SnapshotConflictPolicy.LONGEST_PLAYTIME;
                    break;

                case ConflictResolutionStrategy.UseManual:
                    mANUAL = Types.SnapshotConflictPolicy.MANUAL;
                    break;

                case ConflictResolutionStrategy.UseLastKnownGood:
                    mANUAL = Types.SnapshotConflictPolicy.LAST_KNOWN_GOOD;
                    break;

                case ConflictResolutionStrategy.UseMostRecentlySaved:
                    mANUAL = Types.SnapshotConflictPolicy.MOST_RECENTLY_MODIFIED;
                    break;

                default:
                    mANUAL = Types.SnapshotConflictPolicy.MOST_RECENTLY_MODIFIED;
                    break;
            }
            this.mSnapshotManager.Open(storey.filename, AsDataSource(storey.source), mANUAL, new Action<SnapshotManager.OpenResponse>(storey.<>m__0));
        }

        internal static bool IsValidFilename(string filename)
        {
            if (filename == null)
            {
                return false;
            }
            return ValidFilenameRegex.IsMatch(filename);
        }

        public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
        {
            this.OpenWithAutomaticConflictResolution(filename, source, resolutionStrategy, false, null, completedCallback);
        }

        public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
        {
            <OpenWithAutomaticConflictResolution>c__AnonStorey0 storey = new <OpenWithAutomaticConflictResolution>c__AnonStorey0 {
                resolutionStrategy = resolutionStrategy,
                completedCallback = completedCallback
            };
            Misc.CheckNotNull<string>(filename);
            Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(storey.completedCallback);
            storey.completedCallback = ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(storey.completedCallback);
            if (conflictCallback == null)
            {
                conflictCallback = new ConflictCallback(storey.<>m__0);
            }
            conflictCallback = this.ToOnGameThread(conflictCallback);
            if (!IsValidFilename(filename))
            {
                Logger.e("Received invalid filename: " + filename);
                storey.completedCallback(SavedGameRequestStatus.BadInputError, null);
            }
            else
            {
                this.InternalOpen(filename, source, storey.resolutionStrategy, prefetchDataOnConflict, conflictCallback, storey.completedCallback);
            }
        }

        public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
        {
            Misc.CheckNotNull<string>(filename);
            Misc.CheckNotNull<ConflictCallback>(conflictCallback);
            Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completedCallback);
            conflictCallback = this.ToOnGameThread(conflictCallback);
            completedCallback = ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(completedCallback);
            if (!IsValidFilename(filename))
            {
                Logger.e("Received invalid filename: " + filename);
                completedCallback(SavedGameRequestStatus.BadInputError, null);
            }
            else
            {
                this.InternalOpen(filename, source, ConflictResolutionStrategy.UseManual, prefetchDataOnConflict, conflictCallback, completedCallback);
            }
        }

        public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
        {
            <ReadBinaryData>c__AnonStorey5 storey = new <ReadBinaryData>c__AnonStorey5 {
                completedCallback = completedCallback
            };
            Misc.CheckNotNull<ISavedGameMetadata>(metadata);
            Misc.CheckNotNull<Action<SavedGameRequestStatus, byte[]>>(storey.completedCallback);
            storey.completedCallback = ToOnGameThread<SavedGameRequestStatus, byte[]>(storey.completedCallback);
            NativeSnapshotMetadata metadata2 = metadata as NativeSnapshotMetadata;
            if (metadata2 == null)
            {
                Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
                storey.completedCallback(SavedGameRequestStatus.BadInputError, null);
            }
            else if (!metadata2.IsOpen)
            {
                Logger.e("This method requires an open ISavedGameMetadata.");
                storey.completedCallback(SavedGameRequestStatus.BadInputError, null);
            }
            else
            {
                this.mSnapshotManager.Read(metadata2, new Action<SnapshotManager.ReadResponse>(storey.<>m__0));
            }
        }

        public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
        {
            <ShowSelectSavedGameUI>c__AnonStorey6 storey = new <ShowSelectSavedGameUI>c__AnonStorey6 {
                callback = callback
            };
            Misc.CheckNotNull<string>(uiTitle);
            Misc.CheckNotNull<Action<SelectUIStatus, ISavedGameMetadata>>(storey.callback);
            storey.callback = ToOnGameThread<SelectUIStatus, ISavedGameMetadata>(storey.callback);
            if (maxDisplayedSavedGames <= 0)
            {
                Logger.e("maxDisplayedSavedGames must be greater than 0");
                storey.callback(SelectUIStatus.BadInputError, null);
            }
            else
            {
                this.mSnapshotManager.SnapshotSelectUI(showCreateSaveUI, showDeleteSaveUI, maxDisplayedSavedGames, uiTitle, new Action<SnapshotManager.SnapshotSelectUIResponse>(storey.<>m__0));
            }
        }

        private ConflictCallback ToOnGameThread(ConflictCallback conflictCallback)
        {
            <ToOnGameThread>c__AnonStorey1 storey = new <ToOnGameThread>c__AnonStorey1 {
                conflictCallback = conflictCallback
            };
            return new ConflictCallback(storey.<>m__0);
        }

        private static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
        {
            <ToOnGameThread>c__AnonStorey9<T1, T2> storey = new <ToOnGameThread>c__AnonStorey9<T1, T2> {
                toConvert = toConvert
            };
            return new Action<T1, T2>(storey.<>m__0);
        }

        [CompilerGenerated]
        private sealed class <CommitUpdate>c__AnonStorey7
        {
            internal Action<SavedGameRequestStatus, ISavedGameMetadata> callback;

            internal void <>m__0(SnapshotManager.CommitResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.callback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                }
                else
                {
                    this.callback(SavedGameRequestStatus.Success, response.Data());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FetchAllSavedGames>c__AnonStorey8
        {
            internal Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback;

            internal void <>m__0(SnapshotManager.FetchAllResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.callback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), new List<ISavedGameMetadata>());
                }
                else
                {
                    this.callback(SavedGameRequestStatus.Success, response.Data().Cast<ISavedGameMetadata>().ToList<ISavedGameMetadata>());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <InternalOpen>c__AnonStorey3
        {
            internal Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback;
            internal string filename;
            internal DataSource source;
            internal ConflictResolutionStrategy resolutionStrategy;
            internal bool prefetchDataOnConflict;
            internal ConflictCallback conflictCallback;
            internal NativeSavedGameClient $this;

            internal void <>m__0(SnapshotManager.OpenResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.completedCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                }
                else if (response.ResponseStatus() == CommonErrorStatus.SnapshotOpenStatus.VALID)
                {
                    this.completedCallback(SavedGameRequestStatus.Success, response.Data());
                }
                else if (response.ResponseStatus() == CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
                {
                    <InternalOpen>c__AnonStorey4 storey = new <InternalOpen>c__AnonStorey4 {
                        <>f__ref$3 = this,
                        original = response.ConflictOriginal(),
                        unmerged = response.ConflictUnmerged()
                    };
                    storey.resolver = new NativeSavedGameClient.NativeConflictResolver(this.$this.mSnapshotManager, response.ConflictId(), storey.original, storey.unmerged, this.completedCallback, new Action(storey.<>m__0));
                    if (!this.prefetchDataOnConflict)
                    {
                        this.conflictCallback(storey.resolver, storey.original, null, storey.unmerged, null);
                    }
                    else
                    {
                        NativeSavedGameClient.Prefetcher prefetcher = new NativeSavedGameClient.Prefetcher(new Action<byte[], byte[]>(storey.<>m__1), this.completedCallback);
                        this.$this.mSnapshotManager.Read(storey.original, new Action<SnapshotManager.ReadResponse>(prefetcher.OnOriginalDataRead));
                        this.$this.mSnapshotManager.Read(storey.unmerged, new Action<SnapshotManager.ReadResponse>(prefetcher.OnUnmergedDataRead));
                    }
                }
                else
                {
                    Logger.e("Unhandled response status");
                    this.completedCallback(SavedGameRequestStatus.InternalError, null);
                }
            }

            private sealed class <InternalOpen>c__AnonStorey4
            {
                internal NativeSavedGameClient.NativeConflictResolver resolver;
                internal NativeSnapshotMetadata original;
                internal NativeSnapshotMetadata unmerged;
                internal NativeSavedGameClient.<InternalOpen>c__AnonStorey3 <>f__ref$3;

                internal void <>m__0()
                {
                    this.<>f__ref$3.$this.InternalOpen(this.<>f__ref$3.filename, this.<>f__ref$3.source, this.<>f__ref$3.resolutionStrategy, this.<>f__ref$3.prefetchDataOnConflict, this.<>f__ref$3.conflictCallback, this.<>f__ref$3.completedCallback);
                }

                internal void <>m__1(byte[] originalData, byte[] unmergedData)
                {
                    this.<>f__ref$3.conflictCallback(this.resolver, this.original, originalData, this.unmerged, unmergedData);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OpenWithAutomaticConflictResolution>c__AnonStorey0
        {
            internal ConflictResolutionStrategy resolutionStrategy;
            internal Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback;

            internal void <>m__0(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
            {
                switch (this.resolutionStrategy)
                {
                    case ConflictResolutionStrategy.UseLongestPlaytime:
                        if (original.TotalTimePlayed < unmerged.TotalTimePlayed)
                        {
                            resolver.ChooseMetadata(unmerged);
                            break;
                        }
                        resolver.ChooseMetadata(original);
                        break;

                    case ConflictResolutionStrategy.UseOriginal:
                        resolver.ChooseMetadata(original);
                        return;

                    case ConflictResolutionStrategy.UseUnmerged:
                        resolver.ChooseMetadata(unmerged);
                        return;

                    default:
                        Logger.e("Unhandled strategy " + this.resolutionStrategy);
                        this.completedCallback(SavedGameRequestStatus.InternalError, null);
                        return;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ReadBinaryData>c__AnonStorey5
        {
            internal Action<SavedGameRequestStatus, byte[]> completedCallback;

            internal void <>m__0(SnapshotManager.ReadResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.completedCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                }
                else
                {
                    this.completedCallback(SavedGameRequestStatus.Success, response.Data());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ShowSelectSavedGameUI>c__AnonStorey6
        {
            internal Action<SelectUIStatus, ISavedGameMetadata> callback;

            internal void <>m__0(SnapshotManager.SnapshotSelectUIResponse response)
            {
                this.callback(NativeSavedGameClient.AsUIStatus(response.RequestStatus()), !response.RequestSucceeded() ? null : response.Data());
            }
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey1
        {
            internal ConflictCallback conflictCallback;

            internal void <>m__0(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
            {
                <ToOnGameThread>c__AnonStorey2 storey = new <ToOnGameThread>c__AnonStorey2 {
                    <>f__ref$1 = this,
                    resolver = resolver,
                    original = original,
                    originalData = originalData,
                    unmerged = unmerged,
                    unmergedData = unmergedData
                };
                Logger.d("Invoking conflict callback");
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            private sealed class <ToOnGameThread>c__AnonStorey2
            {
                internal IConflictResolver resolver;
                internal ISavedGameMetadata original;
                internal byte[] originalData;
                internal ISavedGameMetadata unmerged;
                internal byte[] unmergedData;
                internal NativeSavedGameClient.<ToOnGameThread>c__AnonStorey1 <>f__ref$1;

                internal void <>m__0()
                {
                    this.<>f__ref$1.conflictCallback(this.resolver, this.original, this.originalData, this.unmerged, this.unmergedData);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey9<T1, T2>
        {
            internal Action<T1, T2> toConvert;

            internal void <>m__0(T1 val1, T2 val2)
            {
                <ToOnGameThread>c__AnonStoreyA<T1, T2> ya = new <ToOnGameThread>c__AnonStoreyA<T1, T2> {
                    <>f__ref$9 = (NativeSavedGameClient.<ToOnGameThread>c__AnonStorey9<T1, T2>) this,
                    val1 = val1,
                    val2 = val2
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(ya.<>m__0));
            }

            private sealed class <ToOnGameThread>c__AnonStoreyA
            {
                internal T1 val1;
                internal T2 val2;
                internal NativeSavedGameClient.<ToOnGameThread>c__AnonStorey9<T1, T2> <>f__ref$9;

                internal void <>m__0()
                {
                    this.<>f__ref$9.toConvert(this.val1, this.val2);
                }
            }
        }

        private class NativeConflictResolver : IConflictResolver
        {
            private readonly SnapshotManager mManager;
            private readonly string mConflictId;
            private readonly NativeSnapshotMetadata mOriginal;
            private readonly NativeSnapshotMetadata mUnmerged;
            private readonly Action<SavedGameRequestStatus, ISavedGameMetadata> mCompleteCallback;
            private readonly Action mRetryFileOpen;

            internal NativeConflictResolver(SnapshotManager manager, string conflictId, NativeSnapshotMetadata original, NativeSnapshotMetadata unmerged, Action<SavedGameRequestStatus, ISavedGameMetadata> completeCallback, Action retryOpen)
            {
                this.mManager = Misc.CheckNotNull<SnapshotManager>(manager);
                this.mConflictId = Misc.CheckNotNull<string>(conflictId);
                this.mOriginal = Misc.CheckNotNull<NativeSnapshotMetadata>(original);
                this.mUnmerged = Misc.CheckNotNull<NativeSnapshotMetadata>(unmerged);
                this.mCompleteCallback = Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completeCallback);
                this.mRetryFileOpen = Misc.CheckNotNull<Action>(retryOpen);
            }

            public void ChooseMetadata(ISavedGameMetadata chosenMetadata)
            {
                NativeSnapshotMetadata metadata = chosenMetadata as NativeSnapshotMetadata;
                if ((metadata != this.mOriginal) && (metadata != this.mUnmerged))
                {
                    Logger.e("Caller attempted to choose a version of the metadata that was not part of the conflict");
                    this.mCompleteCallback(SavedGameRequestStatus.BadInputError, null);
                }
                else
                {
                    this.mManager.Resolve(metadata, new NativeSnapshotMetadataChange.Builder().Build(), this.mConflictId, delegate (SnapshotManager.OpenResponse response) {
                        if (!response.RequestSucceeded())
                        {
                            this.mCompleteCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                        }
                        else
                        {
                            this.mRetryFileOpen();
                        }
                    });
                }
            }

            public void ResolveConflict(ISavedGameMetadata chosenMetadata, SavedGameMetadataUpdate metadataUpdate, byte[] updatedData)
            {
                NativeSnapshotMetadata metadata = chosenMetadata as NativeSnapshotMetadata;
                if ((metadata != this.mOriginal) && (metadata != this.mUnmerged))
                {
                    Logger.e("Caller attempted to choose a version of the metadata that was not part of the conflict");
                    this.mCompleteCallback(SavedGameRequestStatus.BadInputError, null);
                }
                else
                {
                    NativeSnapshotMetadataChange metadataChange = new NativeSnapshotMetadataChange.Builder().From(metadataUpdate).Build();
                    this.mManager.Resolve(metadata, metadataChange, this.mConflictId, updatedData, delegate (SnapshotManager.OpenResponse response) {
                        if (!response.RequestSucceeded())
                        {
                            this.mCompleteCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                        }
                        else
                        {
                            this.mRetryFileOpen();
                        }
                    });
                }
            }
        }

        private class Prefetcher
        {
            private readonly object mLock = new object();
            private bool mOriginalDataFetched;
            private byte[] mOriginalData;
            private bool mUnmergedDataFetched;
            private byte[] mUnmergedData;
            private Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback;
            private readonly Action<byte[], byte[]> mDataFetchedCallback;
            [CompilerGenerated]
            private static Action<SavedGameRequestStatus, ISavedGameMetadata> <>f__am$cache0;
            [CompilerGenerated]
            private static Action<SavedGameRequestStatus, ISavedGameMetadata> <>f__am$cache1;

            internal Prefetcher(Action<byte[], byte[]> dataFetchedCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
            {
                this.mDataFetchedCallback = Misc.CheckNotNull<Action<byte[], byte[]>>(dataFetchedCallback);
                this.completedCallback = Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completedCallback);
            }

            private void MaybeProceed()
            {
                if (this.mOriginalDataFetched && this.mUnmergedDataFetched)
                {
                    Logger.d("Fetched data for original and unmerged, proceeding");
                    this.mDataFetchedCallback(this.mOriginalData, this.mUnmergedData);
                }
                else
                {
                    Logger.d(string.Concat(new object[] { "Not all data fetched - original:", this.mOriginalDataFetched, " unmerged:", this.mUnmergedDataFetched }));
                }
            }

            internal void OnOriginalDataRead(SnapshotManager.ReadResponse readResponse)
            {
                object mLock = this.mLock;
                lock (mLock)
                {
                    if (!readResponse.RequestSucceeded())
                    {
                        Logger.e("Encountered error while prefetching original data.");
                        this.completedCallback(NativeSavedGameClient.AsRequestStatus(readResponse.ResponseStatus()), null);
                        if (<>f__am$cache0 == null)
                        {
                            <>f__am$cache0 = delegate {
                            };
                        }
                        this.completedCallback = <>f__am$cache0;
                    }
                    else
                    {
                        Logger.d("Successfully fetched original data");
                        this.mOriginalDataFetched = true;
                        this.mOriginalData = readResponse.Data();
                        this.MaybeProceed();
                    }
                }
            }

            internal void OnUnmergedDataRead(SnapshotManager.ReadResponse readResponse)
            {
                object mLock = this.mLock;
                lock (mLock)
                {
                    if (!readResponse.RequestSucceeded())
                    {
                        Logger.e("Encountered error while prefetching unmerged data.");
                        this.completedCallback(NativeSavedGameClient.AsRequestStatus(readResponse.ResponseStatus()), null);
                        if (<>f__am$cache1 == null)
                        {
                            <>f__am$cache1 = delegate {
                            };
                        }
                        this.completedCallback = <>f__am$cache1;
                    }
                    else
                    {
                        Logger.d("Successfully fetched unmerged data");
                        this.mUnmergedDataFetched = true;
                        this.mUnmergedData = readResponse.Data();
                        this.MaybeProceed();
                    }
                }
            }
        }
    }
}

