namespace EPPZ.Cloud.Model
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName="Cloud settings", menuName="eppz!/Cloud/Settings")]
    public class Settings : ScriptableObject
    {
        public KeyValuePair[] keyValuePairs;
        public bool initializeOnStart = true;
        public bool log = true;

        public KeyValuePair KeyValuePairForKey(string key)
        {
            foreach (KeyValuePair pair in this.keyValuePairs)
            {
                if (pair.key == key)
                {
                    return pair;
                }
            }
            Debug.LogWarning("eppz! Cloud: Cannot find registered key for `" + key + "`.");
            return null;
        }
    }
}

