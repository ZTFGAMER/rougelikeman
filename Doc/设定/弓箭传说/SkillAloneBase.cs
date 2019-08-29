using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class SkillAloneBase : AttributeCtrlBase
{
    protected EntityBabyBase CreateBaby(int babyID)
    {
        if (base.m_Entity == null)
        {
            return null;
        }
        object[] args = new object[] { babyID };
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("Game/Baby/BabyNode{0}", args)));
        obj2.transform.parent = GameNode.m_Battle.transform;
        float x = base.m_Entity.position.x + Random.Range((float) -2f, (float) 2f);
        float z = base.m_Entity.position.z + Random.Range((float) -2f, (float) 2f);
        obj2.transform.localPosition = new Vector3(x, 0f, z);
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        return obj2.GetComponent<EntityBabyBase>();
    }

    public static long GetAttack(EntityBase entity, string att)
    {
        Goods_goods.GoodData goodData = Goods_goods.GetGoodData(att);
        return GetAttack(entity, goodData);
    }

    private static long GetAttack(EntityBase entity, Goods_goods.GoodData data)
    {
        long num = 0L;
        string goodType = data.goodType;
        if (goodType != null)
        {
            if (goodType != "Attack")
            {
                if (goodType != "Attack%")
                {
                    return num;
                }
            }
            else
            {
                return data.value;
            }
            if (entity != null)
            {
                num = (long) (((float) (entity.m_EntityData.GetAttackBase() * data.value)) / 100f);
            }
        }
        return num;
    }
}

