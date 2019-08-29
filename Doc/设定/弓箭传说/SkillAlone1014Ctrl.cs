using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAlone1014Ctrl
{
    private EntityBase m_Entity;
    private SkillAloneBase mSkillAlone;
    private float createdis;
    private float currentdis;
    private List<SkillAloneGoodCtrlBase> mList = new List<SkillAloneGoodCtrlBase>();
    private GameObject good;

    private void CreateOne()
    {
        object[] args = new object[] { "Game/SkillPrefab/", this.mSkillAlone.ClassName };
        GameObject obj2 = GameLogic.EffectGet(Utils.GetString(args));
        obj2.transform.SetParent(GameNode.m_PoolParent);
        SkillAlone1014GoodCtrl component = obj2.GetComponent<SkillAlone1014GoodCtrl>();
        component.Init(this.m_Entity, this.mSkillAlone);
        obj2.transform.position = this.m_Entity.transform.position;
        this.mList.Add(component);
        component.OnGoodDeInit = (Action<SkillAloneGoodCtrlBase>) Delegate.Combine(component.OnGoodDeInit, new Action<SkillAloneGoodCtrlBase>(this.OnGoodDeInit));
        this.good = obj2;
    }

    public void DeInit()
    {
        this.m_Entity.Event_PositionBy -= new Action<Vector3>(this.OnPositionBy);
    }

    public void Init(EntityBase entity, SkillAloneBase alone)
    {
        this.m_Entity = entity;
        this.mSkillAlone = alone;
        this.createdis = float.Parse(this.mSkillAlone.m_SkillData.Args[0]);
        this.m_Entity.Event_PositionBy += new Action<Vector3>(this.OnPositionBy);
    }

    private void OnGoodDeInit(SkillAloneGoodCtrlBase ctrl)
    {
        this.mList.Remove(ctrl);
    }

    private void OnPositionBy(Vector3 p)
    {
        this.currentdis += p.magnitude;
        if (this.currentdis >= this.createdis)
        {
            this.currentdis -= this.createdis;
            this.CreateOne();
        }
    }

    public void RemoveGoods()
    {
        for (int i = this.mList.Count - 1; i >= 0; i--)
        {
            this.mList[i].DeInit();
        }
        this.mList.Clear();
    }
}

