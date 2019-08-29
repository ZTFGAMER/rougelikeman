namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class TurnBasedMatchConfig : BaseReferenceHolder
    {
        internal TurnBasedMatchConfig(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            TurnBasedMatchConfig.TurnBasedMatchConfig_Dispose(selfPointer);
        }

        internal long ExclusiveBitMask() => 
            TurnBasedMatchConfig.TurnBasedMatchConfig_ExclusiveBitMask(base.SelfPtr());

        internal uint MaximumAutomatchingPlayers() => 
            TurnBasedMatchConfig.TurnBasedMatchConfig_MaximumAutomatchingPlayers(base.SelfPtr());

        internal uint MinimumAutomatchingPlayers() => 
            TurnBasedMatchConfig.TurnBasedMatchConfig_MinimumAutomatchingPlayers(base.SelfPtr());

        private string PlayerIdAtIndex(UIntPtr index)
        {
            <PlayerIdAtIndex>c__AnonStorey0 storey = new <PlayerIdAtIndex>c__AnonStorey0 {
                index = index,
                $this = this
            };
            return PInvokeUtilities.OutParamsToString(new PInvokeUtilities.OutStringMethod(storey.<>m__0));
        }

        internal IEnumerator<string> PlayerIdsToInvite() => 
            PInvokeUtilities.ToEnumerator<string>(TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_Length(base.SelfPtr()), new Func<UIntPtr, string>(this.PlayerIdAtIndex));

        internal uint Variant() => 
            TurnBasedMatchConfig.TurnBasedMatchConfig_Variant(base.SelfPtr());

        [CompilerGenerated]
        private sealed class <PlayerIdAtIndex>c__AnonStorey0
        {
            internal UIntPtr index;
            internal TurnBasedMatchConfig $this;

            internal UIntPtr <>m__0(byte[] out_string, UIntPtr size) => 
                TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_GetElement(this.$this.SelfPtr(), this.index, out_string, size);
        }
    }
}

