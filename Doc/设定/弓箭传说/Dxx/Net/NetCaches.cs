namespace Dxx.Net
{
    using DG.Tweening;
    using Dxx.Util;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using UnityEngine;

    [Serializable]
    public class NetCaches : LocalSaveBase
    {
        public ulong serveruserid;
        public List<NetCacheOne> mList = new List<NetCacheOne>();
        [JsonIgnore]
        private const float mCheckDelay = 0.1f;
        [JsonIgnore]
        private float mTime = -1.1f;
        [JsonIgnore]
        private int mCount;
        [JsonIgnore]
        private NetCacheOne mCurrent;
        [JsonIgnore]
        private bool bSendLogin;
        [JsonIgnore]
        private float mSendLoginStartTime;
        [JsonIgnore]
        private bool bCurrentSendOver = true;

        public void Add(NetCacheOne data, bool reduce_count)
        {
            if (data.trycount <= 20)
            {
                if (reduce_count)
                {
                    data.trycount++;
                }
                ulong serverUserID = LocalSave.Instance.GetServerUserID();
                if (this.serveruserid != serverUserID)
                {
                    this.Clear();
                }
                else
                {
                    this.mList.Add(data);
                    base.Refresh();
                }
            }
        }

        private void Clear()
        {
            this.mList.Clear();
            ulong serverUserID = LocalSave.Instance.GetServerUserID();
            FileUtils.GetXml<NetCaches>(GetFileName(serverUserID)).Copy(this);
            this.serveruserid = serverUserID;
            base.Refresh();
        }

        public void Copy(NetCaches data)
        {
            data.mList = this.mList;
        }

        public static void DeleteFile(ulong serveruserid)
        {
            string fullPath = FileUtils.GetFullPath(GetFileName(serveruserid));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public static string GetFileName(ulong serveruserid)
        {
            object[] args = new object[] { serveruserid, "netcache.txt" };
            return Utils.FormatString("{0}-{1}", args);
        }

        public void Init()
        {
            ServicePointManager.DefaultConnectionLimit = 50;
            TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.2f), new TweenCallback(this, this.OnUpdate)), -1);
        }

        protected override void OnRefresh()
        {
            LocalSave.Instance.DoThreadSave(LocalSave.EThreadWriteType.eNet);
        }

        private void OnUpdate()
        {
            if (this.serveruserid == 0L)
            {
                this.serveruserid = LocalSave.Instance.GetServerUserID();
                NetManager.UpdateNetConnect();
            }
            else if ((this.serveruserid > 0L) && (this.serveruserid != LocalSave.Instance.GetServerUserID()))
            {
                this.Clear();
            }
            else
            {
                if (this.bSendLogin && ((Time.unscaledTime - this.mSendLoginStartTime) > 0.1f))
                {
                    if (this.mList.Count == 0)
                    {
                        LocalSave.Instance.DoLogin(SendType.eUDP, null);
                    }
                    this.bSendLogin = false;
                }
                if (!GameLogic.InGame && ((Time.unscaledTime - this.mTime) > 0.1f))
                {
                    NetManager.UpdateNetConnect();
                    if (NetManager.IsNetConnect && this.bCurrentSendOver)
                    {
                        this.mCount = this.mList.Count;
                        if (this.mCount > 0)
                        {
                            this.bCurrentSendOver = false;
                            this.mCurrent = this.mList[0];
                            this.mList.RemoveAt(0);
                            base.Refresh();
                            NetManager.SendInternal(this.mCurrent, delegate (NetResponse response) {
                                this.bCurrentSendOver = true;
                                if (this.mList.Count == 0)
                                {
                                    this.mSendLoginStartTime = Time.unscaledTime;
                                    this.bSendLogin = true;
                                }
                            });
                        }
                    }
                    this.mTime = Time.unscaledTime;
                }
            }
        }

        public void Remove(NetCacheOne data)
        {
            if (this.mList.Contains(data))
            {
                this.mList.Remove(data);
                if (this.IsEmpty)
                {
                    LocalSave.Instance.mEquip.check_invalid();
                }
                base.Refresh();
            }
        }

        [JsonIgnore]
        public bool IsEmpty =>
            (this.mList.Count == 0);
    }
}

