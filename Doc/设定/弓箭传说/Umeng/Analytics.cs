namespace Umeng
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Analytics
    {
        private static bool hasInit;
        private static string _AppKey;
        private static string _ChannelId;
        private static AndroidJavaClass _UpdateAgent;

        private static void CreateUmengManger()
        {
            GameObject obj2 = new GameObject();
            obj2.AddComponent<UmengManager>();
            obj2.name = "UmengManager";
        }

        public void Dispose()
        {
            Agent.Dispose();
            Context.Dispose();
        }

        public static void EnableActivityDurationTrack(bool isTraceActivity)
        {
            object[] args = new object[] { isTraceActivity };
            Agent.CallStatic("openActivityDurationTrack", args);
        }

        public static void Event(string eventId)
        {
            object[] args = new object[] { Context, eventId };
            Agent.CallStatic("onEvent", args);
        }

        public static void Event(string eventId, Dictionary<string, string> attributes)
        {
            object[] args = new object[] { Context, eventId, ToJavaHashMap(attributes) };
            Agent.CallStatic("onEvent", args);
        }

        public static void Event(string eventId, string label)
        {
            object[] args = new object[] { Context, eventId, label };
            Agent.CallStatic("onEvent", args);
        }

        public static void Event(string eventId, Dictionary<string, string> attributes, int value)
        {
            try
            {
                if (attributes == null)
                {
                    attributes = new Dictionary<string, string>();
                }
                if (attributes.ContainsKey("__ct__"))
                {
                    attributes["__ct__"] = value.ToString();
                    Event(eventId, attributes);
                }
                else
                {
                    attributes.Add("__ct__", value.ToString());
                    Event(eventId, attributes);
                    attributes.Remove("__ct__");
                }
            }
            catch (Exception)
            {
            }
        }

        public static void EventBegin(string eventId)
        {
            object[] args = new object[] { Context, eventId };
            Agent.CallStatic("onEventBegin", args);
        }

        public static void EventBegin(string eventId, string label)
        {
            object[] args = new object[] { Context, eventId, label };
            Agent.CallStatic("onEventBegin", args);
        }

        public static void EventBeginWithPrimarykeyAndAttributes(string eventId, string primaryKey, Dictionary<string, string> attributes)
        {
            object[] args = new object[] { Context, eventId, ToJavaHashMap(attributes), primaryKey };
            Agent.CallStatic("onKVEventBegin", args);
        }

        public static void EventDuration(string eventId, int milliseconds)
        {
            object[] args = new object[] { Context, eventId, (long) milliseconds };
            Agent.CallStatic("onEventDuration", args);
        }

        public static void EventDuration(string eventId, Dictionary<string, string> attributes, int milliseconds)
        {
            object[] args = new object[] { Context, eventId, ToJavaHashMap(attributes), (long) milliseconds };
            Agent.CallStatic("onEventDuration", args);
        }

        public static void EventDuration(string eventId, string label, int milliseconds)
        {
            object[] args = new object[] { Context, eventId, label, (long) milliseconds };
            Agent.CallStatic("onEventDuration", args);
        }

        public static void EventEnd(string eventId)
        {
            object[] args = new object[] { Context, eventId };
            Agent.CallStatic("onEventEnd", args);
        }

        public static void EventEnd(string eventId, string label)
        {
            object[] args = new object[] { Context, eventId, label };
            Agent.CallStatic("onEventEnd", args);
        }

        public static void EventEndWithPrimarykey(string eventId, string primaryKey)
        {
            object[] args = new object[] { Context, eventId, primaryKey };
            Agent.CallStatic("onKVEventEnd", args);
        }

        [Obsolete("Flush")]
        public static void Flush()
        {
            object[] args = new object[] { Context };
            Agent.CallStatic("flush", args);
        }

        public static string GetDeviceInfo()
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.umeng.analytics.UnityUtil");
            object[] args = new object[] { Context };
            return class2.CallStatic<string>("getDeviceInfo", args);
        }

        public static void onKillProcess()
        {
            object[] args = new object[] { Context };
            Agent.CallStatic("onKillProcess", args);
        }

        public static void onPause()
        {
            object[] args = new object[] { Context };
            Agent.CallStatic("onPause", args);
        }

        public static void onResume()
        {
            object[] args = new object[] { Context };
            Agent.CallStatic("onResume", args);
        }

        public static void PageBegin(string pageName)
        {
            object[] args = new object[] { pageName };
            Agent.CallStatic("onPageStart", args);
        }

        public static void PageEnd(string pageName)
        {
            object[] args = new object[] { pageName };
            Agent.CallStatic("onPageEnd", args);
        }

        public static void SetCheckDevice(bool value)
        {
            object[] args = new object[] { value };
            Agent.CallStatic("setCheckDevice", args);
        }

        public static void SetContinueSessionMillis(long milliseconds)
        {
            object[] args = new object[] { milliseconds };
            Agent.CallStatic("setSessionContinueMillis", args);
        }

        [Obsolete("SetEnableLocation已弃用")]
        public static void SetEnableLocation(bool reportLocation)
        {
            object[] args = new object[] { reportLocation };
            Agent.CallStatic("setAutoLocation", args);
        }

        public static void SetLatency(int value)
        {
            object[] args = new object[] { (long) value };
            Agent.CallStatic("setLatencyWindow", args);
        }

        public static void SetLogEnabled(bool value)
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.umeng.commonsdk.UMConfigure");
            object[] args = new object[] { value };
            class2.CallStatic("setLogEnabled", args);
        }

        public static void SetLogEncryptEnabled(bool value)
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.umeng.commonsdk.UMConfigure");
            object[] args = new object[] { value };
            class2.CallStatic("setEncryptEnabled", args);
        }

        public static void Start()
        {
            UMGameAgentInit();
            if (!hasInit)
            {
                onResume();
                CreateUmengManger();
                hasInit = true;
            }
        }

        private static AndroidJavaObject ToJavaHashMap(Dictionary<string, string> dic)
        {
            AndroidJavaObject obj2 = new AndroidJavaObject("java.util.HashMap", Array.Empty<object>());
            IntPtr methodID = AndroidJNIHelper.GetMethodID(obj2.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            object[] args = new object[2];
            foreach (KeyValuePair<string, string> pair in dic)
            {
                object[] objArray1 = new object[] { pair.Key };
                using (AndroidJavaObject obj3 = new AndroidJavaObject("java.lang.String", objArray1))
                {
                    object[] objArray2 = new object[] { pair.Value };
                    using (AndroidJavaObject obj4 = new AndroidJavaObject("java.lang.String", objArray2))
                    {
                        args[0] = obj3;
                        args[1] = obj4;
                        AndroidJNI.CallObjectMethod(obj2.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }
            }
            return obj2;
        }

        public static void UMGameAgentInit()
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.umeng.analytics.UnityUtil");
            object[] args = new object[] { Context, string.Empty, string.Empty };
            class2.CallStatic("initUnity", args);
        }

        public static string AppKey =>
            _AppKey;

        public static string ChannelId =>
            _ChannelId;

        protected static AndroidJavaClass Agent =>
            SingletonHolder.instance_mobclick;

        protected static AndroidJavaClass UpdateAgent
        {
            get
            {
                if (_UpdateAgent == null)
                {
                    _UpdateAgent = new AndroidJavaClass("com.umeng.update.UmengUpdateAgent");
                }
                return _UpdateAgent;
            }
        }

        protected static AndroidJavaObject Context =>
            SingletonHolder.instance_context;

        private static class SingletonHolder
        {
            public static AndroidJavaClass instance_mobclick = new AndroidJavaClass("com.umeng.analytics.game.UMGameAgent");
            public static AndroidJavaObject instance_context;

            static SingletonHolder()
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    instance_context = class2.GetStatic<AndroidJavaObject>("currentActivity");
                }
            }
        }
    }
}

