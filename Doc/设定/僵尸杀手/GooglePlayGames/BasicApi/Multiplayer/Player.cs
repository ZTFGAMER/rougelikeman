namespace GooglePlayGames.BasicApi.Multiplayer
{
    using GooglePlayGames;
    using System;

    public class Player : PlayGamesUserProfile
    {
        internal Player(string displayName, string playerId, string avatarUrl) : base(displayName, playerId, avatarUrl)
        {
        }
    }
}

