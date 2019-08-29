namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class GameServices : BaseReferenceHolder
    {
        internal GameServices(IntPtr selfPointer) : base(selfPointer)
        {
        }

        public GooglePlayGames.Native.PInvoke.AchievementManager AchievementManager() => 
            new GooglePlayGames.Native.PInvoke.AchievementManager(this);

        internal HandleRef AsHandle() => 
            base.SelfPtr();

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.GameServices.GameServices_Dispose(selfPointer);
        }

        internal bool IsAuthenticated() => 
            GooglePlayGames.Native.Cwrapper.GameServices.GameServices_IsAuthorized(base.SelfPtr());

        public GooglePlayGames.Native.PInvoke.LeaderboardManager LeaderboardManager() => 
            new GooglePlayGames.Native.PInvoke.LeaderboardManager(this);

        public GooglePlayGames.Native.PInvoke.PlayerManager PlayerManager() => 
            new GooglePlayGames.Native.PInvoke.PlayerManager(this);

        internal void SignOut()
        {
            GooglePlayGames.Native.Cwrapper.GameServices.GameServices_SignOut(base.SelfPtr());
        }

        internal void StartAuthorizationUI()
        {
            GooglePlayGames.Native.Cwrapper.GameServices.GameServices_StartAuthorizationUI(base.SelfPtr());
        }

        public GooglePlayGames.Native.PInvoke.StatsManager StatsManager() => 
            new GooglePlayGames.Native.PInvoke.StatsManager(this);
    }
}

