namespace AudienceNetwork
{
    using System;
    using UnityEngine;

    internal class AdLogger
    {
        private static AdLogLevel logLevel = AdLogLevel.Log;
        private static readonly string logPrefix = "Audience Network Unity ";

        private static string levelAsString(AdLogLevel logLevel)
        {
            switch (logLevel)
            {
                case AdLogLevel.Notification:
                    return string.Empty;

                case AdLogLevel.Error:
                    return "<error>: ";

                case AdLogLevel.Warning:
                    return "<warn>: ";

                case AdLogLevel.Log:
                    return "<log>: ";

                case AdLogLevel.Debug:
                    return "<debug>: ";

                case AdLogLevel.Verbose:
                    return "<verbose>: ";
            }
            return string.Empty;
        }

        internal static void Log(string message)
        {
            AdLogLevel log = AdLogLevel.Log;
            if (logLevel >= log)
            {
                Debug.Log(logPrefix + levelAsString(log) + message);
            }
        }

        internal static void LogError(string message)
        {
            AdLogLevel error = AdLogLevel.Error;
            if (logLevel >= error)
            {
                Debug.LogError(logPrefix + levelAsString(error) + message);
            }
        }

        internal static void LogWarning(string message)
        {
            AdLogLevel warning = AdLogLevel.Warning;
            if (logLevel >= warning)
            {
                Debug.LogWarning(logPrefix + levelAsString(warning) + message);
            }
        }

        private enum AdLogLevel
        {
            None,
            Notification,
            Error,
            Warning,
            Log,
            Debug,
            Verbose
        }
    }
}

