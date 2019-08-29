namespace BestHTTP.WebSocket.Frames
{
    using BestHTTP.WebSocket;
    using BestHTTP.WebSocket.Extensions;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    public sealed class WebSocketFrame
    {
        public static readonly byte[] NoData = new byte[0];
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private WebSocketFrameTypes <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsFinal>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte <Header>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] <Data>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <UseExtensions>k__BackingField;

        public WebSocketFrame(BestHTTP.WebSocket.WebSocket webSocket, WebSocketFrameTypes type, byte[] data) : this(webSocket, type, data, true)
        {
        }

        public WebSocketFrame(BestHTTP.WebSocket.WebSocket webSocket, WebSocketFrameTypes type, byte[] data, bool useExtensions) : this(webSocket, type, data, 0L, (data == null) ? ((ulong) 0L) : ((ulong) data.Length), true, useExtensions)
        {
        }

        public WebSocketFrame(BestHTTP.WebSocket.WebSocket webSocket, WebSocketFrameTypes type, byte[] data, bool isFinal, bool useExtensions) : this(webSocket, type, data, 0L, (data == null) ? ((ulong) 0L) : ((ulong) data.Length), isFinal, useExtensions)
        {
        }

        public WebSocketFrame(BestHTTP.WebSocket.WebSocket webSocket, WebSocketFrameTypes type, byte[] data, ulong pos, ulong length, bool isFinal, bool useExtensions)
        {
            this.Type = type;
            this.IsFinal = isFinal;
            this.UseExtensions = useExtensions;
            if (data != null)
            {
                this.Data = new byte[length];
                Array.Copy(data, (int) pos, this.Data, 0, (int) length);
            }
            else
            {
                data = NoData;
            }
            byte num = !this.IsFinal ? ((byte) 0) : ((byte) 0x80);
            this.Header = (byte) (num | this.Type);
            if ((this.UseExtensions && (webSocket != null)) && (webSocket.Extensions != null))
            {
                for (int i = 0; i < webSocket.Extensions.Length; i++)
                {
                    IExtension extension = webSocket.Extensions[i];
                    if (extension != null)
                    {
                        this.Header = (byte) (this.Header | extension.GetFrameHeader(this, this.Header));
                        this.Data = extension.Encode(this);
                    }
                }
            }
        }

        public WebSocketFrame[] Fragment(ushort maxFragmentSize)
        {
            ulong num3;
            if (this.Data == null)
            {
                return null;
            }
            if ((this.Type != WebSocketFrameTypes.Binary) && (this.Type != WebSocketFrameTypes.Text))
            {
                return null;
            }
            if (this.Data.Length <= maxFragmentSize)
            {
                return null;
            }
            this.IsFinal = false;
            this.Header = (byte) (this.Header & 0x7f);
            int num = (this.Data.Length / maxFragmentSize) + (((this.Data.Length % maxFragmentSize) != 0) ? 0 : -1);
            WebSocketFrame[] frameArray = new WebSocketFrame[num];
            for (ulong i = maxFragmentSize; i < this.Data.Length; i += num3)
            {
                num3 = Math.Min((ulong) maxFragmentSize, ((ulong) this.Data.Length) - i);
                frameArray[frameArray.Length - num--] = new WebSocketFrame(null, WebSocketFrameTypes.Continuation, this.Data, i, num3, (i + num3) >= this.Data.Length, false);
            }
            byte[] destinationArray = new byte[maxFragmentSize];
            Array.Copy(this.Data, 0, destinationArray, 0, maxFragmentSize);
            this.Data = destinationArray;
            return frameArray;
        }

        public byte[] Get()
        {
            if (this.Data == null)
            {
                this.Data = NoData;
            }
            using (MemoryStream stream = new MemoryStream(this.Data.Length + 9))
            {
                stream.WriteByte(this.Header);
                if (this.Data.Length < 0x7e)
                {
                    stream.WriteByte((byte) (0x80 | ((byte) this.Data.Length)));
                }
                else if (this.Data.Length < 0xffff)
                {
                    stream.WriteByte(0xfe);
                    byte[] array = BitConverter.GetBytes((ushort) this.Data.Length);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(array, 0, array.Length);
                    }
                    stream.Write(array, 0, array.Length);
                }
                else
                {
                    stream.WriteByte(0xff);
                    byte[] array = BitConverter.GetBytes((ulong) this.Data.Length);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(array, 0, array.Length);
                    }
                    stream.Write(array, 0, array.Length);
                }
                byte[] bytes = BitConverter.GetBytes(this.GetHashCode());
                stream.Write(bytes, 0, bytes.Length);
                for (int i = 0; i < this.Data.Length; i++)
                {
                    stream.WriteByte((byte) (this.Data[i] ^ bytes[i % 4]));
                }
                return stream.ToArray();
            }
        }

        public WebSocketFrameTypes Type { get; private set; }

        public bool IsFinal { get; private set; }

        public byte Header { get; private set; }

        public byte[] Data { get; private set; }

        public bool UseExtensions { get; private set; }
    }
}

