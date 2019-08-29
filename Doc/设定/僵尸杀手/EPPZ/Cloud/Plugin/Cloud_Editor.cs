namespace EPPZ.Cloud.Plugin
{
    using EPPZ.Cloud;
    using EPPZ.Cloud.Model.Simulation;
    using System;
    using UnityEngine;

    public class Cloud_Editor : EPPZ.Cloud.Plugin.Cloud
    {
        public override bool BoolForKey(string key) => 
            this.keyValueStore.KeyValuePairForKey(key).boolValue;

        public override float FloatForKey(string key) => 
            this.keyValueStore.KeyValuePairForKey(key).floatValue;

        public override int IntForKey(string key)
        {
            int intValue = 0;
            try
            {
                intValue = this.keyValueStore.KeyValuePairForKey(key).intValue;
            }
            catch (Exception)
            {
                Debug.LogWarning(key);
            }
            return intValue;
        }

        public override void SetBoolForKey(bool value, string key)
        {
            base.Log(string.Concat(new object[] { "Cloud_Editor.SetBoolForKey(`", value, "`, `", key, "`)" }));
            this.keyValueStore.KeyValuePairForKey(key).boolValue = value;
        }

        public override void SetFloatForKey(float value, string key)
        {
            base.Log(string.Concat(new object[] { "Cloud_Editor.SetFloatForKey(`", value, "`, `", key, "`)" }));
            this.keyValueStore.KeyValuePairForKey(key).floatValue = value;
        }

        public override void SetIntForKey(int value, string key)
        {
            base.Log(string.Concat(new object[] { "Cloud_Editor.SetIntForKey(`", value, "`, `", key, "`)" }));
            this.keyValueStore.KeyValuePairForKey(key).intValue = value;
        }

        public override void SetStringForKey(string value, string key)
        {
            base.Log("Cloud_Editor.SetStringForKey(`" + value + "`, `" + key + "`)");
            this.keyValueStore.KeyValuePairForKey(key).stringValue = value;
        }

        public override string StringForKey(string key) => 
            this.keyValueStore.KeyValuePairForKey(key).stringValue;

        public override void Synchronize()
        {
            this.keyValueStore.SimulateCloudDidChange();
        }

        private KeyValueStore keyValueStore =>
            EPPZ.Cloud.Cloud.SimulationKeyValueStore();
    }
}

