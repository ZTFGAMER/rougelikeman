namespace Analytics
{
    using System;

    public enum EventRecordStatus
    {
        Failed,
        Recorded,
        UniqueCountExceeded,
        ParamsCountExceeded,
        LogCountExceeded,
        LoggingDelayed
    }
}

