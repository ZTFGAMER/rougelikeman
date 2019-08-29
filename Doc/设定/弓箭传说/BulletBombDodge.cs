using Dxx.Util;
using System;
using UnityEngine;

public class BulletBombDodge : BulletBase
{
    [Header("延迟时间")]
    public float DelayTime = 1f;
    [Header("爆炸冲击时间")]
    public float BombTime = 0.5f;
    private Transform effect;
    private Vector3 shadowScaleInit = (Vector3.one * -1f);
    private float height;
    private Vector3 endpos;
    private Vector3 dir;
    private bool bStartBomb;
    private float showCircleTime = 0.5f;
    private const float MaxColliderSize = 20f;
    private float mDelaytime;
    private float mBombTime;
    private bool bColliderUpdate;
    private float addspeed;

    protected override void AwakeInit()
    {
        base.Parabola_MaxHeight = 4f;
        this.effect = base.transform.Find("effect");
    }

    private void create_divide()
    {
        int num = 4;
        float num2 = 360f / ((float) num);
        for (int i = 0; i < num; i++)
        {
            BulletBase base2 = GameLogic.Release.Bullet.CreateBulletInternal(base.m_Entity, 0xbdb, new Vector3(base.mTransform.position.x, 0f, base.mTransform.position.z), i * num2, true);
            base2.SetTarget(null, 1);
            base2.mBulletTransmit.SetAttack(base.mBulletTransmit.GetAttack());
        }
    }

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.OnInit();
        if ((this.shadowScaleInit.x < 0f) && (base.shadow != null))
        {
            this.shadowScaleInit = base.shadow.localScale;
        }
        this.height = base.transform.position.y;
        this.mDelaytime = this.DelayTime;
        this.mBombTime = 0f;
        this.bColliderUpdate = false;
        this.BoxEnable(false);
        base.childMesh.gameObject.SetActive(true);
        this.bStartBomb = false;
        this.addspeed = 1f;
        this.SetEffectScale(0f);
        this.SetEffectShow(false);
        SpriteRenderer component = base.mBulletModel.Find("shadow/child/shadow").GetComponent<SpriteRenderer>();
        component.sortingLayerName = "Default";
        component.sortingOrder = 1;
        SpriteRenderer renderer2 = base.mBulletModel.Find("child/bomb/sprite").GetComponent<SpriteRenderer>();
        renderer2.sortingLayerName = "Default";
        renderer2.sortingOrder = 1;
    }

    protected override void OnUpdate()
    {
        if (!this.bStartBomb)
        {
            Vector3 vector = base.mTransform.position - this.endpos;
            this.dir = vector.normalized;
            this.addspeed *= 1.05f;
            base.mTransform.position = Vector3.MoveTowards(base.mTransform.position, this.endpos, base.FrameDistance * this.addspeed);
            Vector3 vector2 = base.mTransform.position - this.endpos;
            if (vector2.magnitude < 0.1f)
            {
                this.bStartBomb = true;
            }
            float num = base.transform.position.y / this.height;
            if (base.shadow != null)
            {
                base.shadow.localScale = ((this.shadowScaleInit * (1f - num)) * 0.7f) + (this.shadowScaleInit * 0.3f);
            }
        }
    }

    private void SetEffectScale(float value)
    {
        if (this.effect != null)
        {
            this.effect.localScale = new Vector3(value, 1f, value);
        }
    }

    private void SetEffectShow(bool value)
    {
        if (this.effect != null)
        {
            this.effect.gameObject.SetActive(value);
        }
    }

    public void SetEndPos(Vector3 endpos)
    {
        this.endpos = endpos;
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
        if (this.bStartBomb)
        {
            this.mDelaytime -= Updater.delta;
            if ((this.mDelaytime <= 0f) && !this.bColliderUpdate)
            {
                this.bColliderUpdate = true;
                this.mDelaytime = 0f;
                base.childMesh.gameObject.SetActive(false);
                if (base.shadowGameObject != null)
                {
                    base.shadowGameObject.SetActive(false);
                }
                this.SetEffectShow(true);
                GameLogic.Hold.Sound.PlayBulletDead(0x231c4b, base.transform.position);
                this.create_divide();
                this.overDistance();
            }
            if (this.bColliderUpdate)
            {
                this.mBombTime += Updater.delta;
                this.SetEffectScale((this.mBombTime / this.BombTime) * 2f);
                if (base.boxListCount == 2)
                {
                    base.boxList[0].size = new Vector3((this.mBombTime / this.BombTime) * 20f, 1f, base.boxList[0].size.z);
                    base.boxList[1].size = new Vector3(base.boxList[1].size.x, 1f, (this.mBombTime / this.BombTime) * 20f);
                }
                if (this.mBombTime >= 1f)
                {
                    this.overDistance();
                }
                else if (this.mBombTime >= this.BombTime)
                {
                    this.BoxEnable(false);
                }
            }
        }
    }
}

