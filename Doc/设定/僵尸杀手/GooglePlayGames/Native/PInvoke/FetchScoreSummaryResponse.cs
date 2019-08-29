namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class FetchScoreSummaryResponse : BaseReferenceHolder
    {
        internal FetchScoreSummaryResponse(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_Dispose(selfPointer);
        }

        internal static FetchScoreSummaryResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new FetchScoreSummaryResponse(pointer);
        }

        internal NativeScoreSummary GetScoreSummary() => 
            NativeScoreSummary.FromPointer(GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_GetData(base.SelfPtr()));

        internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GetStatus() => 
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_GetStatus(base.SelfPtr());
    }
}

