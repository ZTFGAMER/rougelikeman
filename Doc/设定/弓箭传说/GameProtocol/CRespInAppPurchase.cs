namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CRespInAppPurchase : IProtocol
    {
        public string m_strIAPTransID;
        public uint m_nTotalCoins;
        public uint m_nTotalDiamonds;
        public ushort m_nBattleRebornCount;
        public ushort m_nNormalDiamondItems;
        public ushort m_nLargeDiamondItems;
        public CEquipmentItem[] m_arrEquipInfo;
        public string product_id;

        public byte[] buildPacket()
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

        public void ReadFromStream(BinaryReader reader)
        {
            this.m_strIAPTransID = reader.ReadString();
            this.m_nTotalCoins = reader.ReadUInt32();
            this.m_nTotalDiamonds = reader.ReadUInt32();
            this.m_nBattleRebornCount = reader.ReadUInt16();
            this.m_nNormalDiamondItems = reader.ReadUInt16();
            this.m_nLargeDiamondItems = reader.ReadUInt16();
            ushort num = reader.ReadUInt16();
            this.m_arrEquipInfo = new CEquipmentItem[num];
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.m_arrEquipInfo[i] = new CEquipmentItem();
                this.m_arrEquipInfo[i].ReadFromStream(reader);
            }
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_strIAPTransID);
            writer.Write(this.m_nTotalCoins);
            writer.Write(this.m_nTotalDiamonds);
            writer.Write(this.m_nBattleRebornCount);
            writer.Write(this.m_nNormalDiamondItems);
            writer.Write(this.m_nLargeDiamondItems);
            writer.Write((ushort) this.m_arrEquipInfo.Length);
            for (ushort i = 0; i < ((ushort) this.m_arrEquipInfo.Length); i = (ushort) (i + 1))
            {
                this.m_arrEquipInfo[i].WriteToStream(writer);
            }
        }

        public ushort GetMsgType =>
            15;
    }
}

