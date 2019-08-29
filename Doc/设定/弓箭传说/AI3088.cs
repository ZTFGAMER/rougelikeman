using System;

public class AI3088 : AIBase
{
    private int ran;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        this.ran = GameLogic.Random(2, 5);
        for (int i = 0; i < this.ran; i++)
        {
            base.AddAction(new AIMove1002(base.m_Entity, 800, 0x5dc));
        }
        base.AddAction(base.GetActionDelegate(delegate {
            for (int j = 0; j < 4; j++)
            {
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, base.m_Entity.m_Data.WeaponID, base.m_Entity.m_Body.EffectMask.transform.position, j * 90f).transform.SetParent(base.m_Entity.transform);
            }
        }));
        base.bReRandom = true;
    }
}

