namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class MethodCallMessage : IServerMessage
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Hub>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Method>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object[] <Arguments>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<string, object> <State>k__BackingField;

        void IServerMessage.Parse(object data)
        {
            IDictionary<string, object> dictionary = data as IDictionary<string, object>;
            this.Hub = dictionary["H"].ToString();
            this.Method = dictionary["M"].ToString();
            List<object> list = new List<object>();
            IEnumerator enumerator = (dictionary["A"] as IEnumerable).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    list.Add(current);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
            this.Arguments = list.ToArray();
            if (dictionary.TryGetValue("S", out object obj3))
            {
                this.State = obj3 as IDictionary<string, object>;
            }
        }

        MessageTypes IServerMessage.Type =>
            MessageTypes.MethodCall;

        public string Hub { get; private set; }

        public string Method { get; private set; }

        public object[] Arguments { get; private set; }

        public IDictionary<string, object> State { get; private set; }
    }
}

