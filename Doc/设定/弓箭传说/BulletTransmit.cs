using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TableTool;

public class BulletTransmit
{
    public EntityAttributeBase attribute;
    private EntityBase m_Entity;
    private Weapon_weapon weapondata;
    public EElementType trailType;
    public EElementType headType;
    private long attack;
    private float attackratio;
    public float CritRate;
    public float CritSuperRate;
    private HitStruct m_AttackStruct = new HitStruct();
    public int mThroughEnemy;
    public float mThroughRatio;
    public int mHitCreate2;
    public float mHitCreate2Percent;
    public int mHitSputter;
    private float mThunderRatio;
    private List<int> mDebuffList = new List<int>();

    public BulletTransmit(EntityBase entity, int bulletid, bool clear = false)
    {
        this.m_Entity = entity;
        this.weapondata = LocalModelManager.Instance.Weapon_weapon.GetBeanById(bulletid);
        this.mThunderRatio = 1f;
        this.attackratio = 1f;
        if (this.m_Entity == null)
        {
            this.attribute = new EntityAttributeBase();
        }
        else
        {
            if (!clear)
            {
                this.attribute = new EntityAttributeBase(this.m_Entity.m_Data.CharID);
                this.m_Entity.m_EntityData.attribute.AttributeTo(this.attribute);
                this.CritRate = this.attribute.CritRate.Value;
                this.CritSuperRate = this.attribute.CritSuperRate.Value;
                this.mThroughEnemy = this.m_Entity.m_EntityData.ThroughEnemy;
                this.mThroughRatio = this.m_Entity.m_EntityData.ThroughRatio;
                this.mDebuffList = this.m_Entity.GetDebuffList();
                this.mHitCreate2 = this.m_Entity.m_EntityData.HitCreate2;
                this.mHitCreate2Percent = this.m_Entity.m_EntityData.mHitCreate2Percent;
                this.mHitSputter = this.m_Entity.m_EntityData.BulletSputter;
                this.trailType = this.m_Entity.m_EntityData.ArrowTrailType;
                this.headType = this.m_Entity.m_EntityData.ArrowHeadType;
            }
            else
            {
                this.Clear();
            }
            this.attack = MathDxx.CeilToInt((float) this.m_Entity.m_EntityData.GetAttack(this.weapondata.Attack));
        }
    }

    public void AddAttackRatio(float value)
    {
        this.attackratio *= value;
    }

    public void AddDebuffs(params int[] buffs)
    {
        int index = 0;
        int length = buffs.Length;
        while (index < length)
        {
            this.mDebuffList.Add(buffs[index]);
            index++;
        }
    }

    public void AddDebuffsToTarget(EntityBase target)
    {
        int num = 0;
        int count = this.mDebuffList.Count;
        while (num < count)
        {
            int buffid = this.mDebuffList[num];
            float[] args = new float[] { this.mThunderRatio };
            GameLogic.SendBuff(target, this.m_Entity, buffid, args);
            num++;
        }
    }

    public void ArrowEjectAction(float value)
    {
        this.mThunderRatio *= value;
    }

    private void Clear()
    {
        this.attribute = new EntityAttributeBase(this.m_Entity.m_Data.CharID);
        this.mThroughEnemy = 0;
        this.mHitCreate2 = 0;
        this.mDebuffList.Clear();
        this.trailType = EElementType.eNone;
        this.headType = EElementType.eNone;
    }

    public long GetAttack() => 
        this.attack;

    public HitStruct GetAttackStruct()
    {
        long num = MathDxx.CeilToInt(this.attack * this.attackratio);
        this.m_AttackStruct.before_hit = -num;
        this.m_AttackStruct.type = HitType.Normal;
        return this.m_AttackStruct;
    }

    public bool GetHitCreate2() => 
        (this.mHitCreate2 > 0);

    public bool GetHitSputter() => 
        (this.mHitSputter > 0);

    public bool GetThroughEnemy() => 
        (this.mThroughEnemy > 0);

    public void SetAttack(long attack)
    {
        this.attack = attack;
    }
}

