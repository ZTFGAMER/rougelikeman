namespace GooglePlayGames
{
    using System;

    public static class GameInfo
    {
        private const string UnescapedApplicationId = "APP_ID";
        private const string UnescapedIosClientId = "IOS_CLIENTID";
        private const string UnescapedWebClientId = "WEB_CLIENTID";
        private const string UnescapedNearbyServiceId = "NEARBY_SERVICE_ID";
        public const string ApplicationId = "1019213254355";
        public const string IosClientId = "__IOS_CLIENTID__";
        public const string WebClientId = "";
        public const string NearbyConnectionServiceId = "";

        public static bool ApplicationIdInitialized() => 
            (!string.IsNullOrEmpty("1019213254355") && !"1019213254355".Equals(ToEscapedToken("APP_ID")));

        public static bool IosClientIdInitialized() => 
            (!string.IsNullOrEmpty("__IOS_CLIENTID__") && !"__IOS_CLIENTID__".Equals(ToEscapedToken("IOS_CLIENTID")));

        public static bool NearbyConnectionsInitialized() => 
            (!string.IsNullOrEmpty(string.Empty) && !string.Empty.Equals(ToEscapedToken("NEARBY_SERVICE_ID")));

        private static string ToEscapedToken(string token) => 
            $"__{token}__";

        public static bool WebClientIdInitialized() => 
            (!string.IsNullOrEmpty(string.Empty) && !string.Empty.Equals(ToEscapedToken("WEB_CLIENTID")));
    }
}

