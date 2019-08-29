namespace Analytics
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class Flurry : MonoSingleton<Flurry>, IAnalytics
    {
        private void Awake()
        {
            Application.RegisterLogCallback(new Application.LogCallback(this.ErrorHandler));
        }

        public EventRecordStatus BeginLogEvent(string eventName) => 
            FlurryAndroid.LogEvent(eventName, true);

        public EventRecordStatus BeginLogEvent(string eventName, Dictionary<string, string> parameters) => 
            FlurryAndroid.LogEvent(eventName, parameters, true);

        public void EndLogEvent(string eventName)
        {
            FlurryAndroid.EndTimedEvent(eventName);
        }

        public void EndLogEvent(string eventName, Dictionary<string, string> parameters)
        {
            FlurryAndroid.EndTimedEvent(eventName, parameters);
        }

        private void ErrorHandler(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Error)
            {
                this.LogError("Uncaught Unity Exception", condition, this);
            }
        }

        public void LogAppVersion(string version)
        {
            FlurryAndroid.SetVersionName(version);
        }

        public void LogError(string errorID, string message, object target)
        {
            FlurryAndroid.OnError(errorID, message, target.GetType().Name);
        }

        public EventRecordStatus LogEvent(string eventName) => 
            FlurryAndroid.LogEvent(eventName);

        public EventRecordStatus LogEvent(string eventName, bool timed) => 
            FlurryAndroid.LogEvent(eventName, timed);

        public EventRecordStatus LogEvent(string eventName, Dictionary<string, string> parameters) => 
            FlurryAndroid.LogEvent(eventName, parameters);

        public void LogUserAge(int age)
        {
            FlurryAndroid.SetAge(age);
        }

        public void LogUserGender(UserGender gender)
        {
            FlurryAndroid.SetGender((gender != UserGender.Male) ? ((gender != UserGender.Female) ? ((byte) (-1)) : ((byte) 0)) : ((byte) 1));
        }

        public void LogUserID(string userID)
        {
            FlurryAndroid.SetUserId(userID);
        }

        protected override void OnDestroy()
        {
            FlurryAndroid.Dispose();
            base.OnDestroy();
        }

        public void SetLogLevel(Analytics.LogLevel level)
        {
            FlurryAndroid.SetLogLevel(level);
        }

        public void StartSession(string apiKeyIOS, string apiKeyAndroid)
        {
            FlurryAndroid.Init(apiKeyAndroid);
            FlurryAndroid.OnStartSession();
        }
    }
}

