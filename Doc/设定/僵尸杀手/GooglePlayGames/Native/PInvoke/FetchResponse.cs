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
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_Dispose(base.SelfPtr());
        }

        internal static GooglePlayGames.Native.PInvoke.FetchResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new GooglePlayGames.Native.PInvoke.FetchResponse(pointer);
        }

        internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GetStatus() => 
            GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_GetStatus(base.SelfPtr());

        internal NativeLeaderboard Leaderboard() => 
            NativeLeaderboard.FromPointer(GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_GetData(base.SelfPtr()));
    }
}

