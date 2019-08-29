using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeadGoodCtrl
{
    private Action callback;
    private bool bStart;
    private int currentFrame;
    private int currentindex;
    private List<BattleDropData> goodslist;
    private List<Vector2Int> Goldslist;
    private Vector2Int GoldsCenter;
    private Transform MapGoodsDrop;
    private Vector3 pos;

    public void DeInit()
    {
        this.bStart = false;
        Updater.RemoveUpdate("DeadGoodCtrl", new Action<float>(this.OnUpdate));
    }

    private Vector3 GetRandomEndPosition(List<Vector2Int> Goldslist, Vector2Int GoldsCenter)
    {
        Vector2Int num;
        Vector3 vector2;
        float max = 0.45f;
        Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(GoldsCenter);
        if ((Random.Range(0, 100) < 30) && GameLogic.Release.MapCreatorCtrl.IsEmpty(GoldsCenter))
        {
            num = GoldsCenter;
            vector2 = new Vector3(worldPosition.x + Random.Range(-max, max), 0f, worldPosition.z + Random.Range(-max, max));
        }
        else
        {
            int num3 = Random.Range(0, Goldslist.Count);
            num = Goldslist[num3];
            worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(num);
            vector2 = new Vector3(worldPosition.x + Random.Range(-max, max), 0f, worldPosition.z + Random.Range(-max, max));
        }
        Vector3 zero = Vector3.zero;
        if (num.x == 0)
        {
            zero += new Vector3(0.15f, 0f, 0f);
        }
        else if (num.x == (GameLogic.Release.MapCreatorCtrl.width - 1))
        {
            zero += new Vector3(-0.15f, 0f, 0f);
        }
        if (num.y == 0)
        {
            zero += new Vector3(0f, 0f, -0.2f);
        }
        else if (num.y == (GameLogic.Release.MapCreatorCtrl.height - 1))
        {
            zero += new Vector3(0f, 0f, 0.2f);
        }
        return (vector2 + zero);
    }

    public void Init()
    {
        Updater.AddUpdate("DeadGoodCtrl", new Action<float>(this.OnUpdate), false);
    }

    private void OnUpdate(float delta)
    {
        if (this.bStart)
        {
            if ((this.currentFrame % 3) == 0)
            {
                if (((this.goodslist == null) || (this.currentindex >= this.goodslist.Count)) || (this.currentindex < 0))
                {
                    if (this.callback != null)
                    {
                        this.callback();
                    }
                    this.bStart = false;
                    return;
                }
                string str = string.Empty;
                switch (this.goodslist[this.currentindex].type)
                {
                    case FoodType.eHP:
                    case FoodType.eGold:
                    case FoodType.eExp:
                        str = this.goodslist[this.currentindex].childtype.ToString();
                        break;

                    case FoodType.eEquip:
                    {
                        LocalSave.EquipOne data = this.goodslist[this.currentindex].data as LocalSave.EquipOne;
                        str = "1010101";
                        break;
                    }
                }
                object[] args = new object[] { "Game/Food/", str };
                GameObject obj2 = GameLogic.EffectGet(Utils.GetString(args));
                obj2.transform.parent = this.MapGoodsDrop;
                obj2.transform.position = new Vector3(this.pos.x, 0f, this.pos.z);
                obj2.transform.localScale = Vector3.one;
                FoodBase component = obj2.GetComponent<FoodBase>();
                component.Init(this.goodslist[this.currentindex].data);
                Vector3 randomEndPosition = this.GetRandomEndPosition(this.Goldslist, this.GoldsCenter);
                component.SetEndPosition(this.pos, randomEndPosition);
                this.currentindex++;
            }
            this.currentFrame++;
        }
    }

    public void StartDrop(Vector3 pos, List<BattleDropData> goodslist, int radius, Transform MapGoodsDrop, Action callback)
    {
        this.callback = callback;
        this.pos = pos;
        this.goodslist = goodslist;
        this.MapGoodsDrop = MapGoodsDrop;
        this.GoldsCenter = new Vector2Int();
        this.Goldslist = GameLogic.Release.MapCreatorCtrl.GetNearEmptyList(pos, ref this.GoldsCenter, radius);
        this.bStart = true;
        this.currentFrame = 0;
        this.currentindex = 0;
        if (goodslist.Count == 0)
        {
            this.bStart = false;
        }
    }
}

