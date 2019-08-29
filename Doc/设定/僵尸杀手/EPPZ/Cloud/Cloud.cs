namespace EPPZ.Cloud
{
    using EPPZ.Cloud.Model;
    using EPPZ.Cloud.Model.Simulation;
    using EPPZ.Cloud.Plugin;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Cloud : MonoBehaviour, ICloud
    {
        private static EPPZ.Cloud.Cloud _instance;
        public Settings settings;
        public KeyValueStore simulationKeyValueStore;
        public static OnCloudChange onCloudChange;
        private ChangeReason latestChangeReason;
        private EPPZ.Cloud.Plugin.Cloud _plugin;

        public void _CloudDidChange(string message)
        {
            this.plugin.CloudDidChange(message);
        }

        private void _Initialize()
        {
            this.plugin.InitializeWithGameObjectName(base.name);
        }

        public void _OnCloudChange(string[] changedKeys, ChangeReason changeReason)
        {
            Log(string.Concat(new object[] { "Cloud._OnCloudChange(`", changedKeys, "`, `", changeReason, "`)" }));
            this.latestChangeReason = changeReason;
            if ((onCloudChange == null) || (onCloudChange(changedKeys, changeReason) != Should.StopUpdateKeys))
            {
                foreach (string str in changedKeys)
                {
                    foreach (EPPZ.Cloud.Model.KeyValuePair pair in this.settings.keyValuePairs)
                    {
                        if (pair.key == str)
                        {
                            pair.InvokeOnValueChangedAction();
                        }
                    }
                }
            }
        }

        private void _OnKeyChange(string key, object action, int priority)
        {
            EPPZ.Cloud.Model.KeyValuePair pair = this.settings.KeyValuePairForKey(key);
            if (pair != null)
            {
                pair.AddOnChangeAction(action, priority);
            }
        }

        private void _RemoveOnKeyChangeAction(string key, object action)
        {
            EPPZ.Cloud.Model.KeyValuePair pair = this.settings.KeyValuePairForKey(key);
            if (pair != null)
            {
                pair.RemoveOnChangeAction(action);
            }
        }

        private void _RemoveOnKeyChangeActions()
        {
            foreach (EPPZ.Cloud.Model.KeyValuePair pair in this.settings.keyValuePairs)
            {
                pair.RemoveOnChangeActions();
            }
        }

        private void Awake()
        {
            _instance = this;
        }

        public static bool BoolForKey(string key) => 
            _instance.plugin.BoolForKey(key);

        public static float FloatForKey(string key) => 
            _instance.plugin.FloatForKey(key);

        public static void Initialize()
        {
            _instance._Initialize();
        }

        public static int IntForKey(string key) => 
            _instance.plugin.IntForKey(key);

        public static void InvokeOnKeysChanged(string[] changedKeys, ChangeReason changeReason)
        {
            Log(string.Concat(new object[] { "Cloud.InvokeOnKeysChanged(`", changedKeys, "`, `", changeReason, "`)" }));
            _instance._OnCloudChange(changedKeys, changeReason);
        }

        public static ChangeReason LatestChangeReason() => 
            _instance.latestChangeReason;

        public static void Log(string message)
        {
            if (_instance.settings.log)
            {
                Debug.Log(message);
            }
        }

        private void OnDestroy()
        {
            this._RemoveOnKeyChangeActions();
        }

        public static void OnKeyChange(string key, Action<bool> action, int priority = 0)
        {
            _instance._OnKeyChange(key, action, priority);
        }

        public static void OnKeyChange(string key, Action<int> action, int priority = 0)
        {
            _instance._OnKeyChange(key, action, priority);
        }

        public static void OnKeyChange(string key, Action<float> action, int priority = 0)
        {
            _instance._OnKeyChange(key, action, priority);
        }

        public static void OnKeyChange(string key, Action<string> action, int priority = 0)
        {
            _instance._OnKeyChange(key, action, priority);
        }

        public static void RemoveOnKeyChangeAction(string key, Action<bool> action)
        {
            _instance._RemoveOnKeyChangeAction(key, action);
        }

        public static void RemoveOnKeyChangeAction(string key, Action<int> action)
        {
            _instance._RemoveOnKeyChangeAction(key, action);
        }

        public static void RemoveOnKeyChangeAction(string key, Action<float> action)
        {
            _instance._RemoveOnKeyChangeAction(key, action);
        }

        public static void RemoveOnKeyChangeAction(string key, Action<string> action)
        {
            _instance._RemoveOnKeyChangeAction(key, action);
        }

        public static void SetBoolForKey(bool value, string key)
        {
            _instance.plugin.SetBoolForKey(value, key);
        }

        public static void SetFloatForKey(float value, string key)
        {
            _instance.plugin.SetFloatForKey(value, key);
        }

        public static void SetIntForKey(int value, string key)
        {
            _instance.plugin.SetIntForKey(value, key);
        }

        public static void SetStringForKey(string value, string key)
        {
            _instance.plugin.SetStringForKey(value, key);
        }

        public static KeyValueStore SimulationKeyValueStore() => 
            _instance.simulationKeyValueStore;

        private void Start()
        {
            if (this.settings.initializeOnStart)
            {
                this._Initialize();
            }
        }

        public static string StringForKey(string key) => 
            _instance.plugin.StringForKey(key);

        public static void Synchrnonize()
        {
            _instance.plugin.Synchronize();
        }

        private EPPZ.Cloud.Plugin.Cloud plugin
        {
            get
            {
                if (this._plugin == null)
                {
                    this._plugin = EPPZ.Cloud.Plugin.Cloud.NativePluginInstance(this);
                }
                return this._plugin;
            }
        }

        public delegate EPPZ.Cloud.Cloud.Should OnCloudChange(string[] changedKeys, ChangeReason changeReason);

        public enum Should
        {
            UpdateKeys,
            StopUpdateKeys
        }
    }
}

