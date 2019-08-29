namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CMaterialItem : IProtocol
    {
        public const ushort MsgType = 0xffff;
        public uint m_nEquipID;
        public uint m_nMaterial;

        public byte[] buildPacket() => 
            null;

        public void ReadFromStream(BinaryReader reader)
        {
            this.m_nEquipID = reader.ReadUInt32();
            this.m_nMaterial = reader.ReadUInt32();
        }

        public void WriteToStream(BinaryWriter writter)
        {
            writter.Write(this.m_nEquipID);
            writter.Write(this.m_nMaterial);
        }

        public ushort GetMsgType =>
            0xffff;
    }
}

