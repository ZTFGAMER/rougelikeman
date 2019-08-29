using GameProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class Samples : MonoBehaviour
{
    private void Start()
    {
        base.GetComponent<Button>().onClick.AddListener(() => base.StartCoroutine(this.testNetwork()));
    }

    [DebuggerHidden]
    private IEnumerator testNetwork() => 
        new <testNetwork>c__Iterator0();

    private void Update()
    {
    }

    [CompilerGenerated]
    private sealed class <testNetwork>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal uint <nHTTPPort>__0;
        internal CUserLoginPacket <userLogin>__0;
        internal byte[] <postUserLoginData>__0;
        internal string <hexString>__0;
        internal UnityWebRequest <loginRequest>__0;
        internal string <loginResp>__0;
        internal CCommonRespMsg <retMsg>__0;
        internal CRespUserLoginPacket <respUserLoginPacket>__0;
        internal CustomBinaryReader <reader>__0;
        internal byte <headerTag>__0;
        internal ushort <code>__0;
        internal ushort <len>__0;
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
                {
                    this.<nHTTPPort>__0 = 0x1151;
                    this.<userLogin>__0 = new CUserLoginPacket();
                    this.<userLogin>__0.m_strUserID = "0F24582A64D26";
                    this.<userLogin>__0.m_strDeviceID = SystemInfo.deviceUniqueIdentifier;
                    this.<postUserLoginData>__0 = this.<userLogin>__0.buildPacket();
                    this.<hexString>__0 = BitConverter.ToString(this.<postUserLoginData>__0);
                    this.<hexString>__0 = this.<hexString>__0.Replace("-", string.Empty);
                    object[] args = new object[] { this.<postUserLoginData>__0.Length, this.<hexString>__0 };
                    Debug.LogFormat("postUserLoginData[{0}: {1}]", args);
                    this.<loginRequest>__0 = UnityWebRequest.Put("https://o2matrix.top:" + Convert.ToString(this.<nHTTPPort>__0), this.<postUserLoginData>__0);
                    this.<loginRequest>__0.set_method("PUT");
                    this.$current = this.<loginRequest>__0.SendWebRequest();
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                case 1:
                {
                    this.<loginResp>__0 = BitConverter.ToString(this.<loginRequest>__0.get_downloadHandler().get_data());
                    this.<loginResp>__0 = this.<loginResp>__0.Replace("-", string.Empty);
                    object[] args = new object[] { this.<loginRequest>__0.get_downloadHandler().get_data().Length, this.<loginResp>__0 };
                    Debug.LogFormat("loginResp[{0}]: {1}", args);
                    this.<retMsg>__0 = new CCommonRespMsg();
                    this.<respUserLoginPacket>__0 = new CRespUserLoginPacket();
                    this.<reader>__0 = new CustomBinaryReader(new MemoryStream(this.<loginRequest>__0.get_downloadHandler().get_data()));
                    this.<headerTag>__0 = this.<reader>__0.ReadByte();
                    this.<code>__0 = this.<reader>__0.ReadUInt16();
                    this.<len>__0 = this.<reader>__0.ReadUInt16();
                    object[] objArray3 = new object[] { this.<headerTag>__0, this.<code>__0, this.<len>__0 };
                    Debug.LogFormat("headerTag[{0}] code[{1}] len[{2}]", objArray3);
                    if (this.<headerTag>__0 == 13)
                    {
                        if (this.<code>__0 != 8)
                        {
                            if (this.<code>__0 == 6)
                            {
                                this.<retMsg>__0.ReadFromStream(this.<reader>__0);
                            }
                            break;
                        }
                        this.<respUserLoginPacket>__0.ReadFromStream(this.<reader>__0);
                    }
                    break;
                }
                default:
                    goto Label_0277;
            }
            this.$PC = -1;
        Label_0277:
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
}

