namespace BestHTTP.Extensions
{
    using System;

    public interface IHeartbeat
    {
        void OnHeartbeatUpdate(TimeSpan dif);
    }
}

