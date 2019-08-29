using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class RayCastManager
{
    private static int _groundlayer = -1;
    private static int _flylayer = -1;

    public static void CastMinDistance(Vector3 startpos, float angle, bool fly, out float mindis)
    {
        Vector3 dir = new Vector3(MathDxx.Sin(angle), 0f, MathDxx.Cos(angle));
        CastMinDistance(startpos, dir, fly, out mindis);
    }

    public static void CastMinDistance(Vector3 startpos, Vector3 dir, bool fly, out float mindis)
    {
        CastMinDistance(startpos, dir, fly, out mindis, out _, out _);
    }

    public static void CastMinDistance(Vector3 startpos, Vector3 dir, bool fly, out float mindis, out Vector3 minpos)
    {
        CastMinDistance(startpos, dir, fly, out mindis, out minpos, out _);
    }

    public static void CastMinDistance(Vector3 startpos, Vector3 dir, bool fly, out float mindis, out Vector3 minpos, out Collider minCollider)
    {
        RaycastHit[] hitArray = Physics.RaycastAll(startpos, dir, 50f, !fly ? groundLayer : flyLayer);
        mindis = 40f;
        minpos = startpos;
        minCollider = null;
        int index = 0;
        int length = hitArray.Length;
        while (index < length)
        {
            RaycastHit hit = hitArray[index];
            float distance = hit.distance;
            if (distance < mindis)
            {
                mindis = distance;
                minCollider = hit.collider;
                minpos = hit.point;
            }
            index++;
        }
    }

    public static int groundLayer
    {
        get
        {
            if (_groundlayer < 0)
            {
                _groundlayer = (((int) 1) << LayerManager.Bullet2Map) | (((int) 1) << LayerManager.MapOutWall);
            }
            return _groundlayer;
        }
    }

    public static int flyLayer
    {
        get
        {
            if (_flylayer < 0)
            {
                _flylayer = ((int) 1) << LayerManager.MapOutWall;
            }
            return _flylayer;
        }
    }
}

