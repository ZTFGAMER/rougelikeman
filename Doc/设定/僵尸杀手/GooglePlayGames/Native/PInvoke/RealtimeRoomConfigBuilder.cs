namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class RealtimeRoomConfigBuilder : BaseReferenceHolder
    {
        internal RealtimeRoomConfigBuilder(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal RealtimeRoomConfigBuilder AddInvitedPlayer(string playerId)
        {
            RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_AddPlayerToInvite(base.SelfPtr(), playerId);
            return this;
        }

        internal RealtimeRoomConfig Build() => 
            new RealtimeRoomConfig(RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Create(base.SelfPtr()));

        protected override void CallDispose(HandleRef selfPointer)
        {
            RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Dispose(selfPointer);
        }

        internal static RealtimeRoomConfigBuilder Create() => 
            new RealtimeRoomConfigBuilder(RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Construct());

        internal RealtimeRoomConfigBuilder PopulateFromUIResponse(PlayerSelectUIResponse response)
        {
            RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_PopulateFromPlayerSelectUIResponse(base.SelfPtr(), response.AsPointer());
            return this;
        }

        internal RealtimeRoomConfigBuilder SetExclusiveBitMask(ulong bitmask)
        {
            RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetExclusiveBitMask(base.SelfPtr(), bitmask);
            return this;
        }

        internal RealtimeRoomConfigBuilder SetMaximumAutomatchingPlayers(uint maximum)
        {
            RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetMaximumAutomatchingPlayers(base.SelfPtr(), maximum);
            return this;
        }

        internal RealtimeRoomConfigBuilder SetMinimumAutomatchingPlayers(uint minimum)
        {
            RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetMinimumAutomatchingPlayers(base.SelfPtr(), minimum);
            return this;
        }

        internal RealtimeRoomConfigBuilder SetVariant(uint variantValue)
        {
            uint variant = (variantValue != 0) ? variantValue : uint.MaxValue;
            RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetVariant(base.SelfPtr(), variant);
            return this;
        }
    }
}

