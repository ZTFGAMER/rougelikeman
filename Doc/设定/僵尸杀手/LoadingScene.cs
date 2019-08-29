using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image[] loadingImages;
    [SerializeField]
    private Image backGround;
    [SerializeField]
    private GameObject camX;
    private Vector2[] startpos;
    private Coroutine loading;
    private int currentImg;
    private bool camOnPos;
    private AsyncOperation operation;
    private Vector2 startSize;

    [DebuggerHidden]
    private IEnumerator CamSpeed() => 
        new <CamSpeed>c__Iterator1 { $this = this };

    [DebuggerHidden]
    private IEnumerator Decrease(Image img) => 
        new <Decrease>c__Iterator4 { img = img };

    private void DetectCam()
    {
        base.StartCoroutine("CamSpeed");
    }

    [DebuggerHidden]
    private IEnumerator EndLoading() => 
        new <EndLoading>c__Iterator0 { $this = this };

    private int GetCorrectID(int maxLength, int nextID)
    {
        if (nextID >= maxLength)
        {
            nextID -= maxLength;
        }
        if (nextID < 0)
        {
            nextID += maxLength;
        }
        return nextID;
    }

    [DebuggerHidden]
    private IEnumerator Loading() => 
        new <Loading>c__Iterator2 { $this = this };

    [DebuggerHidden]
    private IEnumerator LoadingAnimation() => 
        new <LoadingAnimation>c__Iterator3 { $this = this };

    [DebuggerHidden]
    private IEnumerator SetOnPos(RectTransform rect, Vector2 pos) => 
        new <SetOnPos>c__Iterator5 { 
            rect = rect,
            pos = pos
        };

    private void Start()
    {
        this.startSize = this.loadingImages[0].get_rectTransform().sizeDelta;
        for (int i = 0; i < this.loadingImages.Length; i++)
        {
            this.loadingImages[i].get_rectTransform().sizeDelta = Vector2.zero;
        }
    }

    public void StartLoading()
    {
        base.StartCoroutine(this.Loading());
    }

    [CompilerGenerated]
    private sealed class <CamSpeed>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal GameObject <cam>__0;
        internal Vector3 <oldPos>__0;
        internal LoadingScene $this;
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
                    this.<cam>__0 = UnityEngine.Object.FindObjectOfType<FreeLookCam>().gameObject;
                    this.<oldPos>__0 = this.<cam>__0.transform.position;
                    break;

                case 1:
                    if (this.<oldPos>__0 != this.<cam>__0.transform.position)
                    {
                        break;
                    }
                    this.$this.camOnPos = true;
                    this.$PC = -1;
                    goto Label_00AF;

                default:
                    goto Label_00AF;
            }
            this.<oldPos>__0 = this.<cam>__0.transform.position;
            this.$current = new WaitForFixedUpdate();
            if (!this.$disposing)
            {
                this.$PC = 1;
            }
            return true;
        Label_00AF:
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
    private sealed class <Decrease>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Image img;
        internal Color <color>__0;
        internal float <t>__1;
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
                    this.<color>__0 = this.img.get_color();
                    break;

                case 1:
                    break;

                default:
                    goto Label_0107;
            }
            if (this.img.get_rectTransform().sizeDelta.x > 0f)
            {
                this.<t>__1 = Time.deltaTime * 75f;
                this.<color>__0.a -= Time.deltaTime;
                this.img.set_color(this.<color>__0);
                this.img.get_rectTransform().sizeDelta = new Vector2(this.img.get_rectTransform().sizeDelta.x - this.<t>__1, this.img.get_rectTransform().sizeDelta.y - this.<t>__1);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$PC = -1;
        Label_0107:
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
    private sealed class <EndLoading>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Color <color>__1;
        internal LoadingScene $this;
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
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_01FD;

                case 1:
                case 2:
                    if (!this.$this.operation.isDone)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(this.$this.camX);
                        this.$this.camOnPos = false;
                        this.$current = new WaitForSeconds(0.1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                    }
                    goto Label_01FD;

                case 3:
                    this.$this.DetectCam();
                    break;

                case 4:
                    break;

                case 5:
                    goto Label_01C7;

                default:
                    goto Label_01FB;
            }
            while (!this.$this.camOnPos || !DataLoader.initialized)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 4;
                }
                goto Label_01FD;
            }
            this.$this.StopCoroutine(this.$this.loading);
            this.$this.StartCoroutine(this.$this.Decrease(this.$this.loadingImages[this.$this.GetCorrectID(this.$this.loadingImages.Length, this.$this.currentImg - 1)]));
        Label_01C7:
            while (this.$this.backGround.get_color().a > 0f)
            {
                this.<color>__1 = this.$this.backGround.get_color();
                this.<color>__1.a -= Time.deltaTime;
                this.$this.backGround.set_color(this.<color>__1);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 5;
                }
                goto Label_01FD;
            }
            SceneManager.UnloadSceneAsync("Loading");
            this.$PC = -1;
        Label_01FB:
            return false;
        Label_01FD:
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

    [CompilerGenerated]
    private sealed class <Loading>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal LoadingScene $this;
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
                    this.$current = new WaitForSeconds(0.5f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_00CA;

                case 1:
                    this.$this.loading = this.$this.StartCoroutine(this.$this.LoadingAnimation());
                    break;

                case 2:
                    break;

                default:
                    goto Label_00C8;
            }
            if (!GPGSCloudSave.syncWithCloud)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_00CA;
            }
            this.$this.operation = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
            this.$this.StartCoroutine(this.$this.EndLoading());
            this.$PC = -1;
        Label_00C8:
            return false;
        Label_00CA:
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

    [CompilerGenerated]
    private sealed class <LoadingAnimation>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Image <img>__1;
        internal LoadingScene $this;
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
                case 1:
                    this.<img>__1 = this.$this.loadingImages[this.$this.GetCorrectID(this.$this.loadingImages.Length, this.$this.currentImg)];
                    this.<img>__1.get_rectTransform().sizeDelta = this.$this.startSize;
                    this.<img>__1.set_color(new Color(this.<img>__1.get_color().r, this.<img>__1.get_color().g, this.<img>__1.get_color().b, 1f));
                    this.$this.loadingImages[this.$this.GetCorrectID(this.$this.loadingImages.Length, this.$this.currentImg)].get_rectTransform().sizeDelta = this.$this.startSize;
                    this.$this.StartCoroutine(this.$this.Decrease(this.$this.loadingImages[this.$this.GetCorrectID(this.$this.loadingImages.Length, this.$this.currentImg - 1)]));
                    this.$this.currentImg++;
                    if (this.$this.currentImg == this.$this.loadingImages.Length)
                    {
                        this.$this.currentImg = 0;
                    }
                    this.$current = new WaitForSeconds(0.08f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
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

    [CompilerGenerated]
    private sealed class <SetOnPos>c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal RectTransform rect;
        internal Vector2 pos;
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
                case 1:
                    if (this.rect.anchoredPosition != this.pos)
                    {
                        this.rect.anchoredPosition = Vector2.MoveTowards(this.rect.anchoredPosition, this.pos, Time.deltaTime * 250f);
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
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

