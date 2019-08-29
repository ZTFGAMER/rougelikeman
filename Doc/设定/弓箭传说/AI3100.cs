using System;

public class AI3100 : AIBase
{
    private bool call;
    private int count;
    private int range = 4;
    private int callid = 0xbba;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        int num = GameLogic.Random(1, 3);
        for (int i = 0; i < num; i++)
        {
            base.AddAction(new AIMove1002(base.m_Entity, 500, 0x3e8));
        }
        this.call = GameLogic.Random(0, 100) < 50;
        if (this.call && base.GetCanCall(this.callid))
        {
            base.AddActionAddCall(this.callid, 0x439);
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.InitCallData(this.callid, 3, 0x7fffffff, 1, 1, 10);
    }
}

