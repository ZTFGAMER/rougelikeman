namespace GooglePlayGames.OurUtils
{
    using System;
    using UnityEngine;

    public static class PlatformUtils
    {
        public static bool Supported
        {
            get
            {
                AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject obj3 = class2.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getPackageManager", new object[0]);
                AndroidJavaObject obj4 = null;
                try
                {
                    object[] args = new object[] { "com.google.android.play.games" };
                    obj4 = obj3.Call<AndroidJavaObject>("getLaunchIntentForPackage", args);
                }
                catch (Exception)
                {
                    return false;
                }
                return (obj4 != null);
            }
        }
    }
}

