using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private Image[] loadingImages;
    [SerializeField]
    private Image backGround;
    private Vector2 startSize = Vector2.zero;
    private int currentImg;
    private Coroutine loadingAnim;

    [DebuggerHidden]
    private IEnumerator CamSpeed() => 
        new <CamSpeed>c__Iterator2 { $this = this };

    [DebuggerHidden]
    private IEnumerator Decrease(Image img) => 
        new <Decrease>c__Iterator1 { img = img };

    public void EndLoading()
    {
        if (base.gameObject.activeSelf)
        {
            base.StartCoroutine(this.CamSpeed());
        }
    }

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
    private IEnumerator LoadingAnimation() => 
        new <LoadingAnimation>c__Iterator0 { $this = this };

    public void StartLoading()
    {
        this.backGround.set_color(this.backGround.get_color() + new Color(this.backGround.get_color().r, this.backGround.get_color().g, this.backGround.get_color().b, 1.1f));
        if (this.startSize == Vector2.zero)
        {
            this.startSize = this.loadingImages[0].get_rectTransform().sizeDelta;
        }
        for (int i = 0; i < this.loadingImages.Length; i++)
        {
            this.loadingImages[i].get_rectTransform().sizeDelta = Vector2.zero;
        }
        base.gameObject.SetActive(true);
        this.loadingAnim = base.StartCoroutine(this.LoadingAnimation());
    }

    [CompilerGenerated]
    private sealed class <CamSpeed>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal GameObject <cam>__0;
        internal Vector3 <oldPos>__0;
        internal Color <color>__1;
        internal Loading $this;
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
                    this.$current = new WaitForSeconds(0.1f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_020A;

                case 1:
                    this.<cam>__0 = UnityEngine.Object.FindObjectOfType<FreeLookCam>().gameObject;
                    this.<oldPos>__0 = this.<cam>__0.transform.position;
                    break;

                case 2:
                    if (this.<oldPos>__0 != this.<cam>__0.transform.position)
                    {
                        break;
                    }
                    this.$this.StopCoroutine(this.$this.loadingAnim);
                    this.$this.StartCoroutine(this.$this.Decrease(this.$this.loadingImages[this.$this.GetCorrectID(this.$this.loadingImages.Length, this.$this.currentImg - 1)]));
                    goto Label_0189;

                case 3:
                    goto Label_0189;

                default:
                    goto Label_0208;
            }
            this.<oldPos>__0 = this.<cam>__0.transform.position;
            this.$current = new WaitForFixedUpdate();
            if (!this.$disposing)
            {
                this.$PC = 2;
            }
            goto Label_020A;
        Label_0189:
            if (this.$this.backGround.get_color().a > 0f)
            {
                this.<color>__1 = this.$this.backGround.get_color();
                this.<color>__1.a -= Time.deltaTime;
                this.$this.backGround.set_color(this.<color>__1);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
                goto Label_020A;
            }
            this.$this.StopAllCoroutines();
            this.$this.gameObject.SetActive(false);
            if (GameManager.instance.currentGameMode == GameManager.GameModes.Arena)
            {
                DataLoader.gui.pauseReady = true;
                DataLoader.gui.OnApplicationPause(true);
                DataLoader.gui.Resume();
                GameManager.instance.StartArenaTimer();
            }
            this.$PC = -1;
        Label_0208:
            return false;
        Label_020A:
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
    private sealed class <Decrease>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
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
    private sealed class <LoadingAnimation>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Image <img>__1;
        internal Loading $this;
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
}

