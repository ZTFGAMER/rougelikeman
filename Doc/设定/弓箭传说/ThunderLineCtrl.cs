using Dxx.Util;
using System;
using UnityEngine;

public class ThunderLineCtrl : MonoBehaviour
{
    private Transform child;
    private EntityBase from;
    private EntityBase to;
    private Vector3 frompos;
    private Vector3 topos;
    private MeshRenderer[] meshes;

    private void Awake()
    {
        this.child = base.transform.Find("line");
        this.meshes = base.GetComponentsInChildren<MeshRenderer>();
        int index = 0;
        int length = this.meshes.Length;
        while (index < length)
        {
            this.meshes[index].sortingLayerName = "BulletEffect";
            index++;
        }
    }

    private void Update()
    {
        this.UpdateEntity();
    }

    private void UpdateEntity()
    {
        if (((this.from != null) && (this.to != null)) && (((this.child != null) && (this.from.m_Body != null)) && (this.to.m_Body != null)))
        {
            this.frompos = this.from.m_Body.DeadNode.transform.position;
            this.topos = this.to.m_Body.DeadNode.transform.position;
            float y = Vector3.Distance(this.frompos, this.topos);
            float num2 = Utils.getAngle(this.frompos - this.topos);
            base.transform.rotation = Quaternion.Euler(0f, num2, 0f);
            base.transform.position = this.frompos;
            base.transform.LookAt(this.to.m_Body.DeadNode.transform);
            this.child.localScale = new Vector3(this.child.localScale.x, y, this.child.localScale.z);
            this.child.localPosition = new Vector3(0f, 0f, y / 2f);
        }
    }

    public void UpdateEntity(EntityBase from, EntityBase to)
    {
        this.from = from;
        this.to = to;
        this.UpdateEntity();
    }
}

