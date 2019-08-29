namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CRespDimaonToCoin : IProtocol
    {
        public uint m_nCoins;
        public uint m_nDiamonds;

        public byte[] buildPacket()
        {
            BinaryWriter writer = ProtocolBuffer.Writer;
            writer.Write((byte) 13);
            writer.Write(this.GetMsgType);
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
            this.m_nCoins = reader.ReadUInt32();
            this.m_nDiamonds = reader.ReadUInt32();
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nCoins);
            writer.Write(this.m_nDiamonds);
        }

        public ushort GetMsgType =>
            10;
    }
}

