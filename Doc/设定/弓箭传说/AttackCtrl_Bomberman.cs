using Dxx.Util;
using System;
using UnityEngine;

public class AttackCtrl_Bomberman
{
    private Transform groundlight;
    private RectTransform progress;
    private ProgressCtrl mProgressCtrl;
    private EntityBase m_Entity;
    private float updatetime;
    private float starttime;
    private bool bStand;
    private bool bProgressShow = true;
    private Vector3 bombpos;

    private void CreateBomb()
    {
        BulletBase base2 = GameLogic.Release.Bullet.CreateBullet(null, 0xbd9, this.bombpos, 0f);
        if (base2 != null)
        {
            base2.transform.rotation = Quaternion.identity;
            base2.SetTarget(null, 1);
            base2.mBulletTransmit.SetAttack(20L);
        }
    }

    public void DeInit()
    {
        if (this.groundlight != null)
        {
            Object.Destroy(this.groundlight.gameObject);
        }
        if (this.progress != null)
        {
            Object.Destroy(this.progress.gameObject);
        }
    }

    public void Init(EntityBase entity)
    {
        this.m_Entity = entity;
        this.m_Entity.OnMoveEvent = (Action<bool>) Delegate.Combine(this.m_Entity.OnMoveEvent, new Action<bool>(this.OnMove));
        this.updatetime = GameLogic.Hold.BattleData.Challenge_BombermanTime();
        this.starttime = Updater.AliveTime;
        if (this.m_Entity.IsSelf)
        {
            this.groundlight = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Bomberman/groundlight")).transform;
            this.progress = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Bomberman/bomb_progress")).transform as RectTransform;
            this.progress.SetParentNormal(GameNode.m_InGame);
            this.mProgressCtrl = this.progress.GetComponent<ProgressCtrl>();
            this.SetProgressShow(false);
        }
    }

    private void OnMove(bool value)
    {
        this.bStand = !value;
        if (this.bStand)
        {
            this.starttime = Updater.AliveTime;
        }
    }

    private void SetProgressShow(bool value)
    {
        if (this.bProgressShow != value)
        {
            this.bProgressShow = value;
            if (this.progress != null)
            {
                this.progress.gameObject.SetActive(value);
            }
        }
    }

    private void SetProgressValue(float value)
    {
        if (this.mProgressCtrl != null)
        {
            this.mProgressCtrl.Value = value;
        }
    }

    public void Update()
    {
        Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(this.m_Entity.position);
        this.bombpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(roomXY);
        if (this.groundlight != null)
        {
            this.groundlight.position = this.bombpos;
        }
        if (this.bStand)
        {
            if (!GameLogic.Release.MapCreatorCtrl.Bomberman_is_empty(this.m_Entity.position))
            {
                this.starttime = Updater.AliveTime;
                this.SetProgressShow(false);
            }
            else
            {
                float num2 = (Updater.AliveTime - this.starttime) / this.updatetime;
                num2 = MathDxx.Clamp01(num2);
                this.SetProgressShow(true);
                this.SetProgressValue(num2);
                if (this.progress != null)
                {
                    Vector3 vector = Utils.World2Screen(this.bombpos);
                    float x = vector.x;
                    float y = vector.y;
                    this.progress.position = new Vector3(x, y - 50f, 0f);
                }
                if (num2 >= 1f)
                {
                    this.starttime += this.updatetime;
                    this.CreateBomb();
                }
            }
        }
        else
        {
            this.SetProgressShow(false);
        }
    }
}

