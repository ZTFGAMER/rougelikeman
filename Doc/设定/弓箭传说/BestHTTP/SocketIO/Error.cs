namespace BestHTTP.SocketIO
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class Error
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SocketIOErrors <Code>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Message>k__BackingField;

        public Error(SocketIOErrors code, string msg)
        {
            this.Code = code;
            this.Message = msg;
        }

        public override string ToString() => 
            $"Code: {this.Code.ToString()} Message: "{this.Message}"";

        public SocketIOErrors Code { get; private set; }

        public string Message { get; private set; }
    }
}

