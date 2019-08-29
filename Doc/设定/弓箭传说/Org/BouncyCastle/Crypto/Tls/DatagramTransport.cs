namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public interface DatagramTransport
    {
        void Close();
        int GetReceiveLimit();
        int GetSendLimit();
        int Receive(byte[] buf, int off, int len, int waitMillis);
        void Send(byte[] buf, int off, int len);
    }
}

