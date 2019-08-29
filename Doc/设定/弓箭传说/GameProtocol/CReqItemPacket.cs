namespace GameProtocol
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    [Serializable]
    public sealed class CReqItemPacket : CProtocolBase
    {
        public uint m_nTransID;
        public ushort m_nPacketType;
        public ushort m_nFromType;
        public uint m_nExtraInfo;
        public uint m_nCoinAmount;
        public uint m_nDiamondAmount;
        public ushort m_nLife;
        public ushort m_nExperince;
        public CEquipmentItem[] arrayEquipItems;
        public ushort m_nNormalDiamondItem;
        public ushort m_nLargeDiamondItem;
        public ushort m_nRebornCount;

        protected override void OnReadFromStream(BinaryReader reader)
        {
            this.m_nTransID = reader.ReadUInt32();
            this.m_nPacketType = reader.ReadUInt16();
            this.m_nFromType = reader.ReadUInt16();
            this.m_nExtraInfo = reader.ReadUInt32();
            this.m_nCoinAmount = reader.ReadUInt32();
            this.m_nDiamondAmount = reader.ReadUInt32();
            this.m_nLife = reader.ReadUInt16();
            this.m_nExperince = reader.ReadUInt16();
            ushort num = reader.ReadUInt16();
            this.arrayEquipItems = new CEquipmentItem[num];
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.arrayEquipItems[i] = new CEquipmentItem();
                this.arrayEquipItems[i].ReadFromStream(reader);
            }
            this.m_nNormalDiamondItem = reader.ReadUInt16();
            this.m_nLargeDiamondItem = reader.ReadUInt16();
            this.m_nRebornCount = reader.ReadUInt16();
        }

        protected override void OnWriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nTransID);
            writer.Write(this.m_nPacketType);
            writer.Write(this.m_nFromType);
            writer.Write(this.m_nExtraInfo);
            writer.Write(this.m_nCoinAmount);
            writer.Write(this.m_nDiamondAmount);
            writer.Write(this.m_nLife);
            writer.Write(this.m_nExperince);
            ushort num = (this.arrayEquipItems != null) ? ((ushort) this.arrayEquipItems.Length) : ((ushort) 0);
            writer.Write(num);
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.arrayEquipItems[i].WriteToStream(writer);
            }
            writer.Write(this.m_nNormalDiamondItem);
            writer.Write(this.m_nLargeDiamondItem);
            writer.Write(this.m_nRebornCount);
        }

        [JsonIgnore]
        public override ushort GetMsgType =>
            7;

        public enum eItemType
        {
            EBattleType = 1,
            ETimeType = 2,
            ELevelType = 3,
            ELayerType = 4,
            EDiamondType = 5,
            EMailType = 6,
            EDiamondToCoinType = 7,
            EItemUpgrade = 8,
            EEquipItemTrans = 9,
            EDiamondToLifeTrans = 10,
            ECoinToPotionTrans = 11,
            EDiamondToPotionTrans = 12,
            EObtainTreasureTrans = 13,
            EBuyDiamondsFromShop = 14,
            EFirstRewardFromShop = 15,
            EEquipCompositeTrans = 0x10,
            EGameHarvestType = 0x11,
            EAdGetLifeType = 0x12,
            EAdGetLuckyType = 0x13,
            EInvalidType = 20
        }
    }
}

