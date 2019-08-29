namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto.Prng;
    using Org.BouncyCastle.Security;
    using System;

    public interface TlsContext
    {
        byte[] ExportKeyingMaterial(string asciiLabel, byte[] context_value, int length);

        IRandomGenerator NonceRandomGenerator { get; }

        Org.BouncyCastle.Security.SecureRandom SecureRandom { get; }

        Org.BouncyCastle.Crypto.Tls.SecurityParameters SecurityParameters { get; }

        bool IsServer { get; }

        ProtocolVersion ClientVersion { get; }

        ProtocolVersion ServerVersion { get; }

        TlsSession ResumableSession { get; }

        object UserObject { get; set; }
    }
}

