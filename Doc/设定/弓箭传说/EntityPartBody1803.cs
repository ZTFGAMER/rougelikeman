using System;
using UnityEngine;

public class EntityPartBody1803 : EntityPartBodyBase
{
    private void OnTriggerEnter(Collider o)
    {
        GameObject gameObject = null;
        if (o != null)
        {
            gameObject = o.gameObject;
        }
        if ((gameObject.layer == LayerManager.Player) || (gameObject.layer == LayerManager.Fly))
        {
            EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(gameObject);
            if (!GameLogic.IsSameTeam(this, entityByChild))
            {
                int num = -base.m_EntityData.GetBodyHit();
                GameLogic.SendHit_Body(entityByChild, this, (long) num, 0x3e8fa1);
                GameLogic.Release.Entity.RemovePartBody(this, false);
            }
        }
    }
}

