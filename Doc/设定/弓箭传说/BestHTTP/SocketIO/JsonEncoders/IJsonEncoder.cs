namespace BestHTTP.SocketIO.JsonEncoders
{
    using System;
    using System.Collections.Generic;

    public interface IJsonEncoder
    {
        List<object> Decode(string json);
        string Encode(List<object> obj);
    }
}

