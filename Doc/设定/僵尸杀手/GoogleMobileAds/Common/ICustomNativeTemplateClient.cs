namespace GoogleMobileAds.Common
{
    using System;
    using System.Collections.Generic;

    public interface ICustomNativeTemplateClient
    {
        List<string> GetAvailableAssetNames();
        byte[] GetImageByteArray(string key);
        string GetTemplateId();
        string GetText(string key);
        void PerformClick(string assetName);
        void RecordImpression();
    }
}

