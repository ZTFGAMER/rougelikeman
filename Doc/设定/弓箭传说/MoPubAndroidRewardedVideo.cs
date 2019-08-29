using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoPubAndroidRewardedVideo
{
    private readonly AndroidJavaObject _plugin;
    private readonly Dictionary<MoPubBase.Reward, AndroidJavaObject> _rewardsDict = new Dictionary<MoPubBase.Reward, AndroidJavaObject>();

    public MoPubAndroidRewardedVideo(string adUnitId)
    {
        object[] args = new object[] { adUnitId };
        this._plugin = new AndroidJavaObject("com.mopub.unity.MoPubRewardedVideoUnityPlugin", args);
    }

    public List<MoPubBase.Reward> GetAvailableRewards()
    {
        this._rewardsDict.Clear();
        using (AndroidJavaObject obj2 = this._plugin.Call<AndroidJavaObject>("getAvailableRewards", Array.Empty<object>()))
        {
            AndroidJavaObject[] objArray = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(obj2.GetRawObject());
            if (objArray.Length <= 1)
            {
                return new List<MoPubBase.Reward>(this._rewardsDict.Keys);
            }
            foreach (AndroidJavaObject obj3 in objArray)
            {
                MoPubBase.Reward key = new MoPubBase.Reward {
                    Label = obj3.Call<string>("getLabel", Array.Empty<object>()),
                    Amount = obj3.Call<int>("getAmount", Array.Empty<object>())
                };
                this._rewardsDict.Add(key, obj3);
            }
        }
        return new List<MoPubBase.Reward>(this._rewardsDict.Keys);
    }

    public bool HasRewardedVideo() => 
        this._plugin.Call<bool>("hasRewardedVideo", Array.Empty<object>());

    public void RequestRewardedVideo(List<MoPubBase.LocalMediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
    {
        string str = MoPubBase.LocalMediationSetting.ToJson(mediationSettings);
        object[] args = new object[] { str, keywords, userDataKeywords, latitude, longitude, customerId };
        this._plugin.Call("requestRewardedVideo", args);
    }

    public void SelectReward(MoPubBase.Reward selectedReward)
    {
        if (this._rewardsDict.TryGetValue(selectedReward, out AndroidJavaObject obj2))
        {
            object[] args = new object[] { obj2 };
            this._plugin.Call("selectReward", args);
        }
        else
        {
            Debug.LogWarning($"Selected reward {selectedReward} is not available.");
        }
    }

    public void ShowRewardedVideo(string customData)
    {
        object[] args = new object[] { customData };
        this._plugin.Call("showRewardedVideo", args);
    }
}

