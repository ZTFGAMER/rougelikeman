using System;
using UnityEngine;

public class Weapon1027 : Weapon1024
{
    private int bulletcount = 4;
    private float perangle;

    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < this.bulletcount; i++)
        {
            base.CreateBulletOverride(Vector3.zero, this.perangle * i);
        }
    }

    protected override void OnInstall()
    {
        this.perangle = 360f / ((float) this.bulletcount);
        base.effectparent = base.m_Entity.m_Body.BulletList[0].transform;
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
    }
}

