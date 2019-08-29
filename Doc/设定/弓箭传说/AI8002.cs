using System;

public class AI8002 : AIBase
{
    private ActionBasic action = new ActionBasic();

    private void CreateBullet()
    {
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xfa3, base.m_Entity.m_Body.LeftBullet.transform.position, this.BulletAngle);
    }

    protected override void OnAIDeInit()
    {
        this.action.DeInit();
        MapCreator mapCreatorCtrl = GameLogic.Release.MapCreatorCtrl;
        mapCreatorCtrl.Event_Button1101 = (Action) Delegate.Remove(mapCreatorCtrl.Event_Button1101, new Action(this.OnAttack));
    }

    private void OnAttack()
    {
        this.CreateBullet();
        for (int i = 0; i < 2; i++)
        {
            this.action.AddActionWaitDelegate(0.2f, () => this.CreateBullet());
        }
    }

    protected override void OnInit()
    {
        this.action.Init(false);
        MapCreator mapCreatorCtrl = GameLogic.Release.MapCreatorCtrl;
        mapCreatorCtrl.Event_Button1101 = (Action) Delegate.Combine(mapCreatorCtrl.Event_Button1101, new Action(this.OnAttack));
    }

    protected virtual float BulletAngle =>
        270f;
}

