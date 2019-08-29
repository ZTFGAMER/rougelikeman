using System;
using UnityEngine;

public class EntityMove1007Ctrl : MonoBehaviour
{
    public Action<Collision> CollisionEnterAction;

    private void OnCollisionEnter(Collision o)
    {
        if (this.CollisionEnterAction != null)
        {
            this.CollisionEnterAction(o);
        }
    }
}

