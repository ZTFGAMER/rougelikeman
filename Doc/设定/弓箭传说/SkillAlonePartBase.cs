using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;

public class SkillAlonePartBase : SkillAloneBase
{
    private const string String_CallID = "CallID";
    private const string String_Time = "Time";
    private const string String_Weight = "Weight";
    private int partid;
    private float time;
    private int weight = 1;
    private List<Goods_goods.GoodData> mAttrs = new List<Goods_goods.GoodData>();

    private void DeadAction(EntityBase entity)
    {
        if (entity != null)
        {
            EntityPartBodyBase partbody = base.m_Entity.CreatePartBody(this.partid, entity.position, this.time);
            if (partbody != null)
            {
                partbody.SetEntityType(EntityType.PartBody);
                this.OnDeadAction(entity, partbody);
                if (partbody.m_EntityData != null)
                {
                    int num = 0;
                    int count = this.mAttrs.Count;
                    while (num < count)
                    {
                        partbody.m_EntityData.ExcuteAttributes(this.mAttrs[num]);
                        num++;
                    }
                }
            }
        }
    }

    private void Excute(string str)
    {
        bool flag = true;
        Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
        switch (goodData.goodType)
        {
            case "CallID":
                this.partid = (int) goodData.value;
                break;

            case "Time":
                this.time = ((float) goodData.value) / 1000f;
                break;

            case "Weight":
                this.weight = (int) goodData.value;
                break;

            default:
                flag = false;
                break;
        }
        if (!flag)
        {
            this.mAttrs.Add(goodData);
        }
    }

    protected virtual void OnDeadAction(EntityBase deadentity, EntityPartBodyBase partbody)
    {
    }

    protected override void OnInstall()
    {
        int length = base.m_SkillData.Args.Length;
        if (length > 0)
        {
            for (int i = 0; i < length; i++)
            {
                this.Excute(base.m_SkillData.Args[i]);
            }
        }
        base.m_Entity.m_EntityData.AddDeadCall(new DeadCallData(this.partid, new Action<EntityBase>(this.DeadAction), this.weight));
    }

    protected override void OnUninstall()
    {
    }
}

