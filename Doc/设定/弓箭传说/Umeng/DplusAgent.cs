namespace Umeng
{
    using System;
    using UnityEngine;

    public class DplusAgent
    {
        private static AndroidJavaObject _DplusAgent = new AndroidJavaClass("com.umeng.analytics.dplus.UMADplus");
        private static AndroidJavaObject Context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        private static AndroidJavaClass UnityUtil = new AndroidJavaClass("com.umeng.analytics.UnityUtil");

        public static void clearSuperProperties()
        {
            object[] args = new object[] { Context };
            _DplusAgent.CallStatic("clearSuperProperties", args);
        }

        public static JSONObject getSuperProperties()
        {
            object[] args = new object[] { Context };
            return (JSONObject) JSON.Parse(_DplusAgent.CallStatic<string>("getSuperProperties", args));
        }

        public static JSONNode getSuperProperty(string propertyName)
        {
            object[] args = new object[] { Context, propertyName };
            return JSON.Parse(UnityUtil.CallStatic<string>("getSuperProperty", args))["__umeng_internal_data_"];
        }

        public static void registerSuperProperty(JSONObject jsonObject)
        {
            object[] args = new object[] { Context, jsonObject.ToString() };
            UnityUtil.CallStatic("registerSuperPropertyAll", args);
        }

        public static void setFirstLaunchEvent(string[] trackID)
        {
            object[] args = new object[] { Context, string.Join(";=umengUnity=;", trackID) };
            UnityUtil.CallStatic("setFirstLaunchEvent", args);
        }

        public static void track(string eventName)
        {
            object[] args = new object[] { Context, eventName };
            _DplusAgent.CallStatic("track", args);
        }

        public static void track(string eventName, JSONObject jsonObject)
        {
            object[] args = new object[] { Context, eventName, jsonObject.ToString() };
            UnityUtil.CallStatic("track", args);
        }

        public static void unregisterSuperProperty(string propertyName)
        {
            object[] args = new object[] { Context, propertyName };
            _DplusAgent.CallStatic("unregisterSuperProperty", args);
        }
    }
}

