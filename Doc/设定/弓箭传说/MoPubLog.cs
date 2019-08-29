using System;
using System.Collections.Generic;
using UnityEngine;

public static class MoPubLog
{
    private static readonly Dictionary<string, MoPubBase.LogLevel> logLevelMap;

    static MoPubLog()
    {
        Dictionary<string, MoPubBase.LogLevel> dictionary = new Dictionary<string, MoPubBase.LogLevel> {
            { 
                "SDK initialization started",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "SDK initialized and ready to display ads.  Log Level: {0}",
                MoPubBase.LogLevel.MPLogLevelInfo
            },
            { 
                "Consent changed to {0} from {1}: PII can{2} be collected. Reason: {3}",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Attempting to load consent dialog",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Consent dialog loaded",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Consent dialog failed: ({0}) {1}",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Consent dialog attempting to show",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Sucessfully showed consent dialog",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Attempting to load ad",
                MoPubBase.LogLevel.MPLogLevelInfo
            },
            { 
                "Ad loaded",
                MoPubBase.LogLevel.MPLogLevelInfo
            },
            { 
                "Ad failed to load: ({0}) {1}",
                MoPubBase.LogLevel.MPLogLevelInfo
            },
            { 
                "Attempting to show ad",
                MoPubBase.LogLevel.MPLogLevelInfo
            },
            { 
                "Ad shown",
                MoPubBase.LogLevel.MPLogLevelInfo
            },
            { 
                "Ad tapped",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Ad expanded",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Ad collapsed",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Ad did disappear",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Ad should reward user with {0} {1}",
                MoPubBase.LogLevel.MPLogLevelDebug
            },
            { 
                "Ad expired since it was not shown within {0} minutes of it being loaded",
                MoPubBase.LogLevel.MPLogLevelDebug
            }
        };
        logLevelMap = dictionary;
    }

    public static void Log(string callerMethod, string message, params object[] args)
    {
        if (!logLevelMap.TryGetValue(message, out MoPubBase.LogLevel mPLogLevelDebug))
        {
            mPLogLevelDebug = MoPubBase.LogLevel.MPLogLevelDebug;
        }
        if (MoPubBase.logLevel <= mPLogLevelDebug)
        {
            string format = "[MoPub-Unity] [" + callerMethod + "] " + message;
            try
            {
                Debug.LogFormat(format, args);
            }
            catch (FormatException)
            {
                Debug.Log(format);
            }
        }
    }

    public static class AdLogEvent
    {
        public const string LoadAttempted = "Attempting to load ad";
        public const string LoadSuccess = "Ad loaded";
        public const string LoadFailed = "Ad failed to load: ({0}) {1}";
        public const string ShowAttempted = "Attempting to show ad";
        public const string ShowSuccess = "Ad shown";
        public const string Tapped = "Ad tapped";
        public const string Expanded = "Ad expanded";
        public const string Collapsed = "Ad collapsed";
        public const string Dismissed = "Ad did disappear";
        public const string ShouldReward = "Ad should reward user with {0} {1}";
        public const string Expired = "Ad expired since it was not shown within {0} minutes of it being loaded";
    }

    public static class ConsentLogEvent
    {
        public const string Updated = "Consent changed to {0} from {1}: PII can{2} be collected. Reason: {3}";
        public const string LoadAttempted = "Attempting to load consent dialog";
        public const string LoadSuccess = "Consent dialog loaded";
        public const string LoadFailed = "Consent dialog failed: ({0}) {1}";
        public const string ShowAttempted = "Consent dialog attempting to show";
        public const string ShowSuccess = "Sucessfully showed consent dialog";
    }

    public static class SdkLogEvent
    {
        public const string InitStarted = "SDK initialization started";
        public const string InitFinished = "SDK initialized and ready to display ads.  Log Level: {0}";
    }
}

