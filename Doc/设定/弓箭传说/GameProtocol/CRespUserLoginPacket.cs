namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CRespUserLoginPacket : IProtocol
    {
        public CEquipmentItem[] m_arrayEquipData;
        public CRestoreItem[] m_arrayRestoreData;
        public CTimestampItem[] m_arrayTimestampData;
        public uint m_nTransID;
        public uint m_nCoins;
        public uint m_nDiamonds;
        public ushort m_nMaxLayer;
        public ushort m_nLayerBoxID;
        public ushort m_nLevel;
        public uint m_nExperince;
        public uint m_nTreasureRandomCount;
        public ushort m_nBattleRebornCount;
        public string m_strUserAccessToken;
        public ulong m_nUserRawId;
        public ushort m_nExtraNormalDiamondItem;
        public ushort m_nExtraLargeDiamondItem;
        public long m_nGameSystemMask;

        private byte[] buildPacket()
        {
            BinaryWriter writer = ProtocolBuffer.Writer;
            writer.Write((byte) 13);
            writer.Write(this.GetMsgType);
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer2 = new CustomBinaryWriter(stream);
            this.WriteToStream(writer2);
            ushort length = (ushort) stream.ToArray().Length;
            writer.Write(length);
            writer.Write(stream.ToArray());
            return ProtocolBuffer.CacheStream.ToArray();
        }

        public ulong GetHarvestTime() => 
            this.GetTime(CTimestampItem.EItemIndex.EHarvestItemIndex);

        public CRestoreItem GetRestore(CRestoreItem.EItemIndex type)
        {
            CRestoreItem item = null;
            int index = (int) type;
            if ((index >= 0) && (index < this.m_arrayRestoreData.Length))
            {
                item = this.m_arrayRestoreData[index];
            }
            return item;
        }

        public ulong GetServerTime() => 
            this.GetTime(CTimestampItem.EItemIndex.ECurSrvItemIndex);

        private ulong GetTime(CTimestampItem.EItemIndex type)
        {
            ulong num = 0L;
            int index = (int) type;
            if ((index >= 0) && (index < this.m_arrayTimestampData.Length))
            {
                num = this.m_arrayTimestampData[index].m_i64Timestamp;
            }
            return num;
        }

        public void ReadFromStream(BinaryReader reader)
        {
            ushort num = reader.ReadUInt16();
            this.m_arrayEquipData = new CEquipmentItem[num];
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.m_arrayEquipData[i] = new CEquipmentItem();
                this.m_arrayEquipData[i].ReadFromStream(reader);
            }
            num = reader.ReadUInt16();
            this.m_arrayRestoreData = new CRestoreItem[num];
            for (ushort j = 0; j < num; j = (ushort) (j + 1))
            {
                this.m_arrayRestoreData[j] = new CRestoreItem();
                this.m_arrayRestoreData[j].ReadFromStream(reader);
            }
            num = reader.ReadUInt16();
            this.m_arrayTimestampData = new CTimestampItem[num];
            for (ushort k = 0; k < num; k = (ushort) (k + 1))
            {
                this.m_arrayTimestampData[k] = new CTimestampItem();
                this.m_arrayTimestampData[k].ReadFromStream(reader);
            }
            this.m_nTransID = reader.ReadUInt32();
            this.m_nCoins = reader.ReadUInt32();
            this.m_nDiamonds = reader.ReadUInt32();
            this.m_nMaxLayer = reader.ReadUInt16();
            this.m_nLayerBoxID = reader.ReadUInt16();
            this.m_nLevel = reader.ReadUInt16();
            this.m_nExperince = reader.ReadUInt32();
            this.m_nTreasureRandomCount = reader.ReadUInt32();
            this.m_nBattleRebornCount = reader.ReadUInt16();
            this.m_strUserAccessToken = reader.ReadString();
            this.m_nUserRawId = reader.ReadUInt64();
            this.m_nExtraNormalDiamondItem = reader.ReadUInt16();
            this.m_nExtraLargeDiamondItem = reader.ReadUInt16();
            this.m_nGameSystemMask = reader.ReadInt64();
        }

        public void WriteToStream(BinaryWriter writer)
        {
            ushort length = (ushort) this.m_arrayEquipData.Length;
            writer.Write(length);
            for (ushort i = 0; i < length; i = (ushort) (i + 1))
            {
                this.m_arrayEquipData[i].WriteToStream(writer);
            }
            length = (ushort) this.m_arrayRestoreData.Length;
            writer.Write(length);
            for (ushort j = 0; j < length; j = (ushort) (j + 1))
            {
                this.m_arrayRestoreData[j].WriteToStream(writer);
            }
            length = (ushort) this.m_arrayTimestampData.Length;
            writer.Write(length);
            for (ushort k = 0; k < length; k = (ushort) (k + 1))
            {
                this.m_arrayTimestampData[k].WriteToStream(writer);
            }
            writer.Write(this.m_nTransID);
            writer.Write(this.m_nCoins);
            writer.Write(this.m_nDiamonds);
            writer.Write(this.m_nMaxLayer);
            writer.Write(this.m_nLayerBoxID);
            writer.Write(this.m_nLevel);
            writer.Write(this.m_nExperince);
            writer.Write(this.m_nTreasureRandomCount);
            writer.Write(this.m_nBattleRebornCount);
            writer.Write(this.m_strUserAccessToken);
            writer.Write(this.m_nUserRawId);
            writer.Write(this.m_nExtraNormalDiamondItem);
            writer.Write(this.m_nExtraLargeDiamondItem);
            writer.Write(this.m_nGameSystemMask);
        }

        public ushort GetMsgType =>
            8;
    }
}

