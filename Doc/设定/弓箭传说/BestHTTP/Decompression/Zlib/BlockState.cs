namespace BestHTTP.Decompression.Zlib
{
    using System;

    internal enum BlockState
    {
        NeedMore,
        BlockDone,
        FinishStarted,
        FinishDone
    }
}

