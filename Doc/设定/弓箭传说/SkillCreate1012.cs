using DG.Tweening;
using System;
using UnityEngine;

public class SkillCreate1012 : SkillCreateBase
{
    private Transform child;
    private float hitratio;
    private float range;
    private Sequence seq;
    private Bullet1056 bullet;

    protected override void OnAwake()
    {
        this.child = base.transform.Find("child");
    }

    protected override void OnDeinit()
    {
        if (this.bullet != null)
        {
            this.bullet.DeInit();
            this.bullet = null;
        }
    }

    protected override void OnInit(string[] args)
    {
        base.time = float.Parse(args[0]);
        this.range = float.Parse(args[1]);
        this.hitratio = float.Parse(args[2]);
        this.bullet = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x420, this.child.position, 0f) as Bullet1056;
        this.bullet.InitData(this.range, this.hitratio);
    }
}

