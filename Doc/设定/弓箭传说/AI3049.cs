using System;

public class AI3049 : AIBase
{
    private ActionBattle action = new ActionBattle();
    private int bulletid;

    private void CreateBullets()
    {
        int num = 10;
        float num2 = 360f / ((float) num);
        for (int i = 0; i < num; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, this.bulletid, base.m_Entity.m_Body.LeftBullet.transform.position, num2 * i);
        }
    }

    protected override void OnAIDeInit()
    {
        this.action.DeInit();
    }

    protected override void OnInit()
    {
        this.bulletid = base.m_Entity.m_Data.WeaponID;
        this.action.Init(base.m_Entity);
        base.AddAction(base.GetActionWait(string.Empty, 0xbb8));
        base.AddAction(base.GetActionDelegate(() => this.CreateBullets()));
    }
}

