using System;

public class ScrollIntStageListCtrl : ScrollIntCtrl<StageListOneCtrl>
{
    protected override void Awake()
    {
        base.SetScale(1f, 1.75f);
        base.Awake();
    }
}

