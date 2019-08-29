using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Transparent : MonoBehaviour
{
    [SerializeField]
    private Shader shader;
    private List<GameObject> objects = new List<GameObject>();

    private bool ContainsObject(GameObject go)
    {
        for (int i = 0; i < this.objects.Count; i++)
        {
            if (this.objects[i] == go)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            this.objects.Add(other.gameObject);
            Renderer component = other.GetComponent<Renderer>();
            if (component != null)
            {
                component.material.shader = this.shader;
                base.StartCoroutine(this.SmoothRenderer(component, 1f));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        <OnTriggerExit>c__AnonStorey1 storey = new <OnTriggerExit>c__AnonStorey1 {
            other = other
        };
        if (storey.other.tag == "Obstacle")
        {
            this.objects.Remove(Enumerable.First<GameObject>(this.objects, new Func<GameObject, bool>(storey.<>m__0)));
            Renderer component = storey.other.GetComponent<Renderer>();
            if ((component != null) && !this.ContainsObject(storey.other.gameObject))
            {
                base.StartCoroutine(this.SmoothRenderer(component, 0.25f));
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator SmoothRenderer(Renderer rend, float value) => 
        new <SmoothRenderer>c__Iterator0 { 
            value = value,
            rend = rend
        };

    [CompilerGenerated]
    private sealed class <OnTriggerExit>c__AnonStorey1
    {
        internal Collider other;

        internal bool <>m__0(GameObject obj) => 
            (obj == this.other.gameObject);
    }

    [CompilerGenerated]
    private sealed class <SmoothRenderer>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float value;
        internal Renderer rend;
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
                    if (this.value != 1f)
                    {
                        goto Label_00FA;
                    }
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_00FA;

                default:
                    goto Label_012B;
            }
            if (this.value > 0.25f)
            {
                this.value -= Time.deltaTime * 3f;
                this.rend.material.color = Color.white * this.value;
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_012D;
            }
            goto Label_0124;
        Label_00FA:
            while (this.value < 1f)
            {
                this.value += Time.deltaTime * 3f;
                this.rend.material.color = Color.white * this.value;
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_012D;
            }
            this.rend.material.shader = Shader.Find("Unlit/Texture");
        Label_0124:
            this.$PC = -1;
        Label_012B:
            return false;
        Label_012D:
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

