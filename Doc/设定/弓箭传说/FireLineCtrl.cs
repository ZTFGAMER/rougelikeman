using System;
using UnityEngine;

public class FireLineCtrl : MonoBehaviour
{
    private LineRenderer child;
    private Transform endeffect;
    private const float textureLengthScale = 3f;
    private const float textureScrollSpeed = 8f;

    private void Awake()
    {
        this.child = base.transform.Find("child").GetComponent<LineRenderer>();
        this.child.sortingLayerName = "Hit";
        this.endeffect = base.transform.Find("FrostBeamImpact");
    }

    public void UpdateLine(Vector3 startpos, Vector3 endpos)
    {
        this.child.positionCount = 2;
        this.child.SetPosition(0, startpos);
        this.child.SetPosition(1, endpos);
        float num = Vector3.Distance(startpos, endpos);
        this.child.material.mainTextureScale = new Vector2(num / 3f, 1f);
        Material material = this.child.material;
        material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
        this.endeffect.position = endpos;
    }
}

