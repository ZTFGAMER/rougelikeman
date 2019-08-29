namespace GoogleMobileAds.Android
{
    using GoogleMobileAds.Api;
    using GoogleMobileAds.Api.Mediation;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class Utils
    {
        public const string AdListenerClassName = "com.google.android.gms.ads.AdListener";
        public const string AdRequestClassName = "com.google.android.gms.ads.AdRequest";
        public const string AdRequestBuilderClassName = "com.google.android.gms.ads.AdRequest$Builder";
        public const string AdSizeClassName = "com.google.android.gms.ads.AdSize";
        public const string AdMobExtrasClassName = "com.google.android.gms.ads.mediation.admob.AdMobExtras";
        public const string PlayStorePurchaseListenerClassName = "com.google.android.gms.ads.purchase.PlayStorePurchaseListener";
        public const string MobileAdsClassName = "com.google.android.gms.ads.MobileAds";
        public const string BannerViewClassName = "com.google.unity.ads.Banner";
        public const string InterstitialClassName = "com.google.unity.ads.Interstitial";
        public const string RewardBasedVideoClassName = "com.google.unity.ads.RewardBasedVideo";
        public const string NativeExpressAdViewClassName = "com.google.unity.ads.NativeExpressAd";
        public const string NativeAdLoaderClassName = "com.google.unity.ads.NativeAdLoader";
        public const string UnityAdListenerClassName = "com.google.unity.ads.UnityAdListener";
        public const string UnityRewardBasedVideoAdListenerClassName = "com.google.unity.ads.UnityRewardBasedVideoAdListener";
        public const string UnityAdLoaderListenerClassName = "com.google.unity.ads.UnityAdLoaderListener";
        public const string PluginUtilsClassName = "com.google.unity.ads.PluginUtils";
        public const string UnityActivityClassName = "com.unity3d.player.UnityPlayer";
        public const string BundleClassName = "android.os.Bundle";
        public const string DateClassName = "java.util.Date";

        public static AndroidJavaObject GetAdRequestJavaObject(AdRequest request)
        {
            AndroidJavaObject obj2 = new AndroidJavaObject("com.google.android.gms.ads.AdRequest$Builder", new object[0]);
            foreach (string str in request.Keywords)
            {
                object[] objArray1 = new object[] { str };
                obj2.Call<AndroidJavaObject>("addKeyword", objArray1);
            }
            foreach (string str2 in request.TestDevices)
            {
                if (str2 == "SIMULATOR")
                {
                    string @static = new AndroidJavaClass("com.google.android.gms.ads.AdRequest").GetStatic<string>("DEVICE_ID_EMULATOR");
                    object[] objArray2 = new object[] { @static };
                    obj2.Call<AndroidJavaObject>("addTestDevice", objArray2);
                }
                else
                {
                    object[] objArray3 = new object[] { str2 };
                    obj2.Call<AndroidJavaObject>("addTestDevice", objArray3);
                }
            }
            if (request.Birthday.HasValue)
            {
                DateTime valueOrDefault = request.Birthday.GetValueOrDefault();
                object[] objArray4 = new object[] { valueOrDefault.Year, valueOrDefault.Month, valueOrDefault.Day };
                AndroidJavaObject obj3 = new AndroidJavaObject("java.util.Date", objArray4);
                object[] objArray5 = new object[] { obj3 };
                obj2.Call<AndroidJavaObject>("setBirthday", objArray5);
            }
            if (request.Gender.HasValue)
            {
                int? nullable4 = null;
                switch (request.Gender.GetValueOrDefault())
                {
                    case Gender.Unknown:
                        nullable4 = new int?(new AndroidJavaClass("com.google.android.gms.ads.AdRequest").GetStatic<int>("GENDER_UNKNOWN"));
                        break;

                    case Gender.Male:
                        nullable4 = new int?(new AndroidJavaClass("com.google.android.gms.ads.AdRequest").GetStatic<int>("GENDER_MALE"));
                        break;

                    case Gender.Female:
                        nullable4 = new int?(new AndroidJavaClass("com.google.android.gms.ads.AdRequest").GetStatic<int>("GENDER_FEMALE"));
                        break;
                }
                if (nullable4.HasValue)
                {
                    object[] objArray6 = new object[] { nullable4 };
                    obj2.Call<AndroidJavaObject>("setGender", objArray6);
                }
            }
            if (request.TagForChildDirectedTreatment.HasValue)
            {
                object[] objArray7 = new object[] { request.TagForChildDirectedTreatment.GetValueOrDefault() };
                obj2.Call<AndroidJavaObject>("tagForChildDirectedTreatment", objArray7);
            }
            object[] args = new object[] { "unity-3.12.0" };
            obj2.Call<AndroidJavaObject>("setRequestAgent", args);
            AndroidJavaObject obj4 = new AndroidJavaObject("android.os.Bundle", new object[0]);
            foreach (KeyValuePair<string, string> pair in request.Extras)
            {
                object[] objArray9 = new object[] { pair.Key, pair.Value };
                obj4.Call("putString", objArray9);
            }
            object[] objArray10 = new object[] { "is_unity", "1" };
            obj4.Call("putString", objArray10);
            object[] objArray11 = new object[] { obj4 };
            AndroidJavaObject obj5 = new AndroidJavaObject("com.google.android.gms.ads.mediation.admob.AdMobExtras", objArray11);
            object[] objArray12 = new object[] { obj5 };
            obj2.Call<AndroidJavaObject>("addNetworkExtras", objArray12);
            foreach (MediationExtras extras in request.MediationExtras)
            {
                AndroidJavaObject obj6 = new AndroidJavaObject(extras.AndroidMediationExtraBuilderClassName, new object[0]);
                AndroidJavaObject obj7 = new AndroidJavaObject("java.util.HashMap", new object[0]);
                foreach (KeyValuePair<string, string> pair2 in extras.Extras)
                {
                    object[] objArray13 = new object[] { pair2.Key, pair2.Value };
                    obj7.Call<string>("put", objArray13);
                }
                object[] objArray14 = new object[] { obj7 };
                AndroidJavaObject obj8 = obj6.Call<AndroidJavaObject>("buildExtras", objArray14);
                if (obj8 != null)
                {
                    object[] objArray15 = new object[] { obj6.Call<AndroidJavaClass>("getAdapterClass", new object[0]), obj8 };
                    obj2.Call<AndroidJavaObject>("addNetworkExtrasBundle", objArray15);
                }
            }
            return obj2.Call<AndroidJavaObject>("build", new object[0]);
        }

        public static AndroidJavaObject GetAdSizeJavaObject(AdSize adSize)
        {
            if (adSize.IsSmartBanner)
            {
                return new AndroidJavaClass("com.google.android.gms.ads.AdSize").GetStatic<AndroidJavaObject>("SMART_BANNER");
            }
            return new AndroidJavaObject("com.google.android.gms.ads.AdSize", new object[] { adSize.Width, adSize.Height });
        }
    }
}

