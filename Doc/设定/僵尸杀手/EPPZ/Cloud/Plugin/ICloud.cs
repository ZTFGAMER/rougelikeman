namespace EPPZ.Cloud.Plugin
{
    using EPPZ.Cloud;
    using System;

    public interface ICloud
    {
        void _CloudDidChange(string message);
        void _OnCloudChange(string[] changedKeys, ChangeReason changeReason);
    }
}

