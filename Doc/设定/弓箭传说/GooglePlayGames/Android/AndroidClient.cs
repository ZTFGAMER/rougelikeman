namespace GooglePlayGames.Android
{
    using Com.Google.Android.Gms.Common.Api;
    using Com.Google.Android.Gms.Games;
    using Com.Google.Android.Gms.Games.Stats;
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AndroidClient : IClientImpl
    {
        internal const string BridgeActivityClass = "com.google.games.bridge.NativeBridgeActivity";
        private const string LaunchBridgeMethod = "launchBridgeIntent";
        private const string LaunchBridgeSignature = "(Landroid/app/Activity;Landroid/content/Intent;)V";
        private TokenClient tokenClient;
        private static AndroidJavaObject invisible;
        [CompilerGenerated]
        private static Action<IntPtr> <>f__am$cache0;

        public PlatformConfiguration CreatePlatformConfiguration(PlayGamesClientConfiguration clientConfig)
        {
            AndroidPlatformConfiguration configuration = AndroidPlatformConfiguration.Create();
            using (AndroidJavaObject obj2 = AndroidTokenClient.GetActivity())
            {
                configuration.SetActivity(obj2.GetRawObject());
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate (IntPtr intent) {
                        <CreatePlatformConfiguration>c__AnonStorey0 storey = new <CreatePlatformConfiguration>c__AnonStorey0 {
                            intentRef = AndroidJNI.NewGlobalRef(intent)
                        };
                        PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
                    };
                }
                configuration.SetOptionalIntentHandlerForUI(<>f__am$cache0);
                if (clientConfig.IsHidingPopups)
                {
                    configuration.SetOptionalViewForPopups(AndroidTokenClient.CreateInvisibleView());
                }
            }
            return configuration;
        }

        public TokenClient CreateTokenClient(bool reset)
        {
            if (this.tokenClient == null)
            {
                this.tokenClient = new AndroidTokenClient();
            }
            else if (reset)
            {
                this.tokenClient.Signout();
            }
            return this.tokenClient;
        }

        public void GetPlayerStats(IntPtr apiClient, Action<CommonStatusCodes, PlayerStats> callback)
        {
            StatsResultCallback callback2;
            <GetPlayerStats>c__AnonStorey1 storey = new <GetPlayerStats>c__AnonStorey1 {
                callback = callback
            };
            GoogleApiClient client = new GoogleApiClient(apiClient);
            try
            {
                callback2 = new StatsResultCallback(new Action<int, PlayerStats>(storey.<>m__0));
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                storey.callback(CommonStatusCodes.DeveloperError, null);
                return;
            }
            Com.Google.Android.Gms.Games.Games.Stats.loadPlayerStats(client, true).setResultCallback(callback2);
        }

        private static void LaunchBridgeIntent(IntPtr bridgedIntent)
        {
            object[] args = new object[2];
            jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(args);
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.google.games.bridge.NativeBridgeActivity"))
                {
                    using (AndroidJavaObject obj2 = AndroidTokenClient.GetActivity())
                    {
                        IntPtr methodID = AndroidJNI.GetStaticMethodID(class2.GetRawClass(), "launchBridgeIntent", "(Landroid/app/Activity;Landroid/content/Intent;)V");
                        jvalueArray[0].l = obj2.GetRawObject();
                        jvalueArray[1].l = bridgedIntent;
                        AndroidJNI.CallStaticVoidMethod(class2.GetRawClass(), methodID, jvalueArray);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.e("Exception launching bridge intent: " + exception.Message);
                Logger.e(exception.ToString());
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
        }

        public void SetGravityForPopups(IntPtr apiClient, Gravity gravity)
        {
            GoogleApiClient client = new GoogleApiClient(apiClient);
            Com.Google.Android.Gms.Games.Games.setGravityForPopups(client, ((int) gravity) | 1);
        }

        public void Signout()
        {
            if (this.tokenClient != null)
            {
                this.tokenClient.Signout();
            }
        }

        [CompilerGenerated]
        private sealed class <CreatePlatformConfiguration>c__AnonStorey0
        {
            internal IntPtr intentRef;

            internal void <>m__0()
            {
                try
                {
                    AndroidClient.LaunchBridgeIntent(this.intentRef);
                }
                finally
                {
                    AndroidJNI.DeleteGlobalRef(this.intentRef);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetPlayerStats>c__AnonStorey1
        {
            internal Action<CommonStatusCodes, PlayerStats> callback;

            internal void <>m__0(int result, PlayerStats stats)
            {
                Debug.Log("Result for getStats: " + result);
                PlayerStats stats2 = null;
                if (stats != null)
                {
                    stats2 = new PlayerStats {
                        AvgSessonLength = stats.getAverageSessionLength(),
                        DaysSinceLastPlayed = stats.getDaysSinceLastPlayed(),
                        NumberOfPurchases = stats.getNumberOfPurchases(),
                        NumberOfSessions = stats.getNumberOfSessions(),
                        SessPercentile = stats.getSessionPercentile(),
                        SpendPercentile = stats.getSpendPercentile(),
                        ChurnProbability = stats.getChurnProbability(),
                        SpendProbability = stats.getSpendProbability(),
                        HighSpenderProbability = stats.getHighSpenderProbability(),
                        TotalSpendNext28Days = stats.getTotalSpendNext28Days()
                    };
                }
                this.callback((CommonStatusCodes) result, stats2);
            }
        }

        private class StatsResultCallback : ResultCallbackProxy<Stats_LoadPlayerStatsResultObject>
        {
            private Action<int, PlayerStats> callback;

            public StatsResultCallback(Action<int, PlayerStats> callback)
            {
                this.callback = callback;
            }

            public override void OnResult(Stats_LoadPlayerStatsResultObject arg_Result_1)
            {
                this.callback(arg_Result_1.getStatus().getStatusCode(), arg_Result_1.getPlayerStats());
            }
        }
    }
}

