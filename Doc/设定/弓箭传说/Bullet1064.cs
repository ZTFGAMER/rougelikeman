using DG.Tweening;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet1064 : BulletBase
{
    private GameObject fire;

    protected override void AwakeInit()
    {
        base.Parabola_MaxHeight = 4f;
        this.fire = base.mTransform.Find("fire").gameObject;
    }

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.BulletModelShow(true);
        this.fire.SetActive(false);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        base.Speed += 0.1f;
    }

    protected override void ParabolaOver()
    {
        base.mTransform.rotation = Quaternion.identity;
        base.bMoveEnable = false;
        base.BulletModelShow(false);
        this.fire.SetActive(true);
        this.BoxEnable(true);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(base.mSeqPool.Get(), base.m_Data.AliveTime - 0.4f), new TweenCallback(this, this.<ParabolaOver>m__0));
        base.mCondition = AIMoveBase.GetConditionTime(base.m_Data.AliveTime);
    }

    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        if (base.PosFromStart2Target < 3f)
        {
            base.PosFromStart2Target = 3f;
        }
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
        if (base.childMesh != null)
        {
            base.childMesh.localRotation = Quaternion.Euler(0f, 0f, base.childMesh.localEulerAngles.z + 20f);
        }
    }

    protected override bool bFlyCantHit =>
        true;
}

