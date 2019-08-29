namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CTruncateUser : CProtocolBase
    {
        protected override void OnReadFromStream(BinaryReader reader)
        {
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
        }

        public override ushort GetMsgType =>
            12;
    }
}

