namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class FetchResponse : BaseReferenceHolder
    {
        internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            LeaderboardManager.LeaderboardManager_FetchResponse_Dispose(base.SelfPtr());
        }

        internal static FetchResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new FetchResponse(pointer);
        }

        internal CommonErrorStatus.ResponseStatus GetStatus() => 
            LeaderboardManager.LeaderboardManager_FetchResponse_GetStatus(base.SelfPtr());

        internal NativeLeaderboard Leaderboard() => 
            NativeLeaderboard.FromPointer(LeaderboardManager.LeaderboardManager_FetchResponse_GetData(base.SelfPtr()));
    }
}

