namespace BestHTTP.WebSocket.Frames
{
    using BestHTTP.Extensions;
    using BestHTTP.WebSocket;
    using BestHTTP.WebSocket.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed class WebSocketFrameReader
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte <Header>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsFinal>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private WebSocketFrameTypes <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HasMask>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ulong <Length>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] <Mask>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] <Data>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <DataAsText>k__BackingField;

        public void Assemble(List<WebSocketFrameReader> fragments)
        {
            fragments.Add(this);
            ulong num = 0L;
            for (int i = 0; i < fragments.Count; i++)
            {
                num += fragments[i].Length;
            }
            byte[] destinationArray = new byte[num];
            ulong num3 = 0L;
            for (int j = 0; j < fragments.Count; j++)
            {
                Array.Copy(fragments[j].Data, 0, destinationArray, (int) num3, (int) fragments[j].Length);
                num3 += fragments[j].Length;
            }
            this.Type = fragments[0].Type;
            this.Header = fragments[0].Header;
            this.Length = num;
            this.Data = destinationArray;
        }

        public void DecodeWithExtensions(BestHTTP.WebSocket.WebSocket webSocket)
        {
            if (webSocket.Extensions != null)
            {
                for (int i = 0; i < webSocket.Extensions.Length; i++)
                {
                    IExtension extension = webSocket.Extensions[i];
                    if (extension != null)
                    {
                        this.Data = extension.Decode(this.Header, this.Data);
                    }
                }
            }
            if ((this.Type == WebSocketFrameTypes.Text) && (this.Data != null))
            {
                this.DataAsText = Encoding.UTF8.GetString(this.Data, 0, this.Data.Length);
            }
        }

        internal void Read(Stream stream)
        {
            this.Header = this.ReadByte(stream);
            this.IsFinal = (this.Header & 0x80) != 0;
            this.Type = (WebSocketFrameTypes) ((byte) (this.Header & 15));
            byte num = this.ReadByte(stream);
            this.HasMask = (num & 0x80) != 0;
            this.Length = (ulong) (num & 0x7f);
            if (this.Length == 0x7eL)
            {
                byte[] buffer = new byte[2];
                stream.ReadBuffer(buffer);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buffer, 0, buffer.Length);
                }
                this.Length = BitConverter.ToUInt16(buffer, 0);
            }
            else if (this.Length == 0x7fL)
            {
                byte[] buffer = new byte[8];
                stream.ReadBuffer(buffer);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buffer, 0, buffer.Length);
                }
                this.Length = BitConverter.ToUInt64(buffer, 0);
            }
            if (this.HasMask)
            {
                this.Mask = new byte[4];
                if (stream.Read(this.Mask, 0, 4) < this.Mask.Length)
                {
                    throw ExceptionHelper.ServerClosedTCPStream();
                }
            }
            this.Data = new byte[this.Length];
            if (this.Length != 0L)
            {
                int offset = 0;
                do
                {
                    int num3 = stream.Read(this.Data, offset, this.Data.Length - offset);
                    if (num3 <= 0)
                    {
                        throw ExceptionHelper.ServerClosedTCPStream();
                    }
                    offset += num3;
                }
                while (offset < this.Data.Length);
                if (this.HasMask)
                {
                    for (int i = 0; i < this.Data.Length; i++)
                    {
                        this.Data[i] = (byte) (this.Data[i] ^ this.Mask[i % 4]);
                    }
                }
            }
        }

        private byte ReadByte(Stream stream)
        {
            int num = stream.ReadByte();
            if (num < 0)
            {
                throw ExceptionHelper.ServerClosedTCPStream();
            }
            return (byte) num;
        }

        public byte Header { get; private set; }

        public bool IsFinal { get; private set; }

        public WebSocketFrameTypes Type { get; private set; }

        public bool HasMask { get; private set; }

        public ulong Length { get; private set; }

        public byte[] Mask { get; private set; }

        public byte[] Data { get; private set; }

        public string DataAsText { get; private set; }
    }
}

