using System;

public class BulletBomberman : BulletBombBase
{
    protected override void OnInit()
    {
        base.OnInit();
        GameLogic.Release.MapCreatorCtrl.Bomberman_Use(base.transform.position, 5);
    }

    protected override void OnOverDistance()
    {
        GameLogic.Release.MapCreatorCtrl.Bomberman_Cache(base.transform.position);
        base.OnOverDistance();
    }
}

