namespace BestHTTP.Logger
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    public class DefaultLogger : ILogger
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Loglevels <Level>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FormatVerbose>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FormatInfo>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FormatWarn>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FormatErr>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FormatEx>k__BackingField;

        public DefaultLogger()
        {
            this.FormatVerbose = "D [{0}]: {1}";
            this.FormatInfo = "I [{0}]: {1}";
            this.FormatWarn = "W [{0}]: {1}";
            this.FormatErr = "Err [{0}]: {1}";
            this.FormatEx = "Ex [{0}]: {1} - Message: {2}  StackTrace: {3}";
            this.Level = !Debug.isDebugBuild ? Loglevels.Error : Loglevels.Warning;
        }

        public void Error(string division, string err)
        {
            if (this.Level <= Loglevels.Error)
            {
                try
                {
                    Debug.LogError(string.Format(this.FormatErr, division, err));
                }
                catch
                {
                }
            }
        }

        public void Exception(string division, string msg, System.Exception ex)
        {
            if (this.Level <= Loglevels.Exception)
            {
                try
                {
                    string str = string.Empty;
                    if (ex == null)
                    {
                        str = "null";
                    }
                    else
                    {
                        StringBuilder builder = new StringBuilder();
                        System.Exception innerException = ex;
                        int num = 1;
                        while (innerException != null)
                        {
                            builder.AppendFormat("{0}: {1} {2}", num++.ToString(), ex.Message, ex.StackTrace);
                            innerException = innerException.InnerException;
                            if (innerException != null)
                            {
                                builder.AppendLine();
                            }
                        }
                        str = builder.ToString();
                    }
                    Debug.LogError(string.Format(this.FormatEx, new object[] { division, msg, str, (ex == null) ? "null" : ex.StackTrace }));
                }
                catch
                {
                }
            }
        }

        public void Information(string division, string info)
        {
            if (this.Level <= Loglevels.Information)
            {
                try
                {
                    Debug.Log(string.Format(this.FormatInfo, division, info));
                }
                catch
                {
                }
            }
        }

        public void Verbose(string division, string verb)
        {
            if (this.Level <= Loglevels.All)
            {
                try
                {
                    Debug.Log(string.Format(this.FormatVerbose, division, verb));
                }
                catch
                {
                }
            }
        }

        public void Warning(string division, string warn)
        {
            if (this.Level <= Loglevels.Warning)
            {
                try
                {
                    Debug.LogWarning(string.Format(this.FormatWarn, division, warn));
                }
                catch
                {
                }
            }
        }

        public Loglevels Level { get; set; }

        public string FormatVerbose { get; set; }

        public string FormatInfo { get; set; }

        public string FormatWarn { get; set; }

        public string FormatErr { get; set; }

        public string FormatEx { get; set; }
    }
}

