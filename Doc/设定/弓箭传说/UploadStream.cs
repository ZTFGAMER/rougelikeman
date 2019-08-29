using BestHTTP;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

public sealed class UploadStream : Stream
{
    private MemoryStream ReadBuffer;
    private MemoryStream WriteBuffer;
    private bool noMoreData;
    private AutoResetEvent ARE;
    private object locker;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string <Name>k__BackingField;

    public UploadStream()
    {
        this.ReadBuffer = new MemoryStream();
        this.WriteBuffer = new MemoryStream();
        this.ARE = new AutoResetEvent(false);
        this.locker = new object();
        this.ReadBuffer = new MemoryStream();
        this.WriteBuffer = new MemoryStream();
        this.Name = string.Empty;
    }

    public UploadStream(string name) : this()
    {
        this.Name = name;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            HTTPManager.Logger.Information("UploadStream", $"{this.Name} - Dispose");
            this.ReadBuffer.Dispose();
            this.ReadBuffer = null;
            this.WriteBuffer.Dispose();
            this.WriteBuffer = null;
            this.ARE.Close();
            this.ARE = null;
        }
        base.Dispose(disposing);
    }

    public void Finish()
    {
        if (this.noMoreData)
        {
            throw new ArgumentException("noMoreData already set!");
        }
        HTTPManager.Logger.Information("UploadStream", $"{this.Name} - Finish");
        this.noMoreData = true;
        this.ARE.Set();
    }

    public override void Flush()
    {
        this.Finish();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (this.noMoreData)
        {
            if (this.ReadBuffer.Position != this.ReadBuffer.Length)
            {
                return this.ReadBuffer.Read(buffer, offset, count);
            }
            if (this.WriteBuffer.Length <= 0L)
            {
                HTTPManager.Logger.Information("UploadStream", $"{this.Name} - Read - End Of Stream");
                return -1;
            }
            this.SwitchBuffers();
        }
        if (this.IsReadBufferEmpty)
        {
            this.ARE.WaitOne();
            object obj2 = this.locker;
            lock (obj2)
            {
                if (this.IsReadBufferEmpty && (this.WriteBuffer.Length > 0L))
                {
                    this.SwitchBuffers();
                }
            }
        }
        object locker = this.locker;
        lock (locker)
        {
            return this.ReadBuffer.Read(buffer, offset, count);
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    private bool SwitchBuffers()
    {
        object locker = this.locker;
        lock (locker)
        {
            if (this.ReadBuffer.Position == this.ReadBuffer.Length)
            {
                this.WriteBuffer.Seek(0L, SeekOrigin.Begin);
                this.ReadBuffer.SetLength(0L);
                MemoryStream writeBuffer = this.WriteBuffer;
                this.WriteBuffer = this.ReadBuffer;
                this.ReadBuffer = writeBuffer;
                return true;
            }
        }
        return false;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (this.noMoreData)
        {
            throw new ArgumentException("noMoreData already set!");
        }
        object locker = this.locker;
        lock (locker)
        {
            this.WriteBuffer.Write(buffer, offset, count);
            this.SwitchBuffers();
        }
        this.ARE.Set();
    }

    public string Name { get; private set; }

    private bool IsReadBufferEmpty
    {
        get
        {
            object locker = this.locker;
            lock (locker)
            {
                return (this.ReadBuffer.Position == this.ReadBuffer.Length);
            }
        }
    }

    public override bool CanRead
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override bool CanSeek
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override bool CanWrite
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override long Length
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override long Position
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }
}

