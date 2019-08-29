namespace BestHTTP
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class HTTPRange
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <FirstBytePos>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LastBytePos>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ContentLength>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsValid>k__BackingField;

        internal HTTPRange()
        {
            this.ContentLength = -1;
            this.IsValid = false;
        }

        internal HTTPRange(int contentLength)
        {
            this.ContentLength = contentLength;
            this.IsValid = false;
        }

        internal HTTPRange(int firstBytePosition, int lastBytePosition, int contentLength)
        {
            this.FirstBytePos = firstBytePosition;
            this.LastBytePos = lastBytePosition;
            this.ContentLength = contentLength;
            this.IsValid = (this.FirstBytePos <= this.LastBytePos) && (this.ContentLength > this.LastBytePos);
        }

        public override string ToString() => 
            $"{this.FirstBytePos}-{this.LastBytePos}/{this.ContentLength} (valid: {this.IsValid})";

        public int FirstBytePos { get; private set; }

        public int LastBytePos { get; private set; }

        public int ContentLength { get; private set; }

        public bool IsValid { get; private set; }
    }
}

