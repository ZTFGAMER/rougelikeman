namespace EPPZ.Cloud
{
    using System;

    public enum ChangeReason
    {
        ServerChange,
        InitialSyncChange,
        QuotaViolationChange,
        AccountChange
    }
}

