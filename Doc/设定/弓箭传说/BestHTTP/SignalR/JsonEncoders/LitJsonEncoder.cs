namespace BestHTTP.SignalR.JsonEncoders
{
    using LitJson;
    using System;
    using System.Collections.Generic;

    public sealed class LitJsonEncoder : IJsonEncoder
    {
        public IDictionary<string, object> DecodeMessage(string json)
        {
            JsonReader reader = new JsonReader(json);
            return JsonMapper.ToObject<Dictionary<string, object>>(reader);
        }

        public string Encode(object obj)
        {
            JsonWriter writer = new JsonWriter();
            JsonMapper.ToJson(obj, writer);
            return writer.ToString();
        }
    }
}

