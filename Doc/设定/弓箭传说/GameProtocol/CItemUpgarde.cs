namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CItemUpgarde : CProtocolBase
    {
        public uint m_nTransID;
        public ulong m_nRowID;
        public uint m_nCoins;
        public uint m_nDiamonds;
        public CMaterialItem[] arrayItems;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nTransID = reader.ReadUInt32();
            this.m_nRowID = reader.ReadUInt64();
            this.m_nCoins = reader.ReadUInt32();
            this.m_nDiamonds = reader.ReadUInt32();
            ushort num = reader.ReadUInt16();
            this.arrayItems = new CMaterialItem[num];
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.arrayItems[i] = new CMaterialItem();
                this.arrayItems[i].ReadFromStream(reader);
            }
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nTransID);
            writer.Write(this.m_nRowID);
            writer.Write(this.m_nCoins);
            writer.Write(this.m_nDiamonds);
            ushort length = (ushort) this.arrayItems.Length;
            writer.Write(length);
            for (ushort i = 0; i < length; i = (ushort) (i + 1))
            {
                this.arrayItems[i].WriteToStream(writer);
            }
        }

        public override ushort GetMsgType =>
            9;
    }
}

