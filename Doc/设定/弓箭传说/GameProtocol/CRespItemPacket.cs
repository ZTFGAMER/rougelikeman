namespace GameProtocol
{
    using Dxx.Util;
    using System;
    using System.IO;

    public sealed class CRespItemPacket : IProtocol
    {
        public const ushort MsgType = 7;
        public CCommonRespMsg m_commMsg;
        public CEquipmentItem[] m_arrayEquipItems;

        public byte[] buildPacket()
        {
            BinaryWriter writer = ProtocolBuffer.Writer;
            writer.Write((byte) 13);
            writer.Write((ushort) 7);
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
            try
            {
                this.m_commMsg = new CCommonRespMsg();
                this.m_commMsg.ReadFromStream(reader);
                ushort num = reader.ReadUInt16();
                this.m_arrayEquipItems = new CEquipmentItem[num];
                for (ushort i = 0; i < num; i = (ushort) (i + 1))
                {
                    this.m_arrayEquipItems[i] = new CEquipmentItem();
                    this.m_arrayEquipItems[i].ReadFromStream(reader);
                }
                LocalSave.Instance.SetEquips(this);
            }
            catch
            {
                Debugger.Log("!!!!!!!!!!!!!!!!!!!! resp equips error");
                SdkManager.Bugly_Report("CRespItemPacket", Utils.FormatString("ReadFromStream error", Array.Empty<object>()));
            }
        }

        public void WriteToStream(BinaryWriter writer)
        {
            this.m_commMsg.WriteToStream(writer);
            ushort num = (this.m_arrayEquipItems != null) ? ((ushort) this.m_arrayEquipItems.Length) : ((ushort) 0);
            writer.Write(num);
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.m_arrayEquipItems[i].WriteToStream(writer);
            }
        }

        public ushort GetMsgType =>
            7;
    }
}

