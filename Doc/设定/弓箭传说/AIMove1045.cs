using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AIMove1045 : AIMoveBase
{
    private EntityBase target;
    private float findrange;
    private Vector3 dir;
    private int flyframe;
    private float g;
    private float endx;
    private float endz;
    private float perendx;
    private float perendz;
    private float delaytime;
    private float starttime;
    private float alltime;
    private float halftime;
    private Vector3 startpos;

    public AIMove1045(EntityBase entity, float findrange) : base(entity)
    {
        this.flyframe = 20;
        this.g = 3f;
        this.alltime = 0.6f;
        this.findrange = findrange;
    }

    private void MoveNormal()
    {
        if (Updater.AliveTime >= this.delaytime)
        {
            this.OnFly();
        }
    }

    private void OnFly()
    {
        float num = 1f - (((this.delaytime + this.alltime) - Updater.AliveTime) / this.alltime);
        float num2 = (((-4f * this.g) * (num - 0.5f)) * (num - 0.5f)) + this.g;
        if (Updater.AliveTime < (this.delaytime + this.halftime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + ((this.perendx * num) * this.flyframe), this.startpos.y + num2, this.startpos.z + ((this.perendz * num) * this.flyframe)));
        }
        else if (Updater.AliveTime < (this.delaytime + this.alltime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + ((this.perendx * num) * this.flyframe), this.startpos.y + num2, this.startpos.z + ((this.perendz * num) * this.flyframe)));
        }
        else
        {
            base.m_Entity.SetPosition(new Vector3(base.m_Entity.position.x, 0f, base.m_Entity.position.z));
            base.End();
        }
    }

    protected override void OnInitBase()
    {
        this.delaytime = Updater.AliveTime + 0.15f;
        this.starttime = Updater.AliveTime;
        this.halftime = this.alltime / 2f;
        this.startpos = base.m_Entity.position;
        this.target = GameLogic.Self;
        Vector3 vector = base.m_Entity.position - this.target.position;
        if (vector.magnitude < this.findrange)
        {
            this.endx = this.target.position.x;
            this.endz = this.target.position.z;
        }
        else
        {
            GameLogic.Release.MapCreatorCtrl.RandomItem(base.m_Entity, 3, out this.endx, out this.endz);
        }
        this.perendx = (this.endx - base.m_Entity.position.x) / ((float) this.flyframe);
        this.perendz = (this.endz - base.m_Entity.position.z) / ((float) this.flyframe);
        float angle = Utils.getAngle(this.perendx, this.perendz);
        base.m_Entity.m_AttackCtrl.RotateHero(angle);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    private void RandomItem(out float endx, out float endz)
    {
        int[,] findPathRect = GameLogic.Release.MapCreatorCtrl.GetFindPathRect();
        int width = GameLogic.Release.MapCreatorCtrl.width;
        int height = GameLogic.Release.MapCreatorCtrl.height;
        Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.m_Entity.position);
        int num4 = 1;
        List<Vector2Int> list = new List<Vector2Int>();
        int x = roomXY.x - num4;
        int num6 = (roomXY.x + num4) + 1;
        while (x < num6)
        {
            if ((x >= 0) && (x < width))
            {
                int y = roomXY.y - num4;
                int num8 = (roomXY.y + num4) + 1;
                while (y < num8)
                {
                    if ((((y >= 0) && (y < height)) && ((x != roomXY.x) || (y != roomXY.y))) && (((x == roomXY.x) || (y == roomXY.y)) && (findPathRect[x, y] == 0)))
                    {
                        list.Add(new Vector2Int(x, y));
                    }
                    y++;
                }
            }
            x++;
        }
        if (list.Count == 0)
        {
            endx = base.m_Entity.position.x;
            endz = base.m_Entity.position.z;
        }
        else
        {
            int num9 = GameLogic.Random(0, list.Count);
            Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(list[num9]);
            endx = worldPosition.x;
            endz = worldPosition.z;
        }
    }
}

