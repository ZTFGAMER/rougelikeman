using System;
using UnityEngine;

public class Goods1101 : GoodsBase
{
    private Transform button;
    private float y_up = 0.128f;
    private float y_down = 0.07f;

    public override void ChildTriggerEnter(GameObject o)
    {
        if ((GameLogic.Self != null) && (o == GameLogic.Self.gameObject))
        {
            this.button.transform.localPosition = new Vector3(this.button.transform.localPosition.x, this.y_down, this.button.transform.localPosition.z);
            if (GameLogic.Release.MapCreatorCtrl.Event_Button1101 != null)
            {
                GameLogic.Release.MapCreatorCtrl.Event_Button1101();
            }
        }
    }

    public override void ChildTriggetExit(GameObject o)
    {
        if (o == GameLogic.Self.gameObject)
        {
            this.button.transform.localPosition = new Vector3(this.button.transform.localPosition.x, this.y_up, this.button.transform.localPosition.z);
        }
    }

    protected override void Init()
    {
        base.Init();
        this.button = base.transform.Find("button");
    }

    protected override void StartInit()
    {
        base.StartInit();
    }
}

