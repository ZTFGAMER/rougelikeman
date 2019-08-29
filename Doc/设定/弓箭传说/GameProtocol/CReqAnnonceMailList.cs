namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CReqAnnonceMailList : CProtocolBase
    {
        public uint m_nLastMailID;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nLastMailID = reader.ReadUInt32();
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nLastMailID);
        }

        public override ushort GetMsgType =>
            4;
    }
}

