namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CEquipCompositeTrans : CProtocolBase
    {
        public uint m_nTransID;
        public CEquipmentItem[] m_arrCompositeInfo;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nTransID = reader.ReadUInt32();
            ushort num = reader.ReadUInt16();
            this.m_arrCompositeInfo = new CEquipmentItem[num];
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.m_arrCompositeInfo[i] = new CEquipmentItem();
                this.m_arrCompositeInfo[i].ReadFromStream(reader);
            }
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nTransID);
            writer.Write((ushort) this.m_arrCompositeInfo.Length);
            for (ushort i = 0; i < ((ushort) this.m_arrCompositeInfo.Length); i = (ushort) (i + 1))
            {
                this.m_arrCompositeInfo[i].WriteToStream(writer);
            }
        }

        public override ushort GetMsgType =>
            0x12;
    }
}

