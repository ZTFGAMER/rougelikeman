using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyMask : PauseObject
{
    public GameObject LeftWeapon;
    public GameObject RightWeapon;
    public GameObject LeftBullet;
    public GameObject Body;
    public GameObject EffectMask;
    public GameObject HPMask;
    public GameObject FootMask;
    public GameObject HeadMask;
    public GameObject RotateMask;
    public GameObject BulletHitMask;
    public GameObject HeadTopEffect;
    public GameObject SpecialHitMask;
    public List<GameObject> BulletList;
    public List<SkinnedMeshRenderer> Body_Extra;
    [NonSerialized]
    public GameObject ZeroMask;
    public GameObject AnimatorBodyObj;
    [NonSerialized]
    public HeroPlayMakerControl mHeroPlayMakerCtrl;
    protected EntityBase m_Entity;
    private Dictionary<int, Transform> mWeaponPosList = new Dictionary<int, Transform>();
    private Animation ani;
    private bool bOffset;
    private bool bFlyStone;
    private float m_fAddScale;
    private BodyMaskCamera mCamera;
    private BodyShaderBase mShaderBase;
    public Transform BodyCenter;
    private Dictionary<EElementType, BodyElementData> ElementColor;
    private bool bVisible;
    protected bool bHittedColor;
    protected float mHittedTime;
    private bool bTargetColor;
    private Dictionary<int, GameObject> mHeadTopList;
    private List<int> mHeadIDs;

    public BodyMask()
    {
        Dictionary<EElementType, BodyElementData> dictionary = new Dictionary<EElementType, BodyElementData>();
        BodyElementData data = new BodyElementData {
            color = new Color(1f, 0.227451f, 0f)
        };
        dictionary.Add(EElementType.eFire, data);
        data = new BodyElementData {
            color = new Color(0f, 0.7098039f, 1f)
        };
        dictionary.Add(EElementType.eIce, data);
        this.ElementColor = dictionary;
        this.mHeadTopList = new Dictionary<int, GameObject>();
        this.mHeadIDs = new List<int>();
    }

    public void AddElement(EElementType type)
    {
        BodyElementData local1 = this.ElementColor[type];
        local1.count++;
        if (!this.bHittedColor)
        {
            this.UpdateElement();
        }
    }

    public void AddHeadTop(int skillaloneid, GameObject o)
    {
        if (!this.mHeadTopList.ContainsKey(skillaloneid))
        {
            this.mHeadTopList.Add(skillaloneid, o);
            this.mHeadIDs.Add(skillaloneid);
            o.transform.SetParent(this.HeadTopEffect.transform);
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            this.UpdateHeadTop();
        }
    }

    public void AddScale(float scale)
    {
        this.m_fAddScale += scale;
        if (((this.EffectMask != null) && (this.RotateMask != null)) && ((this.m_Entity != null) && (this.m_Entity.m_Data != null)))
        {
            this.EffectMask.transform.parent.localScale = Vector3.one * (this.m_Entity.m_Data.BodyScale + this.m_fAddScale);
            this.RotateMask.transform.localScale = this.EffectMask.transform.parent.localScale;
            this.ZeroMask.transform.localScale = this.EffectMask.transform.parent.localScale;
        }
    }

    private void Awake()
    {
        SdkManager.Bugly_Report(this.BulletHitMask != null, "BodyMask.cs", " BulletHitMask == null");
        SdkManager.Bugly_Report(this.Body != null, "BodyMask.cs", " Body == null");
        SdkManager.Bugly_Report(this.EffectMask != null, "BodyMask.cs", " EffectMask == null");
        SdkManager.Bugly_Report(this.HPMask != null, "BodyMask.cs", " HPMask == null");
        SdkManager.Bugly_Report(this.HeadMask != null, "BodyMask.cs", " HeadMask == null");
        this.ani = this.AnimatorBodyObj.GetComponent<Animation>();
        this.ZeroMask = new GameObject("Zero");
        this.ZeroMask.SetParentNormal(base.transform);
    }

    protected virtual void AwakeInit()
    {
    }

    public void CacheEffect()
    {
        this.CacheNode(this.EffectMask);
    }

    private void CacheNode(GameObject node)
    {
        if (node != null)
        {
            this.CacheNode(node.transform);
        }
    }

    private void CacheNode(Transform node)
    {
        if (node != null)
        {
            for (int i = node.childCount - 1; i >= 0; i--)
            {
                GameLogic.EffectCache(node.GetChild(i).gameObject);
            }
        }
    }

    public void DeadDown()
    {
        this.bHittedColor = false;
        Dictionary<EElementType, BodyElementData>.Enumerator enumerator = this.ElementColor.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<EElementType, BodyElementData> current = enumerator.Current;
            current.Value.count = 0;
        }
        if (this.RotateMask.transform.localPosition.y > 0f)
        {
            ShortcutExtensions.DOKill(this.RotateMask.transform, false);
            ShortcutExtensions.DOLocalMoveY(this.RotateMask.transform, 0f, 0.3f, false);
        }
    }

    public void DeInit()
    {
        if (this.mCamera != null)
        {
            this.mCamera.DeInit();
        }
        this.mShaderBase.ReturnToDefault();
        if (this != null)
        {
            base.transform.localScale = Vector3.one;
        }
    }

    public float GetBodyScale() => 
        base.transform.localScale.x;

    public Transform GetBullet(int index)
    {
        if ((this.BulletList.Count > index) && (index >= 0))
        {
            return this.BulletList[index].transform;
        }
        if (this.LeftBullet != null)
        {
            return this.LeftBullet.transform;
        }
        return this.EffectMask.transform;
    }

    public Vector3 GetHeadPosition(int skillaloneid)
    {
        if (this.mHeadTopList.TryGetValue(skillaloneid, out GameObject obj2))
        {
            return obj2.transform.position;
        }
        object[] args = new object[] { skillaloneid };
        SdkManager.Bugly_Report("BodyMask_HeadTop.cs", Utils.FormatString("GetHeadPosition[{0}] dont have!", args));
        return base.transform.position;
    }

    public bool GetIsInCamera() => 
        this.bVisible;

    public Transform GetWeaponNode(int index, Transform t)
    {
        if (!this.mWeaponPosList.TryGetValue(index, out Transform transform))
        {
            object[] args = new object[] { index.ToString() };
            GameObject obj2 = new GameObject(Utils.FormatString("WeaponNode_{0}", args));
            transform = obj2.transform;
            transform.SetParentNormal(this.m_Entity.m_Body.EffectMask.transform.parent);
            transform.position = t.position;
            this.mWeaponPosList.Add(index, transform);
        }
        return transform;
    }

    public virtual void Hitted(Vector3 HittedDirection, HitType type)
    {
        this.bHittedColor = true;
        this.mHittedTime = Updater.AliveTime;
        this.mShaderBase.SetHitted();
        this.OnHittedColorBefore();
    }

    private void OnDisable()
    {
        this.ani.enabled = false;
    }

    private void OnElite()
    {
    }

    private void OnEnable()
    {
        this.ani.enabled = true;
    }

    protected virtual void OnHittedColor()
    {
        if (this.bHittedColor)
        {
            float hittedWhiteByTime = this.m_Entity.m_HitEdit.GetHittedWhiteByTime(Updater.AliveTime - this.mHittedTime);
            this.mShaderBase.OnUpdateHitted(hittedWhiteByTime);
            if (this.m_Entity.m_HitEdit.IsHittedWhiteEnd(Updater.AliveTime - this.mHittedTime))
            {
                this.bHittedColor = false;
                this.UpdateElement();
            }
        }
    }

    protected virtual void OnHittedColorBefore()
    {
    }

    public void RemoveElement(EElementType type)
    {
        BodyElementData data = this.ElementColor[type];
        data.count--;
        if ((data.count == 0) && !this.bHittedColor)
        {
            this.mShaderBase.ReturnToDefault();
        }
    }

    public void RemoveHeadTop(int skillaloneid)
    {
        if (this.mHeadTopList.ContainsKey(skillaloneid))
        {
            this.mHeadTopList.Remove(skillaloneid);
            this.mHeadIDs.Remove(skillaloneid);
            this.UpdateHeadTop();
        }
    }

    public void SetBodyScale(float value)
    {
        if (this.Body != null)
        {
            this.HPMask.transform.parent.localScale = Vector3.one * value;
            base.transform.localScale = Vector3.one * value;
        }
    }

    public void SetEntity(EntityBase entity)
    {
        this.m_Entity = entity;
        if (!this.m_Entity.IsSelf)
        {
            if (GameLogic.Hold.BattleData.GetMode() == GameMode.eGold1)
            {
                this.mShaderBase = new BodyShaderGold();
            }
            else
            {
                this.mShaderBase = new BodyShaderDefault();
            }
        }
        else
        {
            this.mShaderBase = new BodyShaderDefault();
        }
        this.mShaderBase.Init(this.m_Entity);
        this.mCamera = new BodyMaskCamera(entity);
        this.mHeroPlayMakerCtrl = new HeroPlayMakerControl();
        this.mHeroPlayMakerCtrl.Init(entity);
        this.SetTexture(this.m_Entity.m_Data.TextureID);
        this.mShaderBase.ReturnToDefault();
        this.AddScale(0f);
        if (this.m_Entity.IsElite)
        {
            this.AddScale(0.2f);
            this.OnElite();
        }
    }

    public void SetFlyStone(bool fly)
    {
        this.bFlyStone = fly;
    }

    public void SetIsVislble(bool value)
    {
        this.bVisible = value;
    }

    public void SetLocalPosition(float x, float z)
    {
        base.transform.localPosition = new Vector3(x, 0f, z);
    }

    public void SetOrder()
    {
        if (((this != null) && (base.transform != null)) && (this.m_Entity != null))
        {
            int order = (int) ((-base.transform.position.z - 1f) - 0.666f);
            if (this.bFlyStone)
            {
                order = 0x3e7;
            }
            this.mShaderBase.SetOrder(order);
            if (this.m_Entity.m_Weapon != null)
            {
                this.m_Entity.m_Weapon.SetOrder(order);
            }
        }
    }

    public void SetStrengh()
    {
        if (this.mShaderBase != null)
        {
            this.mShaderBase.SetStrengh();
        }
    }

    public void SetTarget(bool value)
    {
    }

    public void SetTexture(string value)
    {
        if (this.mShaderBase != null)
        {
            this.mShaderBase.SetTexture(value);
        }
    }

    public void SetTextureWithoutInit(string value)
    {
        object[] args = new object[] { value };
        Texture texture = ResourceManager.Load<Texture>(Utils.FormatString("Game/ModelsTexture/{0}", args));
        if (texture != null)
        {
            SkinnedMeshRenderer component = this.Body.GetComponent<SkinnedMeshRenderer>();
            if (component != null)
            {
                component.material.SetTexture("_MainTex", texture);
            }
            else
            {
                MeshRenderer renderer2 = this.Body.GetComponent<MeshRenderer>();
                if (renderer2 != null)
                {
                    renderer2.material.SetTexture("_MainTex", texture);
                }
            }
        }
    }

    private void UpdateElement()
    {
        Dictionary<EElementType, BodyElementData>.Enumerator enumerator = this.ElementColor.GetEnumerator();
        bool flag = false;
        while (enumerator.MoveNext())
        {
            KeyValuePair<EElementType, BodyElementData> current = enumerator.Current;
            BodyElementData data = current.Value;
            if (data.count > 0)
            {
                this.mShaderBase.SetElement(data.color);
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            this.mShaderBase.ReturnToDefault();
        }
    }

    private void UpdateHeadTop()
    {
        float num = 120f;
        float num2 = (this.mHeadIDs.Count - 1) * num;
        float num3 = num2 / 2f;
        int num7 = 0;
        int count = this.mHeadIDs.Count;
        while (num7 < count)
        {
            GameObject obj2 = this.mHeadTopList[this.mHeadIDs[num7]];
            float angle = (num7 * num) - num3;
            float x = MathDxx.Sin(angle);
            float z = MathDxx.Cos(angle);
            obj2.transform.localPosition = new Vector3(x, 0f, z) * 1.5f;
            num7++;
        }
    }

    protected override void UpdateProcess()
    {
        this.OnHittedColor();
    }

    public Transform DeadNode
    {
        get
        {
            if (this.BodyCenter == null)
            {
                return base.transform;
            }
            return this.BodyCenter;
        }
    }

    public class BodyElementData
    {
        public Color color;
        public int count;
    }
}

