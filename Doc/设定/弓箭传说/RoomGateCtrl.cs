using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class RoomGateCtrl : MonoBehaviour
{
    public SpriteRenderer gata1;
    private GameObject effect;

    public void OpenDoor(bool value)
    {
        if ((this.effect == null) && (this.gata1 != null))
        {
            object[] args = new object[] { LocalModelManager.Instance.Stage_Level_stagechapter.GetStyleID() };
            string path = Utils.FormatString("Game/Map/DoorEffect/dooreffect{0:D2}", args);
            this.effect = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(path));
            this.effect.SetParentNormal(this.gata1.transform);
            this.effect.SetActive(false);
        }
        if (value)
        {
            if (this.gata1 != null)
            {
                Sprite map = SpriteManager.GetMap("gateopen");
                if (map != null)
                {
                    this.gata1.sprite = map;
                }
                else
                {
                    this.gata1.sprite = null;
                }
            }
        }
        else if (this.gata1 != null)
        {
            Sprite map = SpriteManager.GetMap("gateclose");
            if (map != null)
            {
                this.gata1.sprite = map;
            }
            else
            {
                this.gata1.sprite = null;
            }
        }
        if (this.effect != null)
        {
            this.effect.SetActive(value);
        }
    }
}

