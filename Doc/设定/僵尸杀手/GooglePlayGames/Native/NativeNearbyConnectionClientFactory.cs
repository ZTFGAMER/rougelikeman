namespace GooglePlayGames.Native
{
    using GooglePlayGames.Android;
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class NativeNearbyConnectionClientFactory
    {
        private static volatile NearbyConnectionsManager sManager;
        private static Action<INearbyConnectionClient> sCreationCallback;
        [CompilerGenerated]
        private static Action<NearbyConnectionsStatus.InitializationStatus> <>f__mg$cache0;

        public static void Create(Action<INearbyConnectionClient> callback)
        {
            if (sManager == null)
            {
                sCreationCallback = callback;
                InitializeFactory();
            }
            else
            {
                callback(new NativeNearbyConnectionsClient(GetManager()));
            }
        }

        internal static NearbyConnectionsManager GetManager() => 
            sManager;

        internal static void InitializeFactory()
        {
            PlayGamesHelperObject.CreateObject();
            NearbyConnectionsManager.ReadServiceId();
            NearbyConnectionsManagerBuilder builder = new NearbyConnectionsManagerBuilder();
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Action<NearbyConnectionsStatus.InitializationStatus>(NativeNearbyConnectionClientFactory.OnManagerInitialized);
            }
            builder.SetOnInitializationFinished(<>f__mg$cache0);
            PlatformConfiguration configuration = new AndroidClient().CreatePlatformConfiguration(PlayGamesClientConfiguration.DefaultConfiguration);
            Debug.Log("Building manager Now");
            sManager = builder.Build(configuration);
        }

        internal static void OnManagerInitialized(NearbyConnectionsStatus.InitializationStatus status)
        {
            Debug.Log(string.Concat(new object[] { "Nearby Init Complete: ", status, " sManager = ", sManager }));
            if (status == NearbyConnectionsStatus.InitializationStatus.VALID)
            {
                if (sCreationCallback != null)
                {
                    sCreationCallback(new NativeNearbyConnectionsClient(GetManager()));
                    sCreationCallback = null;
                }
            }
            else
            {
                Debug.LogError("ERROR: NearbyConnectionManager not initialized: " + status);
            }
        }
    }
}

