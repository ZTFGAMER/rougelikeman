namespace PureMVC.Patterns
{
    using PureMVC.Interfaces;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Proxy : Notifier, IProxy, INotifier
    {
        public static string NAME = "Proxy";
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ProxyName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <Data>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action <Event_Para0>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<object> <Event_Para1>k__BackingField;

        public Proxy() : this(NAME, null)
        {
        }

        public Proxy(string proxyName) : this(proxyName, null)
        {
        }

        public Proxy(string proxyName, object data)
        {
            if (proxyName == null)
            {
            }
            this.ProxyName = NAME;
            if (data != null)
            {
                this.Data = data;
            }
        }

        public virtual void OnRegister()
        {
        }

        public virtual void OnRemove()
        {
        }

        public string ProxyName { get; protected set; }

        public object Data { get; set; }

        public Action Event_Para0 { get; set; }

        public Action<object> Event_Para1 { get; set; }
    }
}

