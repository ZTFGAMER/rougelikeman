namespace AudienceNetwork.Utility
{
    using System;
    using UnityEngine;

    internal class AdUtilityBridgeAndroid : AdUtilityBridge
    {
        public override double convert(double deviceSize) => 
            (deviceSize / this.density());

        private double density() => 
            ((double) this.getPropertyOfDisplayMetrics<float>("density"));

        public override double deviceHeight() => 
            ((double) this.getPropertyOfDisplayMetrics<int>("heightPixels"));

        public override double deviceWidth() => 
            ((double) this.getPropertyOfDisplayMetrics<int>("widthPixels"));

        private T getPropertyOfDisplayMetrics<T>(string property)
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            return class2.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext", new object[0]).Call<AndroidJavaObject>("getResources", new object[0]).Call<AndroidJavaObject>("getDisplayMetrics", new object[0]).Get<T>(property);
        }

        public override double height() => 
            this.convert(this.deviceHeight());

        public override void prepare()
        {
            try
            {
                AndroidJavaClass class2 = new AndroidJavaClass("com.facebook.ads.internal.DisplayAdController");
                object[] args = new object[] { true };
                class2.CallStatic("setMainThreadForced", args);
            }
            catch (Exception)
            {
            }
            try
            {
                new AndroidJavaClass("android.os.Looper").CallStatic("prepare", new object[0]);
            }
            catch (Exception)
            {
            }
        }

        public override double width() => 
            this.convert(this.deviceWidth());
    }
}

