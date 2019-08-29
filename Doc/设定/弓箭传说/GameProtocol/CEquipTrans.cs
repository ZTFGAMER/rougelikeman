namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CEquipTrans : CProtocolBase
    {
        public uint m_nTransID;
        public CEquipmentItem m_stEquipItem;
        public ushort m_nCoins;
        public ushort m_nDiamonds;
        public byte m_nType;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nTransID = reader.ReadUInt32();
            this.m_stEquipItem = new CEquipmentItem();
            this.m_stEquipItem.ReadFromStream(reader);
            this.m_nCoins = reader.ReadUInt16();
            this.m_nDiamonds = reader.ReadUInt16();
            this.m_nType = reader.ReadByte();
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nTransID);
            this.m_stEquipItem.WriteToStream(writer);
            writer.Write(this.m_nCoins);
            writer.Write(this.m_nDiamonds);
            writer.Write(this.m_nType);
        }

        public override ushort GetMsgType =>
            11;

        public enum eEquipTransType
        {
            ETransBuyType = 1,
            ETransSellType = 2,
            ETransInvalidType = 3
        }
    }
}

