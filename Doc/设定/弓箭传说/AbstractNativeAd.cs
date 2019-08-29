using MoPubInternal.ThirdParty.MiniJSON;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractNativeAd : MonoBehaviour
{
    public string AdUnitId;
    [Header("Text")]
    public UnityEngine.UI.Text Title;
    public UnityEngine.UI.Text Text;
    public UnityEngine.UI.Text CallToAction;
    [Header("Images")]
    public Renderer MainImage;
    public Renderer IconImage;
    public Renderer PrivacyInformationIconImage;

    protected AbstractNativeAd()
    {
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Data
    {
        public Uri MainImageUrl;
        public Uri IconImageUrl;
        public Uri ClickDestinationUrl;
        public string CallToAction;
        public string Title;
        public string Text;
        public double StarRating;
        public Uri PrivacyInformationIconClickThroughUrl;
        public Uri PrivacyInformationIconImageUrl;
        public static Uri ToUri(object value)
        {
            Uri uri = value as Uri;
            if (uri != null)
            {
                return uri;
            }
            string str = value as string;
            if (!string.IsNullOrEmpty(str))
            {
                if (Uri.IsWellFormedUriString(str, UriKind.Absolute))
                {
                    return new Uri(str, UriKind.Absolute);
                }
                Debug.LogError("Invalid URL: " + str);
            }
            return null;
        }

        public static AbstractNativeAd.Data FromJson(string json)
        {
            Dictionary<string, object> dictionary;
            Dictionary<string, object> dictionary1 = Json.Deserialize(json) as Dictionary<string, object>;
            if (dictionary1 != null)
            {
                dictionary = dictionary1;
            }
            else
            {
                dictionary = new Dictionary<string, object>();
            }
            return new AbstractNativeAd.Data { 
                MainImageUrl = !dictionary.TryGetValue("mainImageUrl", out object obj2) ? null : ToUri(obj2),
                IconImageUrl = !dictionary.TryGetValue("iconImageUrl", out obj2) ? null : ToUri(obj2),
                ClickDestinationUrl = !dictionary.TryGetValue("clickDestinationUrl", out obj2) ? null : ToUri(obj2),
                CallToAction = !dictionary.TryGetValue("callToAction", out obj2) ? string.Empty : (obj2 as string),
                Title = !dictionary.TryGetValue("title", out obj2) ? string.Empty : (obj2 as string),
                Text = !dictionary.TryGetValue("text", out obj2) ? string.Empty : (obj2 as string),
                StarRating = !dictionary.TryGetValue("starRating", out obj2) ? 0.0 : ((double) obj2),
                PrivacyInformationIconClickThroughUrl = !dictionary.TryGetValue("privacyInformationIconClickThroughUrl", out obj2) ? null : ToUri(obj2),
                PrivacyInformationIconImageUrl = !dictionary.TryGetValue("privacyInformationIconImageUrl", out obj2) ? null : ToUri(obj2)
            };
        }

        public override string ToString() => 
            $"mainImageUrl: {this.MainImageUrl}
iconImageUrl: {this.IconImageUrl}
clickDestinationUrl: {this.ClickDestinationUrl}
callToAction: {this.CallToAction}
title: {this.Title}
text: {this.Text}
starRating: {this.StarRating}
privacyInformationIconClickThroughUrl: {this.PrivacyInformationIconClickThroughUrl}
privacyInformationIconImageUrl: {this.PrivacyInformationIconImageUrl}";
    }
}

