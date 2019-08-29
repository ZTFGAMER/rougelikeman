using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RedLineCtrl
{
    private EntityBase m_Entity;
    private bool bEnd;
    private List<GameObject> RedLineList = new List<GameObject>();
    private List<BulletRedLineCtrl> lineCtrlList = new List<BulletRedLineCtrl>();
    private float resultangle;
    private Vector3 resultpos;
    private Vector3 nextpos;
    private float lastangle;
    private int ReboundCount;
    private float offsetangle;
    private bool bThroughWall;
    private int layerMask;

    private void CreateRedLine()
    {
        while (this.RedLineList.Count < (this.ReboundCount + 1))
        {
            GameObject item = GameLogic.EffectGet("Game/Bullet/Bullet1007_RedLine");
            item.transform.SetParent(GameNode.m_PoolParent);
            item.transform.localPosition = this.m_Entity.m_Body.EffectMask.transform.position;
            item.transform.localScale = Vector3.one;
            item.transform.rotation = this.m_Entity.m_Body.EffectMask.transform.rotation;
            item.transform.rotation = Quaternion.Euler(item.transform.eulerAngles.x, item.transform.eulerAngles.y + this.offsetangle, item.transform.eulerAngles.z);
            this.RedLineList.Add(item);
            this.lineCtrlList.Add(item.GetComponent<BulletRedLineCtrl>());
        }
        this.UpdateLinesLength();
    }

    public void DeInit()
    {
        this.RemoveRedLine();
    }

    public void Init(EntityBase entity, bool throughwall, int ReboundCount, float offsetangle)
    {
        this.m_Entity = entity;
        this.ReboundCount = ReboundCount;
        this.offsetangle = offsetangle;
        this.bThroughWall = throughwall;
        if (this.bThroughWall)
        {
            this.layerMask = ((int) 1) << LayerManager.MapOutWall;
        }
        else
        {
            this.layerMask = (((int) 1) << LayerManager.Bullet2Map) | (((int) 1) << LayerManager.MapOutWall);
        }
        this.CreateRedLine();
    }

    private void RemoveRedLine()
    {
        int num = 0;
        int count = this.RedLineList.Count;
        while (num < count)
        {
            GameObject o = this.RedLineList[num];
            if (o != null)
            {
                GameLogic.EffectCache(o);
            }
            num++;
        }
        this.RedLineList.Clear();
        this.lineCtrlList.Clear();
    }

    public void Update()
    {
        if ((this.m_Entity != null) && (this.m_Entity.m_HatredTarget != null))
        {
            this.UpdateLinesLength();
        }
    }

    private void UpdateLineLength(int index)
    {
        this.RedLineList[index].transform.position = new Vector3(this.resultpos.x, this.m_Entity.m_Body.LeftBullet.transform.position.y, this.resultpos.z);
        this.RedLineList[index].transform.rotation = Quaternion.Euler(0f, this.resultangle, 0f);
        Vector3 vector2 = this.nextpos - this.resultpos;
        RayCastManager.CastMinDistance(this.RedLineList[index].transform.position, vector2.normalized, this.bThroughWall, out float num, out this.resultpos, out Collider collider);
        this.lineCtrlList[index].SetLine(index == (this.RedLineList.Count - 1), num);
        if (collider != null)
        {
            this.resultangle = Utils.ExcuteReboundWallRedLine(this.RedLineList[index].transform, collider);
        }
        float x = MathDxx.Sin(this.resultangle);
        float z = MathDxx.Cos(this.resultangle);
        this.nextpos = this.resultpos + (new Vector3(x, 0f, z) * 40f);
    }

    private void UpdateLinesData()
    {
        if (this.m_Entity.m_HatredTarget != null)
        {
            float x = this.m_Entity.m_HatredTarget.position.x - this.m_Entity.position.x;
            float y = this.m_Entity.m_HatredTarget.position.z - this.m_Entity.position.z;
            this.resultangle = Utils.getAngle(x, y) + this.offsetangle;
            this.resultpos = this.m_Entity.m_Body.LeftBullet.transform.position;
            float num3 = MathDxx.Sin(this.resultangle);
            float z = MathDxx.Cos(this.resultangle);
            Vector3 vector6 = new Vector3(num3, 0f, z);
            Vector3 normalized = vector6.normalized;
            this.nextpos = this.m_Entity.position + (normalized * 40f);
            this.lastangle = this.resultangle;
        }
    }

    private void UpdateLinesLength()
    {
        this.UpdateLinesData();
        int index = 0;
        int count = this.RedLineList.Count;
        while (index < count)
        {
            this.UpdateLineLength(index);
            index++;
        }
    }
}

