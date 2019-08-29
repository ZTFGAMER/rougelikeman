namespace BestHTTP.Extensions
{
    using System;
    using System.Collections.Generic;

    public sealed class HeartbeatManager
    {
        private List<IHeartbeat> Heartbeats = new List<IHeartbeat>();
        private IHeartbeat[] UpdateArray;
        private DateTime LastUpdate = DateTime.MinValue;

        public void Subscribe(IHeartbeat heartbeat)
        {
            object heartbeats = this.Heartbeats;
            lock (heartbeats)
            {
                if (!this.Heartbeats.Contains(heartbeat))
                {
                    this.Heartbeats.Add(heartbeat);
                }
            }
        }

        public void Unsubscribe(IHeartbeat heartbeat)
        {
            object heartbeats = this.Heartbeats;
            lock (heartbeats)
            {
                this.Heartbeats.Remove(heartbeat);
            }
        }

        public void Update()
        {
            if (this.LastUpdate == DateTime.MinValue)
            {
                this.LastUpdate = DateTime.UtcNow;
            }
            else
            {
                TimeSpan dif = (TimeSpan) (DateTime.UtcNow - this.LastUpdate);
                this.LastUpdate = DateTime.UtcNow;
                int count = 0;
                object heartbeats = this.Heartbeats;
                lock (heartbeats)
                {
                    if ((this.UpdateArray == null) || (this.UpdateArray.Length < this.Heartbeats.Count))
                    {
                        Array.Resize<IHeartbeat>(ref this.UpdateArray, this.Heartbeats.Count);
                    }
                    this.Heartbeats.CopyTo(0, this.UpdateArray, 0, this.Heartbeats.Count);
                    count = this.Heartbeats.Count;
                }
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        this.UpdateArray[i].OnHeartbeatUpdate(dif);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}

