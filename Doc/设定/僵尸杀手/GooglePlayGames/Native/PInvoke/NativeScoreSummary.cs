namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeScoreSummary : BaseReferenceHolder
    {
        internal NativeScoreSummary(IntPtr selfPtr) : base(selfPtr)
        {
        }

        internal ulong ApproximateResults() => 
            ScoreSummary.ScoreSummary_ApproximateNumberOfScores(base.SelfPtr());

        protected override void CallDispose(HandleRef selfPointer)
        {
            ScoreSummary.ScoreSummary_Dispose(selfPointer);
        }

        internal static NativeScoreSummary FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeScoreSummary(pointer);
        }

        internal NativeScore LocalUserScore() => 
            NativeScore.FromPointer(ScoreSummary.ScoreSummary_CurrentPlayerScore(base.SelfPtr()));
    }
}

