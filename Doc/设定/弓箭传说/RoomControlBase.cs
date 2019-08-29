using DG.Tweening;
using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class RoomControlBase : MonoBehaviour
{
    protected bool m_bOpenDoor;
    protected object mInitData;
    private Transform cloudparent;
    private Transform cloud01;
    private Transform cloud02;
    private const float CloudMinDistance = 4f;
    private bool bCloudInit;
    private float mCloud01MoveTime;
    private float mCloud02MoveTime;
    private float cloud01y = -100f;
    private float cloud02y = -200f;
    private float randomheight;
    private Transform Collider_Parent;
    private Transform Collider_Door;
    private Transform Collider_Up;
    private Transform Collider_UpSide;
    private Transform Collider_Left;
    private Transform Collider_Right;
    private Transform Collider_Down;
    private Transform _GoodsDropParent;
    private Transform _GoodsParent;
    private TextMesh[] texts_layer;
    private MeshRenderer textMeshrenderer;

    private void Awake()
    {
        this.ColliderAwake();
        this.CloudAwake();
        this.LayerAwake();
        this.OnAwake();
    }

    public void Clear()
    {
        this.ClearGoods();
        this.ClearGoodsDrop();
    }

    public void ClearGoods()
    {
        this.OnClearGoods();
    }

    public void ClearGoodsDrop()
    {
        this.OnClearGoodsDrop();
    }

    private void CloudAwake()
    {
        this.cloudparent = base.transform.Find("Cloud");
        if (this.cloudparent != null)
        {
            this.cloud01 = this.cloudparent.Find("cloud01");
            this.cloud02 = this.cloudparent.Find("cloud02");
        }
    }

    private void CloudUpdate()
    {
    }

    private void Collider_OpenDoor(bool open)
    {
        if (this.Collider_Door != null)
        {
            this.Collider_Door.gameObject.SetActive(open);
        }
        if (this.Collider_Up != null)
        {
            this.Collider_Up.gameObject.SetActive(!open);
        }
        if (this.Collider_UpSide != null)
        {
            this.Collider_UpSide.gameObject.SetActive(open);
        }
    }

    private void ColliderAwake()
    {
        this.Collider_Parent = base.transform.Find("Collider");
        if (this.Collider_Parent != null)
        {
            this.Collider_Door = this.Collider_Parent.Find("door");
            this.Collider_Up = this.Collider_Parent.Find("up");
            this.Collider_UpSide = this.Collider_Parent.Find("upside");
            this.Collider_Left = this.Collider_Parent.Find("left");
            this.Collider_Right = this.Collider_Parent.Find("right");
            this.Collider_Down = this.Collider_Parent.Find("down");
        }
    }

    public Transform GetGoodsDropParent() => 
        this.OnGetGoodsDropParent();

    public void Init(object data = null)
    {
        this.mInitData = data;
        this.InitCloud();
        this.OnInit(data);
    }

    private void InitCloud()
    {
        if (!this.bCloudInit)
        {
            this.bCloudInit = true;
            this.randomheight = 9.5f;
            Mode_LevelData mInitData = this.mInitData as Mode_LevelData;
            if ((mInitData != null) && (mInitData.room != null))
            {
                this.randomheight = ((float) mInitData.room.RoomHeight) / 2.2f;
            }
            if (GameLogic.Random(0, 100) < 50)
            {
                this.mCloud01MoveTime = GameLogic.Random((float) 70f, (float) 90f);
                this.mCloud02MoveTime = GameLogic.Random((float) 120f, (float) 140f);
            }
            else
            {
                this.mCloud02MoveTime = GameLogic.Random((float) 70f, (float) 90f);
                this.mCloud01MoveTime = GameLogic.Random((float) 120f, (float) 140f);
            }
            if (this.cloud01 != null)
            {
                this.RandomCloudY(ref this.cloud01y, this.cloud02y);
                this.cloud01.localPosition = new Vector3(15f, 0f, this.cloud01y);
                TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.OnStepComplete<Tweener>(ShortcutExtensions.DOLocalMoveX(this.cloud01, -15f, this.mCloud01MoveTime, false), new TweenCallback(this, this.<InitCloud>m__0)), -1), 1);
            }
            if (this.cloud02 != null)
            {
                float x = GameLogic.Random((float) -5f, (float) 5f);
                float num2 = (x + 15f) / 30f;
                this.RandomCloudY(ref this.cloud02y, this.cloud01y);
                this.cloud02.localPosition = new Vector3(x, 0f, this.cloud02y);
                TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.cloud02, -15f, this.mCloud02MoveTime * num2, false), 1), new TweenCallback(this, this.<InitCloud>m__1));
            }
        }
    }

    public bool IsDoorOpen() => 
        this.m_bOpenDoor;

    private void LayerAwake()
    {
        this.texts_layer = base.transform.GetComponentsInChildren<TextMesh>(true);
        int index = 0;
        int length = this.texts_layer.Length;
        while (index < length)
        {
            this.textMeshrenderer = this.texts_layer[index].GetComponent<MeshRenderer>();
            this.textMeshrenderer.sortingLayerName = "Player";
            this.textMeshrenderer.sortingOrder = 990 + index;
            index++;
        }
    }

    public void LayerShow(bool value)
    {
        this.OnLayerShow(value);
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnClearGoods()
    {
    }

    protected virtual void OnClearGoodsDrop()
    {
    }

    private void OnDisable()
    {
        this.OnDisabled();
    }

    protected virtual void OnDisabled()
    {
    }

    private void OnEnable()
    {
        this.OnEnabled();
    }

    protected virtual void OnEnabled()
    {
    }

    protected virtual Transform OnGetGoodsDropParent() => 
        null;

    protected virtual void OnInit(object data = null)
    {
    }

    protected virtual void OnLayerShow(bool value)
    {
    }

    protected virtual void OnOpenDoor(bool value)
    {
    }

    protected virtual void OnReceiveEvent(string eventName, object data)
    {
    }

    protected virtual void OnSetText(string value)
    {
    }

    public void OpenDoor(bool value)
    {
        this.Collider_OpenDoor(value);
        this.OnOpenDoor(value);
    }

    private void RandomCloudY(ref float clouda, float cloudb)
    {
        clouda = GameLogic.Random(-this.randomheight, this.randomheight);
        while (MathDxx.Abs((float) (clouda - cloudb)) < 4f)
        {
            clouda = GameLogic.Random(-this.randomheight, this.randomheight);
        }
    }

    public void SendEvent(string eventName, object data = null)
    {
        this.OnReceiveEvent(eventName, data);
    }

    protected void SetLayer(int layer)
    {
        this.SetLayer(layer.ToString());
    }

    protected void SetLayer(string value)
    {
        int index = 0;
        int length = this.texts_layer.Length;
        while (index < length)
        {
            this.texts_layer[index].text = value;
            index++;
        }
    }

    public void SetText(string value)
    {
        this.OnSetText(value);
    }

    protected Transform GoodsDropParent
    {
        get
        {
            if (this._GoodsDropParent == null)
            {
                GameObject obj2 = new GameObject("GoodsDropParent") {
                    transform = { 
                        parent = base.transform,
                        localPosition = Vector3.zero,
                        localScale = Vector3.one,
                        localRotation = Quaternion.identity
                    }
                };
                this._GoodsDropParent = obj2.transform;
            }
            return this._GoodsDropParent;
        }
    }

    public Transform GoodsParent
    {
        get
        {
            if (this._GoodsParent == null)
            {
                GameObject obj2 = new GameObject("GoodsParent") {
                    transform = { 
                        parent = base.transform,
                        localPosition = Vector3.zero,
                        localScale = Vector3.one,
                        localRotation = Quaternion.identity
                    }
                };
                this._GoodsParent = obj2.transform;
            }
            return this._GoodsParent;
        }
    }

    public class Mode_LevelData
    {
        public RoomGenerateBase.Room room;
        public RoomGenerateBase.Room nextroom;
    }
}

