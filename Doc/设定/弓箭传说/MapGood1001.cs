using System;
using UnityEngine;

public class MapGood1001 : MapGoodBase
{
    protected int elementid = 1;

    private void DoGood()
    {
        Transform transform = base.transform.Find("child/good");
        if (transform != null)
        {
            SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
            if (component != null)
            {
                Sprite randomElement = GameLogic.Release.MapCreatorCtrl.GetRandomElement(this.elementid);
                if (randomElement != null)
                {
                    component.sprite = randomElement;
                }
            }
        }
    }

    private void DoShadow()
    {
        Transform transform = base.transform.Find("child/shadow");
        if (transform != null)
        {
            SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
            if (component != null)
            {
                Sprite elementShadow = GameLogic.Release.MapCreatorCtrl.GetElementShadow(this.elementid);
                if (elementShadow != null)
                {
                    component.sprite = elementShadow;
                }
            }
        }
    }

    protected override void OnAwake()
    {
        this.DoGood();
        this.DoShadow();
        Object.Destroy(this);
    }
}

