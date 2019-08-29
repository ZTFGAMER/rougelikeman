namespace BestHTTP.SocketIO
{
    using BestHTTP.SocketIO.Transports;
    using PlatformSupport.Collections.ObjectModel;
    using PlatformSupport.Collections.Specialized;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed class SocketOptions
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TransportTypes <ConnectWith>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Reconnection>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ReconnectionAttempts>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <ReconnectionDelay>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <ReconnectionDelayMax>k__BackingField;
        private float randomizationFactor;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <Timeout>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <AutoConnect>k__BackingField;
        private ObservableDictionary<string, string> additionalQueryParams;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <QueryParamsOnlyForHandshake>k__BackingField;
        private string BuiltQueryParams;

        public SocketOptions()
        {
            this.ConnectWith = TransportTypes.Polling;
            this.Reconnection = true;
            this.ReconnectionAttempts = 0x7fffffff;
            this.ReconnectionDelay = TimeSpan.FromMilliseconds(1000.0);
            this.ReconnectionDelayMax = TimeSpan.FromMilliseconds(5000.0);
            this.RandomizationFactor = 0.5f;
            this.Timeout = TimeSpan.FromMilliseconds(20000.0);
            this.AutoConnect = true;
            this.QueryParamsOnlyForHandshake = true;
        }

        private void AdditionalQueryParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.BuiltQueryParams = null;
        }

        internal string BuildQueryParams()
        {
            if ((this.AdditionalQueryParams == null) || (this.AdditionalQueryParams.Count == 0))
            {
                return string.Empty;
            }
            if (!string.IsNullOrEmpty(this.BuiltQueryParams))
            {
                return this.BuiltQueryParams;
            }
            StringBuilder builder = new StringBuilder(this.AdditionalQueryParams.Count * 4);
            foreach (KeyValuePair<string, string> pair in this.AdditionalQueryParams)
            {
                builder.Append("&");
                builder.Append(pair.Key);
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    builder.Append("=");
                    builder.Append(pair.Value);
                }
            }
            return (this.BuiltQueryParams = builder.ToString());
        }

        public TransportTypes ConnectWith { get; set; }

        public bool Reconnection { get; set; }

        public int ReconnectionAttempts { get; set; }

        public TimeSpan ReconnectionDelay { get; set; }

        public TimeSpan ReconnectionDelayMax { get; set; }

        public float RandomizationFactor
        {
            get => 
                this.randomizationFactor;
            set => 
                (this.randomizationFactor = Math.Min(1f, Math.Max(0f, value)));
        }

        public TimeSpan Timeout { get; set; }

        public bool AutoConnect { get; set; }

        public ObservableDictionary<string, string> AdditionalQueryParams
        {
            get => 
                this.additionalQueryParams;
            set
            {
                if (this.additionalQueryParams != null)
                {
                    this.additionalQueryParams.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.AdditionalQueryParams_CollectionChanged);
                }
                this.additionalQueryParams = value;
                this.BuiltQueryParams = null;
                if (value != null)
                {
                    value.CollectionChanged += new NotifyCollectionChangedEventHandler(this.AdditionalQueryParams_CollectionChanged);
                }
            }
        }

        public bool QueryParamsOnlyForHandshake { get; set; }
    }
}

