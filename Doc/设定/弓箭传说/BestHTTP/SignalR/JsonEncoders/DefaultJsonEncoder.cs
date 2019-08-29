namespace BestHTTP.SignalR.JsonEncoders
{
    using BestHTTP.JSON;
    using System;
    using System.Collections.Generic;

    public sealed class DefaultJsonEncoder : IJsonEncoder
    {
        public IDictionary<string, object> DecodeMessage(string json)
        {
            bool success = false;
            IDictionary<string, object> dictionary = Json.Decode(json, ref success) as IDictionary<string, object>;
            return (!success ? null : dictionary);
        }

        public string Encode(object obj) => 
            Json.Encode(obj);
    }
}

