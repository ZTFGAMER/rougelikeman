using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AdHarvestBattleCtrl : MonoBehaviour
{
    public GameObject child;
    public GameObject entitychild;
    private EntityHero m_Entity;
    private SequencePool mSeqPool = new SequencePool();
    private List<Vector3> mPosList;
    private List<int> entityids;

    public AdHarvestBattleCtrl()
    {
        List<Vector3> list = new List<Vector3> {
            new Vector3(8f, 0f, 0f),
            new Vector3(8f, 0f, 1.5f),
            new Vector3(8f, 0f, -1.5f),
            new Vector3(8f, 0f, 0f)
        };
        this.mPosList = list;
        List<int> list2 = new List<int> { 
            0xc2b,
            0xc2c,
            0xc2d
        };
        this.entityids = list2;
    }

    public void DeInit()
    {
        base.StopCoroutine("initie");
        this.entitychild.transform.DestroyChildren();
        this.mSeqPool.Clear();
        this.m_Entity.DeInit();
        GameLogic.Release.Release();
        if (this.m_Entity != null)
        {
            Object.Destroy(this.m_Entity.gameObject);
        }
    }

    public void Init()
    {
        base.StartCoroutine("initie");
    }

    [DebuggerHidden]
    public IEnumerator initie() => 
        new <initie>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <initie>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal GameObject <o>__0;
        internal int <playerid>__0;
        internal EntityHero <player>__0;
        internal Sequence <s>__0;
        internal AdHarvestBattleCtrl $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        internal void <>m__0()
        {
            if (GameLogic.Release.Entity.GetActiveEntityCount() < 4)
            {
                int num = GameLogic.Random(0, this.$this.entityids.Count);
                MapCreator.CreateData data = new MapCreator.CreateData {
                    entityid = this.$this.entityids[num]
                };
                EntityBase base2 = GameLogic.Release.MapCreatorCtrl.CreateEntity(data);
                base2.transform.parent = this.$this.entitychild.transform;
                base2.transform.localPosition = new Vector3(6.8f, 0f, GameLogic.Random((float) -1.5f, (float) 2f));
                base2.m_Body.SetIsVislble(true);
                base2.ShowHP(false);
            }
        }

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
                    goto Label_018D;

                case 1:
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_018D;

                case 2:
                    this.<o>__0 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Player/PlayerNode"));
                    this.<o>__0.transform.parent = this.$this.entitychild.transform;
                    this.<playerid>__0 = 0x3e9;
                    GameLogic.SelfAttribute.Init();
                    this.<player>__0 = this.<o>__0.GetComponent<EntityHero>();
                    this.<player>__0.Init(this.<playerid>__0);
                    this.<player>__0.transform.localPosition = new Vector3(-3.3f, 0f, 0f);
                    this.$this.m_Entity = this.<player>__0;
                    this.$this.m_Entity.SetCollider(false);
                    this.$this.m_Entity.ShowHP(false);
                    this.$this.m_Entity.transform.ChangeChildLayer(LayerManager.Player);
                    this.<s>__0 = this.$this.mSeqPool.Get();
                    TweenSettingsExtensions.AppendCallback(this.<s>__0, new TweenCallback(this, this.<>m__0));
                    TweenSettingsExtensions.AppendInterval(this.<s>__0, 0.5f);
                    TweenSettingsExtensions.SetLoops<Sequence>(this.<s>__0, -1);
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_018D:
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

