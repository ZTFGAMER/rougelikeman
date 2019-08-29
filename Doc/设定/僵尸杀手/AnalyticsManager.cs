using Analytics;
using Facebook.Unity;
using Firebase.Analytics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static AnalyticsManager <instance>k__BackingField;
    public static string uniqueUserId;
    public static string START = "SessionStart";
    [Header("Flurry Settings"), SerializeField]
    private string _iosApiKey = string.Empty;
    [SerializeField]
    private string _androidApiKey = string.Empty;
    [Header("Appsflyer"), SerializeField, Space]
    private string _appsFlyerDevKey = string.Empty;
    [SerializeField]
    private string _iosAppsKey;
    [SerializeField]
    private string _androidAppId;

    public void AppsFlyerInit()
    {
        AppsFlyer.setAppsFlyerKey(this._appsFlyerDevKey);
        AppsFlyer.setAppID(this._androidAppId);
        AppsFlyer.init(this._appsFlyerDevKey, "AppsFlyerTrackerCallbacks");
        UnityEngine.Debug.Log("AppsFlyer initialized");
    }

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }
        if (!FB.IsInitialized)
        {
            FB.Init(new InitDelegate(this.InitCallback), new HideUnityDelegate(this.OnHideUnity), null);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void Init()
    {
        FirebaseAnalytics.SetUserId(uniqueUserId);
        FirebaseAnalytics.LogEvent(START);
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        if (MonoSingleton<Flurry>.Instance != null)
        {
            MonoSingleton<Flurry>.Instance.SetLogLevel(Analytics.LogLevel.CriticalOnly);
            MonoSingleton<Flurry>.Instance.StartSession(this._iosApiKey, this._androidApiKey);
            MonoSingleton<Flurry>.Instance.LogUserID(uniqueUserId);
            MonoSingleton<Flurry>.Instance.LogEvent(START);
        }
        this.AppsFlyerInit();
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            FB.LogAppEvent(START, null, null);
        }
        else
        {
            UnityEngine.Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    public void LogEvent(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName.Replace(" ", string.Empty));
        if (MonoSingleton<Flurry>.Instance != null)
        {
            MonoSingleton<Flurry>.Instance.LogEvent(eventName);
        }
        FB.LogAppEvent(eventName, null, null);
        AppsFlyer.trackEvent(eventName, null);
    }

    public void LogEvent(string eventName, Dictionary<string, string> eventParameters)
    {
        Parameter[] parameters = new Parameter[eventParameters.Count];
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        int index = 0;
        foreach (KeyValuePair<string, string> pair in eventParameters)
        {
            string parameterName = pair.Key.Replace(" ", string.Empty);
            parameters[index] = new Parameter(parameterName, pair.Value.Replace(" ", string.Empty));
            index++;
            dictionary.Add(pair.Key, pair.Value);
        }
        FirebaseAnalytics.LogEvent(eventName.Replace(" ", string.Empty), parameters);
        if (MonoSingleton<Flurry>.Instance != null)
        {
            MonoSingleton<Flurry>.Instance.LogEvent(eventName, eventParameters);
        }
        FB.LogAppEvent(eventName, null, dictionary);
        AppsFlyer.trackRichEvent(eventName, eventParameters);
    }

    public void LogPurchaseEvent(string eventName, Dictionary<string, string> eventParameters, float logPurchase, string currency)
    {
        Parameter[] parameters = new Parameter[eventParameters.Count];
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        int index = 0;
        foreach (KeyValuePair<string, string> pair in eventParameters)
        {
            string parameterName = pair.Key.Replace(" ", string.Empty);
            parameters[index] = new Parameter(parameterName, pair.Value.Replace(" ", string.Empty));
            index++;
            dictionary.Add(pair.Key, pair.Value);
        }
        FirebaseAnalytics.LogEvent(eventName.Replace(" ", string.Empty), parameters);
        if (MonoSingleton<Flurry>.Instance != null)
        {
            MonoSingleton<Flurry>.Instance.LogEvent(eventName, eventParameters);
        }
        FB.LogPurchase(logPurchase, currency, dictionary);
        eventParameters.Add("af_revenue", logPurchase.ToString());
        eventParameters.Add("af_currency", currency);
        AppsFlyer.trackRichEvent(eventName, eventParameters);
    }

    public void OnAdvertisingIdentifierRecieved(string advertisingId, bool trackingEnabeld, string error)
    {
        uniqueUserId = advertisingId;
        this.Init();
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void Start()
    {
        FlurryAndroid.SetLogEnabled(true);
        if (!Application.RequestAdvertisingIdentifierAsync(new Application.AdvertisingIdentifierCallback(this.OnAdvertisingIdentifierRecieved)))
        {
            this.Init();
        }
    }

    public static AnalyticsManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }
}

