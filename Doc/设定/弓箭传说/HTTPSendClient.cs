using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPSendClient : MonoBehaviour
{
    private const string SHA_KEY = "80E232B550B746E8B6CD0894C03913B519606";
    public ushort sendcode;
    public SendType mSendType;
    private int sendcount = 10;
    private Dictionary<int, UnityWebRequest> uwrlist = new Dictionary<int, UnityWebRequest>();
    private Dictionary<int, float> starttimes = new Dictionary<int, float>();
    private float starttime;
    private bool bShowMask;
    private float mStartTime;
    private byte[] receive;
    private int sendlastcount = 1;
    private int timeout = 10;
    private int count = 2;
    private string mIP = string.Empty;
    private List<byte> sha_list = new List<byte>();

    private void CacheError(NetCacheOne data, bool reduce_count)
    {
        if (this.IsCache)
        {
            NetManager.mNetCache.Add(data, reduce_count);
        }
    }

    private bool check_done(int index)
    {
        if (this.isTimeOut(index))
        {
            return true;
        }
        UnityWebRequest request = null;
        return ((this.uwrlist.TryGetValue(index, out request) && (request != null)) && request.get_isDone());
    }

    private IProtocol CreateProtocol(ushort code, CustomBinaryReader reader)
    {
        switch (code)
        {
            case 4:
                return new CRespMailList();

            case 7:
            case 11:
            case 0x12:
                return new CRespItemPacket();

            case 8:
                return new CRespUserLoginPacket();

            case 10:
                return new CRespDimaonToCoin();

            case 15:
                return new CRespInAppPurchase();

            case 0x11:
                return new CRespFirstRewardInfo();
        }
        return null;
    }

    private void DeInit()
    {
        this.DeInitBefore();
        Object.Destroy(this);
    }

    private void DeInitBefore()
    {
        if (this.IsForce && this.bShowMask)
        {
            this.bShowMask = false;
            WindowUI.ShowMask(false);
            WindowUI.ShowNetDoing(false, NetDoingType.netdoing_http);
        }
        base.StopAllCoroutines();
        Dictionary<int, UnityWebRequest>.Enumerator enumerator = this.uwrlist.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, UnityWebRequest> current = enumerator.Current;
            this.KillRequest(current.Value);
        }
        this.uwrlist.Clear();
    }

    private void DoResponse(ushort code, byte[] postbytes, IProtocol data)
    {
        object[] args = new object[] { code, data.GetType().FullName };
        Debugger.Log(Utils.FormatString("静默处理 code:{0} class:{1}", args));
        switch (code)
        {
            case 7:
            {
                Debugger.Log("宝箱开启缓存请求 静默处理");
                CustomBinaryReader reader = new CustomBinaryReader(new MemoryStream(postbytes));
                byte num = reader.ReadByte();
                ushort num2 = reader.ReadUInt16();
                ushort num3 = reader.ReadUInt16();
                if (num2 == code)
                {
                    IProtocol protocol = this.CreateProtocol(code, reader);
                    if (protocol != null)
                    {
                        protocol.ReadFromStream(reader);
                        CReqItemPacket packet = protocol as CReqItemPacket;
                        object[] objArray2 = new object[] { packet.m_nCoinAmount, packet.m_nExperince, packet.arrayEquipItems.Length };
                        Debugger.Log(Utils.FormatString("金币:{0} 经验:{1} 装备数量:{2}", objArray2));
                        for (int i = 0; i < packet.arrayEquipItems.Length; i++)
                        {
                            object[] objArray3 = new object[] { i, packet.arrayEquipItems[i].m_nEquipID, packet.arrayEquipItems[i].m_nFragment };
                            Debugger.Log(Utils.FormatString("装备[{0}] id:{1} count:{2}", objArray3));
                        }
                    }
                }
                break;
            }
        }
    }

    private float get_starttime(int index)
    {
        float num = 0f;
        if (this.starttimes.TryGetValue(index, out num))
        {
            return num;
        }
        return this.set_starttime(index);
    }

    private float get_timeout(int index)
    {
        UnityWebRequest request = null;
        if (this.uwrlist.TryGetValue(index, out request))
        {
            return (float) request.get_timeout();
        }
        return 0f;
    }

    private string GetSHA256(long time, byte[] body)
    {
        this.sha_list.Clear();
        this.sha_list.AddRange(Encoding.Default.GetBytes("80E232B550B746E8B6CD0894C03913B519606"));
        this.sha_list.AddRange(Encoding.Default.GetBytes(time.ToString()));
        this.sha_list.AddRange(body);
        byte[] buffer = SHA256.Create().ComputeHash(this.sha_list.ToArray());
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < buffer.Length; i++)
        {
            builder.Append(buffer[i].ToString("X2"));
        }
        return builder.ToString();
    }

    private bool isTimeOut(int index) => 
        ((Time.realtimeSinceStartup - this.get_starttime(index)) >= this.get_timeout(index));

    private void KillRequest(UnityWebRequest request)
    {
        try
        {
            if (request != null)
            {
                request.Abort();
                request.Dispose();
                request = null;
            }
        }
        catch
        {
        }
    }

    [DebuggerHidden]
    private IEnumerator send_delay(int delaytime, NetCacheOne senddata, int index, Action<NetResponse> callback) => 
        new <send_delay>c__Iterator0 { 
            delaytime = delaytime,
            senddata = senddata,
            index = index,
            callback = callback,
            $this = this
        };

    [DebuggerHidden]
    private IEnumerator sendInternal(NetCacheOne senddata, int index, Action<NetResponse> callback) => 
        new <sendInternal>c__Iterator1 { 
            senddata = senddata,
            callback = callback,
            index = index,
            $this = this
        };

    private float set_starttime(int index)
    {
        if (!this.starttimes.ContainsKey(index))
        {
            this.starttimes.Add(index, 0f);
        }
        this.starttimes[index] = Time.realtimeSinceStartup;
        return this.starttimes[index];
    }

    private void start_send(NetCacheOne senddata, Action<NetResponse> callback)
    {
        for (int i = 0; i < this.count; i++)
        {
            base.StartCoroutine(this.send_delay(i * this.timeout, senddata, i, callback));
        }
    }

    public void StartSend(NetCacheOne senddata, Action<NetResponse> callback)
    {
        this.mStartTime = Time.realtimeSinceStartup;
        this.mSendType = SendType.eCache;
        this.start_send(senddata, callback);
    }

    public void StartSend<T1>(T1 packet, SendType sendtype, Action<NetResponse> callback) where T1: CProtocolBase
    {
        this.StartSend<T1>(packet, sendtype, 2, 10, callback);
    }

    public void StartSend<T1>(T1 packet, SendType sendtype, int count, int time, Action<NetResponse> callback) where T1: CProtocolBase
    {
        this.mStartTime = Time.realtimeSinceStartup;
        this.mSendType = sendtype;
        this.timeout = time;
        this.count = count;
        NetCacheOne senddata = new NetCacheOne {
            sendcode = packet.GetMsgType,
            data = packet
        };
        this.start_send(senddata, callback);
    }

    private bool IsForce =>
        (((this.mSendType == SendType.eCacheForce) || (this.mSendType == SendType.eForceLoop)) || (this.mSendType == SendType.eForceOnce));

    private bool IsCache =>
        ((this.mSendType == SendType.eCache) || (this.mSendType == SendType.eCacheForce));

    private bool IsLoop =>
        ((this.mSendType == SendType.eForceLoop) || (this.mSendType == SendType.eLoop));

    [CompilerGenerated]
    private sealed class <send_delay>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int delaytime;
        internal NetCacheOne senddata;
        internal int index;
        internal Action<NetResponse> callback;
        internal HTTPSendClient $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSecondsRealtime((float) this.delaytime);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.StartCoroutine(this.$this.sendInternal(this.senddata, this.index, this.callback));
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <sendInternal>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal NetCacheOne senddata;
        internal Action<NetResponse> callback;
        internal byte[] <postbytes>__0;
        internal UnityWebRequest <_uwr>__0;
        internal int index;
        internal long <time>__1;
        internal CustomBinaryReader <reader>__1;
        internal byte <headerTag>__1;
        internal HTTPSendClient $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (NetManager.IsNetConnect)
                    {
                        if (this.$this.IsForce && !this.$this.bShowMask)
                        {
                            this.$this.bShowMask = true;
                            WindowUI.ShowMask(true);
                            WindowUI.ShowNetDoing(true, NetDoingType.netdoing_http);
                        }
                        if (this.$this.sendcount < 0)
                        {
                            if (this.$this.IsForce)
                            {
                                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                            }
                            if (this.callback != null)
                            {
                                this.callback(new NetResponse());
                            }
                            this.$this.CacheError(this.senddata, true);
                            this.$this.DeInit();
                            goto Label_0B19;
                        }
                        this.$this.sendcode = this.senddata.sendcode;
                        this.<postbytes>__0 = this.senddata.data.buildPacket();
                        this.$this.starttime = Time.realtimeSinceStartup;
                        this.<_uwr>__0 = null;
                        if (!this.$this.uwrlist.TryGetValue(this.index, out this.<_uwr>__0))
                        {
                            this.<_uwr>__0 = UnityWebRequest.Put(NetConfig.GetPath(this.$this.sendcode, this.$this.mIP), this.<postbytes>__0);
                            this.$this.uwrlist.Add(this.index, this.<_uwr>__0);
                        }
                        else
                        {
                            this.$this.KillRequest(this.<_uwr>__0);
                            this.<_uwr>__0 = UnityWebRequest.Put(NetConfig.GetPath(this.$this.sendcode, this.$this.mIP), this.<postbytes>__0);
                            this.$this.uwrlist[this.index] = this.<_uwr>__0;
                        }
                        this.<_uwr>__0.set_timeout((this.$this.count - this.index) * this.$this.timeout);
                        this.<_uwr>__0.set_method("PUT");
                        this.<_uwr>__0.set_certificateHandler(new HTTPSendClient.CArcherSSLCertVerify());
                        this.$this.starttime = Time.realtimeSinceStartup;
                        this.$this.set_starttime(this.index);
                        this.<time>__1 = Utils.GetTimeStamp();
                        this.<_uwr>__0.SetRequestHeader("HabbyTime", this.<time>__1.ToString());
                        this.<_uwr>__0.SetRequestHeader("HabbyCheck", this.$this.GetSHA256(this.<time>__1, this.<postbytes>__0));
                        if (!string.IsNullOrEmpty(this.$this.mIP))
                        {
                            this.<_uwr>__0.SetRequestHeader("host", this.$this.mIP);
                        }
                        this.<_uwr>__0.SendWebRequest();
                        break;
                    }
                    Debugger.Log(string.Concat(new object[] { "无网络 ", this.senddata.sendcode, " cache ", this.$this.IsCache }));
                    if (this.$this.IsForce)
                    {
                        CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                    }
                    if (this.callback != null)
                    {
                        this.callback(new NetResponse());
                    }
                    this.$this.CacheError(this.senddata, false);
                    this.$this.DeInit();
                    goto Label_0B19;

                case 1:
                    break;

                default:
                    goto Label_0B19;
            }
            while (!this.$this.check_done(this.index))
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            if (this.$this.isTimeOut(this.index) || (this.<_uwr>__0.get_responseCode() != 200L))
            {
                if (this.$this.isTimeOut(this.index))
                {
                    Debugger.Log(Debugger.Tag.eHTTP, string.Concat(new object[] { "超时 sendcode = ", this.$this.sendcode, " response ", this.<_uwr>__0.get_responseCode(), " isdone ", this.<_uwr>__0.get_isDone() }));
                }
                else
                {
                    Debugger.Log(Debugger.Tag.eHTTP, string.Concat(new object[] { "返回数据错误 sendcode = ", this.$this.sendcode, " response ", this.<_uwr>__0.get_responseCode(), " isdone ", this.<_uwr>__0.get_isDone() }));
                }
                if (this.$this.IsLoop || !this.$this.isTimeOut(this.index))
                {
                    this.$this.mIP = NetConfig.RandomIP();
                    this.$this.DeInitBefore();
                    this.$this.StartCoroutine(this.$this.sendInternal(this.senddata, this.index, this.callback));
                }
                else
                {
                    if (this.$this.IsForce && this.$this.isTimeOut(this.index))
                    {
                        object[] args = new object[] { this.$this.sendcode };
                        SdkManager.send_event_http(Utils.FormatString("send timeout sendcode : {0}", args));
                        CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                    }
                    if (this.callback != null)
                    {
                        this.callback(new NetResponse());
                    }
                    this.$this.CacheError(this.senddata, false);
                    this.$this.DeInit();
                }
            }
            else
            {
                this.$this.receive = this.<_uwr>__0.get_downloadHandler().get_data();
                if (this.<_uwr>__0.GetResponseHeader("Habby") == "archero_zip")
                {
                    this.$this.receive = GZipHelper.Depress(this.$this.receive);
                }
                this.<reader>__1 = new CustomBinaryReader(new MemoryStream(this.$this.receive));
                this.<headerTag>__1 = this.<reader>__1.ReadByte();
                if (this.<headerTag>__1 == 13)
                {
                    ushort code = this.<reader>__1.ReadUInt16();
                    ushort num3 = this.<reader>__1.ReadUInt16();
                    if (code == this.senddata.sendcode)
                    {
                        NetResponse response = new NetResponse {
                            data = this.$this.CreateProtocol(code, this.<reader>__1)
                        };
                        if (response.data != null)
                        {
                            try
                            {
                                response.data.ReadFromStream(this.<reader>__1);
                            }
                            catch
                            {
                                object[] objArray5 = new object[] { code };
                                SdkManager.Bugly_Report("HttpSendClient", Utils.FormatString("read {0} stream error", objArray5));
                                response.data = null;
                                object[] objArray6 = new object[] { code };
                                SdkManager.send_event_http(Utils.FormatString("readfromstream error code : {0}", objArray6));
                            }
                            try
                            {
                                if (this.callback != null)
                                {
                                    this.callback(response);
                                }
                                else
                                {
                                    this.$this.DoResponse(code, this.<postbytes>__0, response.data);
                                }
                            }
                            catch
                            {
                                object[] objArray7 = new object[] { code };
                                SdkManager.send_event_http(Utils.FormatString("readfromstream success but callback error code : {0}", objArray7));
                                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                            }
                            goto Label_0B07;
                        }
                        object[] args = new object[] { this.senddata.sendcode };
                        object[] objArray9 = new object[] { code };
                        SdkManager.Bugly_Report(Utils.FormatString("HTTPSendClient.SendCode:{0} ", args), Utils.FormatString("DoReader code;{0} dont deal.", objArray9));
                        object[] objArray10 = new object[] { code };
                        SdkManager.send_event_http(Utils.FormatString("DoReader code : {0} dont deal.", objArray10));
                        if (this.callback != null)
                        {
                            this.callback(new NetResponse());
                        }
                        this.$this.CacheError(this.senddata, true);
                        this.$this.DeInit();
                    }
                    else if (code == 6)
                    {
                        CCommonRespMsg msg = new CCommonRespMsg();
                        try
                        {
                            msg.ReadFromStream(this.<reader>__1);
                        }
                        catch
                        {
                            object[] objArray11 = new object[] { code };
                            SdkManager.Bugly_Report("HttpSendClient", Utils.FormatString("MSG_RESP_RETURN_MESSAGE read {0} stream error", objArray11));
                            object[] objArray12 = new object[] { code };
                            SdkManager.send_event_http(Utils.FormatString("MSG_RESP_RETURN_MESSAGE read {0} steam error", objArray12));
                        }
                        NetResponse response2 = new NetResponse {
                            error = msg
                        };
                        try
                        {
                            if (this.callback != null)
                            {
                                this.callback(response2);
                            }
                        }
                        catch
                        {
                            object[] objArray13 = new object[] { code };
                            string step = Utils.FormatString("MSG_RESP_RETURN_MESSAGE callback error code : {0}", objArray13);
                            if (msg != null)
                            {
                                object[] objArray14 = new object[] { msg.m_nStatusCode, msg.m_strInfo };
                                step = step + Utils.FormatString(" resp code : {0} info : {1}", objArray14);
                            }
                            SdkManager.send_event_http(step);
                            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                        }
                        if ((msg.m_nStatusCode == 0) || (msg.m_nStatusCode == 1))
                        {
                            goto Label_0B07;
                        }
                        bool flag = true;
                        if (msg.m_nStatusCode < 0)
                        {
                            flag = false;
                            if (!GameLogic.InGame)
                            {
                                object[] objArray15 = new object[] { this.$this.sendcode, msg.m_nStatusCode };
                                CInstance<TipsUIManager>.Instance.Show(Utils.FormatString("发送：{0} 通用信息错误：{1}", objArray15));
                            }
                        }
                        object[] args = new object[] { this.senddata.sendcode };
                        object[] objArray17 = new object[] { code, (short) msg.m_nStatusCode, msg.m_strInfo };
                        SdkManager.Bugly_Report(Utils.FormatString("HTTPSendClient.SendCode:{0} ", args), Utils.FormatString("通用信息返回 : receivecode:{0} statecode:{1}, info:{2}", objArray17));
                        object[] objArray18 = new object[] { this.$this.sendcode, msg.m_nStatusCode, msg.m_strInfo };
                        SdkManager.send_event_http(Utils.FormatString("sendcode : {0} normal response error code : {1} info : {2}", objArray18));
                        this.$this.CacheError(this.senddata, flag);
                        this.$this.DeInit();
                    }
                    else
                    {
                        this.$this.DeInitBefore();
                        this.$this.sendcount--;
                        this.$this.StartCoroutine(this.$this.sendInternal(this.senddata, this.index, this.callback));
                    }
                }
                else
                {
                    this.$this.DeInitBefore();
                    this.$this.sendcount--;
                    this.$this.StartCoroutine(this.$this.sendInternal(this.senddata, this.index, this.callback));
                }
            }
            goto Label_0B19;
        Label_0B07:
            this.$this.DeInit();
            this.$PC = -1;
        Label_0B19:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    private class CArcherSSLCertVerify : CertificateHandler
    {
        private static string[] PUB_KEYS = new string[] { "3082010A028201010091EBF42D6BFAB6BB6C00EAD9BC33172FDB94C6E977B357581CD1E3D3F21DAA2E4E0FE9653BE457E75D9D48BF9CD610CBDBFA33E89B85B3F0747781E61481B5867D5D101B8C3AFBFA619D0FFE443E4A4397832CFCEC81BAA1BE7A4E9602A416473DCB4A80517CA553522967282D3676A1C9CEE03E34DC38DBD7DDF59D9652D4861540B7CC9F5FBFD5A13A09042E42440A42DB234121528874551D8BBB26C589AA39D8928960EC9EB2D27111B6FB1FAF87759622319A332C94F89DE32056F73DAA0A4DB02A388B1389B1CCE7B9D9767E0A45A785EDEB090F0C29A1877E098588AF9194011C50F0744F6B98F81482374C84FD1402B1BA16ED7294E151AB4DE492A50203010001" };
        private static string[] CERT_HASHS = new string[] { "54239C1DC87F77A0EBF1AE2EB25A485DF621072A" };

        protected override bool ValidateCertificate(byte[] certificateData)
        {
            X509Certificate2 certificate = new X509Certificate2(certificateData);
            string publicKeyString = certificate.GetPublicKeyString();
            string certHashString = certificate.GetCertHashString();
            for (int i = 0; i < PUB_KEYS.Length; i++)
            {
                if (publicKeyString.Equals(PUB_KEYS[i]) && certHashString.Equals(CERT_HASHS[i]))
                {
                    return true;
                }
            }
            object[] args = new object[] { publicKeyString, certHashString };
            Debug.LogFormat("!!!!!!!!!SSL[{0}][{1}] Certificate Info Verify Failed!!!!!!!!!", args);
            return false;
        }
    }
}

