using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static BombManager instance;
    [SerializeField]
    private GameObject bombardmentWarningUI;
    [SerializeField]
    private GameObject dropPlace;
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private float dropDistance = 5f;
    [SerializeField]
    private bool oneTime = true;
    [SerializeField]
    private float bombardmentDelay = 20f;
    [SerializeField]
    private float bombardmentTime = 10f;
    [SerializeField]
    private float dropDelay = 2f;
    [SerializeField]
    private float delayInOneDrop = 0.5f;
    [SerializeField]
    private int minCountBombs = 1;
    [SerializeField]
    private int maxCountBombs = 3;
    [Header("Ability Settings"), SerializeField]
    private float dropAbilityDistance = 5f;
    [SerializeField]
    private int minAbilityCountBombs = 1;
    [SerializeField]
    private int maxAbilityCountBombs = 3;
    private float startBombardmentTime;

    private void Awake()
    {
        instance = this;
    }

    [DebuggerHidden]
    private IEnumerator CreateAbilityBombs(Vector3 position) => 
        new <CreateAbilityBombs>c__Iterator1 { 
            position = position,
            $this = this
        };

    [DebuggerHidden]
    private IEnumerator CreateBombs(Vector3 position) => 
        new <CreateBombs>c__Iterator0 { 
            position = position,
            $this = this
        };

    private void DropBomb()
    {
        base.StartCoroutine(this.CreateBombs(this.cameraTarget.position));
        if ((Time.time - this.startBombardmentTime) >= this.bombardmentTime)
        {
            if (!this.oneTime)
            {
                base.Invoke("DropBomb", this.bombardmentDelay);
            }
            this.startBombardmentTime = Time.time + this.bombardmentDelay;
            base.Invoke("HideWarning", 2f);
        }
        else
        {
            base.Invoke("DropBomb", this.dropDelay);
            this.bombardmentWarningUI.SetActive(true);
        }
    }

    private void HideWarning()
    {
        this.bombardmentWarningUI.SetActive(false);
    }

    public void OneMoreBomb()
    {
        base.StartCoroutine(this.CreateAbilityBombs(this.cameraTarget.position));
    }

    public void StartGame()
    {
        this.startBombardmentTime = Time.time + this.bombardmentDelay;
        base.Invoke("DropBomb", this.bombardmentDelay);
        this.bombardmentWarningUI.SetActive(false);
    }

    public void StopIt()
    {
        base.CancelInvoke("DropBomb");
        base.StopAllCoroutines();
        base.CancelInvoke("HideWarning");
        this.bombardmentWarningUI.SetActive(false);
    }

    [CompilerGenerated]
    private sealed class <CreateAbilityBombs>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <bombsCount>__0;
        internal int <i>__1;
        internal Vector3 position;
        internal Vector3 <dropPos>__2;
        internal BombManager $this;
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
                    this.<bombsCount>__0 = UnityEngine.Random.Range(this.$this.minAbilityCountBombs, this.$this.maxAbilityCountBombs + 1);
                    this.<i>__1 = 0;
                    break;

                case 1:
                    this.<i>__1++;
                    break;

                default:
                    goto Label_015E;
            }
            if (this.<i>__1 < this.<bombsCount>__0)
            {
                this.<dropPos>__2 = new Vector3(this.position.x + UnityEngine.Random.Range(-this.$this.dropAbilityDistance, this.$this.dropAbilityDistance), 100f, this.position.z + UnityEngine.Random.Range(-this.$this.dropAbilityDistance, this.$this.dropAbilityDistance));
                UnityEngine.Object.Instantiate<GameObject>(this.$this.dropPlace, new Vector3(this.<dropPos>__2.x, this.$this.dropPlace.transform.position.y, this.<dropPos>__2.z), this.$this.dropPlace.transform.rotation);
                this.$current = new WaitForSeconds(this.$this.delayInOneDrop);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$PC = -1;
        Label_015E:
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
    private sealed class <CreateBombs>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <bombsCount>__0;
        internal int <i>__1;
        internal Vector3 position;
        internal Vector3 <dropPos>__2;
        internal BombManager $this;
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
                    this.<bombsCount>__0 = UnityEngine.Random.Range(this.$this.minCountBombs, this.$this.maxCountBombs + 1);
                    this.<i>__1 = 0;
                    break;

                case 1:
                    this.<i>__1++;
                    break;

                default:
                    goto Label_015E;
            }
            if (this.<i>__1 < this.<bombsCount>__0)
            {
                this.<dropPos>__2 = new Vector3(this.position.x + UnityEngine.Random.Range(-this.$this.dropDistance, this.$this.dropDistance), 100f, this.position.z + UnityEngine.Random.Range(-this.$this.dropDistance, this.$this.dropDistance));
                UnityEngine.Object.Instantiate<GameObject>(this.$this.dropPlace, new Vector3(this.<dropPos>__2.x, this.$this.dropPlace.transform.position.y, this.<dropPos>__2.z), this.$this.dropPlace.transform.rotation);
                this.$current = new WaitForSeconds(this.$this.delayInOneDrop);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$PC = -1;
        Label_015E:
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

