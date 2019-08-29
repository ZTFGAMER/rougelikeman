using System;
using UnityEngine;

public class GoodsColliderBase : MonoBehaviour
{
    private GoodsBase m_Goods;
    private BoxCollider box;
    private float lasttime;
    private RaycastHit[] TriggerTest_Hits;
    private RaycastHit TriggerTest_Hit;
    private int TriggerTest_i;
    private int TriggerTest_Max;
    private Vector3 dir = new Vector3(0f, 1f, 0f);

    private void Awake()
    {
        this.box = base.GetComponent<BoxCollider>();
    }

    public void SetGoods(GoodsBase good)
    {
        this.m_Goods = good;
    }

    private void TriggerTest()
    {
        this.TriggerTest_Hits = Physics.BoxCastAll(base.transform.position, this.box.size / 2f, this.dir, base.transform.rotation, 0f, ((int) 1) << LayerManager.Player);
        this.TriggerTest_Max = this.TriggerTest_Hits.Length;
        this.TriggerTest_i = 0;
        while (this.TriggerTest_i < this.TriggerTest_Max)
        {
            this.m_Goods.ChildTriggerEnter(this.TriggerTest_Hits[this.TriggerTest_i].collider.gameObject);
            this.TriggerTest_i++;
        }
    }

    private void Update()
    {
        if (Time.time != this.lasttime)
        {
            this.lasttime = Time.time;
            this.TriggerTest();
        }
    }
}

