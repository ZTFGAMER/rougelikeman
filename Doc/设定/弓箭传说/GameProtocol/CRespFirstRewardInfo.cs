namespace GameProtocol
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using TableTool;

    public class CRespFirstRewardInfo : IProtocol
    {
        public string m_strProductId;
        public string m_strRewardinfo;
        private string[] mInfoList;

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

        public List<Drop_DropModel.DropData> GetList()
        {
            List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
            if (this.IsValid)
            {
                if ((this.mInfoList == null) || (this.mInfoList.Length == 0))
                {
                    return list;
                }
                int index = 0;
                int length = this.mInfoList.Length;
                while (index < length)
                {
                    string str = this.mInfoList[index];
                    if (!string.IsNullOrEmpty(str))
                    {
                        char[] separator = new char[] { ',' };
                        string[] strArray = str.Split(separator);
                        if (strArray.Length == 3)
                        {
                            int result = 0;
                            int.TryParse(strArray[0], out result);
                            int num4 = 0;
                            int.TryParse(strArray[1], out num4);
                            int num5 = 0;
                            int.TryParse(strArray[2], out num5);
                            if (((result != 0) && (num4 != 0)) && (num5 != 0))
                            {
                                Drop_DropModel.DropData item = new Drop_DropModel.DropData {
                                    type = (PropType) result,
                                    id = num4,
                                    count = num5
                                };
                                list.Add(item);
                            }
                        }
                    }
                    index++;
                }
            }
            return list;
        }

        public void ReadFromStream(BinaryReader reader)
        {
            this.m_strProductId = reader.ReadString();
            this.m_strRewardinfo = reader.ReadString();
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_strProductId);
            writer.Write(this.m_strRewardinfo);
        }

        public ushort GetMsgType =>
            0x11;

        public bool IsValid
        {
            get
            {
                this.mInfoList = null;
                if (string.IsNullOrEmpty(this.m_strProductId))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(this.m_strRewardinfo))
                {
                    return false;
                }
                try
                {
                    JArray array = JArray.Parse(this.m_strRewardinfo);
                    if (array != null)
                    {
                        this.mInfoList = array.ToObject<string[]>();
                        if ((this.mInfoList == null) || (this.mInfoList.Length == 0))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
    }
}

