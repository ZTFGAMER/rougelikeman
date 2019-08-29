namespace GameProtocol
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    public sealed class CMailInfo
    {
        public bool IsReaded;
        public bool IsGot;
        public uint m_nMailID;
        public string m_strTitle;
        public string m_strContent;
        public ushort m_nMailType;
        public ulong m_i64PubTime;
        public ushort m_nCoins;
        public ushort m_nDiamond;
        public bool m_bIsReaded;
        public bool m_bIsGot;

        public byte[] buildPacket() => 
            null;

        private bool checkValid() => 
            (this.m_nMailType < 4);

        public void ReadFromStream(BinaryReader reader)
        {
            this.m_nMailID = reader.ReadUInt32();
            this.m_strTitle = reader.ReadString();
            this.m_strContent = reader.ReadString();
            this.m_nMailType = reader.ReadUInt16();
            this.m_i64PubTime = reader.ReadUInt64();
            this.m_nCoins = reader.ReadUInt16();
            this.m_nDiamond = reader.ReadUInt16();
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nMailID);
            writer.Write(this.m_strTitle);
            writer.Write(this.m_strContent);
            writer.Write(this.m_nMailType);
            writer.Write(this.m_i64PubTime);
            writer.Write(this.m_nCoins);
            writer.Write(this.m_nDiamond);
        }

        [JsonIgnore]
        public bool IsHaveReward =>
            (((this.m_nCoins > 0) || (this.m_nDiamond > 0)) && (this.m_nMailType == 2));

        [JsonIgnore]
        public bool IsShowRed =>
            (!this.IsReaded || (this.IsHaveReward && !this.IsGot));

        public enum eMailType
        {
            ENormalMailType = 1,
            EReimburseMailType = 2,
            eForcePopType = 3,
            EInvalidMailType = 4
        }
    }
}

