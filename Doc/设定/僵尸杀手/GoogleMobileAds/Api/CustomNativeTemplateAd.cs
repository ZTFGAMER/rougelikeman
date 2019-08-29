namespace GoogleMobileAds.Api
{
    using GoogleMobileAds.Common;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CustomNativeTemplateAd
    {
        private ICustomNativeTemplateClient client;

        internal CustomNativeTemplateAd(ICustomNativeTemplateClient client)
        {
            this.client = client;
        }

        public List<string> GetAvailableAssetNames() => 
            this.client.GetAvailableAssetNames();

        public string GetCustomTemplateId() => 
            this.client.GetTemplateId();

        public string GetText(string key) => 
            this.client.GetText(key);

        public Texture2D GetTexture2D(string key)
        {
            byte[] imageByteArray = this.client.GetImageByteArray(key);
            if (imageByteArray == null)
            {
                return null;
            }
            return Utils.GetTexture2DFromByteArray(imageByteArray);
        }

        public void PerformClick(string assetName)
        {
            this.client.PerformClick(assetName);
        }

        public void RecordImpression()
        {
            this.client.RecordImpression();
        }
    }
}

