namespace GoogleMobileAds.Android
{
    using GoogleMobileAds.Common;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class CustomNativeTemplateClient : ICustomNativeTemplateClient
    {
        private AndroidJavaObject customNativeAd;

        public CustomNativeTemplateClient(AndroidJavaObject customNativeAd)
        {
            this.customNativeAd = customNativeAd;
        }

        public List<string> GetAvailableAssetNames() => 
            new List<string>(this.customNativeAd.Call<string[]>("getAvailableAssetNames", new object[0]));

        public byte[] GetImageByteArray(string key)
        {
            object[] args = new object[] { key };
            byte[] buffer = this.customNativeAd.Call<byte[]>("getImage", args);
            if (buffer.Length == 0)
            {
                return null;
            }
            return buffer;
        }

        public string GetTemplateId() => 
            this.customNativeAd.Call<string>("getTemplateId", new object[0]);

        public string GetText(string key)
        {
            object[] args = new object[] { key };
            string str = this.customNativeAd.Call<string>("getText", args);
            if (str.Equals(string.Empty))
            {
                return null;
            }
            return str;
        }

        public void PerformClick(string assetName)
        {
            object[] args = new object[] { assetName };
            this.customNativeAd.Call("performClick", args);
        }

        public void RecordImpression()
        {
            this.customNativeAd.Call("recordImpression", new object[0]);
        }
    }
}

