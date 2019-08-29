using Dxx.Util;
using System;

public class Weapon1096 : Weapon1020
{
    protected override void OnAttack(object[] args)
    {
        if (MathDxx.RandomBool())
        {
            int count = 3;
            for (int i = 0; i < count; i++)
            {
                base.CreateBullet1020(Utils.GetBulletAngle(i, count, 90f));
            }
        }
        else
        {
            int num3 = 3;
            for (int i = 0; i < num3; i++)
            {
                base.action.AddActionDelegate(() => base.CreateBullet1020(0f));
                base.action.AddActionWait(0.15f);
            }
        }
    }

    protected override void OnInstall()
    {
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
    }
}

