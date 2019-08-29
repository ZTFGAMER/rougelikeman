using System;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public class RoomControl : RoomControlBase
{
    private static Color[] ColorLights = new Color[] { Color.white, new Color(0.9686275f, 1f, 0f, 1f), new Color(0f, 1f, 0.9568627f, 1f), new Color(1f, 0.6f, 0f, 1f), new Color(0f, 0.6705883f, 1f, 1f) };
    private static Color[] ColorShadows = new Color[] { Color.white, new Color(0.1372549f, 0.2862745f, 0.02745098f, 1f), new Color(0.1372549f, 0.2862745f, 0.02745098f, 1f), new Color(0.5058824f, 0.345098f, 0.01176471f, 1f), new Color(0.03921569f, 0.3882353f, 0.3686275f, 1f) };
    private const string DoorAnimationName = "MapDoor_Miss";
    private RoomGenerateBase.Room room;
    private RoomGenerateBase.Room nextRoom;
    private RoomGateCtrl mGateCtrl;
    private GameObject layerObj;
    private GameObject bossObj;

    private void DoorDownShow()
    {
    }

    private void ExcuteLayer()
    {
        if (this.nextRoom != null)
        {
            bool flag = false;
            if (this.room != null)
            {
                flag = this.room.RoomID == 0;
            }
            this.layerObj.SetActive(!this.nextRoom.IsBossRoom && !flag);
            this.bossObj.SetActive(this.nextRoom.IsBossRoom);
        }
        else
        {
            this.layerObj.SetActive((this.room == null) || (this.room.RoomID > 0));
            this.bossObj.SetActive(false);
        }
    }

    private void InitStage3()
    {
        if (this.room != null)
        {
            Transform transform = base.transform.Find("WallUp/nowater");
            if (transform != null)
            {
                transform.gameObject.SetActive(!GameLogic.Release.MapCreatorCtrl.GetStage3MiddleWater());
            }
        }
    }

    protected override void OnAwake()
    {
        this.layerObj = base.transform.Find("WallUp/Layer/Map_LayerIcon").gameObject;
        this.bossObj = base.transform.Find("WallUp/Layer/BossParent").gameObject;
        this.mGateCtrl = base.transform.Find("WallUp/gate").GetComponent<RoomGateCtrl>();
    }

    protected override void OnClearGoods()
    {
        base.GoodsParent.DestroyChildren();
    }

    protected override void OnClearGoodsDrop()
    {
        base.GoodsDropParent.DestroyChildren();
    }

    protected override Transform OnGetGoodsDropParent() => 
        base.GoodsDropParent;

    protected override void OnInit(object data = null)
    {
        base.OpenDoor(false);
        GameLogic.Hold.Guide.GuideBattleNext();
        if (data != null)
        {
            RoomControlBase.Mode_LevelData data2 = (RoomControlBase.Mode_LevelData) data;
            if (data2 != null)
            {
                this.room = data2.room;
                this.nextRoom = data2.nextroom;
            }
        }
        if (this.room != null)
        {
            base.SetLayer(this.room.RoomID);
        }
        this.DoorDownShow();
        this.ExcuteLayer();
        this.OnInitStage();
    }

    private void OnInitStage()
    {
        if (LocalModelManager.Instance.Stage_Level_stagechapter.GetStyleID() == 3)
        {
            this.InitStage3();
        }
    }

    protected override void OnLayerShow(bool value)
    {
        this.layerObj.SetActive(value);
        this.bossObj.SetActive(false);
    }

    protected override void OnOpenDoor(bool show)
    {
        base.m_bOpenDoor = show;
        this.mGateCtrl.OpenDoor(show);
    }

    protected override void OnSetText(string value)
    {
        base.SetLayer(value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            this.OnOpenDoor(true);
        }
    }
}

