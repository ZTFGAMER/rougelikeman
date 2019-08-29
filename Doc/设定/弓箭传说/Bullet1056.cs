using Dxx.Util;
using System;
using UnityEngine;

public class Bullet1056 : BulletBase
{
    private GameObject mStartObj;
    private GameObject mEndObj;
    private LineRenderer line;
    private const float textureLengthScale = 3f;
    private const float textureScrollSpeed = 8f;
    private float range;
    private float hitratio;

    public void CheckBulletLength()
    {
        float x = MathDxx.Sin(base.bulletAngle);
        float z = MathDxx.Cos(base.bulletAngle);
        Vector3 vector = new Vector3(x, 0f, z);
        this.mStartObj.transform.position = base.mTransform.position;
        Vector3 position = base.mTransform.position + (vector * this.range);
        this.mEndObj.transform.position = position;
        this.line.positionCount = 2;
        this.line.SetPosition(0, base.mTransform.position);
        this.line.SetPosition(1, position);
        float num3 = Vector3.Distance(base.mTransform.position, position);
        this.line.material.mainTextureScale = new Vector2(num3 / 3f, 1f);
        Material material = this.line.material;
        material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
        base.boxList[0].center = new Vector3(0f, 0f, this.range / 2f);
        base.boxList[0].size = new Vector3(base.boxList[0].size.x, base.boxList[0].size.y, this.range);
    }

    public void InitData(float range, float hitratio)
    {
        this.range = range;
        this.hitratio = hitratio;
        base.mBulletTransmit.SetAttack((long) (base.m_Entity.m_EntityData.GetAttackBase() * hitratio));
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.mStartObj = base.mBulletModel.Find("FireBeamStart").gameObject;
        this.mEndObj = base.mBulletModel.Find("FireBeamEnd").gameObject;
        this.line = base.mBulletModel.Find("Fire Beam").GetComponent<LineRenderer>();
        this.line.sortingLayerName = "Player";
        this.line.sortingOrder = 0;
        this.CheckBulletLength();
    }

    protected override void OnUpdate()
    {
        base.bulletAngle += 5f;
        base.UpdateMoveDirection();
        this.CheckBulletLength();
    }
}

