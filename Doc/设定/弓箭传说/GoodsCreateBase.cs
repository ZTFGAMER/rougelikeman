using System;
using UnityEngine;

public class GoodsCreateBase : MonoBehaviour
{
    private void Awake()
    {
        this.OnAwake();
    }

    protected void Cache()
    {
        GameLogic.Release.GoodsCreate.Cache(base.gameObject);
    }

    public void Init()
    {
        this.OnInit();
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnStart()
    {
    }

    private void OnTriggerEnter(Collider o)
    {
        this.TriggerEnter(o);
    }

    protected virtual void OnUpdate()
    {
    }

    private void Start()
    {
        this.OnStart();
    }

    protected virtual void TriggerEnter(Collider o)
    {
    }

    private void Update()
    {
        this.OnUpdate();
    }
}

