namespace BestHTTP
{
    using System;

    public interface IProtocol
    {
        void HandleEvents();

        bool IsClosed { get; }
    }
}

