namespace GooglePlayGames
{
    using GooglePlayGames.Android;
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native;
    using GooglePlayGames.OurUtils;
    using System;
    using UnityEngine;

    internal class PlayGamesClientFactory
    {
        internal static IPlayGamesClient GetPlatformPlayGamesClient(PlayGamesClientConfiguration config)
        {
            if (Application.isEditor)
            {
                Logger.d("Creating IPlayGamesClient in editor, using DummyClient.");
                return new DummyClient();
            }
            Logger.d("Creating Android IPlayGamesClient Client");
            return new NativeClient(config, new AndroidClient());
        }
    }
}

