namespace EPPZ.Cloud.Model.Simulation
{
    using EPPZ.Cloud;
    using EPPZ.Cloud.Plugin;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(fileName="Key-value store simulation", menuName="eppz!/Cloud/Key-value store simulation")]
    public class KeyValueStore : ScriptableObject
    {
        public ChangeReason changeReason;
        public KeyValuePair[] keyValuePairs;
        public Cloud_Editor plugin;

        public virtual void EnumerateKeyValuePairs(Action<KeyValuePair> action)
        {
            foreach (KeyValuePair pair in this.keyValuePairs)
            {
                action(pair);
            }
        }

        public virtual KeyValuePair KeyValuePairForKey(string key)
        {
            foreach (KeyValuePair pair in this.keyValuePairs)
            {
                if (pair.key == key)
                {
                    return pair;
                }
            }
            Debug.LogWarning("eppz! Cloud: No Key-value pair defined for key `" + key + "`");
            return null;
        }

        [ContextMenu("Simulate `CloudDidChange`")]
        public virtual void SimulateCloudDidChange()
        {
            <SimulateCloudDidChange>c__AnonStorey0 storey = new <SimulateCloudDidChange>c__AnonStorey0();
            Debug.Log("SimulateCloudDidChange()");
            storey.changedKeys = new List<string>();
            this.EnumerateKeyValuePairs(new Action<KeyValuePair>(storey.<>m__0));
            Debug.Log("changedKeys: `" + storey.changedKeys.ToArray() + "`");
            EPPZ.Cloud.Cloud.InvokeOnKeysChanged(storey.changedKeys.ToArray(), this.changeReason);
        }

        [CompilerGenerated]
        private sealed class <SimulateCloudDidChange>c__AnonStorey0
        {
            internal List<string> changedKeys;

            internal void <>m__0(KeyValuePair eachKeyValuePair)
            {
                if (eachKeyValuePair.isChanged)
                {
                    this.changedKeys.Add(eachKeyValuePair.key);
                    eachKeyValuePair.isChanged = false;
                }
            }
        }
    }
}

