using System;

public class AI3032 : AIBase
{
    private int bulletid;

    protected override void OnInit()
    {
        base.AddAction(new AIMove1052(base.m_Entity, 4));
        base.AddAction(base.GetActionWait(string.Empty, 100));
        base.AddAction(base.GetActionAttack(string.Empty, this.bulletid, true));
        base.AddAction(base.GetActionWait(string.Empty, 400));
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        this.bulletid = base.m_Entity.m_Data.WeaponID;
        if (base.m_Entity.IsElite)
        {
            this.bulletid = 0x448;
        }
    }
}

