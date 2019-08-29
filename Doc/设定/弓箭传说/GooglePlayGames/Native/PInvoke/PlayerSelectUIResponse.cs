namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class PlayerSelectUIResponse : BaseReferenceHolder, IEnumerable<string>, IEnumerable
    {
        internal PlayerSelectUIResponse(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_Dispose(selfPointer);
        }

        internal static PlayerSelectUIResponse FromPointer(IntPtr pointer)
        {
            if (PInvokeUtilities.IsNull(pointer))
            {
                return null;
            }
            return new PlayerSelectUIResponse(pointer);
        }

        public IEnumerator<string> GetEnumerator() => 
            PInvokeUtilities.ToEnumerator<string>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_Length(base.SelfPtr()), new Func<UIntPtr, string>(this.PlayerIdAtIndex));

        internal uint MaximumAutomatchingPlayers() => 
            TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMaximumAutomatchingPlayers(base.SelfPtr());

        internal uint MinimumAutomatchingPlayers() => 
            TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMinimumAutomatchingPlayers(base.SelfPtr());

        private string PlayerIdAtIndex(UIntPtr index)
        {
            <PlayerIdAtIndex>c__AnonStorey0 storey = new <PlayerIdAtIndex>c__AnonStorey0 {
                index = index,
                $this = this
            };
            return PInvokeUtilities.OutParamsToString(new PInvokeUtilities.OutStringMethod(storey.<>m__0));
        }

        internal CommonErrorStatus.UIStatus Status() => 
            TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetStatus(base.SelfPtr());

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        [CompilerGenerated]
        private sealed class <PlayerIdAtIndex>c__AnonStorey0
        {
            internal UIntPtr index;
            internal PlayerSelectUIResponse $this;

            internal UIntPtr <>m__0(byte[] out_string, UIntPtr size) => 
                TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_GetElement(this.$this.SelfPtr(), this.index, out_string, size);
        }
    }
}

