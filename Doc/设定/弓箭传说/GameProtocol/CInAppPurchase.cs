namespace GameProtocol
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    [Serializable]
    public sealed class CInAppPurchase : CProtocolBase
    {
        public uint m_nTransID;
        public ushort m_nPlatformIndex;
        public string m_nProductID;
        public string m_strReceiptData;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nTransID = reader.ReadUInt32();
            this.m_nPlatformIndex = reader.ReadUInt16();
            this.m_nProductID = reader.ReadString();
            this.m_strReceiptData = reader.ReadString();
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nTransID);
            writer.Write(this.m_nPlatformIndex);
            writer.Write(this.m_nProductID);
            writer.Write(this.m_strReceiptData);
        }

        [JsonIgnore]
        public override ushort GetMsgType =>
            15;
    }
}

