namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public abstract class TlsExtensionsUtilities
    {
        protected TlsExtensionsUtilities()
        {
        }

        public static void AddEncryptThenMacExtension(IDictionary extensions)
        {
            extensions[0x16] = CreateEncryptThenMacExtension();
        }

        public static void AddExtendedMasterSecretExtension(IDictionary extensions)
        {
            extensions[0x17] = CreateExtendedMasterSecretExtension();
        }

        public static void AddHeartbeatExtension(IDictionary extensions, HeartbeatExtension heartbeatExtension)
        {
            extensions[15] = CreateHeartbeatExtension(heartbeatExtension);
        }

        public static void AddMaxFragmentLengthExtension(IDictionary extensions, byte maxFragmentLength)
        {
            extensions[1] = CreateMaxFragmentLengthExtension(maxFragmentLength);
        }

        public static void AddPaddingExtension(IDictionary extensions, int dataLength)
        {
            extensions[0x15] = CreatePaddingExtension(dataLength);
        }

        public static void AddServerNameExtension(IDictionary extensions, ServerNameList serverNameList)
        {
            extensions[0] = CreateServerNameExtension(serverNameList);
        }

        public static void AddStatusRequestExtension(IDictionary extensions, CertificateStatusRequest statusRequest)
        {
            extensions[5] = CreateStatusRequestExtension(statusRequest);
        }

        public static void AddTruncatedHMacExtension(IDictionary extensions)
        {
            extensions[4] = CreateTruncatedHMacExtension();
        }

        public static byte[] CreateEmptyExtensionData() => 
            TlsUtilities.EmptyBytes;

        public static byte[] CreateEncryptThenMacExtension() => 
            CreateEmptyExtensionData();

        public static byte[] CreateExtendedMasterSecretExtension() => 
            CreateEmptyExtensionData();

        public static byte[] CreateHeartbeatExtension(HeartbeatExtension heartbeatExtension)
        {
            if (heartbeatExtension == null)
            {
                throw new TlsFatalAlert(80);
            }
            MemoryStream output = new MemoryStream();
            heartbeatExtension.Encode(output);
            return output.ToArray();
        }

        public static byte[] CreateMaxFragmentLengthExtension(byte maxFragmentLength) => 
            new byte[] { maxFragmentLength };

        public static byte[] CreatePaddingExtension(int dataLength)
        {
            TlsUtilities.CheckUint16(dataLength);
            return new byte[dataLength];
        }

        public static byte[] CreateServerNameExtension(ServerNameList serverNameList)
        {
            if (serverNameList == null)
            {
                throw new TlsFatalAlert(80);
            }
            MemoryStream output = new MemoryStream();
            serverNameList.Encode(output);
            return output.ToArray();
        }

        public static byte[] CreateStatusRequestExtension(CertificateStatusRequest statusRequest)
        {
            if (statusRequest == null)
            {
                throw new TlsFatalAlert(80);
            }
            MemoryStream output = new MemoryStream();
            statusRequest.Encode(output);
            return output.ToArray();
        }

        public static byte[] CreateTruncatedHMacExtension() => 
            CreateEmptyExtensionData();

        public static IDictionary EnsureExtensionsInitialised(IDictionary extensions) => 
            ((extensions != null) ? extensions : Platform.CreateHashtable());

        public static HeartbeatExtension GetHeartbeatExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 15);
            return ((extensionData != null) ? ReadHeartbeatExtension(extensionData) : null);
        }

        public static short GetMaxFragmentLengthExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 1);
            return ((extensionData != null) ? ReadMaxFragmentLengthExtension(extensionData) : ((short) (-1)));
        }

        public static int GetPaddingExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 0x15);
            return ((extensionData != null) ? ReadPaddingExtension(extensionData) : -1);
        }

        public static ServerNameList GetServerNameExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 0);
            return ((extensionData != null) ? ReadServerNameExtension(extensionData) : null);
        }

        public static CertificateStatusRequest GetStatusRequestExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 5);
            return ((extensionData != null) ? ReadStatusRequestExtension(extensionData) : null);
        }

        public static bool HasEncryptThenMacExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 0x16);
            return ((extensionData != null) ? ReadEncryptThenMacExtension(extensionData) : false);
        }

        public static bool HasExtendedMasterSecretExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 0x17);
            return ((extensionData != null) ? ReadExtendedMasterSecretExtension(extensionData) : false);
        }

        public static bool HasTruncatedHMacExtension(IDictionary extensions)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 4);
            return ((extensionData != null) ? ReadTruncatedHMacExtension(extensionData) : false);
        }

        private static bool ReadEmptyExtensionData(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            if (extensionData.Length != 0)
            {
                throw new TlsFatalAlert(0x2f);
            }
            return true;
        }

        public static bool ReadEncryptThenMacExtension(byte[] extensionData) => 
            ReadEmptyExtensionData(extensionData);

        public static bool ReadExtendedMasterSecretExtension(byte[] extensionData) => 
            ReadEmptyExtensionData(extensionData);

        public static HeartbeatExtension ReadHeartbeatExtension(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            MemoryStream input = new MemoryStream(extensionData, false);
            HeartbeatExtension extension = HeartbeatExtension.Parse(input);
            TlsProtocol.AssertEmpty(input);
            return extension;
        }

        public static short ReadMaxFragmentLengthExtension(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            if (extensionData.Length != 1)
            {
                throw new TlsFatalAlert(50);
            }
            return extensionData[0];
        }

        public static int ReadPaddingExtension(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            for (int i = 0; i < extensionData.Length; i++)
            {
                if (extensionData[i] != 0)
                {
                    throw new TlsFatalAlert(0x2f);
                }
            }
            return extensionData.Length;
        }

        public static ServerNameList ReadServerNameExtension(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            MemoryStream input = new MemoryStream(extensionData, false);
            ServerNameList list = ServerNameList.Parse(input);
            TlsProtocol.AssertEmpty(input);
            return list;
        }

        public static CertificateStatusRequest ReadStatusRequestExtension(byte[] extensionData)
        {
            if (extensionData == null)
            {
                throw new ArgumentNullException("extensionData");
            }
            MemoryStream input = new MemoryStream(extensionData, false);
            CertificateStatusRequest request = CertificateStatusRequest.Parse(input);
            TlsProtocol.AssertEmpty(input);
            return request;
        }

        public static bool ReadTruncatedHMacExtension(byte[] extensionData) => 
            ReadEmptyExtensionData(extensionData);
    }
}

