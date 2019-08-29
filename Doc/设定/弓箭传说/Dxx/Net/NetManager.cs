namespace Dxx.Net
{
    using Dxx.Util;
    using GameProtocol;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PureMVC.Patterns;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using TableTool;
    using UnityEngine;

    public class NetManager
    {
        private static bool _netconnect = true;
        private static bool _netcurrent;
        private static long _nettime = 0L;
        private static long _localtime = 0L;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static float <unitytime>k__BackingField = 0f;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static int <PingCount>k__BackingField;
        public const int TimeOut = 10;
        private static List<NetCacheOne> mTemps = new List<NetCacheOne>();
        private static NetCaches _NetCache = null;

        private static void check(ulong serveruserid)
        {
            string xmlFileString = FileUtils.GetXmlFileString(NetCaches.GetFileName(serveruserid));
            if (!string.IsNullOrEmpty(xmlFileString))
            {
                try
                {
                    JObject obj2 = JObject.Parse(xmlFileString);
                    JArray array = null;
                    array = JArray.Parse(obj2.get_Item("mList").ToString());
                    int num = 0;
                    int num2 = array.get_Count();
                    while (num < num2)
                    {
                        JObject obj3 = JObject.Parse(array.get_Item(num).ToString());
                        JObject obj4 = JObject.Parse(obj3.get_Item("data").ToString());
                        NetCacheOne item = new NetCacheOne {
                            trycount = 0,
                            sendcode = obj3.get_Item("sendcode").ToObject<ushort>()
                        };
                        bool flag = true;
                        if (item.sendcode == 7)
                        {
                            try
                            {
                                CReqItemPacket packet = JsonConvert.DeserializeObject<CReqItemPacket>(obj4.ToString());
                                item.data = packet;
                                flag = true;
                            }
                            catch
                            {
                                flag = false;
                                SdkManager.Bugly_Report("NetManager_Cache", Utils.FormatString("GameOver DeserializeObject failed.", Array.Empty<object>()));
                            }
                        }
                        else if (item.sendcode == 13)
                        {
                            try
                            {
                                CLifeTransPacket packet2 = JsonConvert.DeserializeObject<CLifeTransPacket>(obj4.ToString());
                                item.data = packet2;
                                flag = true;
                            }
                            catch
                            {
                                flag = false;
                                SdkManager.Bugly_Report("NetManager_Cache", Utils.FormatString("key DeserializeObject failed.", Array.Empty<object>()));
                            }
                        }
                        else if (item.sendcode == 15)
                        {
                            try
                            {
                                CInAppPurchase purchase = JsonConvert.DeserializeObject<CInAppPurchase>(obj4.ToString());
                                item.data = purchase;
                                flag = true;
                            }
                            catch
                            {
                                flag = false;
                                SdkManager.Bugly_Report("NetManager_Cache", Utils.FormatString("iap DeserializeObject failed.", Array.Empty<object>()));
                            }
                        }
                        if (flag)
                        {
                            mTemps.Add(item);
                        }
                        num++;
                    }
                }
                catch
                {
                }
            }
        }

        public static CReqItemPacket GetItemPacket(List<Drop_DropModel.DropData> list, bool addequipmust = false)
        {
            List<Drop_DropModel.DropData> dropEquips = LocalModelManager.Instance.Drop_Drop.GetDropEquips(list);
            CReqItemPacket packet = new CReqItemPacket {
                m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
                m_nCoinAmount = (uint) LocalModelManager.Instance.Drop_Drop.GetDropGold(list),
                m_nDiamondAmount = (uint) LocalModelManager.Instance.Drop_Drop.GetDropDiamond(list),
                m_nLife = (ushort) LocalModelManager.Instance.Drop_Drop.GetDropKey(list),
                m_nExperince = (ushort) LocalModelManager.Instance.Drop_Drop.GetDropExp(list),
                m_nNormalDiamondItem = (ushort) LocalModelManager.Instance.Drop_Drop.GetDropDiamondBoxNormal(list),
                m_nLargeDiamondItem = (ushort) LocalModelManager.Instance.Drop_Drop.GetDropDiamondBoxLarge(list),
                arrayEquipItems = new CEquipmentItem[dropEquips.Count]
            };
            int index = 0;
            int count = dropEquips.Count;
            while (index < count)
            {
                packet.arrayEquipItems[index] = new CEquipmentItem();
                packet.arrayEquipItems[index].m_nUniqueID = Utils.GenerateUUID();
                packet.arrayEquipItems[index].m_nEquipID = (uint) dropEquips[index].id;
                packet.arrayEquipItems[index].m_nLevel = 1;
                packet.arrayEquipItems[index].m_nFragment = (uint) dropEquips[index].count;
                if (addequipmust)
                {
                    LocalSave.Instance.AddProp(packet.arrayEquipItems[index]);
                }
                index++;
            }
            return packet;
        }

        public static void RefreshUnityTime()
        {
            unitytime = Time.realtimeSinceStartup;
        }

        public static void SendInternal(NetCacheOne senddata, Action<NetResponse> callback)
        {
            GameNode.m_Net.AddComponent<HTTPSendClient>().StartSend(senddata, callback);
        }

        public static void SendInternal<T1>(T1 packet, SendType sendtype, Action<NetResponse> callback) where T1: CProtocolBase
        {
            GameNode.m_Net.AddComponent<HTTPSendClient>().StartSend<T1>(packet, sendtype, callback);
        }

        public static void SendInternal<T1>(T1 packet, SendType sendtype, int count, int time, Action<NetResponse> callback) where T1: CProtocolBase
        {
            GameNode.m_Net.AddComponent<HTTPSendClient>().StartSend<T1>(packet, sendtype, count, time, callback);
        }

        public static void SetNetTime(long time)
        {
            _nettime = time;
            RefreshUnityTime();
        }

        public static void StartPing()
        {
        }

        public static void UpdateNetConnect()
        {
            _netcurrent = IsNetConnect;
            if (_netcurrent != _netconnect)
            {
                _netconnect = _netcurrent;
                if (IsNetConnect)
                {
                    NetCaches.DeleteFile(0L);
                    LocalSave.Instance.DoLogin(SendType.eLoop, null);
                }
                Facade.Instance.SendNotification("PUB_NETCONNECT_UPDATE");
            }
        }

        public static bool IsNetConnect =>
            (Application.internetReachability != NetworkReachability.NotReachable);

        public static long NetTime =>
            _nettime;

        public static long LocalTime
        {
            get
            {
                if (_localtime == 0L)
                {
                    _localtime = Utils.GetLocalTime();
                }
                return _localtime;
            }
        }

        public static float unitytime
        {
            [CompilerGenerated]
            get => 
                <unitytime>k__BackingField;
            [CompilerGenerated]
            private set => 
                (<unitytime>k__BackingField = value);
        }

        public static int PingCount
        {
            [CompilerGenerated]
            get => 
                <PingCount>k__BackingField;
            [CompilerGenerated]
            private set => 
                (<PingCount>k__BackingField = value);
        }

        public static NetCaches mNetCache
        {
            get
            {
                if (_NetCache == null)
                {
                    check(0L);
                    NetCaches.DeleteFile(0L);
                    if (LocalSave.Instance.GetServerUserID() != 0L)
                    {
                        check(LocalSave.Instance.GetServerUserID());
                    }
                    _NetCache = new NetCaches();
                    _NetCache.serveruserid = LocalSave.Instance.GetServerUserID();
                    _NetCache.mList = mTemps;
                    _NetCache.Refresh();
                }
                return _NetCache;
            }
        }
    }
}

