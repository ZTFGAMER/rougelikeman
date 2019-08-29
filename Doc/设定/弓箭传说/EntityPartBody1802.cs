using System;
using UnityEngine;

public class EntityPartBody1802 : EntityPartBodyBase
{
    protected override void OnDeInitLogic()
    {
        base.m_Parent.RemoveRotateFollow(this);
        base.OnDeInitLogic();
    }

    protected override void OnGotoNextRooms(RoomGenerateBase.Room room)
    {
        float x = base.m_Parent.position.x + GameLogic.Random((float) -2f, (float) 2f);
        float z = base.m_Parent.position.z + GameLogic.Random((float) 0f, (float) 2f);
        base.transform.position = new Vector3(x, 0f, z);
    }

    private void OnTriggerEnter(Collider o)
    {
        if ((o.gameObject.layer == LayerManager.Player) || (o.gameObject.layer == LayerManager.Fly))
        {
            EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
            if (!GameLogic.IsSameTeam(this, entityByChild))
            {
                int num = -base.m_EntityData.GetBodyHit();
                GameLogic.SendHit_Body(entityByChild, this, (long) num, 0x3e8fa1);
                GameLogic.Release.Entity.RemovePartBody(this, false);
            }
        }
    }

    protected override void UpdateProcess(float delta)
    {
        base.UpdateProcess(delta);
    }
}

