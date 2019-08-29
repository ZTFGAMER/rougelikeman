using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5028 : WeaponBase
{
    private float getRandomAngle() => 
        (GameLogic.Random((float) 3f, (float) 10f) * ((GameLogic.Random(0, 2) != 0) ? ((float) (-1)) : ((float) 1)));

    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 2; i++)
        {
            <OnAttack>c__AnonStorey0 storey = new <OnAttack>c__AnonStorey0 {
                $this = this,
                index = i
            };
            base.action.AddActionDelegate(new Action(storey.<>m__0));
            base.action.AddActionWait(0.2f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey0
    {
        internal int index;
        internal Weapon5028 $this;

        internal void <>m__0()
        {
            this.$this.CreateBulletOverride(Vector3.zero, (this.index != 0) ? this.$this.getRandomAngle() : 0f);
        }
    }
}

