using Dxx.Util;
using System;

public class AIMove1047 : AIMove1009
{
    private int mTimeID;
    private float angle;

    public AIMove1047(EntityBase entity) : base(entity)
    {
        base.name = "AIMove1047";
        base.runString = "CastSpell";
        base.Move_BackTime = 0.8f;
        base.Move_NextDurationTime = 1.6f;
        base.runAniSpeed = 0.55f;
    }

    private void CreateBullets()
    {
        this.angle += 22f;
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13c5, base.m_Entity.m_Body.LeftBullet.transform.position, this.angle);
    }

    protected override void OnBackEvent()
    {
        this.mTimeID = TimeRegister.Register("AIMove1047", 0.05f, new Action(this.CreateBullets), false, 0f);
        this.angle = base.m_Entity.eulerAngles.y;
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        TimeRegister.UnRegister(this.mTimeID);
        base.m_Entity.SetSuperArmor(false);
    }

    protected override void OnInitBase()
    {
        base.OnInitBase();
        base.m_Entity.SetSuperArmor(true);
    }

    protected override float moveRatio =>
        24f;
}

