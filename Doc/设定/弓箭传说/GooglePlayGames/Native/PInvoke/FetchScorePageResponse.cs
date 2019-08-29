namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class FetchScorePageResponse : BaseReferenceHolder
    {
        internal FetchScorePageResponse(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            LeaderboardManager.LeaderboardManager_FetchScorePageResponse_Dispose(base.SelfPtr());
        }

        internal static FetchScorePageResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new FetchScorePageResponse(pointer);
        }

        internal NativeScorePage GetScorePage() => 
            NativeScorePage.FromPointer(LeaderboardManager.LeaderboardManager_FetchScorePageResponse_GetData(base.SelfPtr()));

        internal CommonErrorStatus.ResponseStatus GetStatus() => 
            LeaderboardManager.LeaderboardManager_FetchScorePageResponse_GetStatus(base.SelfPtr());
    }
}

