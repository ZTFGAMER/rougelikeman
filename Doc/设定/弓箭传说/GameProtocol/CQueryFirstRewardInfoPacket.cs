namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CQueryFirstRewardInfoPacket : CProtocolBase
    {
        protected override void OnReadFromStream(BinaryReader reader)
        {
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
        }

        public override ushort GetMsgType =>
            0x11;
    }
}

