using System;
using UnityEngine;

public class MapGood1004 : MapGoodBase
{
    protected int elementid = 1;

    private void DoGood()
    {
        Transform transform = base.transform.Find("child/spike/spikes/sprite");
        if (transform != null)
        {
            SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
            if (component != null)
            {
                Sprite map = SpriteManager.GetMap("element1201");
                if (map != null)
                {
                    component.sprite = map;
                }
            }
        }
    }

    protected override void OnAwake()
    {
        this.DoGood();
        Object.Destroy(this);
    }
}

