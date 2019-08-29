namespace BestHTTP.Logger
{
    using System;

    public interface ILogger
    {
        void Error(string division, string err);
        void Exception(string division, string msg, System.Exception ex);
        void Information(string division, string info);
        void Verbose(string division, string verb);
        void Warning(string division, string warn);

        Loglevels Level { get; set; }

        string FormatVerbose { get; set; }

        string FormatInfo { get; set; }

        string FormatWarn { get; set; }

        string FormatErr { get; set; }

        string FormatEx { get; set; }
    }
}

