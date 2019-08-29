namespace GameProtocol
{
    using System;

    public sealed class MessageType
    {
        public const byte MSG_HEADER_TAG = 13;
        public const ushort MSG_REQ_BATTLE_RESULT_REQUEST = 1;
        public const ushort MSG_GAME_SETTLEMENT_RESPONSE = 2;
        public const ushort MSG_REQ_MAIL_LIST = 4;
        public const ushort MSG_RESP_MAIL_LIST = 5;
        public const ushort MSG_RESP_RETURN_MESSAGE = 6;
        public const ushort MSG_REQ_ITEM_PACKET = 7;
        public const ushort MSG_REQ_USER_LOGIN = 8;
        public const ushort MSG_REQ_ITEM_UPGRADE = 9;
        public const ushort MSG_REQ_DIAMOND_TO_COIN = 10;
        public const ushort MSG_REQ_EQUIP_TRANS = 11;
        public const ushort MSG_REQ_TRUNCATE_USER = 12;
        public const ushort MSG_REQ_LIFE_TRANS = 13;
        public const ushort MSG_REQ_OBTAIN_TREASURE = 14;
        public const ushort MSG_REQ_APP_IN_PURCHASE = 15;
        public const ushort MSG_QUERY_IN_APP_PURCHASE = 0x10;
        public const ushort MSG_QUERY_FIRST_REWARD_INFO = 0x11;
        public const ushort MSG_REQ_EQUIP_COMPOSITE_TRANS = 0x12;
        public const ushort MSG_SYNC_GAME_SYSTEM_MASK = 0x13;
    }
}

