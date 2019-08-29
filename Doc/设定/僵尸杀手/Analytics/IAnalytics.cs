namespace Analytics
{
    using System;
    using System.Collections.Generic;

    public interface IAnalytics
    {
        EventRecordStatus BeginLogEvent(string eventName);
        EventRecordStatus BeginLogEvent(string eventName, Dictionary<string, string> parameters);
        void EndLogEvent(string eventName);
        void EndLogEvent(string eventName, Dictionary<string, string> parameters);
        void LogAppVersion(string version);
        void LogError(string errorID, string message, object target);
        EventRecordStatus LogEvent(string eventName);
        EventRecordStatus LogEvent(string eventName, Dictionary<string, string> parameters);
        void LogUserAge(int age);
        void LogUserGender(UserGender gender);
        void LogUserID(string userID);
        void SetLogLevel(Analytics.LogLevel level);
        void StartSession(string apiKeyIOS, string apiKeyAndroid);
    }
}

