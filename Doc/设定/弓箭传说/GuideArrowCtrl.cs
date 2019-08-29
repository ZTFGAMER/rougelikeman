using System;
using UnityEngine;

public class GuideArrowCtrl : MonoBehaviour
{
    private Animation ani;
    private Vector3 pos;
    private bool bShow = true;
    private float mindis = 5f;

    private void Awake()
    {
        this.ani = base.transform.Find("arrowparent/arrow").GetComponent<Animation>();
    }

    private void Start()
    {
        this.pos = base.transform.position;
        this.bShow = true;
    }

    private void Update()
    {
        if (GameLogic.Self != null)
        {
            float num = Vector3.Distance(GameLogic.Self.position, this.pos);
            if ((num < this.mindis) && this.bShow)
            {
                this.ani.Play("guidearrow_miss");
                this.bShow = false;
            }
            else if ((num > (this.mindis + 1f)) && !this.bShow)
            {
                this.ani.Play("guidearrow_show");
                this.bShow = true;
            }
        }
    }
}

