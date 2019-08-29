namespace GameProtocol
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    [Serializable]
    public sealed class CEquipmentItem : IProtocol
    {
        public const ushort MsgType = 0xffff;
        public string m_nUniqueID;
        public ulong m_nRowID;
        public uint m_nEquipID;
        public uint m_nLevel;
        public uint m_nFragment;

        public byte[] buildPacket() => 
            null;

        public void ReadFromStream(BinaryReader reader)
        {
            this.m_nUniqueID = reader.ReadString();
            this.m_nRowID = reader.ReadUInt64();
            this.m_nEquipID = reader.ReadUInt32();
            this.m_nLevel = reader.ReadUInt32();
            this.m_nFragment = reader.ReadUInt32();
            if (this.m_nUniqueID.Length > 0x25)
            {
                this.m_nUniqueID = this.m_nUniqueID.Substring(0, 0x25);
            }
        }

        public void WriteToStream(BinaryWriter writter)
        {
            writter.Write(this.m_nUniqueID);
            writter.Write(this.m_nRowID);
            writter.Write(this.m_nEquipID);
            writter.Write(this.m_nLevel);
            writter.Write(this.m_nFragment);
        }

        [JsonIgnore]
        public ushort GetMsgType =>
            0xffff;
    }
}

