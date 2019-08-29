namespace EPPZ.Cloud.Plugin
{
    using EPPZ.Cloud;
    using EPPZ.Cloud.Plugin.iOS;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Cloud_iOS : EPPZ.Cloud.Plugin.Cloud
    {
        public override bool BoolForKey(string key) => 
            EPPZ_Cloud_BoolForKey(key);

        public override void CloudDidChange(string message)
        {
            base.Log("Cloud_iOS.CloudDidChange(`" + message + "`)");
            UserInfo objectToOverwrite = new UserInfo();
            JsonUtility.FromJsonOverwrite(message, objectToOverwrite);
            ChangeReason nSUbiquitousKeyValueStoreChangeReasonKey = (ChangeReason) objectToOverwrite.NSUbiquitousKeyValueStoreChangeReasonKey;
            string[] nSUbiquitousKeyValueStoreChangedKeysKey = objectToOverwrite.NSUbiquitousKeyValueStoreChangedKeysKey;
            base.Log("Cloud_iOS.CloudDidChange.changeReason: `" + nSUbiquitousKeyValueStoreChangeReasonKey + "`");
            base.Log("Cloud_iOS.CloudDidChange.changedKeys: `" + nSUbiquitousKeyValueStoreChangedKeysKey + "`");
            base.cloudObject._OnCloudChange(nSUbiquitousKeyValueStoreChangedKeysKey, nSUbiquitousKeyValueStoreChangeReasonKey);
        }

        [DllImport("__Internal")]
        private static extern bool EPPZ_Cloud_BoolForKey(string key);
        [DllImport("__Internal")]
        private static extern float EPPZ_Cloud_FloatForKey(string key);
        [DllImport("__Internal")]
        private static extern void EPPZ_Cloud_InitializeWithGameObjectName(string gameObjectName);
        [DllImport("__Internal")]
        private static extern int EPPZ_Cloud_IntForKey(string key);
        [DllImport("__Internal")]
        private static extern void EPPZ_Cloud_SetBoolForKey(bool value, string key);
        [DllImport("__Internal")]
        private static extern void EPPZ_Cloud_SetFloatForKey(float value, string key);
        [DllImport("__Internal")]
        private static extern void EPPZ_Cloud_SetIntForKey(int value, string key);
        [DllImport("__Internal")]
        private static extern void EPPZ_Cloud_SetStringForKey(string value, string key);
        [DllImport("__Internal")]
        private static extern string EPPZ_Cloud_StringForKey(string key);
        [DllImport("__Internal")]
        private static extern bool EPPZ_Cloud_Synchronize();
        public override float FloatForKey(string key) => 
            EPPZ_Cloud_FloatForKey(key);

        public override void InitializeWithGameObjectName(string gameObjectName)
        {
            EPPZ_Cloud_InitializeWithGameObjectName(gameObjectName);
        }

        public override int IntForKey(string key) => 
            EPPZ_Cloud_IntForKey(key);

        public override void SetBoolForKey(bool value, string key)
        {
            base.Log(string.Concat(new object[] { "Cloud_iOS.SetBoolForKey(`", value, "`, `", key, "`)" }));
            EPPZ_Cloud_SetBoolForKey(value, key);
        }

        public override void SetFloatForKey(float value, string key)
        {
            base.Log(string.Concat(new object[] { "Cloud_iOS.SetFloatForKey(`", value, "`, `", key, "`)" }));
            EPPZ_Cloud_SetFloatForKey(value, key);
        }

        public override void SetIntForKey(int value, string key)
        {
            base.Log(string.Concat(new object[] { "Cloud_iOS.SetIntForKey(`", value, "`, `", key, "`)" }));
            EPPZ_Cloud_SetIntForKey(value, key);
        }

        public override void SetStringForKey(string value, string key)
        {
            base.Log("Cloud_iOS.SetStringForKey(`" + value + "`, `" + key + "`)");
            EPPZ_Cloud_SetStringForKey(value, key);
        }

        public override string StringForKey(string key) => 
            EPPZ_Cloud_StringForKey(key);

        public override void Synchronize()
        {
            EPPZ_Cloud_Synchronize();
        }
    }
}

