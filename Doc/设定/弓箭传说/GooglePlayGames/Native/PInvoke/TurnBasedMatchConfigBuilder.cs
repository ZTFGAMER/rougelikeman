namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class TurnBasedMatchConfigBuilder : BaseReferenceHolder
    {
        private TurnBasedMatchConfigBuilder(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal TurnBasedMatchConfigBuilder AddInvitedPlayer(string playerId)
        {
            TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_AddPlayerToInvite(base.SelfPtr(), playerId);
            return this;
        }

        internal TurnBasedMatchConfig Build() => 
            new TurnBasedMatchConfig(TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Create(base.SelfPtr()));

        protected override void CallDispose(HandleRef selfPointer)
        {
            TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Dispose(selfPointer);
        }

        internal static TurnBasedMatchConfigBuilder Create() => 
            new TurnBasedMatchConfigBuilder(TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Construct());

        internal TurnBasedMatchConfigBuilder PopulateFromUIResponse(PlayerSelectUIResponse response)
        {
            TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_PopulateFromPlayerSelectUIResponse(base.SelfPtr(), response.AsPointer());
            return this;
        }

        internal TurnBasedMatchConfigBuilder SetExclusiveBitMask(ulong bitmask)
        {
            TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetExclusiveBitMask(base.SelfPtr(), bitmask);
            return this;
        }

        internal TurnBasedMatchConfigBuilder SetMaximumAutomatchingPlayers(uint maximum)
        {
            TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetMaximumAutomatchingPlayers(base.SelfPtr(), maximum);
            return this;
        }

        internal TurnBasedMatchConfigBuilder SetMinimumAutomatchingPlayers(uint minimum)
        {
            TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetMinimumAutomatchingPlayers(base.SelfPtr(), minimum);
            return this;
        }

        internal TurnBasedMatchConfigBuilder SetVariant(uint variant)
        {
            TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetVariant(base.SelfPtr(), variant);
            return this;
        }
    }
}

