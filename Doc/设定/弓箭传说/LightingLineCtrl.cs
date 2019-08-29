using System;
using UnityEngine;

public class LightingLineCtrl : MonoBehaviour
{
    private LineRenderer child;
    private Transform endeffect;
    private ParticleSystem[] ps;
    private const float textureLengthScale = 3f;
    private const float textureScrollSpeed = 8f;
    private Transform ball_child;
    private EntityBase target;

    private void Awake()
    {
        this.child = base.transform.Find("child").GetComponent<LineRenderer>();
        this.child.sortingLayerName = "Hit";
        this.endeffect = base.transform.Find("break");
        this.ps = base.GetComponentsInChildren<ParticleSystem>(true);
    }

    public void Init(Transform child, EntityBase target)
    {
        this.ball_child = child;
        this.target = target;
        this.UpdateLine(this.ball_child.position, target.m_Body.EffectMask.transform.position);
    }

    private void Update()
    {
        if ((this.target != null) && (this.ball_child != null))
        {
            this.UpdateLine(this.ball_child.position, this.target.m_Body.EffectMask.transform.position);
        }
    }

    private void UpdateLine(Vector3 startpos, Vector3 endpos)
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

