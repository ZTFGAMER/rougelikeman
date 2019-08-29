namespace LitJson
{
    using System;

    internal enum ParserToken
    {
        None = 0x10000,
        Number = 0x10001,
        True = 0x10002,
        False = 0x10003,
        Null = 0x10004,
        CharSeq = 0x10005,
        Char = 0x10006,
        Text = 0x10007,
        Object = 0x10008,
        ObjectPrime = 0x10009,
        Pair = 0x1000a,
        PairRest = 0x1000b,
        Array = 0x1000c,
        ArrayPrime = 0x1000d,
        Value = 0x1000e,
        ValueRest = 0x1000f,
        String = 0x10010,
        End = 0x10011,
        Epsilon = 0x10012
    }
}

