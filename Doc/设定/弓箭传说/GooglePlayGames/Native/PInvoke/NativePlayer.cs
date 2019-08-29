namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativePlayer : BaseReferenceHolder
    {
        internal NativePlayer(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal Player AsPlayer() => 
            new Player(this.Name(), this.Id(), this.AvatarURL());

        internal string AvatarURL() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Player.Player_AvatarUrl(base.SelfPtr(), Types.ImageResolution.ICON, out_string, out_size));

        protected override void CallDispose(HandleRef selfPointer)
        {
            Player.Player_Dispose(selfPointer);
        }

        internal string Id() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Player.Player_Id(base.SelfPtr(), out_string, out_size));

        internal string Name() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Player.Player_Name(base.SelfPtr(), out_string, out_size));
    }
}

