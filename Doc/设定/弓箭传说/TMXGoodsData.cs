using System;

public class TMXGoodsData
{
    public TMXGoodsType Type;
    public TMXGoodsParentType ParentType;
    private int TypeId;
    private int GoodsID;
    public int x;
    public int y;

    public TMXGoodsData()
    {
    }

    public TMXGoodsData(int goodsid, int typeid)
    {
        this.SetGoodsId(goodsid);
        this.Init(typeid);
    }

    public void Init(int typeid)
    {
        this.TypeId = typeid;
        this.Type = (TMXGoodsType) typeid;
        if (typeid < 100)
        {
            this.ParentType = TMXGoodsParentType.Obstacle_GroundUp;
        }
        else if (typeid < 200)
        {
            this.ParentType = TMXGoodsParentType.Obstacle_GroundDown;
        }
        else if (typeid < 300)
        {
            this.ParentType = TMXGoodsParentType.Through_Trap;
        }
        else if (typeid < 400)
        {
            this.ParentType = TMXGoodsParentType.Food;
        }
        else if (typeid < 900)
        {
            this.ParentType = TMXGoodsParentType.Equip;
        }
    }

    public bool IsEmpty() => 
        ((this.ParentType != TMXGoodsParentType.Obstacle_GroundUp) && (this.ParentType != TMXGoodsParentType.Obstacle_GroundDown));

    public void SetGoodsId(int goodsid)
    {
        this.GoodsID = goodsid;
    }
}

