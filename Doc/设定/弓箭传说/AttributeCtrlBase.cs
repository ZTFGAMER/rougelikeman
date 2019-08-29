using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class AttributeCtrlBase
{
    protected EntityBase m_Entity;
    private string _className;
    private int _classid;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Skill_alone <m_Data>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Skill_skill <m_SkillData>k__BackingField;
    private List<Goods_goods.GoodData> list = new List<Goods_goods.GoodData>();
    private GameObject effect;
    private bool bInit;

    private void CreateEffect()
    {
        Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(this.m_Data.CreateEffectID);
        if (beanById != null)
        {
            this.effect = GameLogic.EffectGet(beanById.Path);
            this.effect.transform.parent = this.m_Entity.GetKetNode(beanById.Node);
            this.effect.transform.localPosition = Vector3.zero;
            this.effect.transform.localRotation = Quaternion.identity;
            this.effect.transform.localScale = Vector3.one;
        }
    }

    private void ExcuteAttributes()
    {
        string[] attributes = this.m_Data.Attributes;
        int index = 0;
        int length = attributes.Length;
        while (index < length)
        {
            this.list.Add(Goods_goods.GetGoodData(attributes[index]));
            index++;
        }
    }

    public void Install(EntityBase entity, Skill_skill skilldata, Skill_alone skill, params object[] args)
    {
        this.bInit = true;
        this._className = base.GetType().ToString();
        int.TryParse(this.ClassName.Substring(this.ClassName.Length - 4, 4), out this._classid);
        this.m_Entity = entity;
        this.m_Data = skill;
        this.m_SkillData = skilldata;
        this.ExcuteAttributes();
        this.CreateEffect();
        this.InstallAttrs(1);
        if (args.Length > 0)
        {
            this.OnInstall(args);
        }
        else
        {
            this.OnInstall();
        }
    }

    private void InstallAttrs(int symbol)
    {
        int num = 0;
        int count = this.list.Count;
        while (num < count)
        {
            Goods_goods.GoodData data = this.list[num];
            long num3 = data.value * symbol;
            this.m_Entity.m_EntityData.ExcuteAttributes(data.goodType, num3);
            num++;
        }
    }

    protected virtual void OnInstall()
    {
    }

    protected virtual void OnInstall(params object[] args)
    {
        this.OnInstall();
    }

    protected virtual void OnUninstall()
    {
    }

    private void RemoveEffect()
    {
        GameLogic.EffectCache(this.effect);
    }

    public void Uninstall()
    {
        if (this.bInit)
        {
            this.bInit = false;
            this.RemoveEffect();
            this.InstallAttrs(-1);
            this.OnUninstall();
        }
    }

    public string ClassName =>
        this._className;

    public int ClassID =>
        this._classid;

    public Skill_alone m_Data { get; private set; }

    public Skill_skill m_SkillData { get; private set; }
}

