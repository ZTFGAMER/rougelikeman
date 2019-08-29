namespace BestHTTP.SignalR.JsonEncoders
{
    using System;
    using System.Collections.Generic;

    public interface IJsonEncoder
    {
        IDictionary<string, object> DecodeMessage(string json);
        string Encode(object obj);
    }
}

