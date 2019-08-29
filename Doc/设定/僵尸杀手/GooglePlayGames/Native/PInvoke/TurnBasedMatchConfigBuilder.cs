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

        internal GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder AddInvitedPlayer(string playerId)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_AddPlayerToInvite(base.SelfPtr(), playerId);
            return this;
        }

        internal GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig Build() => 
            new GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig(GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Create(base.SelfPtr()));

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Dispose(selfPointer);
        }

        internal static GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder Create() => 
            new GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder(GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Construct());

        internal GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder PopulateFromUIResponse(PlayerSelectUIResponse response)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_PopulateFromPlayerSelectUIResponse(base.SelfPtr(), response.AsPointer());
            return this;
        }

        internal GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder SetExclusiveBitMask(ulong bitmask)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetExclusiveBitMask(base.SelfPtr(), bitmask);
            return this;
        }

        internal GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder SetMaximumAutomatchingPlayers(uint maximum)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetMaximumAutomatchingPlayers(base.SelfPtr(), maximum);
            return this;
        }

        internal GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder SetMinimumAutomatchingPlayers(uint minimum)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetMinimumAutomatchingPlayers(base.SelfPtr(), minimum);
            return this;
        }

        internal GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder SetVariant(uint variant)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetVariant(base.SelfPtr(), variant);
            return this;
        }
    }
}

