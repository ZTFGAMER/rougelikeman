using Dxx.Util;
using System;
using UnityEngine;

public class BodyMaskCamera
{
    private EntityBase m_Entity;
    private float updateTime = 0.1f;
    private float currentTime;
    private RoomState state = RoomState.Runing;

    public BodyMaskCamera(EntityBase entity)
    {
        this.m_Entity = entity;
        if (!this.m_Entity.IsSelf)
        {
            object[] args = new object[] { this.m_Entity.m_EntityData.CharID };
            Updater.AddUpdate(Utils.FormatString("{0}.BodyMaskCamera", args), new Action<float>(this.OnUpdate), false);
        }
    }

    public void DeInit()
    {
        object[] args = new object[] { this.m_Entity.m_EntityData.CharID };
        Updater.RemoveUpdate(Utils.FormatString("{0}.BodyMaskCamera", args), new Action<float>(this.OnUpdate));
    }

    private void OnUpdate(float delta)
    {
        if (((this.m_Entity != null) && (this.m_Entity.m_Body != null)) && (GameLogic.Release.Game.RoomState == RoomState.Runing))
        {
            if (this.state == RoomState.Throughing)
            {
                this.currentTime = -10f;
            }
            this.state = GameLogic.Release.Game.RoomState;
            if ((Updater.AliveTime - this.currentTime) > this.updateTime)
            {
                Vector3 vector = Utils.World2Screen(this.m_Entity.m_Body.transform.position);
                if (((vector.x < 0f) || (vector.x > this.Width)) || ((vector.y < 0f) || (vector.y > this.Height)))
                {
                    if (this.m_Entity.m_Body.GetIsInCamera())
                    {
                        this.m_Entity.m_Body.SetIsVislble(false);
                    }
                }
                else if (!this.m_Entity.m_Body.GetIsInCamera())
                {
                    this.m_Entity.m_Body.SetIsVislble(true);
                }
                this.currentTime = Updater.AliveTime;
            }
        }
    }

    protected virtual float Width =>
        ((float) GameLogic.Width);

    protected virtual float Height =>
        ((float) GameLogic.Height);
}

