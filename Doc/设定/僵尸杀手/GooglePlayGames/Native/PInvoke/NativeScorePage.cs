namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class NativeScorePage : BaseReferenceHolder
    {
        internal NativeScorePage(IntPtr selfPtr) : base(selfPtr)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            ScorePage.ScorePage_Dispose(selfPointer);
        }

        internal static NativeScorePage FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeScorePage(pointer);
        }

        internal Types.LeaderboardCollection GetCollection() => 
            ScorePage.ScorePage_Collection(base.SelfPtr());

        private NativeScoreEntry GetElement(UIntPtr index)
        {
            if (index.ToUInt64() >= this.Length().ToUInt64())
            {
                throw new ArgumentOutOfRangeException();
            }
            return new NativeScoreEntry(ScorePage.ScorePage_Entries_GetElement(base.SelfPtr(), index));
        }

        public IEnumerator<NativeScoreEntry> GetEnumerator() => 
            PInvokeUtilities.ToEnumerator<NativeScoreEntry>(ScorePage.ScorePage_Entries_Length(base.SelfPtr()), index => this.GetElement(index));

        internal string GetLeaderboardId() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => ScorePage.ScorePage_LeaderboardId(base.SelfPtr(), out_string, out_size));

        internal NativeScorePageToken GetNextScorePageToken() => 
            new NativeScorePageToken(ScorePage.ScorePage_NextScorePageToken(base.SelfPtr()));

        internal NativeScorePageToken GetPreviousScorePageToken() => 
            new NativeScorePageToken(ScorePage.ScorePage_PreviousScorePageToken(base.SelfPtr()));

        internal Types.LeaderboardStart GetStart() => 
            ScorePage.ScorePage_Start(base.SelfPtr());

        internal Types.LeaderboardTimeSpan GetTimeSpan() => 
            ScorePage.ScorePage_TimeSpan(base.SelfPtr());

        internal bool HasNextScorePage() => 
            ScorePage.ScorePage_HasNextScorePage(base.SelfPtr());

        internal bool HasPrevScorePage() => 
            ScorePage.ScorePage_HasPreviousScorePage(base.SelfPtr());

        private UIntPtr Length() => 
            ScorePage.ScorePage_Entries_Length(base.SelfPtr());

        internal bool Valid() => 
            ScorePage.ScorePage_Valid(base.SelfPtr());
    }
}

