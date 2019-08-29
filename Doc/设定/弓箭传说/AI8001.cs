using PureMVC.Patterns;
using System;

public class AI8001 : AIBase
{
    private ActionBattle action = new ActionBattle();
    private int ran;
    private int waveid;
    private Action<int> onWaveUpdate;

    private void CreateBullets1_0()
    {
        float num = GameLogic.Random((float) 0f, (float) 100f);
        int num2 = 4;
        float num3 = 360f / ((float) num2);
        for (int i = 0; i < num2; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x40e, base.m_Entity.m_Body.LeftBullet.transform.position, (num3 * i) + num);
        }
    }

    private void CreateBullets2_0()
    {
        float num = GameLogic.Random((float) 0f, (float) 100f);
        int num2 = 8;
        float num3 = 360f / ((float) num2);
        for (int i = 0; i < num2; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x40e, base.m_Entity.m_Body.LeftBullet.transform.position, (num3 * i) + num);
        }
    }

    private void CreateBullets3_0()
    {
        float num = GameLogic.Random((float) 0f, (float) 100f);
        int num2 = 0x10;
        float num3 = 360f / ((float) num2);
        for (int i = 0; i < num2; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x40e, base.m_Entity.m_Body.LeftBullet.transform.position, (num3 * i) + num);
        }
    }

    private void CreateWave1()
    {
        base.AddAction(base.GetActionWaitDelegate(500, () => this.CreateBullets1_0()));
    }

    private void CreateWave2()
    {
        base.AddAction(base.GetActionWaitDelegate(500, () => this.CreateBullets2_0()));
    }

    private void CreateWave3()
    {
        base.AddAction(base.GetActionWaitDelegate(500, () => this.CreateBullets3_0()));
    }

    protected override void OnAIDeInit()
    {
        this.action.DeInit();
    }

    protected override void OnInit()
    {
        switch (this.waveid)
        {
            case 1:
                this.CreateWave1();
                break;

            case 2:
                this.CreateWave2();
                break;

            case 3:
                this.CreateWave3();
                break;

            case 0:
                return;
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        this.action.Init(base.m_Entity);
        this.onWaveUpdate = new Action<int>(this.OnWaveUpdate);
        Facade.Instance.SendNotification("UpdateWave", this.onWaveUpdate);
    }

    private void OnWaveUpdate(int waveid)
    {
        this.waveid = waveid;
        base.ReRandomAI();
    }
}

