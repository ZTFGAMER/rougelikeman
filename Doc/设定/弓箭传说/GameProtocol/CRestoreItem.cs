namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CRestoreItem
    {
        public short m_nMin;
        public ushort m_nMax;
        public ulong m_i64Timestamp;

        public void ReadFromStream(BinaryReader reader)
        {
            this.m_nMin = reader.ReadInt16();
            this.m_nMax = reader.ReadUInt16();
            this.m_i64Timestamp = reader.ReadUInt64();
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nMin);
            writer.Write(this.m_nMax);
            writer.Write(this.m_i64Timestamp);
        }

        public enum EItemIndex
        {
            ELifeItemIndex,
            ENormalDiamondItemIndex,
            ELargeDiamondItemIndex,
            EAdGetLifeItemIndex,
            EAdGetLuckyItemIndex,
            EInvalidItemIndex
        }
    }
}

