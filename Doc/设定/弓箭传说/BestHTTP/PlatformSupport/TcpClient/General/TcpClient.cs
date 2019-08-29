namespace BestHTTP.PlatformSupport.TcpClient.General
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TcpClient : IDisposable
    {
        private NetworkStream stream;
        private bool active;
        private Socket client;
        private bool disposed;
        private Properties values;
        private int recv_timeout;
        private int send_timeout;
        private int recv_buffer_size;
        private int send_buffer_size;
        private LingerOption linger_state;
        private bool no_delay;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <ConnectTimeout>k__BackingField;

        public TcpClient()
        {
            this.Init(AddressFamily.InterNetwork);
            this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
        }

        public TcpClient(IPEndPoint localEP)
        {
            this.Init(localEP.AddressFamily);
            this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
        }

        public TcpClient(AddressFamily family)
        {
            if ((family != AddressFamily.InterNetwork) && (family != AddressFamily.InterNetworkV6))
            {
                throw new ArgumentException("Family must be InterNetwork or InterNetworkV6", "family");
            }
            this.Init(family);
            this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
        }

        public TcpClient(string hostname, int port)
        {
            this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
            this.Connect(hostname, port);
        }

        public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state) => 
            this.client.BeginConnect(address, port, requestCallback, state);

        public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state) => 
            this.client.BeginConnect(addresses, port, requestCallback, state);

        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state) => 
            this.client.BeginConnect(host, port, requestCallback, state);

        private void CheckDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
        }

        public void Close()
        {
            ((IDisposable) this).Dispose();
        }

        public void Connect(IPEndPoint remoteEP)
        {
            try
            {
                if (this.ConnectTimeout > TimeSpan.Zero)
                {
                    <Connect>c__AnonStorey0 storey = new <Connect>c__AnonStorey0 {
                        mre = new ManualResetEvent(false)
                    };
                    IAsyncResult asyncResult = this.client.BeginConnect(remoteEP, new AsyncCallback(storey.<>m__0), null);
                    this.active = storey.mre.WaitOne(this.ConnectTimeout);
                    if (!this.active)
                    {
                        try
                        {
                            this.client.Close();
                        }
                        catch
                        {
                        }
                        throw new TimeoutException("Connection timed out!");
                    }
                    this.client.EndConnect(asyncResult);
                }
                else
                {
                    this.client.Connect(remoteEP);
                    this.active = true;
                }
            }
            finally
            {
                this.CheckDisposed();
            }
        }

        public void Connect(IPAddress address, int port)
        {
            this.Connect(new IPEndPoint(address, port));
        }

        public void Connect(string hostname, int port)
        {
            if (this.ConnectTimeout > TimeSpan.Zero)
            {
                <Connect>c__AnonStorey1 storey = new <Connect>c__AnonStorey1 {
                    mre = new ManualResetEvent(false)
                };
                IAsyncResult asyncResult = Dns.BeginGetHostAddresses(hostname, new AsyncCallback(storey.<>m__0), null);
                if (!storey.mre.WaitOne(this.ConnectTimeout))
                {
                    throw new TimeoutException("DNS resolve timed out!");
                }
                IPAddress[] ipAddresses = Dns.EndGetHostAddresses(asyncResult);
                this.Connect(ipAddresses, port);
            }
            else
            {
                IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
                this.Connect(hostAddresses, port);
            }
        }

        public void Connect(IPAddress[] ipAddresses, int port)
        {
            this.CheckDisposed();
            if (ipAddresses == null)
            {
                throw new ArgumentNullException("ipAddresses");
            }
            for (int i = 0; i < ipAddresses.Length; i++)
            {
                try
                {
                    IPAddress address = ipAddresses[i];
                    if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
                    {
                        throw new SocketException(0x2741);
                    }
                    this.Init(address.AddressFamily);
                    if ((address.AddressFamily != AddressFamily.InterNetwork) && (address.AddressFamily != AddressFamily.InterNetworkV6))
                    {
                        throw new NotSupportedException("This method is only valid for sockets in the InterNetwork and InterNetworkV6 families");
                    }
                    this.Connect(new IPEndPoint(address, port));
                    if (this.values != ((Properties) 0))
                    {
                        this.SetOptions();
                    }
                    this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    try
                    {
                    }
                    catch
                    {
                    }
                    break;
                }
                catch (Exception exception)
                {
                    this.Init(AddressFamily.InterNetwork);
                    if (i == (ipAddresses.Length - 1))
                    {
                        throw exception;
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;
                if (disposing)
                {
                    NetworkStream stream = this.stream;
                    this.stream = null;
                    if (stream != null)
                    {
                        stream.Close();
                        this.active = false;
                        stream = null;
                    }
                    else if (this.client != null)
                    {
                        this.client.Close();
                        this.client = null;
                    }
                }
            }
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            this.client.EndConnect(asyncResult);
        }

        ~TcpClient()
        {
            this.Dispose(false);
        }

        public Stream GetStream()
        {
            Stream stream;
            try
            {
                if (this.stream == null)
                {
                    this.stream = new NetworkStream(this.client, true);
                }
                stream = this.stream;
            }
            finally
            {
                this.CheckDisposed();
            }
            return stream;
        }

        private void Init(AddressFamily family)
        {
            this.active = false;
            if (this.client != null)
            {
                this.client.Close();
                this.client = null;
            }
            this.client = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool IsConnected()
        {
            try
            {
                return !(this.Client.Poll(1, SelectMode.SelectRead) && (this.Client.Available == 0));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetOptions()
        {
            Properties values = this.values;
            this.values = (Properties) 0;
            if ((values & Properties.LingerState) != ((Properties) 0))
            {
                this.LingerState = this.linger_state;
            }
            if ((values & Properties.NoDelay) != ((Properties) 0))
            {
                this.NoDelay = this.no_delay;
            }
            if ((values & Properties.ReceiveBufferSize) != ((Properties) 0))
            {
                this.ReceiveBufferSize = this.recv_buffer_size;
            }
            if ((values & Properties.ReceiveTimeout) != ((Properties) 0))
            {
                this.ReceiveTimeout = this.recv_timeout;
            }
            if ((values & Properties.SendBufferSize) != ((Properties) 0))
            {
                this.SendBufferSize = this.send_buffer_size;
            }
            if ((values & Properties.SendTimeout) != ((Properties) 0))
            {
                this.SendTimeout = this.send_timeout;
            }
        }

        internal void SetTcpClient(Socket s)
        {
            this.Client = s;
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected bool Active
        {
            get => 
                this.active;
            set => 
                (this.active = value);
        }

        public Socket Client
        {
            get => 
                this.client;
            set
            {
                this.client = value;
                this.stream = null;
            }
        }

        public int Available =>
            this.client.Available;

        public bool Connected =>
            this.client.Connected;

        public bool ExclusiveAddressUse
        {
            get => 
                this.client.ExclusiveAddressUse;
            set => 
                (this.client.ExclusiveAddressUse = value);
        }

        public LingerOption LingerState
        {
            get
            {
                if ((this.values & Properties.LingerState) != ((Properties) 0))
                {
                    return this.linger_state;
                }
                return (LingerOption) this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
            }
            set
            {
                if (!this.client.Connected)
                {
                    this.linger_state = value;
                    this.values |= Properties.LingerState;
                }
                else
                {
                    this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
                }
            }
        }

        public bool NoDelay
        {
            get
            {
                if ((this.values & Properties.NoDelay) != ((Properties) 0))
                {
                    return this.no_delay;
                }
                return (bool) this.client.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug);
            }
            set
            {
                if (!this.client.Connected)
                {
                    this.no_delay = value;
                    this.values |= Properties.NoDelay;
                }
                else
                {
                    this.client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, !value ? 0 : 1);
                }
            }
        }

        public int ReceiveBufferSize
        {
            get
            {
                if ((this.values & Properties.ReceiveBufferSize) != ((Properties) 0))
                {
                    return this.recv_buffer_size;
                }
                return (int) this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
            }
            set
            {
                if (!this.client.Connected)
                {
                    this.recv_buffer_size = value;
                    this.values |= Properties.ReceiveBufferSize;
                }
                else
                {
                    this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
                }
            }
        }

        public int ReceiveTimeout
        {
            get
            {
                if ((this.values & Properties.ReceiveTimeout) != ((Properties) 0))
                {
                    return this.recv_timeout;
                }
                return (int) this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
            }
            set
            {
                if (!this.client.Connected)
                {
                    this.recv_timeout = value;
                    this.values |= Properties.ReceiveTimeout;
                }
                else
                {
                    this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
                }
            }
        }

        public int SendBufferSize
        {
            get
            {
                if ((this.values & Properties.SendBufferSize) != ((Properties) 0))
                {
                    return this.send_buffer_size;
                }
                return (int) this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
            }
            set
            {
                if (!this.client.Connected)
                {
                    this.send_buffer_size = value;
                    this.values |= Properties.SendBufferSize;
                }
                else
                {
                    this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
                }
            }
        }

        public int SendTimeout
        {
            get
            {
                if ((this.values & Properties.SendTimeout) != ((Properties) 0))
                {
                    return this.send_timeout;
                }
                return (int) this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
            }
            set
            {
                if (!this.client.Connected)
                {
                    this.send_timeout = value;
                    this.values |= Properties.SendTimeout;
                }
                else
                {
                    this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
                }
            }
        }

        public TimeSpan ConnectTimeout { get; set; }

        [CompilerGenerated]
        private sealed class <Connect>c__AnonStorey0
        {
            internal ManualResetEvent mre;

            internal void <>m__0(IAsyncResult res)
            {
                this.mre.Set();
            }
        }

        [CompilerGenerated]
        private sealed class <Connect>c__AnonStorey1
        {
            internal ManualResetEvent mre;

            internal void <>m__0(IAsyncResult res)
            {
                this.mre.Set();
            }
        }

        private enum Properties : uint
        {
            LingerState = 1,
            NoDelay = 2,
            ReceiveBufferSize = 4,
            ReceiveTimeout = 8,
            SendBufferSize = 0x10,
            SendTimeout = 0x20
        }
    }
}

