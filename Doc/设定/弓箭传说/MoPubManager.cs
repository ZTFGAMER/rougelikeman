using MoPubInternal.ThirdParty.MiniJSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class MoPubManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static MoPubManager <Instance>k__BackingField;
    [CompilerGenerated]
    private static Func<object, string> <>f__am$cache0;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnAdClickedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnAdCollapsedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnAdExpandedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string, string> OnAdFailedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string, float> OnAdLoadedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnConsentDialogFailedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action OnConsentDialogLoadedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action OnConsentDialogShownEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> OnConsentStatusChangedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnInterstitialClickedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnInterstitialDismissedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnInterstitialExpiredEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string, string> OnInterstitialFailedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnInterstitialLoadedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnInterstitialShownEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnRewardedVideoClickedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnRewardedVideoClosedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnRewardedVideoExpiredEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string, string> OnRewardedVideoFailedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string, string> OnRewardedVideoFailedToPlayEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnRewardedVideoLeavingApplicationEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnRewardedVideoLoadedEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string, string, float> OnRewardedVideoReceivedRewardEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnRewardedVideoShownEvent;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event Action<string> OnSdkInitializedEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Object.DontDestroyOnLoad(base.gameObject);
        }
        else
        {
            Object.Destroy(this);
        }
    }

    private string[] DecodeArgs(string argsJson, int min)
    {
        bool flag = false;
        List<object> source = Json.Deserialize(argsJson) as List<object>;
        if (source == null)
        {
            Debug.LogError("Invalid JSON data: " + argsJson);
            source = new List<object>();
            flag = true;
        }
        if (source.Count < min)
        {
            if (!flag)
            {
                Debug.LogError(string.Concat(new object[] { "Missing one or more values: ", argsJson, " (expected ", min, ")" }));
            }
            while (source.Count < min)
            {
                source.Add(string.Empty);
            }
        }
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = v => v.ToString();
        }
        return source.Select<object, string>(<>f__am$cache0).ToArray<string>();
    }

    public void EmitAdClickedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitAdClickedEvent", "Ad tapped", Array.Empty<object>());
        Action<string> onAdClickedEvent = OnAdClickedEvent;
        if (onAdClickedEvent != null)
        {
            onAdClickedEvent(str);
        }
    }

    public void EmitAdCollapsedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitAdCollapsedEvent", "Ad collapsed", Array.Empty<object>());
        Action<string> onAdCollapsedEvent = OnAdCollapsedEvent;
        if (onAdCollapsedEvent != null)
        {
            onAdCollapsedEvent(str);
        }
    }

    public void EmitAdExpandedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitAdExpandedEvent", "Ad expanded", Array.Empty<object>());
        Action<string> onAdExpandedEvent = OnAdExpandedEvent;
        if (onAdExpandedEvent != null)
        {
            onAdExpandedEvent(str);
        }
    }

    public void EmitAdFailedEvent(string argsJson)
    {
        string[] strArray = this.DecodeArgs(argsJson, 2);
        string str = strArray[0];
        string str2 = strArray[1];
        object[] args = new object[] { str2 };
        MoPubLog.Log("EmitAdFailedEvent", "Ad failed to load: ({0}) {1}", args);
        Action<string, string> onAdFailedEvent = OnAdFailedEvent;
        if (onAdFailedEvent != null)
        {
            onAdFailedEvent(str, str2);
        }
    }

    public void EmitAdLoadedEvent(string argsJson)
    {
        string[] strArray = this.DecodeArgs(argsJson, 2);
        string str = strArray[0];
        string s = strArray[1];
        MoPubLog.Log("EmitAdLoadedEvent", "Ad loaded", Array.Empty<object>());
        MoPubLog.Log("EmitAdLoadedEvent", "Ad shown", Array.Empty<object>());
        Action<string, float> onAdLoadedEvent = OnAdLoadedEvent;
        if (onAdLoadedEvent != null)
        {
            onAdLoadedEvent(str, float.Parse(s));
        }
    }

    public void EmitConsentDialogFailedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        object[] args = new object[] { str };
        MoPubLog.Log("EmitConsentDialogFailedEvent", "Consent dialog failed: ({0}) {1}", args);
        Action<string> onConsentDialogFailedEvent = OnConsentDialogFailedEvent;
        if (onConsentDialogFailedEvent != null)
        {
            onConsentDialogFailedEvent(str);
        }
    }

    public void EmitConsentDialogLoadedEvent()
    {
        MoPubLog.Log("EmitConsentDialogLoadedEvent", "Consent dialog loaded", Array.Empty<object>());
        Action onConsentDialogLoadedEvent = OnConsentDialogLoadedEvent;
        if (onConsentDialogLoadedEvent != null)
        {
            onConsentDialogLoadedEvent();
        }
    }

    public void EmitConsentDialogShownEvent()
    {
        MoPubLog.Log("EmitConsentDialogShownEvent", "Sucessfully showed consent dialog", Array.Empty<object>());
        Action onConsentDialogShownEvent = OnConsentDialogShownEvent;
        if (onConsentDialogShownEvent != null)
        {
            onConsentDialogShownEvent();
        }
    }

    public void EmitConsentStatusChangedEvent(string argsJson)
    {
        string[] strArray = this.DecodeArgs(argsJson, 3);
        MoPubBase.Consent.Status status = MoPubBase.Consent.FromString(strArray[0]);
        MoPubBase.Consent.Status status2 = MoPubBase.Consent.FromString(strArray[1]);
        bool flag = strArray[2].ToLower() == "true";
        object[] args = new object[] { status2, flag };
        MoPubLog.Log("EmitConsentStatusChangedEvent", "Consent changed to {0} from {1}: PII can{2} be collected. Reason: {3}", args);
        Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> onConsentStatusChangedEvent = OnConsentStatusChangedEvent;
        if (onConsentStatusChangedEvent != null)
        {
            onConsentStatusChangedEvent(status, status2, flag);
        }
    }

    public void EmitInterstitialClickedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitInterstitialClickedEvent", "Ad tapped", Array.Empty<object>());
        Action<string> onInterstitialClickedEvent = OnInterstitialClickedEvent;
        if (onInterstitialClickedEvent != null)
        {
            onInterstitialClickedEvent(str);
        }
    }

    public void EmitInterstitialDidExpireEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitInterstitialDidExpireEvent", "Ad expired since it was not shown within {0} minutes of it being loaded", Array.Empty<object>());
        Action<string> onInterstitialExpiredEvent = OnInterstitialExpiredEvent;
        if (onInterstitialExpiredEvent != null)
        {
            onInterstitialExpiredEvent(str);
        }
    }

    public void EmitInterstitialDismissedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitInterstitialDismissedEvent", "Ad did disappear", Array.Empty<object>());
        Action<string> onInterstitialDismissedEvent = OnInterstitialDismissedEvent;
        if (onInterstitialDismissedEvent != null)
        {
            onInterstitialDismissedEvent(str);
        }
    }

    public void EmitInterstitialFailedEvent(string argsJson)
    {
        string[] strArray = this.DecodeArgs(argsJson, 2);
        string str = strArray[0];
        string str2 = strArray[1];
        object[] args = new object[] { str2 };
        MoPubLog.Log("EmitInterstitialFailedEvent", "Ad failed to load: ({0}) {1}", args);
        Action<string, string> onInterstitialFailedEvent = OnInterstitialFailedEvent;
        if (onInterstitialFailedEvent != null)
        {
            onInterstitialFailedEvent(str, str2);
        }
    }

    public void EmitInterstitialLoadedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitInterstitialLoadedEvent", "Ad loaded", Array.Empty<object>());
        Action<string> onInterstitialLoadedEvent = OnInterstitialLoadedEvent;
        if (onInterstitialLoadedEvent != null)
        {
            onInterstitialLoadedEvent(str);
        }
    }

    public void EmitInterstitialShownEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitInterstitialShownEvent", "Ad shown", Array.Empty<object>());
        Action<string> onInterstitialShownEvent = OnInterstitialShownEvent;
        if (onInterstitialShownEvent != null)
        {
            onInterstitialShownEvent(str);
        }
    }

    public void EmitRewardedVideoClickedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitRewardedVideoClickedEvent", "Ad tapped", Array.Empty<object>());
        Action<string> onRewardedVideoClickedEvent = OnRewardedVideoClickedEvent;
        if (onRewardedVideoClickedEvent != null)
        {
            onRewardedVideoClickedEvent(str);
        }
    }

    public void EmitRewardedVideoClosedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitRewardedVideoClosedEvent", "Ad did disappear", Array.Empty<object>());
        Action<string> onRewardedVideoClosedEvent = OnRewardedVideoClosedEvent;
        if (onRewardedVideoClosedEvent != null)
        {
            onRewardedVideoClosedEvent(str);
        }
    }

    public void EmitRewardedVideoExpiredEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitRewardedVideoExpiredEvent", "Ad expired since it was not shown within {0} minutes of it being loaded", Array.Empty<object>());
        Action<string> onRewardedVideoExpiredEvent = OnRewardedVideoExpiredEvent;
        if (onRewardedVideoExpiredEvent != null)
        {
            onRewardedVideoExpiredEvent(str);
        }
    }

    public void EmitRewardedVideoFailedEvent(string argsJson)
    {
        string[] strArray = this.DecodeArgs(argsJson, 2);
        string str = strArray[0];
        string str2 = strArray[1];
        object[] args = new object[] { str2 };
        MoPubLog.Log("EmitRewardedVideoFailedEvent", "Ad failed to load: ({0}) {1}", args);
        Action<string, string> onRewardedVideoFailedEvent = OnRewardedVideoFailedEvent;
        if (onRewardedVideoFailedEvent != null)
        {
            onRewardedVideoFailedEvent(str, str2);
        }
    }

    public void EmitRewardedVideoFailedToPlayEvent(string argsJson)
    {
        string[] strArray = this.DecodeArgs(argsJson, 2);
        string str = strArray[0];
        string str2 = strArray[1];
        Action<string, string> onRewardedVideoFailedToPlayEvent = OnRewardedVideoFailedToPlayEvent;
        if (onRewardedVideoFailedToPlayEvent != null)
        {
            onRewardedVideoFailedToPlayEvent(str, str2);
        }
    }

    public void EmitRewardedVideoLeavingApplicationEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        Action<string> onRewardedVideoLeavingApplicationEvent = OnRewardedVideoLeavingApplicationEvent;
        if (onRewardedVideoLeavingApplicationEvent != null)
        {
            onRewardedVideoLeavingApplicationEvent(str);
        }
    }

    public void EmitRewardedVideoLoadedEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitRewardedVideoLoadedEvent", "Ad loaded", Array.Empty<object>());
        Action<string> onRewardedVideoLoadedEvent = OnRewardedVideoLoadedEvent;
        if (onRewardedVideoLoadedEvent != null)
        {
            onRewardedVideoLoadedEvent(str);
        }
    }

    public void EmitRewardedVideoReceivedRewardEvent(string argsJson)
    {
        string[] strArray = this.DecodeArgs(argsJson, 3);
        string str = strArray[0];
        string str2 = strArray[1];
        string s = strArray[2];
        MoPubLog.Log("EmitRewardedVideoReceivedRewardEvent", "Ad should reward user with {0} {1}", Array.Empty<object>());
        Action<string, string, float> onRewardedVideoReceivedRewardEvent = OnRewardedVideoReceivedRewardEvent;
        if (onRewardedVideoReceivedRewardEvent != null)
        {
            onRewardedVideoReceivedRewardEvent(str, str2, float.Parse(s));
        }
    }

    public void EmitRewardedVideoShownEvent(string argsJson)
    {
        string str = this.DecodeArgs(argsJson, 1)[0];
        MoPubLog.Log("EmitRewardedVideoShownEvent", "Ad shown", Array.Empty<object>());
        Action<string> onRewardedVideoShownEvent = OnRewardedVideoShownEvent;
        if (onRewardedVideoShownEvent != null)
        {
            onRewardedVideoShownEvent(str);
        }
    }

    public void EmitSdkInitializedEvent(string argsJson)
    {
        string[] strArray = this.DecodeArgs(argsJson, 1);
        string str = strArray[0];
        MoPubBase.LogLevel mPLogLevelNone = MoPubBase.LogLevel.MPLogLevelNone;
        if (strArray.Length > 1)
        {
            try
            {
                mPLogLevelNone = (MoPubBase.LogLevel) Enum.Parse(typeof(MoPubBase.LogLevel), strArray[1]);
            }
            catch (ArgumentException)
            {
                Debug.LogWarning("Invalid LogLevel received: " + strArray[1]);
            }
        }
        else
        {
            Debug.LogWarning("No LogLevel received");
        }
        object[] args = new object[] { mPLogLevelNone };
        MoPubLog.Log("EmitSdkInitializedEvent", "SDK initialized and ready to display ads.  Log Level: {0}", args);
        Action<string> onSdkInitializedEvent = OnSdkInitializedEvent;
        if (onSdkInitializedEvent != null)
        {
            onSdkInitializedEvent(str);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public static MoPubManager Instance
    {
        [CompilerGenerated]
        get => 
            <Instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Instance>k__BackingField = value);
    }
}

