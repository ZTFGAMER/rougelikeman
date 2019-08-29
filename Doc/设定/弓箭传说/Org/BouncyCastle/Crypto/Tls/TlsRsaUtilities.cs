namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Encodings;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public abstract class TlsRsaUtilities
    {
        protected TlsRsaUtilities()
        {
        }

        public static byte[] GenerateEncryptedPreMasterSecret(TlsContext context, RsaKeyParameters rsaServerPublicKey, Stream output)
        {
            byte[] buffer = new byte[0x30];
            context.SecureRandom.NextBytes(buffer);
            TlsUtilities.WriteVersion(context.ClientVersion, buffer, 0);
            Pkcs1Encoding encoding = new Pkcs1Encoding(new RsaBlindedEngine());
            encoding.Init(true, new ParametersWithRandom(rsaServerPublicKey, context.SecureRandom));
            try
            {
                byte[] buffer2 = encoding.ProcessBlock(buffer, 0, buffer.Length);
                if (TlsUtilities.IsSsl(context))
                {
                    output.Write(buffer2, 0, buffer2.Length);
                    return buffer;
                }
                TlsUtilities.WriteOpaque16(buffer2, output);
            }
            catch (InvalidCipherTextException exception)
            {
                throw new TlsFatalAlert(80, exception);
            }
            return buffer;
        }

        public static byte[] SafeDecryptPreMasterSecret(TlsContext context, RsaKeyParameters rsaServerPrivateKey, byte[] encryptedPreMasterSecret)
        {
            ProtocolVersion clientVersion = context.ClientVersion;
            bool flag = false;
            byte[] buffer = new byte[0x30];
            context.SecureRandom.NextBytes(buffer);
            byte[] buffer2 = Arrays.Clone(buffer);
            try
            {
                Pkcs1Encoding encoding = new Pkcs1Encoding(new RsaBlindedEngine(), buffer);
                encoding.Init(false, new ParametersWithRandom(rsaServerPrivateKey, context.SecureRandom));
                buffer2 = encoding.ProcessBlock(encryptedPreMasterSecret, 0, encryptedPreMasterSecret.Length);
            }
            catch (Exception)
            {
            }
            if (!flag || !clientVersion.IsEqualOrEarlierVersionOf(ProtocolVersion.TLSv10))
            {
                int num = (clientVersion.MajorVersion ^ (buffer2[0] & 0xff)) | (clientVersion.MinorVersion ^ (buffer2[1] & 0xff));
                num |= num >> 1;
                num |= num >> 2;
                num |= num >> 4;
                int num2 = ~((num & 1) - 1);
                for (int i = 0; i < 0x30; i++)
                {
                    buffer2[i] = (byte) ((buffer2[i] & ~num2) | (buffer[i] & num2));
                }
            }
            return buffer2;
        }
    }
}

