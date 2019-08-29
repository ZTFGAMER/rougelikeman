namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class MultiMessage : IServerMessage
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MessageId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsInitialization>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <GroupsToken>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <ShouldReconnect>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan? <PollDelay>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<IServerMessage> <Data>k__BackingField;

        void IServerMessage.Parse(object data)
        {
            IDictionary<string, object> dictionary = data as IDictionary<string, object>;
            this.MessageId = dictionary["C"].ToString();
            if (dictionary.TryGetValue("S", out object obj2))
            {
                this.IsInitialization = int.Parse(obj2.ToString()) == 1;
            }
            else
            {
                this.IsInitialization = false;
            }
            if (dictionary.TryGetValue("G", out obj2))
            {
                this.GroupsToken = obj2.ToString();
            }
            if (dictionary.TryGetValue("T", out obj2))
            {
                this.ShouldReconnect = int.Parse(obj2.ToString()) == 1;
            }
            else
            {
                this.ShouldReconnect = false;
            }
            if (dictionary.TryGetValue("L", out obj2))
            {
                this.PollDelay = new TimeSpan?(TimeSpan.FromMilliseconds(double.Parse(obj2.ToString())));
            }
            IEnumerable enumerable = dictionary["M"] as IEnumerable;
            if (enumerable != null)
            {
                this.Data = new List<IServerMessage>();
                IEnumerator enumerator = enumerable.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        IDictionary<string, object> dictionary2 = current as IDictionary<string, object>;
                        IServerMessage item = null;
                        if (dictionary2 != null)
                        {
                            if (dictionary2.ContainsKey("H"))
                            {
                                item = new MethodCallMessage();
                            }
                            else if (dictionary2.ContainsKey("I"))
                            {
                                item = new ProgressMessage();
                            }
                            else
                            {
                                item = new DataMessage();
                            }
                        }
                        else
                        {
                            item = new DataMessage();
                        }
                        item.Parse(current);
                        this.Data.Add(item);
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
            }
        }

        MessageTypes IServerMessage.Type =>
            MessageTypes.Multiple;

        public string MessageId { get; private set; }

        public bool IsInitialization { get; private set; }

        public string GroupsToken { get; private set; }

        public bool ShouldReconnect { get; private set; }

        public TimeSpan? PollDelay { get; private set; }

        public List<IServerMessage> Data { get; private set; }
    }
}

