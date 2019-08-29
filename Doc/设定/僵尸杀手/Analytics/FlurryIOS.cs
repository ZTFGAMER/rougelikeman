namespace Analytics
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class FlurryIOS
    {
        public static bool ActiveSessionExists() => 
            false;

        public static void AddOrigin(string originName, string originVersion)
        {
        }

        public static void AddOrigin(string originName, string originVersion, Dictionary<string, string> parameters)
        {
        }

        public static void EndTimedEvent(string eventName, Dictionary<string, string> parameters)
        {
        }

        public static string GetFlurryAgentVersion() => 
            string.Empty;

        public static void LogAllPageViewsForTarget(IntPtr target)
        {
            throw new NotSupportedException();
        }

        public static void LogError(string errorID, string message, Exception exception)
        {
        }

        public static EventRecordStatus LogEvent(string eventName) => 
            EventRecordStatus.Failed;

        public static EventRecordStatus LogEvent(string eventName, bool timed) => 
            EventRecordStatus.Failed;

        public static EventRecordStatus LogEvent(string eventName, Dictionary<string, string> parameters) => 
            EventRecordStatus.Failed;

        public static EventRecordStatus LogEvent(string eventName, Dictionary<string, string> parameters, bool timed) => 
            EventRecordStatus.Failed;

        public static void LogPageView()
        {
        }

        public static void PauseBackgroundSession()
        {
        }

        public static void SetAge(int age)
        {
        }

        public static void SetAppVersion(string version)
        {
        }

        public static void SetBackgroundSessionEnabled(bool setBackgroundSessionEnabled)
        {
        }

        public static void SetCrashReportingEnabled(bool value)
        {
        }

        public static void SetDebugLogEnabled(bool value)
        {
        }

        public static void SetEventLoggingEnabled(bool value)
        {
        }

        public static void SetGender(string gender)
        {
        }

        public static void SetLatitude(double latitude, double longitude, float horizontalAccuracy, float verticalAccuracy)
        {
        }

        public static void SetLogLevel(Analytics.LogLevel level)
        {
        }

        public static void SetSessionContinueSeconds(int seconds)
        {
        }

        public static void SetSessionReportsOnCloseEnabled(bool sendSessionReportsOnClose)
        {
        }

        public static void SetSessionReportsOnPauseEnabled(bool setSessionReportsOnPauseEnabled)
        {
        }

        public static void SetShowErrorInLogEnabled(bool value)
        {
        }

        public static void SetUserId(string userID)
        {
        }

        public static void StartSession(string apiKey)
        {
        }

        public static void StopLogPageViewsForTarget(IntPtr target)
        {
            throw new NotSupportedException();
        }

        private static void ToKeyValue(Dictionary<string, string> dictionary, out string keys, out string values)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            int num = 0;
            int count = dictionary.Count;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                builder.Append(pair.Key);
                builder2.Append(pair.Value);
                if (++num < count)
                {
                    builder.Append("\n");
                    builder2.Append("\n");
                }
            }
            keys = builder.ToString();
            values = builder2.ToString();
        }
    }
}

