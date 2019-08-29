namespace BestHTTP.SocketIO
{
    using BestHTTP.JSON;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class HandshakeData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Sid>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> <Upgrades>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <PingInterval>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <PingTimeout>k__BackingField;

        private static object Get(Dictionary<string, object> from, string key)
        {
            if (!from.TryGetValue(key, out object obj2))
            {
                throw new Exception($"Can't get {key} from Handshake data!");
            }
            return obj2;
        }

        private static int GetInt(Dictionary<string, object> from, string key) => 
            ((int) ((double) Get(from, key)));

        private static string GetString(Dictionary<string, object> from, string key) => 
            (Get(from, key) as string);

        private static List<string> GetStringList(Dictionary<string, object> from, string key)
        {
            List<object> list = Get(from, key) as List<object>;
            List<string> list2 = new List<string>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                string item = list[i] as string;
                if (item != null)
                {
                    list2.Add(item);
                }
            }
            return list2;
        }

        public bool Parse(string str)
        {
            bool success = false;
            Dictionary<string, object> from = Json.Decode(str, ref success) as Dictionary<string, object>;
            if (!success)
            {
                return false;
            }
            try
            {
                this.Sid = GetString(from, "sid");
                this.Upgrades = GetStringList(from, "upgrades");
                this.PingInterval = TimeSpan.FromMilliseconds((double) GetInt(from, "pingInterval"));
                this.PingTimeout = TimeSpan.FromMilliseconds((double) GetInt(from, "pingTimeout"));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string Sid { get; private set; }

        public List<string> Upgrades { get; private set; }

        public TimeSpan PingInterval { get; private set; }

        public TimeSpan PingTimeout { get; private set; }
    }
}

