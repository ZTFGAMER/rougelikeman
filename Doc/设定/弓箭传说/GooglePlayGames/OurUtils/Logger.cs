namespace GooglePlayGames.OurUtils
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Logger
    {
        private static bool debugLogEnabled;
        private static bool warningLogEnabled = true;

        public static void d(string msg)
        {
            <d>c__AnonStorey0 storey = new <d>c__AnonStorey0 {
                msg = msg
            };
            if (debugLogEnabled)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
        }

        public static string describe(byte[] b) => 
            ((b != null) ? ("byte[" + b.Length + "]") : "(null)");

        public static void e(string msg)
        {
            <e>c__AnonStorey2 storey = new <e>c__AnonStorey2 {
                msg = msg
            };
            if (warningLogEnabled)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
        }

        private static string ToLogMessage(string prefix, string logType, string msg) => 
            $"{prefix} [Play Games Plugin DLL] {DateTime.Now.ToString("MM/dd/yy H:mm:ss zzz")} {logType}: {msg}";

        public static void w(string msg)
        {
            <w>c__AnonStorey1 storey = new <w>c__AnonStorey1 {
                msg = msg
            };
            if (warningLogEnabled)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }
        }

        public static bool DebugLogEnabled
        {
            get => 
                debugLogEnabled;
            set => 
                (debugLogEnabled = value);
        }

        public static bool WarningLogEnabled
        {
            get => 
                warningLogEnabled;
            set => 
                (warningLogEnabled = value);
        }

        [CompilerGenerated]
        private sealed class <d>c__AnonStorey0
        {
            internal string msg;

            internal void <>m__0()
            {
                Debug.Log(Logger.ToLogMessage(string.Empty, "DEBUG", this.msg));
            }
        }

        [CompilerGenerated]
        private sealed class <e>c__AnonStorey2
        {
            internal string msg;

            internal void <>m__0()
            {
                Debug.LogWarning(Logger.ToLogMessage("***", "ERROR", this.msg));
            }
        }

        [CompilerGenerated]
        private sealed class <w>c__AnonStorey1
        {
            internal string msg;

            internal void <>m__0()
            {
                Debug.LogWarning(Logger.ToLogMessage("!!!", "WARNING", this.msg));
            }
        }
    }
}

