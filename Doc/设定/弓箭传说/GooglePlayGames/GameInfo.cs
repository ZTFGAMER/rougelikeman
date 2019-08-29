namespace GooglePlayGames
{
    using System;

    public static class GameInfo
    {
        private const string UnescapedApplicationId = "APP_ID";
        private const string UnescapedIosClientId = "IOS_CLIENTID";
        private const string UnescapedWebClientId = "WEB_CLIENTID";
        private const string UnescapedNearbyServiceId = "NEARBY_SERVICE_ID";
        public const string ApplicationId = "689911175141";
        public const string IosClientId = "__IOS_CLIENTID__";
        public const string WebClientId = "689911175141-n6mehbb5hf6srhh55dn9ks3mj7dfi73u.apps.googleusercontent.com";
        public const string NearbyConnectionServiceId = "";

        public static bool ApplicationIdInitialized() => 
            (!string.IsNullOrEmpty("689911175141") && !"689911175141".Equals(ToEscapedToken("APP_ID")));

        public static bool IosClientIdInitialized() => 
            (!string.IsNullOrEmpty("__IOS_CLIENTID__") && !"__IOS_CLIENTID__".Equals(ToEscapedToken("IOS_CLIENTID")));

        public static bool NearbyConnectionsInitialized() => 
            (!string.IsNullOrEmpty(string.Empty) && !string.Empty.Equals(ToEscapedToken("NEARBY_SERVICE_ID")));

        private static string ToEscapedToken(string token) => 
            $"__{token}__";

        public static bool WebClientIdInitialized() => 
            (!string.IsNullOrEmpty("689911175141-n6mehbb5hf6srhh55dn9ks3mj7dfi73u.apps.googleusercontent.com") && !"689911175141-n6mehbb5hf6srhh55dn9ks3mj7dfi73u.apps.googleusercontent.com".Equals(ToEscapedToken("WEB_CLIENTID")));
    }
}

