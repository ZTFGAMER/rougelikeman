using Dxx.Util;
using System;
using UnityEngine;

public class GoodsDrop : MonoBehaviour
{
    protected BoxCollider m_Box;
    private Func<int> callback;
    private Action mDropEndAction;
    private Animator Drop_ani;
    private bool Drop_Update;
    private float Drop_x;
    private float Drop_y;
    private float Drop_StartTime;
    private float Drop_startx;
    private float Drop_starty;
    protected float Drop_jumpTime = 0.67f;
    private bool bDelay;
    private float Delay_Time;
    private bool bDropEndAction;
    private float percent;

    private void Awake()
    {
        this.m_Box = base.GetComponent<BoxCollider>();
        this.OnAwake();
    }

    public void DropInitLast(Vector3 startpos, Vector3 endpos)
    {
        this.OnInit();
        this.InitDropAni();
        if ((this.Drop_ani == null) && (base.gameObject != null))
        {
            string name = base.gameObject.name;
            object[] args = new object[] { name };
            string message = Utils.FormatString("{0} don't have Drop_ani!!!", args);
            SdkManager.Bugly_Report("GoodsDrop.cs", message);
        }
        this.m_Box.enabled = false;
        if (this.Drop_ani != null)
        {
            this.Drop_ani.Play(this.JumpAnimation);
        }
        this.Drop_Update = true;
        this.Drop_x = endpos.x - startpos.x;
        this.Drop_y = endpos.z - startpos.z;
        this.Drop_StartTime = Updater.AliveTime;
        this.Drop_startx = startpos.x;
        this.Drop_starty = startpos.z;
        base.transform.position = new Vector3(this.Drop_startx, 0f, this.Drop_starty);
        this.bDelay = false;
        this.bDropEndAction = false;
    }

    private void DropUpdateProcess()
    {
        if (this.Drop_Update)
        {
            this.percent = (Updater.AliveTime - this.Drop_StartTime) / this.Drop_jumpTime;
            if ((this.percent > 0.8f) && !this.bDropEndAction)
            {
                this.bDropEndAction = true;
                this.mDropEndAction();
            }
            if (this.percent <= 1f)
            {
                base.transform.position = new Vector3(this.Drop_startx + (this.Drop_x * this.percent), base.transform.position.y, this.Drop_starty + (this.Drop_y * this.percent));
            }
            else
            {
                base.transform.position = new Vector3(this.Drop_startx + this.Drop_x, 0f, this.Drop_starty + this.Drop_y);
                this.Drop_Update = false;
                this.bDelay = true;
                this.Delay_Time = Updater.AliveTime;
                if (this.callback != null)
                {
                    this.callback();
                }
            }
        }
        if (this.bDelay && ((Updater.AliveTime - this.Delay_Time) > (((float) SettingDebugMediator.AbsorbDelay) / 1000f)))
        {
            this.bDelay = false;
            this.m_Box.enabled = true;
        }
    }

    private void InitDropAni()
    {
        if (this.Drop_ani == null)
        {
            Transform child = base.transform.GetChild(0);
            this.Drop_ani = child.gameObject.AddComponent<Animator>();
            this.Drop_ani.runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("Game/Animator/GoodsJump");
        }
    }

    protected void OnAwake()
    {
    }

    private void OnDisable()
    {
        if (this.Drop_ani != null)
        {
            this.Drop_ani.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (this.Drop_ani != null)
        {
            this.Drop_ani.enabled = true;
        }
    }

    protected virtual void OnInit()
    {
    }

    public void SetCallBack(Func<int> callback)
    {
        this.callback = callback;
    }

    public void SetDropEnd(Action callback)
    {
        this.mDropEndAction = callback;
    }

    private void Update()
    {
        this.DropUpdateProcess();
    }

    protected virtual string JumpAnimation =>
        "GoodsJump";
}

