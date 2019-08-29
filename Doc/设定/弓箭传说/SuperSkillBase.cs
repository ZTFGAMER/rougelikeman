using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;

public class SuperSkillBase
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int <SkillID>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Skill_super <m_Data>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private EntityHero <m_Entity>k__BackingField;
    private float mLastUseTime = -10000f;

    public void DeInit()
    {
        this.OnDeInit();
    }

    public void Init(EntityHero entity)
    {
        this.m_Entity = entity;
        string s = base.GetType().ToString();
        s = s.Substring(s.Length - 4, 4);
        this.SkillID = int.Parse(s);
        this.m_Data = LocalModelManager.Instance.Skill_super.GetBeanById(this.SkillID);
        this.OnInit();
    }

    protected virtual void OnDeInit()
    {
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnUseSkill()
    {
    }

    public void UseSkill()
    {
        if (this.CanUseSkill)
        {
            this.mLastUseTime = Updater.AliveTime;
            this.OnUseSkill();
        }
    }

    public int SkillID { get; private set; }

    public Skill_super m_Data { get; private set; }

    public EntityHero m_Entity { get; private set; }

    public bool CanUseSkill =>
        ((Updater.AliveTime - this.mLastUseTime) >= this.m_Data.CD);
}

