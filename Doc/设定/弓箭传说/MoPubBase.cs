using MoPubInternal.ThirdParty.MiniJSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoPubBase
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static string <ConsentLanguageCode>k__BackingField;
    public const double LatLongSentinel = 99999.0;
    public static readonly string moPubSDKVersion = "5.5.0";
    private static string _pluginName;
    private static bool _allowLegitimateInterest;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static LogLevel <logLevel>k__BackingField;

    protected MoPubBase()
    {
    }

    public static int CompareVersions(string a, string b)
    {
        int[] versionInts = VersionStringToInts(a);
        int[] numArray2 = VersionStringToInts(b);
        for (int i = 0; i < Mathf.Max(versionInts.Length, numArray2.Length); i++)
        {
            if (VersionPiece(versionInts, i) < VersionPiece(numArray2, i))
            {
                return -1;
            }
            if (VersionPiece(versionInts, i) > VersionPiece(numArray2, i))
            {
                return 1;
            }
        }
        return 0;
    }

    protected static void InitManager()
    {
        Type type = typeof(MoPubManager);
        Type[] components = new Type[] { type };
        MoPubManager component = new GameObject("MoPubManager", components).GetComponent<MoPubManager>();
        if (MoPubManager.Instance != component)
        {
            Debug.LogWarning("It looks like you have the " + type.Name + " on a GameObject in your scene. Please remove the script from your scene.");
        }
    }

    protected static void ReportAdUnitNotFound(string adUnitId)
    {
        Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
    }

    protected static Uri UrlFromString(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }
        try
        {
            return new Uri(url);
        }
        catch
        {
            Debug.LogError("Invalid URL: " + url);
            return null;
        }
    }

    protected static void ValidateAdUnitForSdkInit(string adUnitId)
    {
        if (string.IsNullOrEmpty(adUnitId))
        {
            Debug.LogError("A valid ad unit ID is needed to initialize the MoPub SDK.");
        }
    }

    private static int VersionPiece(IList<int> versionInts, int pieceIndex) => 
        ((pieceIndex >= versionInts.Count) ? 0 : versionInts[pieceIndex]);

    private static int[] VersionStringToInts(string version)
    {
        <VersionStringToInts>c__AnonStorey0 storey = new <VersionStringToInts>c__AnonStorey0();
        char[] separator = new char[] { '.' };
        return version.Split(separator).Select<string, int>(new Func<string, int>(storey.<>m__0)).ToArray<int>();
    }

    public static string ConsentLanguageCode
    {
        [CompilerGenerated]
        get => 
            <ConsentLanguageCode>k__BackingField;
        [CompilerGenerated]
        set => 
            (<ConsentLanguageCode>k__BackingField = value);
    }

    public static LogLevel logLevel
    {
        [CompilerGenerated]
        get => 
            <logLevel>k__BackingField;
        [CompilerGenerated]
        protected set => 
            (<logLevel>k__BackingField = value);
    }

    public static string PluginName
    {
        get
        {
            if (_pluginName == null)
            {
            }
            return (_pluginName = "MoPub Unity Plugin v" + moPubSDKVersion);
        }
    }

    [CompilerGenerated]
    private sealed class <VersionStringToInts>c__AnonStorey0
    {
        internal int piece;

        internal int <>m__0(string v) => 
            (!int.TryParse(v, out this.piece) ? 0 : this.piece);
    }

    public enum AdPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        Centered,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public enum BannerType
    {
        Size320x50,
        Size300x250,
        Size728x90,
        Size160x600
    }

    public static class Consent
    {
        public static Status FromString(string status)
        {
            if (status != null)
            {
                if (status == "explicit_yes")
                {
                    return Status.Consented;
                }
                if (status == "explicit_no")
                {
                    return Status.Denied;
                }
                if (status == "dnt")
                {
                    return Status.DoNotTrack;
                }
                if (status == "potential_whitelist")
                {
                    return Status.PotentialWhitelist;
                }
                if (status == "unknown")
                {
                    return Status.Unknown;
                }
            }
            try
            {
                return (Status) Enum.Parse(typeof(Status), status);
            }
            catch
            {
                Debug.LogError("Unknown consent status string: " + status);
                return Status.Unknown;
            }
        }

        public enum Status
        {
            Unknown,
            Denied,
            DoNotTrack,
            PotentialWhitelist,
            Consented
        }

        private static class Strings
        {
            public const string ExplicitYes = "explicit_yes";
            public const string ExplicitNo = "explicit_no";
            public const string Unknown = "unknown";
            public const string PotentialWhitelist = "potential_whitelist";
            public const string Dnt = "dnt";
        }
    }

    public class LocalMediationSetting : Dictionary<string, object>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MediationSettingsClassName>k__BackingField;
        [CompilerGenerated]
        private static Func<MoPubBase.LocalMediationSetting, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<MoPubBase.LocalMediationSetting, string> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<MoPubBase.LocalMediationSetting, MoPubBase.LocalMediationSetting> <>f__am$cache2;

        public LocalMediationSetting()
        {
        }

        public LocalMediationSetting(string adVendor)
        {
            this.MediationSettingsClassName = adVendor;
        }

        public LocalMediationSetting(string android, string ios) : this(android)
        {
        }

        public static string ToJson(IEnumerable<MoPubBase.LocalMediationSetting> localMediationSettings)
        {
            if (localMediationSettings == null)
            {
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = n => (n != null) && !string.IsNullOrEmpty(n.MediationSettingsClassName);
            }
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = n => n.MediationSettingsClassName;
            }
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = n => n;
            }
            return Json.Serialize(Enumerable.Empty<MoPubBase.LocalMediationSetting>().Where<MoPubBase.LocalMediationSetting>(<>f__am$cache0).ToDictionary<MoPubBase.LocalMediationSetting, string, MoPubBase.LocalMediationSetting>(<>f__am$cache1, <>f__am$cache2));
        }

        public string MediationSettingsClassName { get; set; }

        public class AdColony : MoPubBase.LocalMediationSetting
        {
            public AdColony() : base("AdColony")
            {
            }
        }

        public class AdMob : MoPubBase.LocalMediationSetting
        {
            public AdMob() : base("GooglePlayServices", "MPGoogle")
            {
            }
        }

        public class Chartboost : MoPubBase.LocalMediationSetting
        {
            public Chartboost() : base("Chartboost")
            {
            }
        }

        public class Vungle : MoPubBase.LocalMediationSetting
        {
            public Vungle() : base("Vungle")
            {
            }
        }
    }

    public enum LogLevel
    {
        MPLogLevelDebug = 20,
        MPLogLevelInfo = 30,
        MPLogLevelNone = 70
    }

    public class MediatedNetwork
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AdapterConfigurationClassName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MediationSettingsClassName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, object> <NetworkConfiguration>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, object> <MediationSettings>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, object> <MoPubRequestOptions>k__BackingField;

        public string AdapterConfigurationClassName { get; set; }

        public string MediationSettingsClassName { get; set; }

        public Dictionary<string, object> NetworkConfiguration { get; set; }

        public Dictionary<string, object> MediationSettings { get; set; }

        public Dictionary<string, object> MoPubRequestOptions { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Reward
    {
        public string Label;
        public int Amount;
        public override string ToString() => 
            $""{this.Amount} {this.Label}"";

        public bool IsValid() => 
            (!string.IsNullOrEmpty(this.Label) && (this.Amount > 0));
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SdkConfiguration
    {
        public string AdUnitId;
        public MoPubBase.MediatedNetwork[] MediatedNetworks;
        public bool AllowLegitimateInterest;
        private MoPubBase.LogLevel _logLevel;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, bool> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, string> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, bool> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, bool> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, string> <>f__am$cache5;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, Dictionary<string, object>> <>f__am$cache6;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, bool> <>f__am$cache7;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, bool> <>f__am$cache8;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, string> <>f__am$cache9;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, Dictionary<string, object>> <>f__am$cacheA;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, bool> <>f__am$cacheB;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, bool> <>f__am$cacheC;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, string> <>f__am$cacheD;
        [CompilerGenerated]
        private static Func<MoPubBase.MediatedNetwork, Dictionary<string, object>> <>f__am$cacheE;
        public MoPubBase.LogLevel LogLevel
        {
            get => 
                ((this._logLevel == ((MoPubBase.LogLevel) 0)) ? MoPubBase.LogLevel.MPLogLevelNone : this._logLevel);
            set => 
                (this._logLevel = value);
        }
        public string AdditionalNetworksString
        {
            get
            {
                if (this.MediatedNetworks == null)
                {
                }
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = n => (n != null) && !(n is MoPubBase.SupportedNetwork);
                }
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = n => !string.IsNullOrEmpty(n.AdapterConfigurationClassName);
                }
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = n => n.AdapterConfigurationClassName;
                }
                IEnumerable<string> source = Enumerable.Empty<MoPubBase.MediatedNetwork>().Where<MoPubBase.MediatedNetwork>(<>f__am$cache0).Where<MoPubBase.MediatedNetwork>(<>f__am$cache1).Select<MoPubBase.MediatedNetwork, string>(<>f__am$cache2);
                return string.Join(",", source.ToArray<string>());
            }
        }
        public string NetworkConfigurationsJson
        {
            get
            {
                if (this.MediatedNetworks == null)
                {
                }
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = n => n.NetworkConfiguration != null;
                }
                if (<>f__am$cache4 == null)
                {
                    <>f__am$cache4 = n => !string.IsNullOrEmpty(n.AdapterConfigurationClassName);
                }
                if (<>f__am$cache5 == null)
                {
                    <>f__am$cache5 = n => n.AdapterConfigurationClassName;
                }
                if (<>f__am$cache6 == null)
                {
                    <>f__am$cache6 = n => n.NetworkConfiguration;
                }
                return Json.Serialize(Enumerable.Empty<MoPubBase.MediatedNetwork>().Where<MoPubBase.MediatedNetwork>(<>f__am$cache3).Where<MoPubBase.MediatedNetwork>(<>f__am$cache4).ToDictionary<MoPubBase.MediatedNetwork, string, Dictionary<string, object>>(<>f__am$cache5, <>f__am$cache6));
            }
        }
        public string MediationSettingsJson
        {
            get
            {
                if (this.MediatedNetworks == null)
                {
                }
                if (<>f__am$cache7 == null)
                {
                    <>f__am$cache7 = n => n.MediationSettings != null;
                }
                if (<>f__am$cache8 == null)
                {
                    <>f__am$cache8 = n => !string.IsNullOrEmpty(n.MediationSettingsClassName);
                }
                if (<>f__am$cache9 == null)
                {
                    <>f__am$cache9 = n => n.MediationSettingsClassName;
                }
                if (<>f__am$cacheA == null)
                {
                    <>f__am$cacheA = n => n.MediationSettings;
                }
                return Json.Serialize(Enumerable.Empty<MoPubBase.MediatedNetwork>().Where<MoPubBase.MediatedNetwork>(<>f__am$cache7).Where<MoPubBase.MediatedNetwork>(<>f__am$cache8).ToDictionary<MoPubBase.MediatedNetwork, string, Dictionary<string, object>>(<>f__am$cache9, <>f__am$cacheA));
            }
        }
        public string MoPubRequestOptionsJson
        {
            get
            {
                if (this.MediatedNetworks == null)
                {
                }
                if (<>f__am$cacheB == null)
                {
                    <>f__am$cacheB = n => n.MoPubRequestOptions != null;
                }
                if (<>f__am$cacheC == null)
                {
                    <>f__am$cacheC = n => !string.IsNullOrEmpty(n.AdapterConfigurationClassName);
                }
                if (<>f__am$cacheD == null)
                {
                    <>f__am$cacheD = n => n.AdapterConfigurationClassName;
                }
                if (<>f__am$cacheE == null)
                {
                    <>f__am$cacheE = n => n.MoPubRequestOptions;
                }
                return Json.Serialize(Enumerable.Empty<MoPubBase.MediatedNetwork>().Where<MoPubBase.MediatedNetwork>(<>f__am$cacheB).Where<MoPubBase.MediatedNetwork>(<>f__am$cacheC).ToDictionary<MoPubBase.MediatedNetwork, string, Dictionary<string, object>>(<>f__am$cacheD, <>f__am$cacheE));
            }
        }
    }

    public class SupportedNetwork : MoPubBase.MediatedNetwork
    {
        protected SupportedNetwork(string adVendor)
        {
            base.AdapterConfigurationClassName = "com.mopub.mobileads." + adVendor + "AdapterConfiguration";
            base.MediationSettingsClassName = adVendor;
        }

        protected SupportedNetwork(string android, string ios) : this(android)
        {
        }

        public class AdColony : MoPubBase.SupportedNetwork
        {
            public AdColony() : base("AdColony")
            {
            }
        }

        public class AdMob : MoPubBase.SupportedNetwork
        {
            public AdMob() : base("GooglePlayServices", "MPGoogle")
            {
            }
        }

        public class AppLovin : MoPubBase.SupportedNetwork
        {
            public AppLovin() : base("AppLovin")
            {
            }
        }

        public class Chartboost : MoPubBase.SupportedNetwork
        {
            public Chartboost() : base("Chartboost")
            {
            }
        }

        public class Facebook : MoPubBase.SupportedNetwork
        {
            public Facebook() : base("Facebook")
            {
            }
        }

        public class IronSource : MoPubBase.SupportedNetwork
        {
            public IronSource() : base("IronSource")
            {
            }
        }

        public class OnebyAOL : MoPubBase.SupportedNetwork
        {
            public OnebyAOL() : base("Millennial", "MPMillennial")
            {
            }
        }

        public class Tapjoy : MoPubBase.SupportedNetwork
        {
            public Tapjoy() : base("Tapjoy")
            {
            }
        }

        public class Unity : MoPubBase.SupportedNetwork
        {
            public Unity() : base("Unity", "UnityAds")
            {
            }
        }

        public class Vungle : MoPubBase.SupportedNetwork
        {
            public Vungle() : base("Vungle")
            {
            }
        }
    }
}

