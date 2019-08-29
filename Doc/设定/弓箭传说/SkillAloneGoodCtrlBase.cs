using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAloneGoodCtrlBase : PauseObject
{
    protected SkillAloneBase mSkillAlone;
    protected EntityBase m_Entity;
    private float time;
    private float starttime;
    public Action<SkillAloneGoodCtrlBase> OnGoodDeInit;
    private List<EntityBase> mList = new List<EntityBase>();
    private int[] debuffs;

    protected virtual bool CanHitEntity(EntityBase target) => 
        true;

    public void DeInit()
    {
        GameLogic.EffectCache(base.gameObject);
    }

    public void Init(EntityBase entity, SkillAloneBase alone)
    {
        this.m_Entity = entity;
        this.mSkillAlone = alone;
        this.debuffs = this.mSkillAlone.m_Data.DeBuffs;
        this.time = float.Parse(this.mSkillAlone.m_SkillData.Args[1]);
        this.starttime = Updater.AliveTime;
        this.mList.Clear();
        this.OnInit();
    }

    protected virtual void OnInit()
    {
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.layer == LayerManager.Player)
        {
            EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
            if (!GameLogic.IsSameTeam(entityByChild, this.m_Entity) && this.CanHitEntity(entityByChild))
            {
                this.mList.Add(entityByChild);
            }
        }
    }

    private void OnTriggerExit(Collider o)
    {
        if (o.gameObject.layer == LayerManager.Player)
        {
            EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
            if (this.mList.Contains(entityByChild))
            {
                this.mList.Remove(entityByChild);
            }
        }
    }

    protected override void UpdateProcess()
    {
        if ((Updater.AliveTime - this.starttime) > this.time)
        {
            this.DeInit();
            if (this.OnGoodDeInit != null)
            {
                this.OnGoodDeInit(this);
            }
        }
        int num = 0;
        int count = this.mList.Count;
        while (num < count)
        {
            EntityBase target = this.mList[num];
            int index = 0;
            int length = this.debuffs.Length;
            while (index < length)
            {
                GameLogic.SendBuff(target, this.m_Entity, this.debuffs[index], Array.Empty<float>());
                index++;
            }
            num++;
        }
    }
}

