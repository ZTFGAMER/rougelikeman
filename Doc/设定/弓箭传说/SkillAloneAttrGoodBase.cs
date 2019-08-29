using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAloneAttrGoodBase : MonoBehaviour
{
    private static Dictionary<GameObject, EffectClass> mList = new Dictionary<GameObject, EffectClass>();
    private static List<GameObject> mRemoveList = new List<GameObject>();
    protected EntityBase m_Entity;
    protected float[] args;
    public bool bGotoRoomDeInit = true;
    private ParticleSystem[] particles;
    private MeshRenderer[] meshes;
    private Sequence seq;

    public static SkillAloneAttrGoodBase[] Add(EntityBase entity, GameObject o, bool bGotoRoomDeInit, params float[] args)
    {
        if (o == null)
        {
            return null;
        }
        if (mList.ContainsKey(o))
        {
            return null;
        }
        EffectClass class2 = new EffectClass {
            o = o,
            list = o.GetComponentsInChildren<SkillAloneAttrGoodBase>(true),
            bGotoRoomDeInit = bGotoRoomDeInit
        };
        class2.Init(entity, args);
        mList.Add(o, class2);
        return class2.list;
    }

    protected void Attack(EntityBase entity, float hitratio)
    {
        int attack = 20;
        if (this.m_Entity.m_Weapon != null)
        {
            attack = this.m_Entity.m_Weapon.m_Data.Attack;
        }
        long beforehit = (long) (-this.m_Entity.m_EntityData.GetAttack(attack) * hitratio);
        GameLogic.SendHit_Skill(entity, beforehit);
    }

    protected void AttackByArg(EntityBase entity)
    {
        float hitratio = 1f;
        if ((this.args != null) && (this.args.Length > 0))
        {
            hitratio = this.args[0];
        }
        this.Attack(entity, hitratio);
    }

    public void DeInit()
    {
        this.KillSequence();
        this.OnDeInit();
    }

    public static void DeInitData()
    {
        Dictionary<GameObject, EffectClass>.Enumerator enumerator = mList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<GameObject, EffectClass> current = enumerator.Current;
            GameLogic.EffectCache(current.Key);
        }
        mList.Clear();
    }

    public void Init(EntityBase entity, params float[] args)
    {
        this.m_Entity = entity;
        this.args = args;
        this.particles = base.GetComponentsInChildren<ParticleSystem>(true);
        this.meshes = base.GetComponentsInChildren<MeshRenderer>(true);
        int index = 0;
        int length = this.meshes.Length;
        while (index < length)
        {
            this.meshes[index].sortingLayerName = "Hit";
            index++;
        }
        this.OnInit();
    }

    public static void InitData()
    {
        mList.Clear();
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    protected virtual void OnDeInit()
    {
    }

    public static void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        mRemoveList.Clear();
        Dictionary<GameObject, EffectClass>.Enumerator enumerator = mList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<GameObject, EffectClass> current = enumerator.Current;
            if (current.Value.bGotoRoomDeInit)
            {
                KeyValuePair<GameObject, EffectClass> pair2 = enumerator.Current;
                mRemoveList.Add(pair2.Key);
            }
            else
            {
                KeyValuePair<GameObject, EffectClass> pair3 = enumerator.Current;
                pair3.Value.OnGotoNextRoom(room);
            }
        }
        int num = 0;
        int count = mRemoveList.Count;
        while (num < count)
        {
            GameObject o = mRemoveList[num];
            Remove(o);
            GameLogic.EffectCache(o);
            num++;
        }
    }

    protected virtual void OnInit()
    {
    }

    public void OntoNextRoom(RoomGenerateBase.Room room)
    {
        this.KillSequence();
        base.gameObject.SetActive(false);
        this.seq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.2f), new TweenCallback(this, this.<OntoNextRoom>m__0));
        if (this.particles != null)
        {
            int index = 0;
            int length = this.particles.Length;
            while (index < length)
            {
                this.particles[index].Clear();
                index++;
            }
        }
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.layer == LayerManager.Player)
        {
            EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
            if (!GameLogic.IsSameTeam(entityByChild, this.m_Entity))
            {
                this.TriggerEnter(entityByChild);
            }
        }
    }

    public static bool Remove(GameObject o)
    {
        if ((o != null) && mList.TryGetValue(o, out EffectClass class2))
        {
            class2.DeInit();
            return true;
        }
        return false;
    }

    protected virtual void TriggerEnter(EntityBase entity)
    {
        this.Attack(entity, 1f);
    }

    public class EffectClass
    {
        public GameObject o;
        public SkillAloneAttrGoodBase[] list;
        public bool bGotoRoomDeInit;

        public void DeInit()
        {
            if (this.list != null)
            {
                int index = 0;
                int length = this.list.Length;
                while (index < length)
                {
                    this.list[index].DeInit();
                    index++;
                }
            }
        }

        public void Init(EntityBase entity, params float[] args)
        {
            int index = 0;
            int length = this.list.Length;
            while (index < length)
            {
                this.list[index].Init(entity, args);
                index++;
            }
        }

        public void OnGotoNextRoom(RoomGenerateBase.Room room)
        {
            int index = 0;
            int length = this.list.Length;
            while (index < length)
            {
                this.list[index].OntoNextRoom(room);
                index++;
            }
        }
    }
}

