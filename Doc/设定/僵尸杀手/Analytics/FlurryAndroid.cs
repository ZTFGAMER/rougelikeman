namespace Analytics
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class FlurryAndroid
    {
        private static readonly string s_FlurryAgentClassName = "com.flurry.android.FlurryAgent";
        private static readonly string s_UnityPlayerClassName = "com.unity3d.player.UnityPlayer";
        private static readonly string s_UnityPlayerActivityName = "currentActivity";
        private static AndroidJavaClass s_FlurryAgent;

        public static void AddOrigin(string originName, string originVersion)
        {
            object[] args = new object[] { originName, originVersion };
            FlurryAgent.CallStatic("addOrigin", args);
        }

        public static void AddOrigin(string originName, string originVersion, Dictionary<string, string> originParameters)
        {
            using (AndroidJavaObject obj2 = DictionaryToJavaHashMap(originParameters))
            {
                object[] args = new object[] { originName, originVersion, obj2 };
                FlurryAgent.CallStatic("addOrigin", args);
            }
        }

        public static void ClearLocation()
        {
            FlurryAgent.CallStatic("clearLocation", new object[0]);
        }

        private static AndroidJavaObject DictionaryToJavaHashMap(Dictionary<string, string> dictionary)
        {
            AndroidJavaObject obj2 = new AndroidJavaObject("java.util.HashMap", new object[0]);
            IntPtr methodID = AndroidJNIHelper.GetMethodID(obj2.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                object[] args = new object[] { pair.Key };
                using (AndroidJavaObject obj3 = new AndroidJavaObject("java.lang.String", args))
                {
                    object[] objArray2 = new object[] { pair.Value };
                    using (AndroidJavaObject obj4 = new AndroidJavaObject("java.lang.String", objArray2))
                    {
                        object[] objArray3 = new object[] { obj3, obj4 };
                        AndroidJNI.CallObjectMethod(obj2.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(objArray3));
                    }
                }
            }
            return obj2;
        }

        public static void Dispose()
        {
            if (s_FlurryAgent != null)
            {
                s_FlurryAgent.Dispose();
            }
            s_FlurryAgent = null;
        }

        public static void EndTimedEvent(string eventId)
        {
            object[] args = new object[] { eventId };
            FlurryAgent.CallStatic("endTimedEvent", args);
        }

        public static void EndTimedEvent(string eventId, Dictionary<string, string> parameters)
        {
            using (AndroidJavaObject obj2 = DictionaryToJavaHashMap(parameters))
            {
                object[] args = new object[] { eventId, obj2 };
                FlurryAgent.CallStatic("endTimedEvent", args);
            }
        }

        public static int GetAgentVersion() => 
            FlurryAgent.CallStatic<int>("getAgentVersion", new object[0]);

        public static string GetReleaseVersion() => 
            FlurryAgent.CallStatic<string>("getReleaseVersion", new object[0]);

        public static string GetSessionId() => 
            FlurryAgent.CallStatic<string>("getSessionId", new object[0]);

        public static void Init(string apiKey)
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass(s_UnityPlayerClassName))
            {
                using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>(s_UnityPlayerActivityName))
                {
                    object[] args = new object[] { obj2, apiKey };
                    FlurryAgent.CallStatic("init", args);
                }
            }
        }

        public static bool IsSessionActive() => 
            FlurryAgent.CallStatic<bool>("isSessionActive", new object[0]);

        private static EventRecordStatus JavaObjectToEventRecordStatus(AndroidJavaObject javaObject) => 
            javaObject.Call<int>("ordinal", new object[0]);

        public static EventRecordStatus LogEvent(string eventId)
        {
            object[] args = new object[] { eventId };
            return JavaObjectToEventRecordStatus(FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", args));
        }

        public static EventRecordStatus LogEvent(string eventId, bool timed)
        {
            object[] args = new object[] { eventId, timed };
            return JavaObjectToEventRecordStatus(FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", args));
        }

        public static EventRecordStatus LogEvent(string eventId, Dictionary<string, string> parameters)
        {
            using (AndroidJavaObject obj2 = DictionaryToJavaHashMap(parameters))
            {
                object[] args = new object[] { eventId, obj2, false };
                return JavaObjectToEventRecordStatus(FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", args));
            }
        }

        public static EventRecordStatus LogEvent(string eventId, Dictionary<string, string> parameters, bool timed)
        {
            using (AndroidJavaObject obj2 = DictionaryToJavaHashMap(parameters))
            {
                object[] args = new object[] { eventId, obj2, timed };
                return JavaObjectToEventRecordStatus(FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", args));
            }
        }

        public static void OnEndSession()
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass(s_UnityPlayerClassName))
            {
                using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>(s_UnityPlayerActivityName))
                {
                    object[] args = new object[] { obj2 };
                    FlurryAgent.CallStatic("onEndSession", args);
                }
            }
        }

        public static void OnError(string errorId, string message, string errorClass)
        {
            object[] args = new object[] { errorId, message, errorClass };
            FlurryAgent.CallStatic("onError", args);
        }

        public static void OnPageView()
        {
            FlurryAgent.CallStatic("onPageView", new object[0]);
        }

        public static void OnStartSession()
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass(s_UnityPlayerClassName))
            {
                using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>(s_UnityPlayerActivityName))
                {
                    object[] args = new object[] { obj2 };
                    FlurryAgent.CallStatic("onStartSession", args);
                }
            }
        }

        public static void SetAge(int age)
        {
            object[] args = new object[] { age };
            FlurryAgent.CallStatic("setAge", args);
        }

        public static void SetCaptureUncaughtExceptions(bool isEnabled)
        {
            object[] args = new object[] { isEnabled };
            FlurryAgent.CallStatic("setCaptureUncaughtExceptions", args);
        }

        public static void SetContinueSessionMillis(long millis)
        {
            object[] args = new object[] { millis };
            FlurryAgent.CallStatic("setContinueSessionMillis", args);
        }

        public static void SetGender(byte gender)
        {
            object[] args = new object[] { gender };
            FlurryAgent.CallStatic("setGender", args);
        }

        public static void SetLocation(float lat, float lon)
        {
            object[] args = new object[] { lat, lon };
            FlurryAgent.CallStatic("setLocation", args);
        }

        public static void SetLogEnabled(bool isEnabled)
        {
            object[] args = new object[] { isEnabled };
            FlurryAgent.CallStatic("setLogEnabled", args);
        }

        public static void SetLogEvents(bool logEvents)
        {
            object[] args = new object[] { logEvents };
            FlurryAgent.CallStatic("setLogEvents", args);
        }

        public static void SetLogLevel(Analytics.LogLevel logLevel)
        {
            object[] args = new object[] { (int) logLevel };
            FlurryAgent.CallStatic("setLogLevel", args);
        }

        public static void SetPulseEnabled(bool isEnabled)
        {
            object[] args = new object[] { isEnabled };
            FlurryAgent.CallStatic("setPulseEnabled", args);
        }

        public static void SetReportLocation(bool reportLocation)
        {
            object[] args = new object[] { reportLocation };
            FlurryAgent.CallStatic("setReportLocation", args);
        }

        public static void SetUserId(string userId)
        {
            object[] args = new object[] { userId };
            FlurryAgent.CallStatic("setUserId", args);
        }

        public static void SetVersionName(string versionName)
        {
            object[] args = new object[] { versionName };
            FlurryAgent.CallStatic("setVersionName", args);
        }

        private static AndroidJavaClass FlurryAgent
        {
            get
            {
                if (Application.platform != RuntimePlatform.Android)
                {
                    return null;
                }
                if (s_FlurryAgent == null)
                {
                    s_FlurryAgent = new AndroidJavaClass(s_FlurryAgentClassName);
                }
                return s_FlurryAgent;
            }
        }
    }
}

