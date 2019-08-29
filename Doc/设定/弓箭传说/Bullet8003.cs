using System;
using UnityEngine;

public class Bullet8003 : BulletBase
{
    private Transform trailattparent;
    private Material mMaterial;
    private Color meshColor;
    private bool bHasColor;
    private Transform trailtran;
    private TrailRenderer trailrender;

    protected override void AwakeInit()
    {
        base.meshAlphaAction = new Action<float>(this.OnMeshAlpha);
        base.HitWallAction = new Action<Collider>(this.OnHitWalls);
        base.OnTrailShowEvent = new Action<bool>(this.OnTrailShowEvents);
    }

    protected override void BoxEnable(bool enable)
    {
        base.BoxEnable(enable);
    }

    protected override Transform GetTrailAttParent()
    {
        if (this.trailattparent != null)
        {
            return this.trailattparent;
        }
        return base.mTransform;
    }

    protected override void OnHitHero(EntityBase entity)
    {
    }

    private void OnHitWalls(Collider o)
    {
        base.m_Entity.PlayEffect(0x157c03, base.mTransform.position);
        base.Catapult();
    }

    protected override void OnInit()
    {
        base.OnInit();
        if ((this.mMaterial == null) && (base.childMeshRender != null))
        {
            this.mMaterial = base.childMeshRender.sharedMaterial;
            this.bHasColor = this.mMaterial.HasProperty("_Color");
            if (this.bHasColor)
            {
                this.meshColor = this.mMaterial.GetColor("_Color");
            }
        }
        if (this.bHasColor)
        {
            this.mMaterial.SetColor("_Color", this.meshColor);
        }
        if (base.childMeshRender != null)
        {
            base.childMeshRender.sharedMaterial = this.mMaterial;
        }
        this.trailattparent = base.mBulletModel.Find("child/child/trailattparent");
        this.trailtran = base.mBulletModel.Find("child/child/trail");
        if (this.trailtran != null)
        {
            this.trailrender = this.trailtran.GetComponent<TrailRenderer>();
        }
    }

    private void OnMeshAlpha(float value)
    {
        if (this.bHasColor)
        {
            this.mMaterial.SetColor("_Color", new Color(this.meshColor.r, this.meshColor.g, this.meshColor.b, value));
        }
        if (base.childMeshRender != null)
        {
            base.childMeshRender.material = this.mMaterial;
        }
    }

    protected override void OnOverDistance()
    {
    }

    protected override void OnThroughTrailShow(bool show)
    {
        if (base.mTrailCtrl != null)
        {
            if (show)
            {
                base.mTrailCtrl.SetTrailTime(2f);
            }
            else
            {
                base.mTrailCtrl.SetTrailTime(1f);
            }
        }
    }

    private void OnTrailShowEvents(bool show)
    {
        if (this.trailtran != null)
        {
            this.trailtran.gameObject.SetActive(show);
        }
        if (this.trailrender != null)
        {
            this.trailrender.Clear();
        }
    }

    protected override void OnUpdate()
    {
        float frameDistance = base.FrameDistance;
        base.UpdateMoveDirection();
        base.mTransform.position += new Vector3(base.moveX, 0f, base.moveY) * frameDistance;
        base.childMesh.rotation = Quaternion.Euler(base.childMesh.eulerAngles.x, base.childMesh.eulerAngles.y + base.m_Data.RotateSpeed, base.childMesh.eulerAngles.z);
        base.CurrentDistance += frameDistance;
        if (base.CurrentDistance >= base.Distance)
        {
            this.overDistance();
        }
    }
}

