using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class AdsRequestHelper : MonoBehaviour
{
    protected static AdsRequestHelper inst;
    private AdsConfiguration config;
    protected MsgQueue queue = new MsgQueue();
    protected List<RequestLoop> loops = new List<RequestLoop>();
    private AdsAdapter interstitialAdapter;
    private AdsAdapter rewardedAdapter;
    private static DummyAdapter interstitialAdapterDummy;
    private static DummyAdapter rewardedAdapterDummy;

    public static void DebugLog(string msg)
    {
    }

    public static AdsAdapter getInterstitialAdapter()
    {
        if (inst == null)
        {
            if (interstitialAdapterDummy == null)
            {
                interstitialAdapterDummy = new DummyAdapter();
            }
            return interstitialAdapterDummy;
        }
        try
        {
            if (interstitialAdapterDummy != null)
            {
                interstitialAdapterDummy.SetAdapter(inst.getInterstitialAdapterInternal());
            }
            return inst.getInterstitialAdapterInternal();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            if (interstitialAdapterDummy == null)
            {
                interstitialAdapterDummy = new DummyAdapter();
            }
            return interstitialAdapterDummy;
        }
    }

    protected AdsAdapter getInterstitialAdapterInternal()
    {
        if (this.interstitialAdapter == null)
        {
            DebugLog("AdsRequestHelper.getInterstitialAdapter()");
            Dictionary<char, BaseDriver> driverList = new Dictionary<char, BaseDriver>();
            List<string> values = new List<string>();
            driverList['M'] = new WrappedDriver(new MopubInterstitialDriver(this.config.mopubInterstitial));
            values.Add("M");
            this.interstitialAdapter = new WrappedAdapter(new CombinedDriver(driverList, string.Join(",", values)));
        }
        return this.interstitialAdapter;
    }

    public static AdsAdapter getRewardedAdapter()
    {
        if (inst == null)
        {
            if (rewardedAdapterDummy == null)
            {
                rewardedAdapterDummy = new DummyAdapter();
            }
            return rewardedAdapterDummy;
        }
        try
        {
            if (rewardedAdapterDummy != null)
            {
                rewardedAdapterDummy.SetAdapter(inst.getRewardedAdapterInternal());
            }
            return inst.getRewardedAdapterInternal();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            if (rewardedAdapterDummy == null)
            {
                rewardedAdapterDummy = new DummyAdapter();
            }
            return rewardedAdapterDummy;
        }
    }

    protected AdsAdapter getRewardedAdapterInternal()
    {
        if (this.rewardedAdapter == null)
        {
            DebugLog("AdsRequestHelper.getRewardedAdapter()");
            Dictionary<char, BaseDriver> driverList = new Dictionary<char, BaseDriver>();
            List<string> values = new List<string>();
            driverList['M'] = new WrappedDriver(new MopubRewardedDriver(this.config.mopubRewarded));
            values.Add("M");
            this.rewardedAdapter = new WrappedAdapter(new CombinedDriver(driverList, string.Join(",", values)));
        }
        return this.rewardedAdapter;
    }

    public static void Init()
    {
        string str = string.Empty;
        str = "171f58958cd34cc79d891c222471ec79";
        if (!string.IsNullOrEmpty(str))
        {
            AdsConfiguration config = new AdsConfiguration {
                mopubRewarded = str
            };
            Init(config, null);
        }
    }

    public static void Init(AdsConfiguration config, Action<string> onComplete = null)
    {
        if (inst == null)
        {
            DebugLog("AdsRequestHelper.Init()");
            try
            {
                DebugLog("MoPub.InitializeSdk()");
                if (onComplete != null)
                {
                    MoPubManager.OnSdkInitializedEvent += onComplete;
                }
                MoPubBase.SdkConfiguration sdkConfiguration = new MoPubBase.SdkConfiguration {
                    AdUnitId = config.mopubRewarded,
                    LogLevel = MoPubBase.LogLevel.MPLogLevelDebug
                };
                sdkConfiguration.MediatedNetworks = new MoPubBase.MediatedNetwork[] { new MoPubBase.SupportedNetwork.AdMob(), new MoPubBase.SupportedNetwork.AppLovin(), new MoPubBase.SupportedNetwork.Facebook(), new MoPubBase.SupportedNetwork.IronSource(), new MoPubBase.SupportedNetwork.Unity() };
                MoPubAndroid.InitializeSdk(sdkConfiguration);
                DebugLog("MoPub.LoadRewardedVideoPluginsForAdUnits()");
                string[] rewardedVideoAdUnitIds = new string[] { config.mopubRewarded };
                MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(rewardedVideoAdUnitIds);
                Type[] components = new Type[] { typeof(AdsRequestHelper) };
                GameObject target = new GameObject("AdsHelperObject", components);
                Object.DontDestroyOnLoad(target);
                inst = target.GetComponent<AdsRequestHelper>();
                inst.config = config;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }

    public static bool isInitialized() => 
        (inst != null);

    private void Update()
    {
        try
        {
            this.queue.Update();
            foreach (RequestLoop loop in this.loops)
            {
                loop.checkRequest();
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    public interface AdsAdapter : AdsRequestHelper.AdsDriver, AdsRequestHelper.CallbackManager
    {
        bool Show(AdsRequestHelper.AdsCallback enabledCallback);
        void UpdateConfig(string config);
    }

    public interface AdsCallback
    {
        void onClick(AdsRequestHelper.AdsDriver sender, string networkName);
        void onClose(AdsRequestHelper.AdsDriver sender, string networkName);
        void onFail(AdsRequestHelper.AdsDriver sender, string msg);
        void onLoad(AdsRequestHelper.AdsDriver sender, string networkName);
        void onOpen(AdsRequestHelper.AdsDriver sender, string networkName);
        void onRequest(AdsRequestHelper.AdsDriver sender, string networkName);
        void onReward(AdsRequestHelper.AdsDriver sender, string networkName);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AdsConfiguration
    {
        public string admobApp;
        public string admobInterstitial;
        public string admobRewarded;
        public string mopubApp;
        public string mopubInterstitial;
        public string mopubRewarded;
    }

    public interface AdsDriver
    {
        bool isBusy();
        bool isLoaded();
        bool isPlaying();
        bool Show();
    }

    protected abstract class BaseDriver : AdsRequestHelper.AdsDriver
    {
        protected string adUnitId;
        protected AdsRequestHelper.AdsCallback callback;

        protected BaseDriver()
        {
        }

        public abstract void doRequest();
        public abstract string getName();
        public abstract void Init(AdsRequestHelper.AdsCallback callback);
        public abstract bool isBusy();
        public abstract bool isLoaded();
        public abstract bool isPlaying();
        public void LogFunc(string log)
        {
            AdsRequestHelper.DebugLog(this.getName() + "." + log);
        }

        public abstract bool Show();
        public virtual void updateConfig(string config)
        {
        }
    }

    public interface CallbackManager
    {
        void AddCallback(AdsRequestHelper.AdsCallback callback);
        void RemoveCallback(AdsRequestHelper.AdsCallback callback);
    }

    protected class CallbackRouter : AdsRequestHelper.CallbackManager, AdsRequestHelper.AdsCallback
    {
        private List<AdsRequestHelper.AdsCallback> callbacks = new List<AdsRequestHelper.AdsCallback>();
        private AdsRequestHelper.AdsCallback exclusiveCallback;

        public void AddCallback(AdsRequestHelper.AdsCallback callback)
        {
            AdsRequestHelper.DebugLog("CallbackRouter.AddCallback()");
            this.callbacks.Add(callback);
        }

        public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            if (this.exclusiveCallback != null)
            {
                if (this.callbacks.Contains(this.exclusiveCallback))
                {
                    AdsRequestHelper.DebugLog("CallbackRouter.onClick()");
                    this.exclusiveCallback.onClick(sender, networkName);
                }
                else
                {
                    AdsRequestHelper.DebugLog("CallbackRouter.onClick(null)");
                }
            }
            else
            {
                AdsRequestHelper.DebugLog("CallbackRouter.onClick(" + this.callbacks.Count + ")");
                foreach (AdsRequestHelper.AdsCallback callback in this.callbacks)
                {
                    callback.onClick(sender, networkName);
                }
            }
        }

        public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            if (this.exclusiveCallback != null)
            {
                if (this.callbacks.Contains(this.exclusiveCallback))
                {
                    AdsRequestHelper.DebugLog("CallbackRouter.onClose()");
                    this.exclusiveCallback.onClose(sender, networkName);
                }
                else
                {
                    AdsRequestHelper.DebugLog("CallbackRouter.onClose(null)");
                }
            }
            else
            {
                AdsRequestHelper.DebugLog("CallbackRouter.onClose(" + this.callbacks.Count + ")");
                foreach (AdsRequestHelper.AdsCallback callback in this.callbacks)
                {
                    callback.onClose(sender, networkName);
                }
            }
        }

        public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
        {
            AdsRequestHelper.DebugLog("CallbackRouter.onFail(" + this.callbacks.Count + ")");
            foreach (AdsRequestHelper.AdsCallback callback in this.callbacks)
            {
                callback.onFail(sender, msg);
            }
        }

        public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            AdsRequestHelper.DebugLog("CallbackRouter.onLoad(" + this.callbacks.Count + ")");
            foreach (AdsRequestHelper.AdsCallback callback in this.callbacks)
            {
                callback.onLoad(sender, networkName);
            }
        }

        public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            if (this.exclusiveCallback != null)
            {
                if (this.callbacks.Contains(this.exclusiveCallback))
                {
                    AdsRequestHelper.DebugLog("CallbackRouter.onOpen()");
                    this.exclusiveCallback.onOpen(sender, networkName);
                }
                else
                {
                    AdsRequestHelper.DebugLog("CallbackRouter.onOpen(null)");
                }
            }
            else
            {
                AdsRequestHelper.DebugLog("CallbackRouter.onOpen(" + this.callbacks.Count + ")");
                foreach (AdsRequestHelper.AdsCallback callback in this.callbacks)
                {
                    callback.onOpen(sender, networkName);
                }
            }
        }

        public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            AdsRequestHelper.DebugLog("CallbackRouter.onRequest(" + this.callbacks.Count + ")");
            foreach (AdsRequestHelper.AdsCallback callback in this.callbacks)
            {
                callback.onRequest(sender, networkName);
            }
        }

        public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            if (this.exclusiveCallback != null)
            {
                if (this.callbacks.Contains(this.exclusiveCallback))
                {
                    AdsRequestHelper.DebugLog("CallbackRouter.onReward()");
                    this.exclusiveCallback.onReward(sender, networkName);
                }
                else
                {
                    AdsRequestHelper.DebugLog("CallbackRouter.onReward(null)");
                }
            }
            else
            {
                AdsRequestHelper.DebugLog("CallbackRouter.onReward(" + this.callbacks.Count + ")");
                foreach (AdsRequestHelper.AdsCallback callback in this.callbacks)
                {
                    callback.onReward(sender, networkName);
                }
            }
        }

        public void RemoveCallback(AdsRequestHelper.AdsCallback callback)
        {
            AdsRequestHelper.DebugLog("CallbackRouter.RemoveCallback()");
            this.callbacks.Remove(callback);
        }

        public void SetExclusiveCallback(AdsRequestHelper.AdsCallback callback)
        {
            this.exclusiveCallback = callback;
        }
    }

    protected class CombinedDriver : AdsRequestHelper.BaseDriver, AdsRequestHelper.AdsCallback
    {
        private AdsRequestHelper.BaseDriver[] drivers;
        private int[] rates;
        private Dictionary<char, AdsRequestHelper.BaseDriver> driverList;
        private bool loaded;
        private Strategy strategy;

        public CombinedDriver(Dictionary<char, AdsRequestHelper.BaseDriver> driverList, string defaultConfig)
        {
            this.driverList = driverList;
            this.updateConfig(defaultConfig);
        }

        public override void doRequest()
        {
            if (this.driverList != null)
            {
                base.LogFunc("doRequest() This shouldn't be called.");
                foreach (AdsRequestHelper.BaseDriver driver in this.driverList.Values)
                {
                    driver.doRequest();
                }
            }
            this.loaded = false;
        }

        public override string getName() => 
            "CombinedDriver";

        public override void Init(AdsRequestHelper.AdsCallback callback)
        {
            base.LogFunc("Init()");
            base.callback = callback;
            if (this.driverList != null)
            {
                foreach (AdsRequestHelper.BaseDriver driver in this.driverList.Values)
                {
                    driver.Init(this);
                }
            }
            this.loaded = false;
        }

        public override bool isBusy()
        {
            if (this.drivers != null)
            {
                for (int i = 0; i < this.drivers.Length; i++)
                {
                    if (this.drivers[i].isBusy())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool isLoaded()
        {
            if (this.drivers != null)
            {
                for (int i = 0; i < this.drivers.Length; i++)
                {
                    if (((this.strategy != Strategy.RANDOM) || (this.rates[i] != 0)) && this.drivers[i].isLoaded())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool isPlaying()
        {
            if (this.drivers != null)
            {
                for (int i = 0; i < this.drivers.Length; i++)
                {
                    if (this.drivers[i].isPlaying())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            base.LogFunc("onClick()");
            base.callback.onClick(this, networkName);
        }

        public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            base.LogFunc("onClose()");
            base.callback.onClose(this, networkName);
        }

        public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
        {
            base.LogFunc("onFail()");
            for (int i = 0; i < this.drivers.Length; i++)
            {
                if (this.drivers[i] == sender)
                {
                    this.loaded = false;
                    base.callback.onFail(this, msg);
                    break;
                }
            }
        }

        public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            base.LogFunc("onLoad()");
            for (int i = 0; i < this.drivers.Length; i++)
            {
                if (this.drivers[i] == sender)
                {
                    this.loaded = true;
                    base.callback.onLoad(this, networkName);
                    break;
                }
            }
        }

        public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            base.LogFunc("onOpen()");
            base.callback.onOpen(this, networkName);
        }

        public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            base.LogFunc("onRequest()");
            for (int i = 0; i < this.drivers.Length; i++)
            {
                if (this.drivers[i] == sender)
                {
                    base.callback.onRequest(this, networkName);
                    break;
                }
            }
        }

        public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            base.LogFunc("onReward()");
            base.callback.onReward(this, networkName);
        }

        public override bool Show()
        {
            if (this.drivers != null)
            {
                base.LogFunc("Show()");
                int num = 0;
                List<int> list = new List<int>();
                for (int i = 0; i < this.drivers.Length; i++)
                {
                    base.LogFunc(string.Concat(new object[] { "drivers[", i, "] = ", this.drivers[i].getName(), ", isLoaded = ", this.drivers[i].isLoaded() }));
                    if (this.drivers[i].isLoaded())
                    {
                        if (this.strategy == Strategy.PRIORITIZED)
                        {
                            this.drivers[i].Show();
                            return true;
                        }
                        num += this.rates[i];
                        list.Add(i);
                    }
                }
                if (this.strategy == Strategy.RANDOM)
                {
                    if (num <= 0)
                    {
                        return false;
                    }
                    float num3 = Random.Range(0f, (float) num);
                    foreach (int num4 in list)
                    {
                        num3 -= this.rates[num4];
                        if (num3 <= 0f)
                        {
                            this.drivers[num4].Show();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void updateConfig(string config)
        {
            base.LogFunc("UpdateConfig(" + config + ")");
            if ((config != null) && (this.driverList != null))
            {
                char[] separator = new char[] { ',' };
                string[] strArray = config.Split(separator);
                Strategy pRIORITIZED = Strategy.PRIORITIZED;
                List<AdsRequestHelper.BaseDriver> list = new List<AdsRequestHelper.BaseDriver>();
                List<int> list2 = new List<int>();
                for (int i = 0; i < strArray.Length; i++)
                {
                    if ((strArray[i] != null) && (strArray[i].Length >= 1))
                    {
                        char key = strArray[i][0];
                        if (this.driverList.ContainsKey(key))
                        {
                            if (strArray[i].Length > 1)
                            {
                                if (!int.TryParse(strArray[i].Substring(1), out int num2) || (num2 <= 0))
                                {
                                    continue;
                                }
                                pRIORITIZED = Strategy.RANDOM;
                            }
                            else
                            {
                                num2 = 0;
                            }
                            list.Add(this.driverList[key]);
                            list2.Add(num2);
                        }
                    }
                }
                this.rates = list2.ToArray();
                this.drivers = list.ToArray();
                this.strategy = pRIORITIZED;
                base.LogFunc(string.Concat(new object[] { "UpdateConfig(", config, ") SUCCESS, strategy = ", this.strategy.ToString(), ", drivers = ", this.drivers.Length }));
                for (int j = 0; j < this.drivers.Length; j++)
                {
                    base.LogFunc(string.Concat(new object[] { "drivers[", j, "] = ", this.drivers[j].getName(), ", rates[", j, "] = ", this.rates[j] }));
                }
            }
        }

        private enum Strategy
        {
            PRIORITIZED,
            RANDOM
        }
    }

    protected class DummyAdapter : AdsRequestHelper.AdsAdapter, AdsRequestHelper.AdsDriver, AdsRequestHelper.CallbackManager
    {
        private List<AdsRequestHelper.AdsCallback> callbacks = new List<AdsRequestHelper.AdsCallback>();
        private AdsRequestHelper.AdsAdapter adapter;
        private string config;

        public void AddCallback(AdsRequestHelper.AdsCallback callback)
        {
            AdsRequestHelper.DebugLog("DummyAdapter.AddCallback()");
            try
            {
                if (this.adapter != null)
                {
                    this.adapter.AddCallback(callback);
                }
                else
                {
                    this.callbacks.Add(callback);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public bool isBusy() => 
            ((this.adapter != null) && this.adapter.isBusy());

        public bool isLoaded() => 
            ((this.adapter != null) && this.adapter.isLoaded());

        public bool isPlaying() => 
            ((this.adapter != null) && this.adapter.isPlaying());

        public void RemoveCallback(AdsRequestHelper.AdsCallback callback)
        {
            AdsRequestHelper.DebugLog("DummyAdapter.RemoveCallback()");
            if (this.adapter != null)
            {
                this.adapter.RemoveCallback(callback);
            }
            else
            {
                this.callbacks.Remove(callback);
            }
        }

        public void SetAdapter(AdsRequestHelper.AdsAdapter adapter)
        {
            AdsRequestHelper.DebugLog("DummyAdapter.SetAdapter()");
            foreach (AdsRequestHelper.AdsCallback callback in this.callbacks)
            {
                adapter.AddCallback(callback);
            }
            this.callbacks.Clear();
            if (this.config != null)
            {
                adapter.UpdateConfig(this.config);
                this.config = null;
            }
            this.adapter = adapter;
        }

        public bool Show()
        {
            AdsRequestHelper.DebugLog("DummyAdapter.Show()");
            return ((this.adapter != null) && this.adapter.Show());
        }

        public bool Show(AdsRequestHelper.AdsCallback enabledCallback)
        {
            AdsRequestHelper.DebugLog("DummyAdapter.Show(callback)");
            return ((this.adapter != null) && this.adapter.Show(enabledCallback));
        }

        public void UpdateConfig(string config)
        {
            if (this.adapter != null)
            {
                this.adapter.UpdateConfig(config);
            }
            else
            {
                this.config = config;
            }
        }
    }

    protected class MopubInterstitialDriver : AdsRequestHelper.BaseDriver
    {
        private bool playing;
        private bool busy;
        private bool loaded;

        public MopubInterstitialDriver(string adUnitId)
        {
            base.adUnitId = adUnitId;
        }

        public override void doRequest()
        {
            base.LogFunc("doRequest()");
            AdsRequestHelper.DebugLog("MoPub.RequestInterstitialAd(" + base.adUnitId + ")");
            MoPubAndroid.RequestInterstitialAd(base.adUnitId, string.Empty, string.Empty);
            base.callback.onRequest(this, "Mopub");
        }

        public override string getName() => 
            "MopubInterstitialDriver";

        public override void Init(AdsRequestHelper.AdsCallback callback)
        {
            <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
                callback = callback,
                $this = this
            };
            base.LogFunc("Init()");
            MoPubManager.OnInterstitialLoadedEvent += new Action<string>(storey.<>m__0);
            MoPubManager.OnInterstitialFailedEvent += new Action<string, string>(storey.<>m__1);
            MoPubManager.OnInterstitialShownEvent += new Action<string>(storey.<>m__2);
            MoPubManager.OnInterstitialDismissedEvent += new Action<string>(storey.<>m__3);
            MoPubManager.OnInterstitialClickedEvent += new Action<string>(storey.<>m__4);
            MoPubManager.OnInterstitialExpiredEvent += new Action<string>(storey.<>m__5);
            base.callback = storey.callback;
        }

        public override bool isBusy() => 
            this.busy;

        public override bool isLoaded() => 
            this.loaded;

        public override bool isPlaying() => 
            this.playing;

        public override bool Show()
        {
            base.LogFunc("Show()");
            if (this.isLoaded())
            {
                this.busy = true;
                AdsRequestHelper.DebugLog("MoPub.ShowInterstitialAd(" + base.adUnitId + ")");
                MoPubAndroid.ShowInterstitialAd(base.adUnitId);
                return true;
            }
            return false;
        }

        [CompilerGenerated]
        private sealed class <Init>c__AnonStorey0
        {
            internal AdsRequestHelper.AdsCallback callback;
            internal AdsRequestHelper.MopubInterstitialDriver $this;

            internal void <>m__0(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnInterstitialLoadedEvent(" + adUnit + ")");
                this.$this.loaded = true;
                this.callback.onLoad(this.$this, "Mopub");
            }

            internal void <>m__1(string adUnit, string msg)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnInterstitialFailedEvent(" + adUnit + ", " + msg + ")");
                this.$this.loaded = false;
                this.callback.onFail(this.$this, msg);
            }

            internal void <>m__2(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnInterstitialShownEvent(" + adUnit + ")");
                this.$this.playing = true;
                this.callback.onOpen(this.$this, "Mopub");
            }

            internal void <>m__3(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnInterstitialDismissedEvent(" + adUnit + ")");
                this.$this.playing = false;
                this.$this.busy = false;
                this.$this.loaded = false;
                this.callback.onClose(this.$this, "Mopub");
            }

            internal void <>m__4(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnInterstitialClickedEvent(" + adUnit + ")");
                this.callback.onClick(this.$this, "Mopub");
            }

            internal void <>m__5(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnInterstitialExpiredEvent(" + adUnit + ")");
                this.$this.loaded = false;
                this.callback.onFail(this.$this, "Mopub ADS Expired");
            }
        }
    }

    protected class MopubRewardedDriver : AdsRequestHelper.BaseDriver
    {
        private bool playing;
        private bool busy;
        private bool loaded;

        public MopubRewardedDriver(string adUnitId)
        {
            base.adUnitId = adUnitId;
        }

        public override void doRequest()
        {
            base.LogFunc("doRequest()");
            AdsRequestHelper.DebugLog("MoPub.RequestRewardedVideo(" + base.adUnitId + ")");
            MoPubAndroid.RequestRewardedVideo(base.adUnitId, null, null, null, 99999.0, 99999.0, null);
            base.callback.onRequest(this, "Mopub");
        }

        public override string getName() => 
            "MopubRewardedDriver";

        public override void Init(AdsRequestHelper.AdsCallback callback)
        {
            <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
                callback = callback,
                $this = this
            };
            base.LogFunc("Init()");
            MoPubManager.OnRewardedVideoLoadedEvent += new Action<string>(storey.<>m__0);
            MoPubManager.OnRewardedVideoFailedEvent += new Action<string, string>(storey.<>m__1);
            MoPubManager.OnRewardedVideoShownEvent += new Action<string>(storey.<>m__2);
            MoPubManager.OnRewardedVideoFailedToPlayEvent += new Action<string, string>(storey.<>m__3);
            MoPubManager.OnRewardedVideoClosedEvent += new Action<string>(storey.<>m__4);
            MoPubManager.OnRewardedVideoClickedEvent += new Action<string>(storey.<>m__5);
            MoPubManager.OnRewardedVideoLeavingApplicationEvent += new Action<string>(storey.<>m__6);
            MoPubManager.OnRewardedVideoExpiredEvent += new Action<string>(storey.<>m__7);
            MoPubManager.OnRewardedVideoReceivedRewardEvent += new Action<string, string, float>(storey.<>m__8);
            base.callback = storey.callback;
        }

        public override bool isBusy() => 
            this.busy;

        public override bool isLoaded()
        {
            if (this.loaded != MoPubAndroid.HasRewardedVideo(base.adUnitId))
            {
                base.LogFunc(string.Concat(new object[] { "loaded = ", this.loaded, ", MoPub.HasRewardedVideo(", base.adUnitId, ") = ", !this.loaded }));
            }
            return MoPubAndroid.HasRewardedVideo(base.adUnitId);
        }

        public override bool isPlaying() => 
            this.playing;

        public override bool Show()
        {
            base.LogFunc("Show()");
            if (this.isLoaded())
            {
                this.busy = true;
                AdsRequestHelper.DebugLog("MoPub.ShowRewardedVideo(" + base.adUnitId + ")");
                MoPubAndroid.ShowRewardedVideo(base.adUnitId, null);
                return true;
            }
            return false;
        }

        [CompilerGenerated]
        private sealed class <Init>c__AnonStorey0
        {
            internal AdsRequestHelper.AdsCallback callback;
            internal AdsRequestHelper.MopubRewardedDriver $this;

            internal void <>m__0(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoLoadedEvent(" + adUnit + ")");
                this.$this.loaded = true;
                this.callback.onLoad(this.$this, "Mopub");
            }

            internal void <>m__1(string adUnit, string msg)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoFailedEvent(" + adUnit + ", " + msg + ")");
                this.$this.loaded = false;
                this.callback.onFail(this.$this, msg);
            }

            internal void <>m__2(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoShownEvent(" + adUnit + ")");
                this.$this.playing = true;
                this.callback.onOpen(this.$this, "Mopub");
            }

            internal void <>m__3(string adUnit, string msg)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoFailedToPlayEvent(" + adUnit + ", " + msg + ")");
                this.$this.playing = false;
                this.$this.busy = false;
                this.$this.loaded = false;
                this.callback.onClose(this.$this, "Mopub");
                this.callback.onFail(this.$this, msg);
            }

            internal void <>m__4(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoClosedEvent(" + adUnit + ")");
                this.$this.playing = false;
                this.$this.busy = false;
                this.$this.loaded = false;
                this.callback.onClose(this.$this, "Mopub");
            }

            internal void <>m__5(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoClickedEvent(" + adUnit + ")");
                this.callback.onClick(this.$this, "Mopub");
            }

            internal void <>m__6(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoLeavingApplicationEvent(" + adUnit + ")");
                this.callback.onClick(this.$this, "Mopub");
            }

            internal void <>m__7(string adUnit)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoExpiredEvent(" + adUnit + ")");
                this.$this.loaded = false;
                this.callback.onFail(this.$this, "Mopub ADS Expired");
            }

            internal void <>m__8(string adUnit, string reward, float amount)
            {
                AdsRequestHelper.DebugLog("MoPubManager.OnRewardedVideoReceivedRewardEvent(" + adUnit + ")");
                this.callback.onReward(this.$this, "Mopub");
            }
        }
    }

    protected class MsgQueue
    {
        private readonly Queue<Action> _executionQueue = new Queue<Action>();

        public void Run(Action action)
        {
            object obj2 = this._executionQueue;
            lock (obj2)
            {
                this._executionQueue.Enqueue(action);
            }
        }

        public void Update()
        {
            Action action = null;
            object obj2 = this._executionQueue;
            lock (obj2)
            {
                if (this._executionQueue.Count > 0)
                {
                    action = this._executionQueue.Dequeue();
                    AdsRequestHelper.DebugLog("MsgQueue items count : " + this._executionQueue.Count.ToString());
                }
            }
            if (action != null)
            {
                action();
            }
        }
    }

    protected class RequestLoop
    {
        private float lastRequestTime;
        private float lastLoadedTime;
        private const int MIN_REQUEST_INTERVAL = 10;
        private const int MAX_REQUEST_INTERVAL = 600;
        private const int MAX_LOADED_INTERVAL = 300;
        private const int MAX_SHOW_INTERVAL = 300;
        private Status status;
        private DoRequest doRequest;
        private CheckLoaded checkLoaded;
        private ReportError reportError;

        public RequestLoop(DoRequest doRequest, CheckLoaded checkLoaded, ReportError reportError)
        {
            this.doRequest = doRequest;
            this.checkLoaded = checkLoaded;
            this.reportError = reportError;
        }

        public void checkRequest()
        {
            float realtimeSinceStartup = Time.realtimeSinceStartup;
            switch (this.status)
            {
                case Status.REQUESTING:
                    if ((this.lastRequestTime + 600f) <= realtimeSinceStartup)
                    {
                        AdsRequestHelper.DebugLog("RequestLoop.REQUESTING");
                        this.reportError("ADS Requesting Timeout");
                        this.Request();
                    }
                    break;

                case Status.FAILED:
                    if ((this.lastRequestTime + 10f) <= realtimeSinceStartup)
                    {
                        AdsRequestHelper.DebugLog("RequestLoop.FAILED");
                        this.Request();
                    }
                    break;

                case Status.LOADED:
                    if (((this.lastLoadedTime + 300f) <= realtimeSinceStartup) && !this.checkLoaded())
                    {
                        AdsRequestHelper.DebugLog("RequestLoop.LOADED");
                        this.reportError("ADS Expired");
                        this.Request();
                    }
                    break;

                case Status.SHOWING:
                    if ((this.lastLoadedTime + 300f) <= realtimeSinceStartup)
                    {
                        AdsRequestHelper.DebugLog("RequestLoop.SHOWING");
                        this.reportError("ADS Showing too long");
                        this.Request();
                    }
                    break;
            }
        }

        public void Init()
        {
            AdsRequestHelper.DebugLog("RequestLoop.Init()");
            this.Request();
        }

        public bool isLoaded() => 
            (this.status == Status.LOADED);

        public void onClose()
        {
            AdsRequestHelper.DebugLog("RequestLoop.onClose()");
            this.Request();
        }

        public void onFail()
        {
            AdsRequestHelper.DebugLog("RequestLoop.onFail()");
            this.status = Status.FAILED;
        }

        public void onLoad()
        {
            AdsRequestHelper.DebugLog("RequestLoop.onLoad()");
            this.lastLoadedTime = Time.realtimeSinceStartup;
            this.status = Status.LOADED;
        }

        public void onOpen()
        {
            AdsRequestHelper.DebugLog("RequestLoop.onOpen()");
            this.lastLoadedTime = Time.realtimeSinceStartup;
            this.status = Status.SHOWING;
        }

        private void Request()
        {
            AdsRequestHelper.DebugLog("RequestLoop.Request()");
            this.doRequest();
            this.lastRequestTime = Time.realtimeSinceStartup;
            this.status = Status.REQUESTING;
        }

        public delegate bool CheckLoaded();

        public delegate void DoRequest();

        public delegate void ReportError(string error);

        private enum Status
        {
            UNINITIALIZED,
            REQUESTING,
            FAILED,
            LOADED,
            SHOWING
        }
    }

    protected class WrappedAdapter : AdsRequestHelper.AdsAdapter, AdsRequestHelper.AdsDriver, AdsRequestHelper.CallbackManager
    {
        private AdsRequestHelper.CallbackRouter callbacks = new AdsRequestHelper.CallbackRouter();
        private AdsRequestHelper.BaseDriver driver;

        public WrappedAdapter(AdsRequestHelper.BaseDriver driver)
        {
            this.driver = driver;
            driver.Init(this.callbacks);
        }

        public void AddCallback(AdsRequestHelper.AdsCallback callback)
        {
            AdsRequestHelper.DebugLog("WrappedAdapter.AddCallback()");
            this.callbacks.AddCallback(callback);
        }

        public bool isBusy()
        {
            try
            {
                return this.driver.isBusy();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return false;
            }
        }

        public bool isLoaded()
        {
            try
            {
                return this.driver.isLoaded();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return false;
            }
        }

        public bool isPlaying()
        {
            try
            {
                return this.driver.isPlaying();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return false;
            }
        }

        public void RemoveCallback(AdsRequestHelper.AdsCallback callback)
        {
            AdsRequestHelper.DebugLog("WrappedAdapter.RemoveCallback()");
            this.callbacks.RemoveCallback(callback);
        }

        public bool Show()
        {
            try
            {
                AdsRequestHelper.DebugLog("WrappedAdapter.Show()");
                this.callbacks.SetExclusiveCallback(null);
                return this.driver.Show();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return false;
            }
        }

        public bool Show(AdsRequestHelper.AdsCallback enabledCallback)
        {
            try
            {
                AdsRequestHelper.DebugLog("WrappedAdapter.Show(callback)");
                this.callbacks.SetExclusiveCallback(enabledCallback);
                return this.driver.Show();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return false;
            }
        }

        public void UpdateConfig(string config)
        {
            try
            {
                this.driver.updateConfig(config);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }

    protected class WrappedDriver : AdsRequestHelper.BaseDriver, AdsRequestHelper.AdsCallback
    {
        private AdsRequestHelper.BaseDriver driver;
        private AdsRequestHelper.RequestLoop loop;
        private AdsRequestHelper.MsgQueue queue;

        public WrappedDriver(AdsRequestHelper.BaseDriver driver)
        {
            this.driver = driver;
        }

        public override void doRequest()
        {
            base.LogFunc("doRequest()");
        }

        public override string getName() => 
            (this.driver.getName() + "+WrappedDriver");

        public override void Init(AdsRequestHelper.AdsCallback callback)
        {
            base.LogFunc("Init()");
            base.callback = callback;
            this.queue = AdsRequestHelper.inst.queue;
            this.loop = new AdsRequestHelper.RequestLoop(new AdsRequestHelper.RequestLoop.DoRequest(this.driver.doRequest), new AdsRequestHelper.RequestLoop.CheckLoaded(this.driver.isLoaded), new AdsRequestHelper.RequestLoop.ReportError(this.reportError));
            this.driver.Init(this);
            this.loop.Init();
            AdsRequestHelper.inst.loops.Add(this.loop);
        }

        public override bool isBusy() => 
            this.driver.isBusy();

        public override bool isLoaded() => 
            this.driver.isLoaded();

        public override bool isPlaying() => 
            this.driver.isPlaying();

        public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            <onClick>c__AnonStorey5 storey = new <onClick>c__AnonStorey5 {
                networkName = networkName,
                $this = this
            };
            this.queue.Run(new Action(storey.<>m__0));
        }

        public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            <onClose>c__AnonStorey4 storey = new <onClose>c__AnonStorey4 {
                networkName = networkName,
                $this = this
            };
            this.queue.Run(new Action(storey.<>m__0));
        }

        public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
        {
            <onFail>c__AnonStorey2 storey = new <onFail>c__AnonStorey2 {
                msg = msg,
                $this = this
            };
            this.queue.Run(new Action(storey.<>m__0));
        }

        public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            <onLoad>c__AnonStorey1 storey = new <onLoad>c__AnonStorey1 {
                networkName = networkName,
                $this = this
            };
            this.queue.Run(new Action(storey.<>m__0));
        }

        public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            <onOpen>c__AnonStorey3 storey = new <onOpen>c__AnonStorey3 {
                networkName = networkName,
                $this = this
            };
            this.queue.Run(new Action(storey.<>m__0));
        }

        public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            <onRequest>c__AnonStorey0 storey = new <onRequest>c__AnonStorey0 {
                networkName = networkName,
                $this = this
            };
            this.queue.Run(new Action(storey.<>m__0));
        }

        public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
        {
            <onReward>c__AnonStorey6 storey = new <onReward>c__AnonStorey6 {
                networkName = networkName,
                $this = this
            };
            this.queue.Run(new Action(storey.<>m__0));
        }

        protected void reportError(string error)
        {
            this.onFail(this, error);
        }

        public override bool Show()
        {
            base.LogFunc("Show()");
            if (this.isLoaded())
            {
                this.loop.onOpen();
            }
            return this.driver.Show();
        }

        [CompilerGenerated]
        private sealed class <onClick>c__AnonStorey5
        {
            internal string networkName;
            internal AdsRequestHelper.WrappedDriver $this;

            internal void <>m__0()
            {
                this.$this.LogFunc("onClick()");
                this.$this.callback.onClick(this.$this, this.networkName);
            }
        }

        [CompilerGenerated]
        private sealed class <onClose>c__AnonStorey4
        {
            internal string networkName;
            internal AdsRequestHelper.WrappedDriver $this;

            internal void <>m__0()
            {
                this.$this.LogFunc("onClose()");
                this.$this.callback.onClose(this.$this, this.networkName);
                this.$this.loop.onClose();
            }
        }

        [CompilerGenerated]
        private sealed class <onFail>c__AnonStorey2
        {
            internal string msg;
            internal AdsRequestHelper.WrappedDriver $this;

            internal void <>m__0()
            {
                this.$this.LogFunc("onFail()");
                this.$this.callback.onFail(this.$this, this.msg);
                this.$this.loop.onFail();
            }
        }

        [CompilerGenerated]
        private sealed class <onLoad>c__AnonStorey1
        {
            internal string networkName;
            internal AdsRequestHelper.WrappedDriver $this;

            internal void <>m__0()
            {
                this.$this.LogFunc("onLoad()");
                this.$this.callback.onLoad(this.$this, this.networkName);
                this.$this.loop.onLoad();
            }
        }

        [CompilerGenerated]
        private sealed class <onOpen>c__AnonStorey3
        {
            internal string networkName;
            internal AdsRequestHelper.WrappedDriver $this;

            internal void <>m__0()
            {
                this.$this.LogFunc("onOpen()");
                this.$this.callback.onOpen(this.$this, this.networkName);
            }
        }

        [CompilerGenerated]
        private sealed class <onRequest>c__AnonStorey0
        {
            internal string networkName;
            internal AdsRequestHelper.WrappedDriver $this;

            internal void <>m__0()
            {
                this.$this.LogFunc("onRequest()");
                this.$this.callback.onRequest(this.$this, this.networkName);
            }
        }

        [CompilerGenerated]
        private sealed class <onReward>c__AnonStorey6
        {
            internal string networkName;
            internal AdsRequestHelper.WrappedDriver $this;

            internal void <>m__0()
            {
                this.$this.LogFunc("onReward()");
                this.$this.callback.onReward(this.$this, this.networkName);
            }
        }
    }
}

