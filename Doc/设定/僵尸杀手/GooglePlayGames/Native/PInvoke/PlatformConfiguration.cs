namespace GooglePlayGames.Native.PInvoke
{
    using System;
    using System.Runtime.InteropServices;

    internal abstract class PlatformConfiguration : BaseReferenceHolder
    {
        protected PlatformConfiguration(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal HandleRef AsHandle() => 
            base.SelfPtr();
    }
}

