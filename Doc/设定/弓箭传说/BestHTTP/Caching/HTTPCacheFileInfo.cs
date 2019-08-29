namespace BestHTTP.Caching
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class HTTPCacheFileInfo : IComparable<HTTPCacheFileInfo>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Uri <Uri>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <LastAccess>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <BodyLength>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ETag>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <LastModified>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <Expires>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <Age>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <MaxAge>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <Date>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <MustRevalidate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <Received>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ConstructedPath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ulong <MappedNameIDX>k__BackingField;

        internal HTTPCacheFileInfo(System.Uri uri) : this(uri, DateTime.UtcNow, -1)
        {
        }

        internal HTTPCacheFileInfo(System.Uri uri, DateTime lastAcces, int bodyLength)
        {
            this.Uri = uri;
            this.LastAccess = lastAcces;
            this.BodyLength = bodyLength;
            this.MaxAge = -1L;
            this.MappedNameIDX = HTTPCacheService.GetNameIdx();
        }

        internal HTTPCacheFileInfo(System.Uri uri, BinaryReader reader, int version)
        {
            this.Uri = uri;
            this.LastAccess = DateTime.FromBinary(reader.ReadInt64());
            this.BodyLength = reader.ReadInt32();
            if (version != 2)
            {
                if (version != 1)
                {
                    return;
                }
            }
            else
            {
                this.MappedNameIDX = reader.ReadUInt64();
            }
            this.ETag = reader.ReadString();
            this.LastModified = reader.ReadString();
            this.Expires = DateTime.FromBinary(reader.ReadInt64());
            this.Age = reader.ReadInt64();
            this.MaxAge = reader.ReadInt64();
            this.Date = DateTime.FromBinary(reader.ReadInt64());
            this.MustRevalidate = reader.ReadBoolean();
            this.Received = DateTime.FromBinary(reader.ReadInt64());
        }

        public int CompareTo(HTTPCacheFileInfo other) => 
            this.LastAccess.CompareTo(other.LastAccess);

        internal void Delete()
        {
            if (HTTPCacheService.IsSupported)
            {
                string path = this.GetPath();
                try
                {
                    File.Delete(path);
                }
                catch
                {
                }
                finally
                {
                    this.Reset();
                }
            }
        }

        public Stream GetBodyStream(out int length)
        {
            if (!this.IsExists())
            {
                length = 0;
                return null;
            }
            length = this.BodyLength;
            this.LastAccess = DateTime.UtcNow;
            FileStream stream = new FileStream(this.GetPath(), FileMode.Open, FileAccess.Read, FileShare.Read);
            stream.Seek((long) -length, SeekOrigin.End);
            return stream;
        }

        public string GetPath()
        {
            if (this.ConstructedPath != null)
            {
                return this.ConstructedPath;
            }
            string str = Path.Combine(HTTPCacheService.CacheFolder, this.MappedNameIDX.ToString("X"));
            this.ConstructedPath = str;
            return str;
        }

        internal Stream GetSaveStream(HTTPResponse response)
        {
            if (!HTTPCacheService.IsSupported)
            {
                return null;
            }
            this.LastAccess = DateTime.UtcNow;
            string path = this.GetPath();
            if (File.Exists(path))
            {
                this.Delete();
            }
            if (path.Length > HTTPManager.MaxPathLength)
            {
                return null;
            }
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                object[] values = new object[] { response.StatusCode, response.Message };
                stream.WriteLine("HTTP/1.1 {0} {1}", values);
                foreach (KeyValuePair<string, List<string>> pair in response.Headers)
                {
                    for (int i = 0; i < pair.Value.Count; i++)
                    {
                        object[] objArray2 = new object[] { pair.Key, pair.Value[i] };
                        stream.WriteLine("{0}: {1}", objArray2);
                    }
                }
                stream.WriteLine();
            }
            if (response.IsFromCache && !response.Headers.ContainsKey("content-length"))
            {
                List<string> list = new List<string> {
                    this.BodyLength.ToString()
                };
                response.Headers.Add("content-length", list);
            }
            this.SetUpCachingValues(response);
            return new FileStream(this.GetPath(), FileMode.Append);
        }

        public bool IsExists()
        {
            if (!HTTPCacheService.IsSupported)
            {
                return false;
            }
            return File.Exists(this.GetPath());
        }

        internal HTTPResponse ReadResponseTo(HTTPRequest request)
        {
            if (!this.IsExists())
            {
                return null;
            }
            this.LastAccess = DateTime.UtcNow;
            using (FileStream stream = new FileStream(this.GetPath(), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                HTTPResponse response = new HTTPResponse(request, stream, request.UseStreaming, true) {
                    CacheFileInfo = this
                };
                response.Receive(this.BodyLength, true);
                return response;
            }
        }

        private void Reset()
        {
            this.BodyLength = -1;
            this.ETag = string.Empty;
            this.Expires = DateTime.FromBinary(0L);
            this.LastModified = string.Empty;
            this.Age = 0L;
            this.MaxAge = -1L;
            this.Date = DateTime.FromBinary(0L);
            this.MustRevalidate = false;
            this.Received = DateTime.FromBinary(0L);
        }

        internal void SaveTo(BinaryWriter writer)
        {
            writer.Write(this.LastAccess.ToBinary());
            writer.Write(this.BodyLength);
            writer.Write(this.MappedNameIDX);
            writer.Write(this.ETag);
            writer.Write(this.LastModified);
            writer.Write(this.Expires.ToBinary());
            writer.Write(this.Age);
            writer.Write(this.MaxAge);
            writer.Write(this.Date.ToBinary());
            writer.Write(this.MustRevalidate);
            writer.Write(this.Received.ToBinary());
        }

        private void SetUpCachingValues(HTTPResponse response)
        {
            response.CacheFileInfo = this;
            this.ETag = response.GetFirstHeaderValue("ETag").ToStrOrEmpty();
            this.Expires = response.GetFirstHeaderValue("Expires").ToDateTime(DateTime.FromBinary(0L));
            this.LastModified = response.GetFirstHeaderValue("Last-Modified").ToStrOrEmpty();
            this.Age = response.GetFirstHeaderValue("Age").ToInt64(0L);
            this.Date = response.GetFirstHeaderValue("Date").ToDateTime(DateTime.FromBinary(0L));
            string firstHeaderValue = response.GetFirstHeaderValue("cache-control");
            if (!string.IsNullOrEmpty(firstHeaderValue))
            {
                string[] strArray = firstHeaderValue.FindOption("max-age");
                if ((strArray != null) && double.TryParse(strArray[1], out double num))
                {
                    this.MaxAge = (int) num;
                }
                this.MustRevalidate = firstHeaderValue.ToLower().Contains("must-revalidate");
            }
            this.Received = DateTime.UtcNow;
        }

        internal void SetUpRevalidationHeaders(HTTPRequest request)
        {
            if (this.IsExists())
            {
                if (!string.IsNullOrEmpty(this.ETag))
                {
                    request.AddHeader("If-None-Match", this.ETag);
                }
                if (!string.IsNullOrEmpty(this.LastModified))
                {
                    request.AddHeader("If-Modified-Since", this.LastModified);
                }
            }
        }

        internal void Store(HTTPResponse response)
        {
            if (HTTPCacheService.IsSupported)
            {
                string path = this.GetPath();
                if (path.Length <= HTTPManager.MaxPathLength)
                {
                    if (File.Exists(path))
                    {
                        this.Delete();
                    }
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        object[] values = new object[] { response.StatusCode, response.Message };
                        stream.WriteLine("HTTP/1.1 {0} {1}", values);
                        foreach (KeyValuePair<string, List<string>> pair in response.Headers)
                        {
                            for (int i = 0; i < pair.Value.Count; i++)
                            {
                                object[] objArray2 = new object[] { pair.Key, pair.Value[i] };
                                stream.WriteLine("{0}: {1}", objArray2);
                            }
                        }
                        stream.WriteLine();
                        stream.Write(response.Data, 0, response.Data.Length);
                    }
                    this.BodyLength = response.Data.Length;
                    this.LastAccess = DateTime.UtcNow;
                    this.SetUpCachingValues(response);
                }
            }
        }

        internal bool WillExpireInTheFuture()
        {
            if (!this.IsExists())
            {
                return false;
            }
            if (this.MustRevalidate)
            {
                return false;
            }
            if (this.MaxAge != -1L)
            {
                TimeSpan span = (TimeSpan) (this.Received - this.Date);
                long num2 = Math.Max(Math.Max(0L, (long) span.TotalSeconds), this.Age);
                TimeSpan span2 = (TimeSpan) (DateTime.UtcNow - this.Date);
                long totalSeconds = (long) span2.TotalSeconds;
                long num4 = num2 + totalSeconds;
                return (num4 < this.MaxAge);
            }
            return (this.Expires > DateTime.UtcNow);
        }

        internal System.Uri Uri { get; set; }

        internal DateTime LastAccess { get; set; }

        public int BodyLength { get; set; }

        private string ETag { get; set; }

        private string LastModified { get; set; }

        private DateTime Expires { get; set; }

        private long Age { get; set; }

        private long MaxAge { get; set; }

        private DateTime Date { get; set; }

        private bool MustRevalidate { get; set; }

        private DateTime Received { get; set; }

        private string ConstructedPath { get; set; }

        internal ulong MappedNameIDX { get; set; }
    }
}

