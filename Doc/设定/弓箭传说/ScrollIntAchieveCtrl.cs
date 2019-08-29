using System;

public class ScrollIntAchieveCtrl : ScrollIntCtrl<AchieveItemCtrl>
{
    protected override void Awake()
    {
        base.SetScale(1f, 1.25f);
        base.Awake();
    }
}

