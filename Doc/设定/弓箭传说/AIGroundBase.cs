using System;
using UnityEngine;

public class AIGroundBase : AIBase
{
    private Animation mAni_Ground;

    public void GroundShow(bool value)
    {
        if (value)
        {
            this.mAni_Ground.Play("3028_GroundBreak_Show");
        }
        else
        {
            this.mAni_Ground.Play("3028_GroundBreak_Miss");
        }
    }

    protected override void OnInitOnce()
    {
        if (this.mAni_Ground == null)
        {
            GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Models/3028_GroundBreak"));
            child.SetParentNormal(base.m_Entity.transform);
            this.mAni_Ground = child.transform.Find("scale/sprite").GetComponent<Animation>();
        }
        base.OnInitOnce();
    }
}

