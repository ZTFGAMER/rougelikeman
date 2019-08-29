namespace PlatformSupport.Collections.Specialized
{
    using System;

    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}

