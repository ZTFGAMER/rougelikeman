namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CQueryIAPInfo : CProtocolBase
    {
        public ushort m_nPlatformIndex;
        public string m_strProductID;
        private byte[] m_arrSHA256;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nPlatformIndex = reader.ReadUInt16();
            this.m_strProductID = reader.ReadString();
            ushort count = reader.ReadUInt16();
            if (count > 0)
            {
                this.m_arrSHA256 = reader.ReadBytes(count);
            }
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nPlatformIndex);
            writer.Write(this.m_strProductID);
            writer.Write(this.m_arrSHA256.Length);
            writer.Write(this.m_arrSHA256);
        }

        public override ushort GetMsgType =>
            0x10;
    }
}

