namespace EPPZ.Cloud.Plugin.iOS
{
    using System;

    [Serializable]
    public class UserInfo
    {
        public NSUbiquitousKeyValueStoreChangeReason NSUbiquitousKeyValueStoreChangeReasonKey;
        public string[] NSUbiquitousKeyValueStoreChangedKeysKey;

        public enum NSUbiquitousKeyValueStoreChangeReason
        {
            NSUbiquitousKeyValueStoreServerChange,
            NSUbiquitousKeyValueStoreInitialSyncChange,
            NSUbiquitousKeyValueStoreQuotaViolationChange,
            NSUbiquitousKeyValueStoreAccountChange
        }
    }
}

