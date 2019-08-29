using System;

public class HittedData
{
    public EHittedType type = EHittedType.eNormal;
    public float hitratio = 1f;
    public float backtatio = 1f;
    public BulletBase bullet;
    public HitType hittype;

    public void AddBackRatio(float back)
    {
        this.backtatio *= back;
    }

    public bool GetCanHitted() => 
        (this.type != EHittedType.eInvincible);

    public bool GetPlayHitted() => 
        (this.type != EHittedType.eDefence);

    public void SetBullet(BulletBase bullet)
    {
        this.bullet = bullet;
    }

    public float angle
    {
        get
        {
            if (this.bullet != null)
            {
                return this.bullet.transform.eulerAngles.y;
            }
            return 0f;
        }
    }
}

