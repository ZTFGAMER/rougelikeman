namespace GameProtocol
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    [Serializable]
    public sealed class CLifeTransPacket : CProtocolBase
    {
        public uint m_nTransID;
        public ushort m_nMaterial;
        public byte m_nType;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nTransID = reader.ReadUInt32();
            this.m_nMaterial = reader.ReadUInt16();
            this.m_nType = reader.ReadByte();
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nTransID);
            writer.Write(this.m_nMaterial);
            writer.Write(this.m_nType);
        }

        [JsonIgnore]
        public override ushort GetMsgType =>
            13;

        public enum eLifeTransType
        {
            ETransSpendLife = 1,
            ETransDiamondToLife = 2,
            ETransCoinToPotion = 3,
            ETransDiamondToPotion = 4,
            ETransDiamondToRevival = 5,
            EInvalidType = 6
        }
    }
}

