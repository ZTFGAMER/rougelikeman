namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeScorePageToken : BaseReferenceHolder
    {
        internal NativeScorePageToken(IntPtr selfPtr) : base(selfPtr)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            ScorePage.ScorePage_ScorePageToken_Dispose(selfPointer);
        }
    }
}

