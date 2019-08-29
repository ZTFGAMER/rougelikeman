namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CReqSyncGameSystemMask : CProtocolBase
    {
        public CCommonRespMsg m_syncMsg;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_syncMsg.ReadFromStream(reader);
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            this.m_syncMsg.WriteToStream(writer);
        }

        public override ushort GetMsgType =>
            0x13;
    }
}

