using System;
using UnityEngine;

public class LifeLineCtrl : MonoBehaviour
{
    private GameObject mStartObj;
    private GameObject mEndObj;
    private LineRenderer line;
    private const float textureLengthScale = 3f;
    private const float textureScrollSpeed = 8f;
    private EntityBase m_Entity;
    private EntityBase entity;
    public Action mCacheEvent;
    private bool bStart;

    private void Awake()
    {
        this.mStartObj = base.transform.Find("LifeBeamStart").gameObject;
        this.mEndObj = base.transform.Find("LifeBeamEnd").gameObject;
        this.line = base.transform.Find("Life Beam").GetComponent<LineRenderer>();
        this.line.sortingLayerName = "Hit";
    }

    public void Cache()
    {
        this.bStart = false;
        GameLogic.EffectCache(base.gameObject);
    }

    private void Update()
    {
        if (this.bStart)
        {
            if (((this.m_Entity != null) && !this.m_Entity.GetIsDead()) && ((this.entity != null) && !this.entity.GetIsDead()))
            {
                Vector3 position = this.entity.m_Body.EffectMask.transform.position;
                base.transform.LookAt(position);
                this.mStartObj.transform.position = this.m_Entity.m_Body.EffectMask.transform.position;
                this.mEndObj.transform.position = position;
                this.line.positionCount = 2;
                this.line.SetPosition(0, this.mStartObj.transform.position);
                this.line.SetPosition(1, position);
                float num = Vector3.Distance(this.mStartObj.transform.position, position);
                this.line.material.mainTextureScale = new Vector2(num / 3f, 1f);
                Material material = this.line.material;
                material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
            }
            else
            {
                if (this.mCacheEvent != null)
                {
                    this.mCacheEvent();
                }
                this.Cache();
            }
        }
    }

    public void UpdateEntity(EntityBase m_Entity, EntityBase entity)
    {
        this.m_Entity = m_Entity;
        this.entity = entity;
        this.bStart = true;
    }
}

