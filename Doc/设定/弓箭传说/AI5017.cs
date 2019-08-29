using System;

public class AI5017 : AIBase
{
    private int attackcount;
    private int[] attackids = new int[] { 0x13ab, 0x1405, 0x1406 };

    protected override void OnInit()
    {
        base.AddAction(new AIMove1029(base.m_Entity, 3));
        int index = GameLogic.Random(0, this.attackids.Length);
        base.AddAction(base.GetActionAttack(string.Empty, this.attackids[index], true));
        base.bReRandom = true;
    }
}

