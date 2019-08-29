namespace BestHTTP
{
    using BestHTTP.Caching;
    using BestHTTP.Decompression.Zlib;
    using BestHTTP.Extensions;
    using BestHTTP.Logger;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using UnityEngine;

    public class HTTPResponse : IDisposable
    {
        internal const byte CR = 13;
        internal const byte LF = 10;
        public const int MinBufferSize = 0x1000;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <VersionMajor>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <VersionMinor>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <StatusCode>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Message>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsStreamed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsStreamingFinished>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsFromCache>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HTTPCacheFileInfo <CacheFileInfo>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsCacheOnly>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, List<string>> <Headers>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] <Data>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsUpgraded>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Cookie> <Cookies>k__BackingField;
        protected string dataAsText;
        protected Texture2D texture;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsClosedManually>k__BackingField;
        internal HTTPRequest baseRequest;
        protected System.IO.Stream Stream;
        protected List<byte[]> streamedFragments;
        protected object SyncRoot = new object();
        protected byte[] fragmentBuffer;
        protected int fragmentBufferDataLength;
        protected System.IO.Stream cacheStream;
        protected int allFragmentSize;
        private MemoryStream decompressorInputStream;
        private MemoryStream decompressorOutputStream;
        private GZipStream decompressorGZipStream;
        private byte[] copyBuffer;

        internal HTTPResponse(HTTPRequest request, System.IO.Stream stream, bool isStreamed, bool isFromCache)
        {
            this.baseRequest = request;
            this.Stream = stream;
            this.IsStreamed = isStreamed;
            this.IsFromCache = isFromCache;
            this.IsCacheOnly = request.CacheOnly;
            this.IsClosedManually = false;
        }

        protected void AddHeader(string name, string value)
        {
            name = name.ToLower();
            if (this.Headers == null)
            {
                this.Headers = new Dictionary<string, List<string>>();
            }
            if (!this.Headers.TryGetValue(name, out List<string> list))
            {
                this.Headers.Add(name, list = new List<string>(1));
            }
            list.Add(value);
        }

        protected void AddStreamedFragment(byte[] buffer)
        {
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                if (!this.IsCacheOnly)
                {
                    if (this.streamedFragments == null)
                    {
                        this.streamedFragments = new List<byte[]>();
                    }
                    this.streamedFragments.Add(buffer);
                }
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    this.VerboseLogging($"AddStreamedFragment buffer length: {buffer.Length:N0} streamedFragments: {this.streamedFragments.Count:N0}");
                }
                if (this.cacheStream != null)
                {
                    this.cacheStream.Write(buffer, 0, buffer.Length);
                    this.allFragmentSize += buffer.Length;
                }
            }
        }

        protected void BeginReceiveStreamFragments()
        {
            if ((!this.baseRequest.DisableCache && this.baseRequest.UseStreaming) && (!this.IsFromCache && HTTPCacheService.IsCacheble(this.baseRequest.CurrentUri, this.baseRequest.MethodType, this)))
            {
                this.cacheStream = HTTPCacheService.PrepareStreamed(this.baseRequest.CurrentUri, this);
            }
            this.allFragmentSize = 0;
        }

        protected byte[] DecodeStream(MemoryStream streamToDecode)
        {
            streamToDecode.Seek(0L, SeekOrigin.Begin);
            List<string> list = !this.IsFromCache ? this.GetHeaderValues("content-encoding") : null;
            System.IO.Stream stream = null;
            if (list != null)
            {
                switch (list[0])
                {
                    case "gzip":
                        stream = new GZipStream(streamToDecode, CompressionMode.Decompress);
                        goto Label_008A;

                    case "deflate":
                        stream = new DeflateStream(streamToDecode, CompressionMode.Decompress);
                        goto Label_008A;
                }
            }
            return streamToDecode.ToArray();
        Label_008A:
            using (MemoryStream stream2 = new MemoryStream((int) streamToDecode.Length))
            {
                byte[] buffer = new byte[0x400];
                int count = 0;
                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream2.Write(buffer, 0, count);
                }
                return stream2.ToArray();
            }
        }

        private byte[] Decompress(byte[] data, int offset, int count)
        {
            int num;
            if (this.decompressorInputStream == null)
            {
                this.decompressorInputStream = new MemoryStream(count);
            }
            this.decompressorInputStream.Write(data, offset, count);
            this.decompressorInputStream.Position = 0L;
            if (this.decompressorGZipStream == null)
            {
                this.decompressorGZipStream = new GZipStream(this.decompressorInputStream, CompressionMode.Decompress, CompressionLevel.Default, true);
                this.decompressorGZipStream.FlushMode = FlushType.Sync;
            }
            if (this.decompressorOutputStream == null)
            {
                this.decompressorOutputStream = new MemoryStream();
            }
            this.decompressorOutputStream.SetLength(0L);
            if (this.copyBuffer == null)
            {
                this.copyBuffer = new byte[0x400];
            }
            while ((num = this.decompressorGZipStream.Read(this.copyBuffer, 0, this.copyBuffer.Length)) != 0)
            {
                this.decompressorOutputStream.Write(this.copyBuffer, 0, num);
            }
            this.decompressorGZipStream.SetLength(0L);
            return this.decompressorOutputStream.ToArray();
        }

        public void Dispose()
        {
            if (this.cacheStream != null)
            {
                this.cacheStream.Dispose();
                this.cacheStream = null;
            }
        }

        protected void FeedStreamFragment(byte[] buffer, int pos, int length)
        {
            if (this.fragmentBuffer == null)
            {
                this.fragmentBuffer = new byte[this.baseRequest.StreamFragmentSize];
                this.fragmentBufferDataLength = 0;
            }
            if ((this.fragmentBufferDataLength + length) <= this.baseRequest.StreamFragmentSize)
            {
                Array.Copy(buffer, pos, this.fragmentBuffer, this.fragmentBufferDataLength, length);
                this.fragmentBufferDataLength += length;
                if (this.fragmentBufferDataLength == this.baseRequest.StreamFragmentSize)
                {
                    this.AddStreamedFragment(this.fragmentBuffer);
                    this.fragmentBuffer = null;
                    this.fragmentBufferDataLength = 0;
                }
            }
            else
            {
                int num = this.baseRequest.StreamFragmentSize - this.fragmentBufferDataLength;
                this.FeedStreamFragment(buffer, pos, num);
                this.FeedStreamFragment(buffer, pos + num, length - num);
            }
        }

        internal void FinishStreaming()
        {
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                this.VerboseLogging("FinishStreaming");
            }
            this.IsStreamingFinished = true;
            this.Dispose();
        }

        protected void FlushRemainingFragmentBuffer()
        {
            if (this.fragmentBuffer != null)
            {
                Array.Resize<byte>(ref this.fragmentBuffer, this.fragmentBufferDataLength);
                this.AddStreamedFragment(this.fragmentBuffer);
                this.fragmentBuffer = null;
                this.fragmentBufferDataLength = 0;
            }
            if (this.cacheStream != null)
            {
                this.cacheStream.Dispose();
                this.cacheStream = null;
                HTTPCacheService.SetBodyLength(this.baseRequest.CurrentUri, this.allFragmentSize);
            }
        }

        public string GetFirstHeaderValue(string name)
        {
            if (this.Headers != null)
            {
                name = name.ToLower();
                if (this.Headers.TryGetValue(name, out List<string> list) && (list.Count != 0))
                {
                    return list[0];
                }
            }
            return null;
        }

        public List<string> GetHeaderValues(string name)
        {
            if (this.Headers != null)
            {
                name = name.ToLower();
                if (this.Headers.TryGetValue(name, out List<string> list) && (list.Count != 0))
                {
                    return list;
                }
            }
            return null;
        }

        public HTTPRange GetRange()
        {
            List<string> headerValues = this.GetHeaderValues("content-range");
            if (headerValues == null)
            {
                return null;
            }
            string[] strArray = headerValues[0].Split(new char[] { ' ', '-', '/', '\0' }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray[1] == "*")
            {
                return new HTTPRange(int.Parse(strArray[2]));
            }
            return new HTTPRange(int.Parse(strArray[1]), int.Parse(strArray[2]), (strArray[3] == "*") ? -1 : int.Parse(strArray[3]));
        }

        public List<byte[]> GetStreamedFragments()
        {
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                if ((this.streamedFragments == null) || (this.streamedFragments.Count == 0))
                {
                    if (HTTPManager.Logger.Level == Loglevels.All)
                    {
                        this.VerboseLogging("GetStreamedFragments - no fragments, returning with null");
                    }
                    return null;
                }
                List<byte[]> list2 = new List<byte[]>(this.streamedFragments);
                this.streamedFragments.Clear();
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    this.VerboseLogging($"GetStreamedFragments - returning with {list2.Count.ToString():N0} fragments");
                }
                return list2;
            }
        }

        public bool HasHeader(string headerName)
        {
            if (this.GetHeaderValues(headerName) == null)
            {
                return false;
            }
            return true;
        }

        public bool HasHeaderWithValue(string headerName, string value)
        {
            List<string> headerValues = this.GetHeaderValues(headerName);
            if (headerValues != null)
            {
                for (int i = 0; i < headerValues.Count; i++)
                {
                    if (string.Compare(headerValues[i], value, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool HasStreamedFragments()
        {
            object syncRoot = this.SyncRoot;
            lock (syncRoot)
            {
                return ((this.streamedFragments != null) && (this.streamedFragments.Count > 0));
            }
        }

        public static string NoTrimReadTo(System.IO.Stream stream, byte blocker1, byte blocker2)
        {
            using (MemoryStream stream2 = new MemoryStream())
            {
                for (int i = stream.ReadByte(); ((i != blocker1) && (i != blocker2)) && (i != -1); i = stream.ReadByte())
                {
                    stream2.WriteByte((byte) i);
                }
                return stream2.ToArray().AsciiToString();
            }
        }

        protected void ReadChunked(System.IO.Stream stream)
        {
            this.BeginReceiveStreamFragments();
            string firstHeaderValue = this.GetFirstHeaderValue("Content-Length");
            bool flag = !string.IsNullOrEmpty(firstHeaderValue);
            int result = 0;
            if (flag)
            {
                flag = int.TryParse(firstHeaderValue, out result);
            }
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                this.VerboseLogging($"ReadChunked - hasContentLengthHeader: {flag.ToString()}, contentLengthHeader: {firstHeaderValue} realLength: {result:N0}");
            }
            using (MemoryStream stream2 = new MemoryStream())
            {
                int num2 = this.ReadChunkLength(stream);
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    this.VerboseLogging($"chunkLength: {num2:N0}");
                }
                byte[] array = new byte[num2];
                int num3 = 0;
                this.baseRequest.DownloadLength = !flag ? ((long) num2) : ((long) result);
                this.baseRequest.DownloadProgressChanged = this.IsSuccess || this.IsFromCache;
                string str2 = !this.IsFromCache ? this.GetFirstHeaderValue("content-encoding") : null;
                bool flag2 = !string.IsNullOrEmpty(str2) && (str2 == "gzip");
                while (num2 != 0)
                {
                    if (array.Length < num2)
                    {
                        Array.Resize<byte>(ref array, num2);
                    }
                    int offset = 0;
                    do
                    {
                        int num5 = stream.Read(array, offset, num2 - offset);
                        if (num5 <= 0)
                        {
                            throw ExceptionHelper.ServerClosedTCPStream();
                        }
                        offset += num5;
                        this.baseRequest.Downloaded += num5;
                        this.baseRequest.DownloadProgressChanged = this.IsSuccess || this.IsFromCache;
                    }
                    while (offset < num2);
                    if (this.baseRequest.UseStreaming)
                    {
                        this.WaitWhileHasFragments();
                        if (flag2)
                        {
                            byte[] buffer = this.Decompress(array, 0, offset);
                            this.FeedStreamFragment(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            this.FeedStreamFragment(array, 0, offset);
                        }
                    }
                    else
                    {
                        stream2.Write(array, 0, offset);
                    }
                    ReadTo(stream, 10);
                    num3 += offset;
                    num2 = this.ReadChunkLength(stream);
                    if (HTTPManager.Logger.Level == Loglevels.All)
                    {
                        this.VerboseLogging($"chunkLength: {num2:N0}");
                    }
                    if (!flag)
                    {
                        this.baseRequest.DownloadLength += num2;
                    }
                    this.baseRequest.DownloadProgressChanged = this.IsSuccess || this.IsFromCache;
                }
                if (this.baseRequest.UseStreaming)
                {
                    this.FlushRemainingFragmentBuffer();
                }
                this.ReadHeaders(stream);
                if (!this.baseRequest.UseStreaming)
                {
                    this.Data = this.DecodeStream(stream2);
                }
            }
        }

        protected int ReadChunkLength(System.IO.Stream stream)
        {
            char[] separator = new char[] { ';' };
            string s = ReadTo(stream, 10).Split(separator)[0];
            if (!int.TryParse(s, NumberStyles.AllowHexSpecifier, null, out int num))
            {
                throw new Exception($"Can't parse '{s}' as a hex number!");
            }
            return num;
        }

        protected void ReadHeaders(System.IO.Stream stream)
        {
            for (string str = ReadTo(stream, 0x3a, 10).Trim(); str != string.Empty; str = ReadTo(stream, 0x3a, 10))
            {
                string str2 = ReadTo(stream, 10);
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    this.VerboseLogging($"Header - '{str}': '{str2}'");
                }
                this.AddHeader(str, str2);
            }
        }

        protected bool ReadPayload(int forceReadRawContentLength)
        {
            if (forceReadRawContentLength != -1)
            {
                this.IsFromCache = true;
                this.ReadRaw(this.Stream, (long) forceReadRawContentLength);
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    this.VerboseLogging("ReadPayload Finished!");
                }
                return true;
            }
            if (((this.StatusCode < 100) || (this.StatusCode >= 200)) && (((this.StatusCode != 0xcc) && (this.StatusCode != 0x130)) && (this.baseRequest.MethodType != HTTPMethods.Head)))
            {
                if (this.HasHeaderWithValue("transfer-encoding", "chunked"))
                {
                    this.ReadChunked(this.Stream);
                }
                else
                {
                    List<string> headerValues = this.GetHeaderValues("content-length");
                    List<string> list2 = this.GetHeaderValues("content-range");
                    if ((headerValues != null) && (list2 == null))
                    {
                        this.ReadRaw(this.Stream, long.Parse(headerValues[0]));
                    }
                    else if (list2 != null)
                    {
                        if (headerValues != null)
                        {
                            this.ReadRaw(this.Stream, long.Parse(headerValues[0]));
                        }
                        else
                        {
                            HTTPRange range = this.GetRange();
                            this.ReadRaw(this.Stream, (long) ((range.LastBytePos - range.FirstBytePos) + 1));
                        }
                    }
                    else
                    {
                        this.ReadUnknownSize(this.Stream);
                    }
                }
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    this.VerboseLogging("ReadPayload Finished!");
                }
            }
            return true;
        }

        internal void ReadRaw(System.IO.Stream stream, long contentLength)
        {
            this.BeginReceiveStreamFragments();
            this.baseRequest.DownloadLength = contentLength;
            this.baseRequest.DownloadProgressChanged = this.IsSuccess || this.IsFromCache;
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                this.VerboseLogging($"ReadRaw - contentLength: {contentLength:N0}");
            }
            string str = !this.IsFromCache ? this.GetFirstHeaderValue("content-encoding") : null;
            bool flag = !string.IsNullOrEmpty(str) && (str == "gzip");
            if (!this.baseRequest.UseStreaming && (contentLength > 0x7ffffffeL))
            {
                throw new OverflowException("You have to use STREAMING to download files bigger than 2GB!");
            }
            using (MemoryStream stream2 = new MemoryStream(!this.baseRequest.UseStreaming ? ((int) contentLength) : 0))
            {
                byte[] buffer = new byte[Math.Max(this.baseRequest.StreamFragmentSize, 0x1000)];
                int offset = 0;
                while (contentLength > 0L)
                {
                    offset = 0;
                    do
                    {
                        int num2 = (int) Math.Min(0x7ffffffe, (uint) contentLength);
                        int num3 = stream.Read(buffer, offset, Math.Min(num2, buffer.Length - offset));
                        if (num3 <= 0)
                        {
                            throw ExceptionHelper.ServerClosedTCPStream();
                        }
                        offset += num3;
                        contentLength -= num3;
                        this.baseRequest.Downloaded += num3;
                        this.baseRequest.DownloadProgressChanged = this.IsSuccess || this.IsFromCache;
                    }
                    while ((offset < buffer.Length) && (contentLength > 0L));
                    if (this.baseRequest.UseStreaming)
                    {
                        this.WaitWhileHasFragments();
                        if (flag)
                        {
                            byte[] buffer2 = this.Decompress(buffer, 0, offset);
                            this.FeedStreamFragment(buffer2, 0, buffer2.Length);
                        }
                        else
                        {
                            this.FeedStreamFragment(buffer, 0, offset);
                        }
                    }
                    else
                    {
                        stream2.Write(buffer, 0, offset);
                    }
                }
                if (this.baseRequest.UseStreaming)
                {
                    this.FlushRemainingFragmentBuffer();
                }
                if (!this.baseRequest.UseStreaming)
                {
                    this.Data = this.DecodeStream(stream2);
                }
            }
        }

        public static string ReadTo(System.IO.Stream stream, byte blocker)
        {
            using (MemoryStream stream2 = new MemoryStream())
            {
                for (int i = stream.ReadByte(); (i != blocker) && (i != -1); i = stream.ReadByte())
                {
                    stream2.WriteByte((byte) i);
                }
                return stream2.ToArray().AsciiToString().Trim();
            }
        }

        public static string ReadTo(System.IO.Stream stream, byte blocker1, byte blocker2)
        {
            using (MemoryStream stream2 = new MemoryStream())
            {
                for (int i = stream.ReadByte(); ((i != blocker1) && (i != blocker2)) && (i != -1); i = stream.ReadByte())
                {
                    stream2.WriteByte((byte) i);
                }
                return stream2.ToArray().AsciiToString().Trim();
            }
        }

        protected void ReadUnknownSize(System.IO.Stream stream)
        {
            string str = !this.IsFromCache ? this.GetFirstHeaderValue("content-encoding") : null;
            bool flag = !string.IsNullOrEmpty(str) && (str == "gzip");
            using (MemoryStream stream2 = new MemoryStream())
            {
                byte[] buffer = new byte[Math.Max(this.baseRequest.StreamFragmentSize, 0x1000)];
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    this.VerboseLogging($"ReadUnknownSize - buffer size: {buffer.Length:N0}");
                }
                int offset = 0;
                int num2 = 0;
                do
                {
                    offset = 0;
                    do
                    {
                        num2 = 0;
                        NetworkStream stream3 = stream as NetworkStream;
                        if ((stream3 != null) && this.baseRequest.EnableSafeReadOnUnknownContentLength)
                        {
                            for (int i = offset; (i < buffer.Length) && stream3.DataAvailable; i++)
                            {
                                int num4 = stream.ReadByte();
                                if (num4 >= 0)
                                {
                                    buffer[i] = (byte) num4;
                                    num2++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            num2 = stream.Read(buffer, offset, buffer.Length - offset);
                        }
                        offset += num2;
                        this.baseRequest.Downloaded += num2;
                        this.baseRequest.DownloadLength = this.baseRequest.Downloaded;
                        this.baseRequest.DownloadProgressChanged = this.IsSuccess || this.IsFromCache;
                    }
                    while ((offset < buffer.Length) && (num2 > 0));
                    if (this.baseRequest.UseStreaming)
                    {
                        this.WaitWhileHasFragments();
                        if (flag)
                        {
                            byte[] buffer2 = this.Decompress(buffer, 0, offset);
                            this.FeedStreamFragment(buffer2, 0, buffer2.Length);
                        }
                        else
                        {
                            this.FeedStreamFragment(buffer, 0, offset);
                        }
                    }
                    else
                    {
                        stream2.Write(buffer, 0, offset);
                    }
                }
                while (num2 > 0);
                if (this.baseRequest.UseStreaming)
                {
                    this.FlushRemainingFragmentBuffer();
                }
                if (!this.baseRequest.UseStreaming)
                {
                    this.Data = this.DecodeStream(stream2);
                }
            }
        }

        internal virtual bool Receive(int forceReadRawContentLength = -1, bool readPayloadData = true)
        {
            int num3;
            string str = string.Empty;
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                this.VerboseLogging($"Receive. forceReadRawContentLength: '{forceReadRawContentLength:N0}', readPayloadData: '{readPayloadData:N0}'");
            }
            try
            {
                str = ReadTo(this.Stream, 0x20);
            }
            catch
            {
                if (!this.baseRequest.DisableRetry)
                {
                    HTTPManager.Logger.Warning("HTTPResponse", $"{this.baseRequest.CurrentUri.ToString()} - Failed to read Status Line! Retry is enabled, returning with false.");
                    return false;
                }
                HTTPManager.Logger.Warning("HTTPResponse", $"{this.baseRequest.CurrentUri.ToString()} - Failed to read Status Line! Retry is disabled, re-throwing exception.");
                throw;
            }
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                this.VerboseLogging($"Status Line: '{str}'");
            }
            if (string.IsNullOrEmpty(str))
            {
                if (this.baseRequest.DisableRetry)
                {
                    throw new Exception("Remote server closed the connection before sending response header!");
                }
                return false;
            }
            char[] separator = new char[] { '/', '.' };
            string[] strArray = str.Split(separator);
            this.VersionMajor = int.Parse(strArray[1]);
            this.VersionMinor = int.Parse(strArray[2]);
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                this.VerboseLogging($"HTTP Version: '{this.VersionMajor.ToString()}.{this.VersionMinor.ToString()}'");
            }
            string str2 = NoTrimReadTo(this.Stream, 0x20, 10);
            if (HTTPManager.Logger.Level == Loglevels.All)
            {
                this.VerboseLogging($"Status Code: '{str2}'");
            }
            if (this.baseRequest.DisableRetry)
            {
                num3 = int.Parse(str2);
            }
            else if (!int.TryParse(str2, out num3))
            {
                return false;
            }
            this.StatusCode = num3;
            if (((str2.Length > 0) && (((byte) str2[str2.Length - 1]) != 10)) && (((byte) str2[str2.Length - 1]) != 13))
            {
                this.Message = ReadTo(this.Stream, 10);
                if (HTTPManager.Logger.Level == Loglevels.All)
                {
                    this.VerboseLogging($"Status Message: '{this.Message}'");
                }
            }
            else
            {
                HTTPManager.Logger.Warning("HTTPResponse", $"{this.baseRequest.CurrentUri.ToString()} - Skipping Status Message reading!");
                this.Message = string.Empty;
            }
            this.ReadHeaders(this.Stream);
            this.IsUpgraded = (this.StatusCode == 0x65) && (this.HasHeaderWithValue("connection", "upgrade") || this.HasHeader("upgrade"));
            if (this.IsUpgraded && (HTTPManager.Logger.Level == Loglevels.All))
            {
                this.VerboseLogging("Request Upgraded!");
            }
            return (!readPayloadData || this.ReadPayload(forceReadRawContentLength));
        }

        private void VerboseLogging(string str)
        {
            HTTPManager.Logger.Verbose("HTTPResponse", "'" + this.baseRequest.CurrentUri.ToString() + "' - " + str);
        }

        protected void WaitWhileHasFragments()
        {
            while (this.baseRequest.UseStreaming && this.HasStreamedFragments())
            {
                Thread.Sleep(0x10);
            }
        }

        public int VersionMajor { get; protected set; }

        public int VersionMinor { get; protected set; }

        public int StatusCode { get; protected set; }

        public bool IsSuccess =>
            (((this.StatusCode >= 200) && (this.StatusCode < 300)) || (this.StatusCode == 0x130));

        public string Message { get; protected set; }

        public bool IsStreamed { get; protected set; }

        public bool IsStreamingFinished { get; internal set; }

        public bool IsFromCache { get; internal set; }

        public HTTPCacheFileInfo CacheFileInfo { get; internal set; }

        public bool IsCacheOnly { get; private set; }

        public Dictionary<string, List<string>> Headers { get; protected set; }

        public byte[] Data { get; internal set; }

        public bool IsUpgraded { get; protected set; }

        public List<Cookie> Cookies { get; internal set; }

        public string DataAsText
        {
            get
            {
                if (this.Data == null)
                {
                    return string.Empty;
                }
                if (!string.IsNullOrEmpty(this.dataAsText))
                {
                    return this.dataAsText;
                }
                return (this.dataAsText = Encoding.UTF8.GetString(this.Data, 0, this.Data.Length));
            }
        }

        public Texture2D DataAsTexture2D
        {
            get
            {
                if (this.Data == null)
                {
                    return null;
                }
                if (this.texture == null)
                {
                    this.texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                    this.texture.LoadImage(this.Data);
                }
                return this.texture;
            }
        }

        public bool IsClosedManually { get; protected set; }
    }
}

