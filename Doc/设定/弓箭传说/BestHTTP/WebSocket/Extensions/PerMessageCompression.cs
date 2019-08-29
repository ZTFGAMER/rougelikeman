namespace BestHTTP.WebSocket.Extensions
{
    using BestHTTP;
    using BestHTTP.Decompression.Zlib;
    using BestHTTP.Extensions;
    using BestHTTP.WebSocket;
    using BestHTTP.WebSocket.Frames;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    public sealed class PerMessageCompression : IExtension
    {
        private static readonly byte[] Trailer;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <ClientNoContextTakeover>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <ServerNoContextTakeover>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ClientMaxWindowBits>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ServerMaxWindowBits>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CompressionLevel <Level>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <MinimumDataLegthToCompress>k__BackingField;
        private MemoryStream compressorOutputStream;
        private DeflateStream compressorDeflateStream;
        private MemoryStream decompressorInputStream;
        private MemoryStream decompressorOutputStream;
        private DeflateStream decompressorDeflateStream;
        private byte[] copyBuffer;

        static PerMessageCompression()
        {
            byte[] buffer1 = new byte[4];
            buffer1[2] = 0xff;
            buffer1[3] = 0xff;
            Trailer = buffer1;
        }

        public PerMessageCompression() : this(CompressionLevel.Default, false, false, 15, 15, 10)
        {
        }

        public PerMessageCompression(CompressionLevel level, bool clientNoContextTakeover, bool serverNoContextTakeover, int desiredClientMaxWindowBits, int desiredServerMaxWindowBits, int minDatalengthToCompress)
        {
            this.copyBuffer = new byte[0x400];
            this.Level = level;
            this.ClientNoContextTakeover = clientNoContextTakeover;
            this.ServerNoContextTakeover = this.ServerNoContextTakeover;
            this.ClientMaxWindowBits = desiredClientMaxWindowBits;
            this.ServerMaxWindowBits = desiredServerMaxWindowBits;
            this.MinimumDataLegthToCompress = minDatalengthToCompress;
        }

        public void AddNegotiation(HTTPRequest request)
        {
            string str = "permessage-deflate";
            if (this.ServerNoContextTakeover)
            {
                str = str + "; server_no_context_takeover";
            }
            if (this.ClientNoContextTakeover)
            {
                str = str + "; client_no_context_takeover";
            }
            if (this.ServerMaxWindowBits != 15)
            {
                str = str + "; server_max_window_bits=" + this.ServerMaxWindowBits.ToString();
            }
            else
            {
                this.ServerMaxWindowBits = 15;
            }
            if (this.ClientMaxWindowBits != 15)
            {
                str = str + "; client_max_window_bits=" + this.ClientMaxWindowBits.ToString();
            }
            else
            {
                str = str + "; client_max_window_bits";
                this.ClientMaxWindowBits = 15;
            }
            request.AddHeader("Sec-WebSocket-Extensions", str);
        }

        private byte[] Compress(byte[] data)
        {
            if (this.compressorOutputStream == null)
            {
                this.compressorOutputStream = new MemoryStream();
            }
            this.compressorOutputStream.SetLength(0L);
            if (this.compressorDeflateStream == null)
            {
                this.compressorDeflateStream = new DeflateStream(this.compressorOutputStream, CompressionMode.Compress, this.Level, true, this.ClientMaxWindowBits);
                this.compressorDeflateStream.FlushMode = FlushType.Sync;
            }
            byte[] buffer = null;
            try
            {
                this.compressorDeflateStream.Write(data, 0, data.Length);
                this.compressorDeflateStream.Flush();
                this.compressorOutputStream.Position = 0L;
                this.compressorOutputStream.SetLength(this.compressorOutputStream.Length - 4L);
                buffer = this.compressorOutputStream.ToArray();
            }
            finally
            {
                if (this.ClientNoContextTakeover)
                {
                    this.compressorDeflateStream.Dispose();
                    this.compressorDeflateStream = null;
                }
            }
            return buffer;
        }

        public byte[] Decode(byte header, byte[] data)
        {
            if ((header & 0x40) != 0)
            {
                return this.Decompress(data);
            }
            return data;
        }

        private byte[] Decompress(byte[] data)
        {
            int num;
            if (this.decompressorInputStream == null)
            {
                this.decompressorInputStream = new MemoryStream(data.Length + 4);
            }
            this.decompressorInputStream.Write(data, 0, data.Length);
            this.decompressorInputStream.Write(Trailer, 0, Trailer.Length);
            this.decompressorInputStream.Position = 0L;
            if (this.decompressorDeflateStream == null)
            {
                this.decompressorDeflateStream = new DeflateStream(this.decompressorInputStream, CompressionMode.Decompress, CompressionLevel.Default, true, this.ServerMaxWindowBits);
                this.decompressorDeflateStream.FlushMode = FlushType.Sync;
            }
            if (this.decompressorOutputStream == null)
            {
                this.decompressorOutputStream = new MemoryStream();
            }
            this.decompressorOutputStream.SetLength(0L);
            while ((num = this.decompressorDeflateStream.Read(this.copyBuffer, 0, this.copyBuffer.Length)) != 0)
            {
                this.decompressorOutputStream.Write(this.copyBuffer, 0, num);
            }
            this.decompressorDeflateStream.SetLength(0L);
            byte[] buffer = this.decompressorOutputStream.ToArray();
            if (this.ServerNoContextTakeover)
            {
                this.decompressorDeflateStream.Dispose();
                this.decompressorDeflateStream = null;
            }
            return buffer;
        }

        public byte[] Encode(WebSocketFrame writer)
        {
            if (writer.Data == null)
            {
                return WebSocketFrame.NoData;
            }
            if ((writer.Header & 0x40) != 0)
            {
                return this.Compress(writer.Data);
            }
            return writer.Data;
        }

        public byte GetFrameHeader(WebSocketFrame writer, byte inFlag)
        {
            if (((writer.Type == WebSocketFrameTypes.Binary) || (writer.Type == WebSocketFrameTypes.Text)) && ((writer.Data != null) && (writer.Data.Length >= this.MinimumDataLegthToCompress)))
            {
                return (byte) (inFlag | 0x40);
            }
            return inFlag;
        }

        public bool ParseNegotiation(WebSocketResponse resp)
        {
            List<string> headerValues = resp.GetHeaderValues("Sec-WebSocket-Extensions");
            if (headerValues != null)
            {
                for (int i = 0; i < headerValues.Count; i++)
                {
                    HeaderParser parser = new HeaderParser(headerValues[i]);
                    for (int j = 0; j < parser.Values.Count; j++)
                    {
                        HeaderValue value2 = parser.Values[i];
                        if (!string.IsNullOrEmpty(value2.Key) && value2.Key.StartsWith("permessage-deflate", StringComparison.OrdinalIgnoreCase))
                        {
                            HTTPManager.Logger.Information("PerMessageCompression", "Enabled with header: " + headerValues[i]);
                            if (value2.TryGetOption("client_no_context_takeover", out HeaderValue value3))
                            {
                                this.ClientNoContextTakeover = true;
                            }
                            if (value2.TryGetOption("server_no_context_takeover", out value3))
                            {
                                this.ServerNoContextTakeover = true;
                            }
                            if ((value2.TryGetOption("client_max_window_bits", out value3) && value3.HasValue) && int.TryParse(value3.Value, out int num3))
                            {
                                this.ClientMaxWindowBits = num3;
                            }
                            if ((value2.TryGetOption("server_max_window_bits", out value3) && value3.HasValue) && int.TryParse(value3.Value, out int num4))
                            {
                                this.ServerMaxWindowBits = num4;
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool ClientNoContextTakeover { get; private set; }

        public bool ServerNoContextTakeover { get; private set; }

        public int ClientMaxWindowBits { get; private set; }

        public int ServerMaxWindowBits { get; private set; }

        public CompressionLevel Level { get; private set; }

        public int MinimumDataLegthToCompress { get; set; }
    }
}

