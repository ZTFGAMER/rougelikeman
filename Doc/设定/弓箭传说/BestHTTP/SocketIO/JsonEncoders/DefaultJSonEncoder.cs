namespace BestHTTP.SocketIO.JsonEncoders
{
    using BestHTTP.JSON;
    using System;
    using System.Collections.Generic;

    public sealed class DefaultJSonEncoder : IJsonEncoder
    {
        public List<object> Decode(string json) => 
            (Json.Decode(json) as List<object>);

        public string Encode(List<object> obj) => 
            Json.Encode(obj);
    }
}

