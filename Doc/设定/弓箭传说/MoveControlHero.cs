using System;
using UnityEngine;

public class MoveControlHero : MoveControl
{
    private GameObject footDirection;
    private EntityHero _EntityHero;

    protected override void MoveEndVirtual()
    {
        base.MoveEndVirtual();
        if (this.footDirection != null)
        {
            this.footDirection.SetActive(false);
        }
        this.mEntityHero.DoMoveEnd();
    }

    protected override void MoveStartVirtual()
    {
        base.MoveStartVirtual();
        if (this.footDirection != null)
        {
            this.footDirection.SetActive(true);
        }
        GameLogic.Release.Mode.RoomGenerate.PlayerMove();
        this.mEntityHero.DoMoveStart();
    }

    protected override void MovingVirtual(JoyData data)
    {
        base.MovingVirtual(data);
        if (this.footDirection != null)
        {
            this.footDirection.transform.localPosition = (new Vector3(data.direction.x, 0f, data.direction.z / 1.23f) * data.length) / 60f;
        }
        this.mEntityHero.DoMoving(data);
    }

    protected override void OnInit()
    {
        EntityHero entity = base.m_Entity as EntityHero;
        if ((entity != null) && (entity.FootDirection != null))
        {
            this.footDirection = (base.m_Entity as EntityHero).FootDirection.transform.Find("Direction").gameObject;
            this.footDirection.SetActive(false);
        }
    }

    private EntityHero mEntityHero
    {
        get
        {
            if (this._EntityHero == null)
            {
                this._EntityHero = base.m_Entity as EntityHero;
            }
            return this._EntityHero;
        }
    }
}

