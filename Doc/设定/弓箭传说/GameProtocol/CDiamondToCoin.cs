namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CDiamondToCoin : CProtocolBase
    {
        public uint m_nTransID;
        public uint m_nCoins;
        public uint m_nDiamonds;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nTransID = reader.ReadUInt32();
            this.m_nCoins = reader.ReadUInt32();
            this.m_nDiamonds = reader.ReadUInt32();
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nTransID);
            writer.Write(this.m_nCoins);
            writer.Write(this.m_nDiamonds);
        }

        public override ushort GetMsgType =>
            10;
    }
}

