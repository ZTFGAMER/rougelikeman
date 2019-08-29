namespace Spine
{
    using SharpJson;
    using System;
    using System.IO;

    public static class Json
    {
        public static object Deserialize(TextReader text)
        {
            JsonDecoder decoder = new JsonDecoder {
                parseNumbersAsFloat = true
            };
            return decoder.Decode(text.ReadToEnd());
        }
    }
}

