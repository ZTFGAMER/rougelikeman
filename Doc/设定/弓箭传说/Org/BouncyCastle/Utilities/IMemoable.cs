namespace Org.BouncyCastle.Utilities
{
    using System;

    public interface IMemoable
    {
        IMemoable Copy();
        void Reset(IMemoable other);
    }
}

