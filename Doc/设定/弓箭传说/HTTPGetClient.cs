using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPGetClient : MonoBehaviour
{
    private static GameObject _HTTPGetObject;
    private UnityWebRequest _uwr;
    private float starttime;

    public static void SendGet(string uri, Action<string> callback)
    {
        HTTPGetObject.AddComponent<HTTPGetClient>().StartSendGet(uri, callback);
    }

    [DebuggerHidden]
    public IEnumerator SendGetInternal(string uri, Action<string> callback) => 
        new <SendGetInternal>c__Iterator0 { 
            uri = uri,
            callback = callback,
            $this = this
        };

    public static void SendPost(string uri, WWWForm form, Action<string> callback)
    {
        HTTPGetObject.AddComponent<HTTPGetClient>().StartSendPost(uri, form, callback);
    }

    [DebuggerHidden]
    public IEnumerator SendPostInternal(string uri, WWWForm form, Action<string> callback) => 
        new <SendPostInternal>c__Iterator1 { 
            uri = uri,
            form = form,
            callback = callback,
            $this = this
        };

    public void StartSendGet(string uri, Action<string> callback)
    {
        base.StartCoroutine(this.SendGetInternal(uri, callback));
    }

    public void StartSendPost(string uri, WWWForm form, Action<string> callback)
    {
        base.StartCoroutine(this.SendPostInternal(uri, form, callback));
    }

    private static GameObject HTTPGetObject
    {
        get
        {
            if (_HTTPGetObject == null)
            {
                _HTTPGetObject = new GameObject("HTTPGetClient");
            }
            return _HTTPGetObject;
        }
    }

    private bool isTimeOut =>
        ((Time.realtimeSinceStartup - this.starttime) >= this._uwr.get_timeout());

    [CompilerGenerated]
    private sealed class <SendGetInternal>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal string uri;
        internal UnityWebRequest $locvar0;
        internal Action<string> callback;
        internal string <result>__1;
        internal HTTPGetClient $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        private void <>__Finally0()
        {
            if (this.$locvar0 != null)
            {
                this.$locvar0.Dispose();
            }
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$disposing = true;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<>__Finally0();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    this.$locvar0 = this.$this._uwr = UnityWebRequest.Get(this.uri);
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                default:
                    goto Label_01B5;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        break;

                    default:
                        this.$this._uwr.set_timeout(10);
                        this.$this._uwr.set_method("GET");
                        this.$this.starttime = Time.realtimeSinceStartup;
                        this.$this._uwr.SendWebRequest();
                        break;
                }
                while (!this.$this._uwr.get_isDone() && !this.$this.isTimeOut)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    flag = true;
                    return true;
                }
                if (this.$this.isTimeOut)
                {
                    this.$this.StartCoroutine(this.$this.SendGetInternal(this.uri, this.callback));
                    goto Label_01B5;
                }
                Debugger.Log(string.Concat(new object[] { "length = ", this.$this._uwr.get_downloadHandler().get_text().Length, " text = ", this.$this._uwr.get_downloadHandler().get_text() }));
                this.<result>__1 = this.$this._uwr.get_downloadHandler().get_text();
                this.callback(this.<result>__1);
            }
            finally
            {
                if (!flag)
                {
                }
                this.<>__Finally0();
            }
            this.$PC = -1;
        Label_01B5:
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
    private sealed class <SendPostInternal>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal string uri;
        internal WWWForm form;
        internal UnityWebRequest $locvar0;
        internal Action<string> callback;
        internal string <result>__1;
        internal HTTPGetClient $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        private void <>__Finally0()
        {
            if (this.$locvar0 != null)
            {
                this.$locvar0.Dispose();
            }
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$disposing = true;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<>__Finally0();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    this.$locvar0 = this.$this._uwr = UnityWebRequest.Post(this.uri, this.form);
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                default:
                    goto Label_01C1;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        break;

                    default:
                        this.$this._uwr.set_timeout(10);
                        this.$this._uwr.set_method("POST");
                        this.$this.starttime = Time.realtimeSinceStartup;
                        this.$this._uwr.SendWebRequest();
                        break;
                }
                while (!this.$this._uwr.get_isDone() && !this.$this.isTimeOut)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    flag = true;
                    return true;
                }
                if (this.$this.isTimeOut)
                {
                    this.$this.StartCoroutine(this.$this.SendPostInternal(this.uri, this.form, this.callback));
                    goto Label_01C1;
                }
                Debugger.Log(string.Concat(new object[] { "length = ", this.$this._uwr.get_downloadHandler().get_text().Length, " text = ", this.$this._uwr.get_downloadHandler().get_text() }));
                this.<result>__1 = this.$this._uwr.get_downloadHandler().get_text();
                this.callback(this.<result>__1);
            }
            finally
            {
                if (!flag)
                {
                }
                this.<>__Finally0();
            }
            this.$PC = -1;
        Label_01C1:
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

    public class HTTPGetData
    {
        public string header;
        public string body;
    }
}

