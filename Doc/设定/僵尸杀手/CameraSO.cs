using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraSO : MonoBehaviour
{
    private float defaultSize;
    private DeviceOrientation prevOrientation;

    [DebuggerHidden]
    private IEnumerator Smoothing() => 
        new <Smoothing>c__Iterator0 { $this = this };

    private void Start()
    {
        this.defaultSize = Camera.main.fieldOfView;
        this.prevOrientation = Input.deviceOrientation;
        base.StartCoroutine(this.Smoothing());
    }

    private void Update()
    {
        if (this.prevOrientation != Input.deviceOrientation)
        {
            base.StopAllCoroutines();
            this.prevOrientation = Input.deviceOrientation;
            base.StartCoroutine(this.Smoothing());
        }
    }

    [CompilerGenerated]
    private sealed class <Smoothing>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float <targetField>__1;
        internal CameraSO $this;
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
                    if ((Input.deviceOrientation != DeviceOrientation.LandscapeLeft) && (Input.deviceOrientation != DeviceOrientation.LandscapeRight))
                    {
                        if ((Input.deviceOrientation != DeviceOrientation.Portrait) && (Input.deviceOrientation != DeviceOrientation.PortraitUpsideDown))
                        {
                            goto Label_00EE;
                        }
                        this.<targetField>__1 = this.$this.defaultSize;
                        break;
                    }
                    this.<targetField>__1 = this.$this.defaultSize * (((float) Screen.width) / ((float) Screen.height));
                    break;

                case 1:
                    break;

                default:
                    goto Label_00EE;
            }
            while (((int) Camera.main.fieldOfView) != ((int) this.<targetField>__1))
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, this.<targetField>__1, 0.2f);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$PC = -1;
        Label_00EE:
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

