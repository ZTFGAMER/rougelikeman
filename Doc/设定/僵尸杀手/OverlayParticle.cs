using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OverlayParticle : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab;
    [Tooltip("For test. Click here only if you KNOW what are you doing!")]
    public bool completed = true;
    private Vector3 position;
    [HideInInspector]
    public ParticleSystem currentPS;

    public void CreatePS()
    {
        this.currentPS = UnityEngine.Object.Instantiate<ParticleSystem>(this.particleSystemPrefab, ParticleCamera.Instance.GetCameraPosition(base.transform.position), this.particleSystemPrefab.transform.rotation, TransformParentManager.Instance.fx);
        base.gameObject.SetActive(false);
        ParticleCamera.Instance.Play(this.currentPS.main.duration);
    }

    public void Play()
    {
        if (this.currentPS == null)
        {
            this.CreatePS();
        }
        else
        {
            this.currentPS.transform.position = ParticleCamera.Instance.GetCameraPosition(base.transform.position);
            this.currentPS.Play();
            ParticleCamera.Instance.Play(this.currentPS.main.duration);
        }
    }

    public void Preview()
    {
        base.StartCoroutine(this.PreviewCoroutine());
    }

    [DebuggerHidden]
    public IEnumerator PreviewCoroutine() => 
        new <PreviewCoroutine>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <PreviewCoroutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal RectTransform <canvas>__0;
        internal Camera <cam>__0;
        internal float <height>__0;
        internal float <width>__0;
        internal Vector3 <viewportPosition>__0;
        internal Vector2 <anchoredPosition>__0;
        internal Vector3 <currentPosition>__0;
        internal OverlayParticle $this;
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
                    this.$this.completed = false;
                    this.<canvas>__0 = UnityEngine.Object.FindObjectOfType<GUI>().GetComponent<RectTransform>();
                    this.<cam>__0 = UnityEngine.Object.FindObjectOfType<ParticleCamera>().GetComponent<Camera>();
                    this.<height>__0 = 2f * this.<cam>__0.orthographicSize;
                    this.<width>__0 = this.<height>__0 * this.<cam>__0.aspect;
                    this.<viewportPosition>__0 = Camera.main.ScreenToViewportPoint(this.$this.transform.position);
                    this.<anchoredPosition>__0 = new Vector2((this.<viewportPosition>__0.x * this.<canvas>__0.sizeDelta.x) - (this.<canvas>__0.sizeDelta.x / 2f), (this.<viewportPosition>__0.y * this.<canvas>__0.sizeDelta.y) - (this.<canvas>__0.sizeDelta.y / 2f));
                    this.<currentPosition>__0 = this.$this.transform.position;
                    this.$this.transform.position = new Vector3(this.<cam>__0.transform.position.x + ((this.<anchoredPosition>__0.x * this.<width>__0) / this.<canvas>__0.sizeDelta.x), this.<cam>__0.transform.position.y + ((this.<anchoredPosition>__0.y * this.<height>__0) / this.<canvas>__0.sizeDelta.y), this.<cam>__0.transform.position.z + 10f);
                    this.$this.GetComponent<ParticleSystem>().Play();
                    this.$current = new WaitForSeconds(this.$this.particleSystemPrefab.main.duration);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.transform.position = this.<currentPosition>__0;
                    this.$this.completed = true;
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

