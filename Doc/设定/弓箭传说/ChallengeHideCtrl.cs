using Dxx.Util;
using System;
using UnityEngine;

public class ChallengeHideCtrl : MonoBehaviour
{
    public void DeInit()
    {
        if (GameLogic.Self != null)
        {
            GameLogic.Self.Event_PositionBy -= new Action<Vector3>(this.OnPlayerMove);
        }
    }

    public void Init()
    {
        GameLogic.Self.Event_PositionBy += new Action<Vector3>(this.OnPlayerMove);
    }

    private void OnPlayerMove(Vector3 pos)
    {
        Vector3 vector = Utils.World2Screen(GameLogic.Self.position);
        float x = vector.x;
        float y = vector.y;
        base.transform.position = new Vector3(x, y, 0f);
    }
}

