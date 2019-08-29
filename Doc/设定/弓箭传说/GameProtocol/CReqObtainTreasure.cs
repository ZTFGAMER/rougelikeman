namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CReqObtainTreasure : CProtocolBase
    {
        public uint m_nTransID;
        public uint m_nCoin;
        public CEquipmentItem m_stTreasureItems;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nTransID = reader.ReadUInt32();
            this.m_nCoin = reader.ReadUInt16();
            this.m_stTreasureItems.ReadFromStream(reader);
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nTransID);
            writer.Write(this.m_nCoin);
            this.m_stTreasureItems.WriteToStream(writer);
        }

        public override ushort GetMsgType =>
            14;
    }
}

