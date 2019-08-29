namespace LitJson
{
    using System;

    public enum JsonToken
    {
        None,
        ObjectStart,
        PropertyName,
        ObjectEnd,
        ArrayStart,
        ArrayEnd,
        Int,
        Long,
        Double,
        String,
        Boolean,
        Null
    }
}

