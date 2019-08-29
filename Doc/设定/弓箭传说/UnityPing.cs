using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnityPing : MonoBehaviour
{
    private static string s_ip = string.Empty;
    private static Action<int> s_callback;
    private static UnityPing s_unityPing;
    private static int s_timeout = 2;

    public static void CreatePing(string ip, Action<int> callback)
    {
        if ((!string.IsNullOrEmpty(ip) && (callback != null)) && (s_unityPing == null))
        {
            ip = "14.215.177.39";
            s_ip = ip;
            s_callback = callback;
            GameObject target = new GameObject("UnityPing");
            Object.DontDestroyOnLoad(target);
            s_unityPing = target.AddComponent<UnityPing>();
        }
    }

    private void OnDestroy()
    {
        s_ip = string.Empty;
        s_timeout = 20;
        s_callback = null;
        if (s_unityPing != null)
        {
            s_unityPing = null;
        }
    }

    [DebuggerHidden]
    private IEnumerator PingConnect() => 
        new <PingConnect>c__Iterator0 { $this = this };

    private void Start()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                base.StopCoroutine(this.PingConnect());
                base.StartCoroutine(this.PingConnect());
                return;
        }
        if (s_callback != null)
        {
            s_callback(-1);
            Object.Destroy(base.gameObject);
        }
    }

    public static int Timeout
    {
        get => 
            s_timeout;
        set
        {
            if (value > 0)
            {
                s_timeout = value;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PingConnect>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Ping <ping>__0;
        internal int <addTime>__0;
        internal int <requestCount>__0;
        internal UnityPing $this;
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
                    this.<ping>__0 = new Ping(UnityPing.s_ip);
                    this.<addTime>__0 = 0;
                    this.<requestCount>__0 = UnityPing.s_timeout * 10;
                    break;

                case 1:
                    if (this.<addTime>__0 <= this.<requestCount>__0)
                    {
                        this.<addTime>__0++;
                        break;
                    }
                    this.<addTime>__0 = 0;
                    if (UnityPing.s_callback != null)
                    {
                        UnityPing.s_callback(this.<ping>__0.time);
                        Object.Destroy(this.$this.gameObject);
                    }
                    goto Label_013E;

                case 2:
                    goto Label_0137;

                default:
                    goto Label_013E;
            }
            if (!this.<ping>__0.isDone)
            {
                this.$current = new WaitForSeconds(0.1f);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_0140;
            }
            if (this.<ping>__0.isDone)
            {
                if (UnityPing.s_callback != null)
                {
                    UnityPing.s_callback(this.<ping>__0.time);
                    Object.Destroy(this.$this.gameObject);
                }
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_0140;
            }
        Label_0137:
            this.$PC = -1;
        Label_013E:
            return false;
        Label_0140:
            return true;
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

