using Dxx.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LoadSyncCtrl : MonoBehaviour
{
    private bool bComplete;

    public void DeInit()
    {
        Object.Destroy(base.gameObject);
    }

    public static LoadSyncCtrl Load<T>(string path, Action<T> complete) where T: Object
    {
        if (complete == null)
        {
            return null;
        }
        object[] args = new object[] { path };
        LoadSyncCtrl ctrl = new GameObject(Utils.FormatString("LoadSyncCtrl.Load[{0}]", args)).AddComponent<LoadSyncCtrl>();
        ctrl.LoadInternal<T>(path, complete);
        return ctrl;
    }

    [DebuggerHidden]
    private IEnumerator LoadIE<T>(string path, Action<T> complete) where T: Object => 
        new <LoadIE>c__Iterator0<T> { 
            path = path,
            complete = complete,
            $this = this
        };

    public void LoadInternal<T>(string path, Action<T> complete) where T: Object
    {
        base.StartCoroutine(this.LoadIE<T>(path, complete));
    }

    [CompilerGenerated]
    private sealed class <LoadIE>c__Iterator0<T> : IEnumerator, IDisposable, IEnumerator<object> where T: Object
    {
        internal string path;
        internal ResourceRequest <request>__0;
        internal T <o>__0;
        internal Action<T> complete;
        internal LoadSyncCtrl $this;
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
                    this.<request>__0 = Resources.LoadAsync<GameObject>(this.path);
                    this.$current = this.<request>__0;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.<o>__0 = this.<request>__0.asset as T;
                    this.$this.bComplete = true;
                    this.complete(this.<o>__0);
                    this.$this.DeInit();
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
}

