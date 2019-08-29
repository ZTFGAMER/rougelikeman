namespace BestHTTP.SocketIO
{
    using BestHTTP.JSON;
    using BestHTTP.SocketIO.JsonEncoders;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class Packet
    {
        public const string Placeholder = "_placeholder";
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TransportEventTypes <TransportEvent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SocketIOEventTypes <SocketIOEvent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <AttachmentCount>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Id>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Namespace>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Payload>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <EventName>k__BackingField;
        private List<byte[]> attachments;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsDecoded>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object[] <DecodedArgs>k__BackingField;

        internal Packet()
        {
            this.TransportEvent = TransportEventTypes.Unknown;
            this.SocketIOEvent = SocketIOEventTypes.Unknown;
            this.Payload = string.Empty;
        }

        internal Packet(string from)
        {
            this.Parse(from);
        }

        public Packet(TransportEventTypes transportEvent, SocketIOEventTypes packetType, string nsp, string payload, int attachment = 0, int id = 0)
        {
            this.TransportEvent = transportEvent;
            this.SocketIOEvent = packetType;
            this.Namespace = nsp;
            this.Payload = payload;
            this.AttachmentCount = attachment;
            this.Id = id;
        }

        internal void AddAttachmentFromServer(byte[] data, bool copyFull)
        {
            if ((data != null) && (data.Length != 0))
            {
                if (this.attachments == null)
                {
                    this.attachments = new List<byte[]>(this.AttachmentCount);
                }
                if (copyFull)
                {
                    this.Attachments.Add(data);
                }
                else
                {
                    byte[] destinationArray = new byte[data.Length - 1];
                    Array.Copy(data, 1, destinationArray, 0, data.Length - 1);
                    this.Attachments.Add(destinationArray);
                }
            }
        }

        internal Packet Clone() => 
            new Packet(this.TransportEvent, this.SocketIOEvent, this.Namespace, this.Payload, 0, this.Id) { 
                EventName = this.EventName,
                AttachmentCount = this.AttachmentCount,
                attachments = this.attachments
            };

        public object[] Decode(IJsonEncoder encoder)
        {
            if (!this.IsDecoded && (encoder != null))
            {
                this.IsDecoded = true;
                if (string.IsNullOrEmpty(this.Payload))
                {
                    return this.DecodedArgs;
                }
                List<object> list = encoder.Decode(this.Payload);
                if ((list != null) && (list.Count > 0))
                {
                    if ((this.SocketIOEvent == SocketIOEventTypes.Ack) || (this.SocketIOEvent == SocketIOEventTypes.BinaryAck))
                    {
                        this.DecodedArgs = list.ToArray();
                    }
                    else
                    {
                        list.RemoveAt(0);
                        this.DecodedArgs = list.ToArray();
                    }
                }
            }
            return this.DecodedArgs;
        }

        public string DecodeEventName()
        {
            if (!string.IsNullOrEmpty(this.EventName))
            {
                return this.EventName;
            }
            if (string.IsNullOrEmpty(this.Payload))
            {
                return string.Empty;
            }
            if (this.Payload[0] != '[')
            {
                return string.Empty;
            }
            int num = 1;
            while (((this.Payload.Length > num) && (this.Payload[num] != '"')) && (this.Payload[num] != '\''))
            {
                num++;
            }
            if (this.Payload.Length <= num)
            {
                return string.Empty;
            }
            int startIndex = ++num;
            while (((this.Payload.Length > num) && (this.Payload[num] != '"')) && (this.Payload[num] != '\''))
            {
                num++;
            }
            if (this.Payload.Length <= num)
            {
                return string.Empty;
            }
            string str = this.Payload.Substring(startIndex, num - startIndex);
            this.EventName = str;
            return str;
        }

        internal string Encode()
        {
            StringBuilder builder = new StringBuilder();
            if ((this.TransportEvent == TransportEventTypes.Unknown) && (this.AttachmentCount > 0))
            {
                this.TransportEvent = TransportEventTypes.Message;
            }
            if (this.TransportEvent != TransportEventTypes.Unknown)
            {
                builder.Append(this.TransportEvent.ToString());
            }
            if ((this.SocketIOEvent == SocketIOEventTypes.Unknown) && (this.AttachmentCount > 0))
            {
                this.SocketIOEvent = SocketIOEventTypes.BinaryEvent;
            }
            if (this.SocketIOEvent != SocketIOEventTypes.Unknown)
            {
                builder.Append(this.SocketIOEvent.ToString());
            }
            if ((this.SocketIOEvent == SocketIOEventTypes.BinaryEvent) || (this.SocketIOEvent == SocketIOEventTypes.BinaryAck))
            {
                builder.Append(this.AttachmentCount.ToString());
                builder.Append("-");
            }
            bool flag = false;
            if (this.Namespace != "/")
            {
                builder.Append(this.Namespace);
                flag = true;
            }
            if (this.Id != 0)
            {
                if (flag)
                {
                    builder.Append(",");
                    flag = false;
                }
                builder.Append(this.Id.ToString());
            }
            if (!string.IsNullOrEmpty(this.Payload))
            {
                if (flag)
                {
                    builder.Append(",");
                    flag = false;
                }
                builder.Append(this.Payload);
            }
            return builder.ToString();
        }

        internal byte[] EncodeBinary()
        {
            if ((this.AttachmentCount != 0) || ((this.Attachments != null) && (this.Attachments.Count != 0)))
            {
                if (this.Attachments == null)
                {
                    throw new ArgumentException("packet.Attachments are null!");
                }
                if (this.AttachmentCount != this.Attachments.Count)
                {
                    throw new ArgumentException("packet.AttachmentCount != packet.Attachments.Count. Use the packet.AddAttachment function to add data to a packet!");
                }
            }
            string s = this.Encode();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            byte[] array = this.EncodeData(bytes, PayloadTypes.Textual, null);
            if (this.AttachmentCount != 0)
            {
                int length = array.Length;
                List<byte[]> list = new List<byte[]>(this.AttachmentCount);
                int num2 = 0;
                for (int i = 0; i < this.AttachmentCount; i++)
                {
                    byte[] afterHeaderData = new byte[] { 4 };
                    byte[] item = this.EncodeData(this.Attachments[i], PayloadTypes.Binary, afterHeaderData);
                    list.Add(item);
                    num2 += item.Length;
                }
                Array.Resize<byte>(ref array, array.Length + num2);
                for (int j = 0; j < this.AttachmentCount; j++)
                {
                    byte[] sourceArray = list[j];
                    Array.Copy(sourceArray, 0, array, length, sourceArray.Length);
                    length += sourceArray.Length;
                }
            }
            return array;
        }

        private byte[] EncodeData(byte[] data, PayloadTypes type, byte[] afterHeaderData)
        {
            int num = (afterHeaderData == null) ? 0 : afterHeaderData.Length;
            string str = (data.Length + num).ToString();
            byte[] buffer = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                buffer[i] = (byte) char.GetNumericValue(str[i]);
            }
            byte[] destinationArray = new byte[((data.Length + buffer.Length) + 2) + num];
            destinationArray[0] = (byte) type;
            for (int j = 0; j < buffer.Length; j++)
            {
                destinationArray[1 + j] = buffer[j];
            }
            int destinationIndex = 1 + buffer.Length;
            destinationArray[destinationIndex++] = 0xff;
            if ((afterHeaderData != null) && (afterHeaderData.Length > 0))
            {
                Array.Copy(afterHeaderData, 0, destinationArray, destinationIndex, afterHeaderData.Length);
                destinationIndex += afterHeaderData.Length;
            }
            Array.Copy(data, 0, destinationArray, destinationIndex, data.Length);
            return destinationArray;
        }

        internal void Parse(string from)
        {
            int index = 0;
            this.TransportEvent = (TransportEventTypes) ((int) char.GetNumericValue(from, index++));
            if ((from.Length > index) && (char.GetNumericValue(from, index) >= 0.0))
            {
                this.SocketIOEvent = (SocketIOEventTypes) ((int) char.GetNumericValue(from, index++));
            }
            else
            {
                this.SocketIOEvent = SocketIOEventTypes.Unknown;
            }
            if ((this.SocketIOEvent == SocketIOEventTypes.BinaryEvent) || (this.SocketIOEvent == SocketIOEventTypes.BinaryAck))
            {
                int length = from.IndexOf('-', index);
                if (length == -1)
                {
                    length = from.Length;
                }
                int result = 0;
                int.TryParse(from.Substring(index, length - index), out result);
                this.AttachmentCount = result;
                index = length + 1;
            }
            if ((from.Length > index) && (from[index] == '/'))
            {
                int length = from.IndexOf(',', index);
                if (length == -1)
                {
                    length = from.Length;
                }
                this.Namespace = from.Substring(index, length - index);
                index = length + 1;
            }
            else
            {
                this.Namespace = "/";
            }
            if ((from.Length > index) && (char.GetNumericValue(from[index]) >= 0.0))
            {
                int startIndex = index++;
                while ((from.Length > index) && (char.GetNumericValue(from[index]) >= 0.0))
                {
                    index++;
                }
                int result = 0;
                int.TryParse(from.Substring(startIndex, index - startIndex), out result);
                this.Id = result;
            }
            if (from.Length > index)
            {
                this.Payload = from.Substring(index);
            }
            else
            {
                this.Payload = string.Empty;
            }
        }

        private bool PlaceholderReplacer(Action<string, Dictionary<string, object>> onFound)
        {
            if (string.IsNullOrEmpty(this.Payload))
            {
                return false;
            }
            for (int i = this.Payload.IndexOf("_placeholder"); i >= 0; i = this.Payload.IndexOf("_placeholder"))
            {
                int startIndex = i;
                while (this.Payload[startIndex] != '{')
                {
                    startIndex--;
                }
                int num3 = i;
                while ((this.Payload.Length > num3) && (this.Payload[num3] != '}'))
                {
                    num3++;
                }
                if (this.Payload.Length <= num3)
                {
                    return false;
                }
                string json = this.Payload.Substring(startIndex, (num3 - startIndex) + 1);
                bool success = false;
                Dictionary<string, object> dictionary = Json.Decode(json, ref success) as Dictionary<string, object>;
                if (!success)
                {
                    return false;
                }
                if (!dictionary.TryGetValue("_placeholder", out object obj2) || !((bool) obj2))
                {
                    return false;
                }
                if (!dictionary.TryGetValue("num", out obj2))
                {
                    return false;
                }
                onFound(json, dictionary);
            }
            return true;
        }

        public bool ReconstructAttachmentAsBase64()
        {
            if (!this.HasAllAttachment)
            {
                return false;
            }
            return this.PlaceholderReplacer(delegate (string json, Dictionary<string, object> obj) {
                int num = Convert.ToInt32(obj["num"]);
                this.Payload = this.Payload.Replace(json, $""{Convert.ToBase64String(this.Attachments[num])}"");
                this.IsDecoded = false;
            });
        }

        public bool ReconstructAttachmentAsIndex() => 
            this.PlaceholderReplacer(delegate (string json, Dictionary<string, object> obj) {
                int num = Convert.ToInt32(obj["num"]);
                this.Payload = this.Payload.Replace(json, num.ToString());
                this.IsDecoded = false;
            });

        public string RemoveEventName(bool removeArrayMarks)
        {
            if (string.IsNullOrEmpty(this.Payload))
            {
                return string.Empty;
            }
            if (this.Payload[0] != '[')
            {
                return string.Empty;
            }
            int num = 1;
            while (((this.Payload.Length > num) && (this.Payload[num] != '"')) && (this.Payload[num] != '\''))
            {
                num++;
            }
            if (this.Payload.Length <= num)
            {
                return string.Empty;
            }
            int startIndex = num;
            while (((this.Payload.Length > num) && (this.Payload[num] != ',')) && (this.Payload[num] != ']'))
            {
                num++;
            }
            if (this.Payload.Length <= ++num)
            {
                return string.Empty;
            }
            string str = this.Payload.Remove(startIndex, num - startIndex);
            if (removeArrayMarks)
            {
                str = str.Substring(1, str.Length - 2);
            }
            return str;
        }

        public override string ToString() => 
            this.Payload;

        public TransportEventTypes TransportEvent { get; private set; }

        public SocketIOEventTypes SocketIOEvent { get; private set; }

        public int AttachmentCount { get; private set; }

        public int Id { get; private set; }

        public string Namespace { get; private set; }

        public string Payload { get; private set; }

        public string EventName { get; private set; }

        public List<byte[]> Attachments
        {
            get => 
                this.attachments;
            set
            {
                this.attachments = value;
                this.AttachmentCount = (this.attachments == null) ? 0 : this.attachments.Count;
            }
        }

        public bool HasAllAttachment =>
            ((this.Attachments != null) && (this.Attachments.Count == this.AttachmentCount));

        public bool IsDecoded { get; private set; }

        public object[] DecodedArgs { get; private set; }

        private enum PayloadTypes : byte
        {
            Textual = 0,
            Binary = 1
        }
    }
}

