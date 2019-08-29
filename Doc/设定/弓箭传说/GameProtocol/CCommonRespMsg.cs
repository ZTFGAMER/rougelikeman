namespace GameProtocol
{
    using System;
    using System.IO;

    public sealed class CCommonRespMsg : IProtocol
    {
        public const ushort MsgType = 6;
        public const short MSG_RESPONSE_OK = 0;
        public const short MSG_RESP_DATA_EXIST = 1;
        public const short MSG_SELECT_DATA_FAILED = 2;
        public const short MSG_EXPIRED_LOGIN_INFO = 5;
        public const short MSG_USER_LOGIN_FAILED = 6;
        public const short MSG_ARG_INVALID = 7;
        public const short MSG_NOT_ENOUGH_CURRENCY = 8;
        public const short MSG_NOT_ENOUGH_EXP_UPGRADE = 9;
        public const short MSG_NOT_ENOUGH_MATERIAL = 10;
        public const short MSG_NOT_ENOUGH_LIFE = 11;
        public const short MSG_CLIENT_JSON_FORMAT_FAILED = 12;
        public const short MSG_IAP_VERIFY_SERVER_UNAVAILABLE = 13;
        public const short MSG_IAP_VERIFY_PRODUCT_ID_INVALID = 14;
        public const short MSG_IAP_VERIFY_FAILED = 15;
        public const short MSG_APP_VERSION_TOO_LOW = 0x10;
        public const short MSG_IOS_IAP_JSON_PARSE_FAILED = 0x5208;
        public const short MSG_IOS_IAP_RECEIPT_DATA_MALFORMED = 0x520a;
        public const short MSG_IOS_IAP_RECEIPT_AUTHENTICATED_FAILED1 = 0x520b;
        public const short MSG_IOS_IAP_SECRET_NOT_MATCH = 0x520c;
        public const short MSG_IOS_IAP_RECEIPT_SERVER_UNAVAILABLE = 0x520d;
        public const short MSG_IOS_IAP_RECEIPT_AUTHENTICATED_FAILED2 = 0x5212;
        public const short MSG_FORMAT_ERROR = -1;
        public const short MSG_REGEX_ERROR = -2;
        public const short MSG_THREADING_DATABASE_FAILED = -3;
        public const short MSG_STORE_RESULT_FAILED = -4;
        public const short MSG_BAD_LEXICAL_EXCEPTION = -5;
        public const short MSG_COLUMN_UNEXPECTED = -6;
        public const short MSG_SQL_FETCH_FAILED = -7;
        public const short MSG_SQL_QUERY_FAILED = -8;
        public const short MSG_COMMIT_DATA_FAILED = -9;
        public const short MSG_INTERNAL_ARG_FAILED = -10;
        public const short MSG_WRONG_NUM_COL_FIELDS = -11;
        public const short MSG_INSERT_DATA_FAILED = -12;
        public const short MSG_UPDATE_DATA_FAILED = -13;
        public const short MSG_DELETE_DATA_FAILED = -14;
        public const short MSG_IAP_REQUEST_FAILED = -15;
        public const short MSG_BASE64_ENCODE_FAILED = -16;
        public const short MSG_INTERNAL_JSON_LOADS_FAILED = -17;
        public const short MSG_PARSE_STRING_VALUE_FAILED = -18;
        public ushort m_nStatusCode;
        public string m_strInfo;

        public byte[] buildPacket()
        {
            BinaryWriter writer = ProtocolBuffer.Writer;
            writer.Write((byte) 13);
            writer.Write((ushort) 6);
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer2 = new CustomBinaryWriter(stream);
            this.WriteToStream(writer2);
            ushort length = (ushort) stream.ToArray().Length;
            writer.Write(length);
            writer.Write(stream.ToArray());
            return ProtocolBuffer.CacheStream.ToArray();
        }

        public string dump()
        {
            object[] objArray1 = new object[] { "MsgType: ", this.GetMsgType, "\r\nm_nStatusCode: ", this.m_nStatusCode };
            return string.Concat(objArray1);
        }

        public void ReadFromStream(BinaryReader reader)
        {
            this.m_nStatusCode = reader.ReadUInt16();
            this.m_strInfo = reader.ReadString();
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(this.m_nStatusCode);
            writer.Write(this.m_strInfo);
        }

        public ushort GetMsgType =>
            6;
    }
}

