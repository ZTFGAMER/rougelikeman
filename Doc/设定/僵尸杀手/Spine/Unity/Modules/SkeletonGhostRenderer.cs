namespace Spine.Unity.Modules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SkeletonGhostRenderer : MonoBehaviour
    {
        public float fadeSpeed = 10f;
        private Color32[] colors;
        private Color32 black = new Color32(0, 0, 0, 0);
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            this.meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
            this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
        }

        public void Cleanup()
        {
            if ((this.meshFilter != null) && (this.meshFilter.sharedMesh != null))
            {
                UnityEngine.Object.Destroy(this.meshFilter.sharedMesh);
            }
            UnityEngine.Object.Destroy(base.gameObject);
        }

        [DebuggerHidden]
        private IEnumerator Fade() => 
            new <Fade>c__Iterator0 { $this = this };

        [DebuggerHidden]
        private IEnumerator FadeAdditive() => 
            new <FadeAdditive>c__Iterator1 { $this = this };

        public void Initialize(Mesh mesh, Material[] materials, Color32 color, bool additive, float speed, int sortingLayerID, int sortingOrder)
        {
            base.StopAllCoroutines();
            base.gameObject.SetActive(true);
            this.meshRenderer.sharedMaterials = materials;
            this.meshRenderer.sortingLayerID = sortingLayerID;
            this.meshRenderer.sortingOrder = sortingOrder;
            this.meshFilter.sharedMesh = UnityEngine.Object.Instantiate<Mesh>(mesh);
            this.colors = this.meshFilter.sharedMesh.colors32;
            if ((((color.a + color.r) + color.g) + color.b) > 0)
            {
                for (int i = 0; i < this.colors.Length; i++)
                {
                    this.colors[i] = color;
                }
            }
            this.fadeSpeed = speed;
            if (additive)
            {
                base.StartCoroutine(this.FadeAdditive());
            }
            else
            {
                base.StartCoroutine(this.Fade());
            }
        }

        [CompilerGenerated]
        private sealed class <Fade>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal int <t>__1;
            internal bool <breakout>__2;
            internal Color32 <c>__3;
            internal SkeletonGhostRenderer $this;
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
                        this.<t>__1 = 0;
                        break;

                    case 1:
                        this.<t>__1++;
                        break;

                    default:
                        goto Label_0159;
                }
                if (this.<t>__1 < 500)
                {
                    this.<breakout>__2 = true;
                    for (int i = 0; i < this.$this.colors.Length; i++)
                    {
                        this.<c>__3 = this.$this.colors[i];
                        if (this.<c>__3.a > 0)
                        {
                            this.<breakout>__2 = false;
                        }
                        this.$this.colors[i] = Color32.Lerp(this.<c>__3, this.$this.black, Time.deltaTime * this.$this.fadeSpeed);
                    }
                    this.$this.meshFilter.sharedMesh.colors32 = this.$this.colors;
                    if (!this.<breakout>__2)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                }
                UnityEngine.Object.Destroy(this.$this.meshFilter.sharedMesh);
                this.$this.gameObject.SetActive(false);
                this.$PC = -1;
            Label_0159:
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
        private sealed class <FadeAdditive>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Color32 <black>__0;
            internal int <t>__1;
            internal bool <breakout>__2;
            internal Color32 <c>__3;
            internal SkeletonGhostRenderer $this;
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
                        this.<black>__0 = this.$this.black;
                        this.<t>__1 = 0;
                        break;

                    case 1:
                        this.<t>__1++;
                        break;

                    default:
                        goto Label_019D;
                }
                if (this.<t>__1 < 500)
                {
                    this.<breakout>__2 = true;
                    for (int i = 0; i < this.$this.colors.Length; i++)
                    {
                        this.<c>__3 = this.$this.colors[i];
                        this.<black>__0.a = this.<c>__3.a;
                        if (((this.<c>__3.r > 0) || (this.<c>__3.g > 0)) || (this.<c>__3.b > 0))
                        {
                            this.<breakout>__2 = false;
                        }
                        this.$this.colors[i] = Color32.Lerp(this.<c>__3, this.<black>__0, Time.deltaTime * this.$this.fadeSpeed);
                    }
                    this.$this.meshFilter.sharedMesh.colors32 = this.$this.colors;
                    if (!this.<breakout>__2)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                }
                UnityEngine.Object.Destroy(this.$this.meshFilter.sharedMesh);
                this.$this.gameObject.SetActive(false);
                this.$PC = -1;
            Label_019D:
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
}

