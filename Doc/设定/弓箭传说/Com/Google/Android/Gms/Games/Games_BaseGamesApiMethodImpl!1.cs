namespace Com.Google.Android.Gms.Games
{
    using Com.Google.Android.Gms.Common.Api;
    using Google.Developers;
    using System;

    public class Games_BaseGamesApiMethodImpl<R> : JavaObjWrapper where R: Result
    {
        private const string CLASS_NAME = "com/google/android/gms/games/Games$BaseGamesApiMethodImpl";

        public Games_BaseGamesApiMethodImpl(GoogleApiClient arg_GoogleApiClient_1)
        {
            object[] args = new object[] { arg_GoogleApiClient_1 };
            base.CreateInstance("com/google/android/gms/games/Games$BaseGamesApiMethodImpl", args);
        }

        public Games_BaseGamesApiMethodImpl(IntPtr ptr) : base(ptr)
        {
        }
    }
}

