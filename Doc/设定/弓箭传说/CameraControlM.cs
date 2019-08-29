using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public class CameraControlM : PauseObject
{
    public static CameraControlM Instance;
    public static bool CameraFollow;
    private const float Speed = 30f;
    private float speed = 30f;
    [SerializeField]
    private float minx;
    [SerializeField]
    private float maxx;
    [SerializeField]
    private float miny;
    [SerializeField]
    private float maxy;
    private const float OffsetY = -5f;
    private int RoomStateBoss;
    private float RoomStateBoss_Time;
    private Camera m_Camera;
    private float mCameraStartSize;
    private CameraStartCtrl mStartCtrl;
    public const float CameraStartSize = 6f;
    public const float CameraEndSize = 10.5f;
    [SerializeField]
    private Camera uiCamera;

    public void CameraPositionZero()
    {
        base.transform.position = Vector3.zero;
    }

    public void DeInit()
    {
        this.RemoveStartAnimate();
    }

    private void FixedUpdate()
    {
        if (!GameLogic.Paused && GameLogic.InGame)
        {
            switch (GameLogic.Release.Game.RoomState)
            {
                case RoomState.Runing:
                case RoomState.Throughing:
                    this.Update_Running();
                    break;
            }
        }
    }

    public Vector3 GetCameraEndPosition()
    {
        if (base.transform.position.x == this.minx)
        {
            return new Vector3(this.maxx, base.transform.position.y, 0f);
        }
        return new Vector3(this.minx, base.transform.position.y, 0f);
    }

    public Vector3 GetCameraPosition() => 
        base.transform.position;

    public void PlayStartAnimate(Action callback = null)
    {
        Instance.ResetCameraSize();
        this.RemoveStartAnimate();
        this.mStartCtrl = new CameraStartCtrl();
        this.mStartCtrl.SetCamera(this.m_Camera);
        this.mStartCtrl.OnEnd = callback;
        this.mStartCtrl.Begin();
    }

    private void RemoveStartAnimate()
    {
        if (this.mStartCtrl != null)
        {
            this.mStartCtrl.DeInit();
            this.mStartCtrl = null;
        }
    }

    public void ResetCameraPosition()
    {
        if (GameLogic.Self != null)
        {
            base.transform.position = new Vector3(0f, 0f, GameLogic.Self.position.z);
            this.SetCameraRound();
        }
        else
        {
            this.CameraPositionZero();
        }
    }

    public void ResetCameraSize()
    {
        this.m_Camera.orthographicSize = this.mCameraStartSize;
    }

    public void ResetCameraSpeed()
    {
        this.speed = 30f;
    }

    public void SetCameraPosition(Vector3 pos)
    {
        base.transform.position = pos;
    }

    private void SetCameraRound()
    {
        base.transform.position = new Vector3(Mathf.Clamp(base.transform.position.x, this.minx, this.maxx), 0f, base.transform.position.z);
    }

    public void SetCameraSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetCurrentRoom(int roomid)
    {
        Room_room beanById = LocalModelManager.Instance.Room_room.GetBeanById(roomid);
        this.minx = beanById.CameraRound[0];
        this.maxx = beanById.CameraRound[1];
        this.miny = beanById.CameraRound[2];
        this.maxy = beanById.CameraRound[3];
    }

    private void Start()
    {
        Instance = this;
        this.m_Camera = base.transform.Find("Child/Camera").GetComponent<Camera>();
        this.m_Camera.orthographicSize = (6f * (((float) GameLogic.DesignWidth) / ((float) GameLogic.DesignHeight))) / (((float) GameLogic.Width) / ((float) GameLogic.Height));
        this.mCameraStartSize = this.m_Camera.orthographicSize;
        this.uiCamera.transform.localPosition = new Vector3((float) GameLogic.Width, (float) GameLogic.Height, this.uiCamera.transform.localPosition.z) * 0.5f;
    }

    private void Update_Running()
    {
        if ((GameLogic.Self != null) && !GameLogic.Self.GetIsDead())
        {
            this.Update_Runnings();
        }
    }

    private void Update_Runnings()
    {
        if (CameraFollow)
        {
            base.transform.position = GameLogic.Self.position;
        }
        else
        {
            float num = MathDxx.Abs((float) (base.transform.position.z - GameLogic.Self.position.z));
            if (num < 0.2f)
            {
                base.transform.position = GameLogic.Self.position;
            }
            else
            {
                float t = (this.speed * Updater.delta) / num;
                if (t > 1f)
                {
                    t = 1f;
                }
                float z = ((GameLogic.Self.position.z - base.transform.position.z) * t) + base.transform.position.z;
                base.transform.position = new Vector3(0f, 0f, z);
                base.transform.position = Vector3.Lerp(base.transform.position, GameLogic.Self.position, t);
            }
            this.SetCameraRound();
        }
    }
}

