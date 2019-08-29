using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PingTest : MonoBehaviour
{
    public string IP = "123.206.4.94";
    private Ping ping;
    private float delayTime;

    [DebuggerHidden]
    private IEnumerator Pings() => 
        new <Pings>c__Iterator0 { $this = this };

    private void Start()
    {
        base.StartCoroutine("Pings");
    }

    [CompilerGenerated]
    private sealed class <Pings>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <i>__1;
        internal PingTest $this;
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
                case 2:
                    this.$this.ping = new Ping(this.$this.IP);
                    this.<i>__1 = 0;
                    while (this.<i>__1 < 10)
                    {
                        if (this.$this.ping.isDone)
                        {
                            this.$this.delayTime = this.$this.ping.time;
                            this.$this.ping.DestroyPing();
                            this.$this.ping = null;
                            break;
                        }
                        this.$current = new WaitForSeconds(0.3f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_017E;
                    Label_00C2:
                        this.<i>__1++;
                    }
                    break;

                case 1:
                    goto Label_00C2;

                default:
                    return false;
            }
            if (this.$this.ping != null)
            {
                this.$this.delayTime = 999f;
                Debugger.Log("No network!");
                this.$this.ping.DestroyPing();
                this.$this.ping = null;
            }
            else
            {
                Debugger.Log("Network delay : " + this.$this.delayTime + "ms");
            }
            this.$current = new WaitForSeconds(1f);
            if (!this.$disposing)
            {
                this.$PC = 2;
            }
        Label_017E:
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

