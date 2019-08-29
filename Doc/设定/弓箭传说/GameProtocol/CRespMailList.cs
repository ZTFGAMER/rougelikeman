namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CRespMailList : IProtocol
    {
        public const ushort MsgType = 5;
        public CMailInfo[] mailList;

        public byte[] buildPacket()
        {
            BinaryWriter writer = ProtocolBuffer.Writer;
            writer.Write((byte) 13);
            writer.Write((ushort) 5);
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer2 = new CustomBinaryWriter(stream);
            this.WriteToStream(writer2);
            ushort length = (ushort) stream.ToArray().Length;
            writer.Write(length);
            writer.Write(stream.ToArray());
            return ProtocolBuffer.CacheStream.ToArray();
        }

        public void ReadFromStream(BinaryReader reader)
        {
            ushort num = reader.ReadUInt16();
            this.mailList = new CMailInfo[num];
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.mailList[i] = new CMailInfo();
                this.mailList[i].ReadFromStream(reader);
            }
        }

        public void WriteToStream(BinaryWriter writer)
        {
            ushort num = (this.mailList != null) ? ((ushort) this.mailList.Length) : ((ushort) 0);
            writer.Write(num);
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.mailList[i].WriteToStream(writer);
            }
        }

        public ushort GetMsgType =>
            5;
    }
}

