namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CTimestampItem
    {
        public ulong m_i64Timestamp;

        public void ReadFromStream(BinaryReader reader)
        {
            this.m_i64Timestamp = reader.ReadUInt64();
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_i64Timestamp);
        }

        public enum EItemIndex
        {
            ECurSrvItemIndex,
            EHarvestItemIndex,
            EInvalidItemIndex
        }
    }
}

