using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ErrorLog : MonoBehaviour
{
    public AnimationCurve curve;
    private string message;

    private void MyLogCallback(string condition, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                this.message = "      receive an Error log,condition=" + condition + ",stackTrace=" + stackTrace;
                return;

            case LogType.Assert:
                this.message = "      receive an assert log,condition=" + condition + ",stackTrace=" + stackTrace;
                return;

            case LogType.Warning:
                if (condition.Contains("DOTWEEN"))
                {
                    SdkManager.Bugly_Report("DOTWEEN", condition, stackTrace);
                }
                return;

            case LogType.Exception:
                this.message = "      receive an Exception log,condition=" + condition + ",stackTrace=" + stackTrace;
                return;
        }
        this.message = string.Empty;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= new Application.LogCallback(this.MyLogCallback);
    }

    [DebuggerHidden]
    private IEnumerator Start() => 
        new <Start>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal ErrorLog $this;
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
                    this.$current = new WaitForSeconds(5.5f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    Application.logMessageReceived += new Application.LogCallback(this.$this.MyLogCallback);
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

