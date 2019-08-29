using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour
{
    private Transform mTransform;
    private GameObject child;
    public Text Text_HP;
    public Transform Image_Fg;
    public Transform Image_Fg_Reduce;
    public Transform Image_Fg_Blue;
    public Transform Image_Bg;
    public Transform Image_MPFG;
    public Transform Image_MP_Reduce;
    public Image Line;
    public Transform LineParent;
    private EntityBase entity;
    private bool bReducingHP;
    private float minReduceScale = 0.008f;
    private float reducesHP;
    private bool bReducingMP;
    private float reducesMP;
    private bool bUpdateLine;
    private float bReducingHP_PosX;
    private Color LineColor = new Color(1f, 1f, 1f, 0f);
    private const int LineFrame = 8;
    private int LineIndex;
    private RectTransform HP_Parent;
    private RectTransform HP_BG;
    private RectTransform HP_FG_Reduce;
    private RectTransform HP_FG;
    private RectTransform HP_FG_Blue;
    private float FG_Width;
    private float maxHP;
    private long mPerHP = 200L;
    private const float perHPWidth = 30f;
    private List<Image> mHPLineList = new List<Image>();
    private Queue<Image> mHPLineCacheList = new Queue<Image>();
    private int ShowHPCount = 1;

    private void Awake()
    {
        this.mTransform = base.transform;
        this.child = this.mTransform.Find("HPSlider").gameObject;
        this.HP_Parent = this.child.transform as RectTransform;
        this.HP_BG = this.child.transform.Find("HP_BG") as RectTransform;
        this.HP_FG_Reduce = this.child.transform.Find("HP_FG_Reduce") as RectTransform;
        this.HP_FG = this.child.transform.Find("HP_FG") as RectTransform;
        this.HP_FG_Blue = this.child.transform.Find("HP_FG_Blue") as RectTransform;
        this.FG_Width = this.HP_FG.sizeDelta.x;
    }

    private void CacheHPLine(Image t)
    {
        this.mHPLineCacheList.Enqueue(t);
        t.gameObject.SetActive(false);
    }

    public void DeInit()
    {
        this.entity.OnMaxHpUpdate = (Action<long, long>) Delegate.Remove(this.entity.OnMaxHpUpdate, new Action<long, long>(this.OnMaxHPUpdate));
        Object.Destroy(base.gameObject);
    }

    private Image GetHPLine()
    {
        if (this.mHPLineCacheList.Count > 0)
        {
            Image image = this.mHPLineCacheList.Dequeue();
            image.gameObject.SetActive(true);
            return image;
        }
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/UI/HPLine"));
        obj2.transform.SetParent(this.LineParent);
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        return obj2.GetComponent<Image>();
    }

    public void Init(EntityBase entity)
    {
        this.entity = entity;
        this.maxHP = (((float) entity.m_EntityData.MaxHP) / 4f) * 7f;
        if (entity.IsSelf)
        {
            entity.OnMaxHpUpdate = (Action<long, long>) Delegate.Combine(entity.OnMaxHpUpdate, new Action<long, long>(this.OnMaxHPUpdate));
            this.OnMaxHPUpdateInternal();
        }
        this.UpdateHP();
    }

    private void LateUpdate()
    {
        if ((this.entity != null) && !this.entity.GetIsDead())
        {
            Vector3 vector = Utils.World2Screen(this.entity.position);
            float x = vector.x;
            float y = vector.y + (this.entity.m_Body.HPMask.transform.localPosition.y * 23f);
            this.mTransform.position = new Vector3(x, y, 0f);
        }
    }

    private void LineAniationEnd()
    {
        if (this.Line != null)
        {
            this.bUpdateLine = false;
            this.Line.set_color(this.LineColor);
            this.Line.transform.localScale = Vector3.one;
        }
    }

    private void LineAnimation()
    {
        if (this.bUpdateLine && (this.Line != null))
        {
            this.Line.transform.localScale = new Vector3(this.Line.transform.localScale.x, this.Line.transform.localScale.y + 2f, this.Line.transform.localScale.z);
            this.Line.set_color(new Color(1f, 1f, 1f, ((float) this.LineIndex) / 8f));
            this.LineIndex--;
            if (this.LineIndex < 0)
            {
                this.LineAniationEnd();
            }
        }
    }

    private void LineAnimationStart()
    {
        if (this.Line != null)
        {
            this.bReducingHP_PosX = (this.entity.m_EntityData.GetHPPercent() * 90f) - 45f;
            this.Line.transform.localPosition = new Vector3(this.bReducingHP_PosX, 0f, 0f);
            this.Line.set_color(Color.white);
            this.Line.transform.localScale = new Vector3(1f, 3f, 1f);
            this.LineIndex = 8;
        }
    }

    private void OnMaxHPUpdate(long before, long after)
    {
        GameLogic.ShowHPMaxChange(after - before);
        this.OnMaxHPUpdateInternal();
    }

    private void OnMaxHPUpdateInternal()
    {
        long maxHP = this.entity.m_EntityData.MaxHP;
        long num2 = maxHP / this.mPerHP;
        if (num2 > this.mHPLineList.Count)
        {
            long count = this.mHPLineList.Count;
            long num4 = num2;
            while (count < num4)
            {
                this.mHPLineList.Add(this.GetHPLine());
                count += 1L;
            }
        }
        else if (num2 < this.mHPLineList.Count)
        {
            for (int j = this.mHPLineList.Count - 1; (j >= num2) && (j < this.mHPLineList.Count); j--)
            {
                this.CacheHPLine(this.mHPLineList[j]);
                this.mHPLineList.RemoveAt(j);
            }
        }
        float num6 = maxHP;
        if (num6 > this.maxHP)
        {
            num6 = this.maxHP;
        }
        float num7 = ((float) this.entity.m_EntityData.attribute.GetHPBase()) / 4f;
        float x = (num6 / num7) * 30f;
        float num9 = 3f;
        this.HP_Parent.sizeDelta = new Vector2(x, this.HP_Parent.sizeDelta.y);
        this.HP_FG.sizeDelta = new Vector2(x - (num9 * 2f), this.HP_FG.sizeDelta.y);
        this.HP_FG.anchoredPosition = new Vector2(num9, 0f);
        this.HP_FG_Reduce.anchoredPosition = new Vector2(num9, 0f);
        this.HP_FG_Blue.anchoredPosition = new Vector2(num9, 0f);
        this.HP_FG_Blue.sizeDelta = new Vector2(x - (num9 * 2f), this.HP_FG.sizeDelta.y);
        this.HP_FG_Blue.localScale = new Vector3(0f, 1f, 1f);
        this.HP_FG_Reduce.sizeDelta = new Vector2(x - (num9 * 2f), this.HP_FG_Reduce.sizeDelta.y);
        this.HP_BG.sizeDelta = new Vector2(x, this.HP_BG.sizeDelta.y);
        for (int i = 0; i < num2; i++)
        {
            float num11 = ((float) (i + 1)) / (((float) maxHP) / ((float) this.mPerHP));
            float num12 = (num11 * x) - (x / 2f);
            Image image = this.mHPLineList[i];
            image.transform.localPosition = new Vector3(num12, 0f, 0f);
            bool flag = ((i + 1) % 5) == 0;
        }
        this.UpdateHP();
    }

    public void ShowHP(bool show)
    {
        this.ShowHPCount += !show ? -1 : 1;
        this.child.gameObject.SetActive(this.ShowHPCount > 0);
    }

    private void Update()
    {
        if (this.bReducingHP)
        {
            this.reducesHP = (this.Image_Fg_Reduce.localScale.x - this.Image_Fg.localScale.x) / 70f;
            this.reducesHP = (this.reducesHP >= this.minReduceScale) ? this.reducesHP : this.minReduceScale;
            if ((this.Image_Fg_Reduce.localScale.x - this.reducesHP) < this.Image_Fg.localScale.x)
            {
                this.reducesHP = this.Image_Fg_Reduce.localScale.x - this.Image_Fg.localScale.x;
                this.Image_Fg_Reduce.localScale = this.Image_Fg.localScale;
                this.bReducingHP = false;
                this.bUpdateLine = true;
                return;
            }
            this.Image_Fg_Reduce.localScale = new Vector3(this.Image_Fg_Reduce.localScale.x - this.reducesHP, 1f, 1f);
        }
        if (this.bReducingMP)
        {
            this.reducesMP = (this.Image_MP_Reduce.localScale.x - this.Image_MPFG.localScale.x) / 70f;
            this.reducesMP = (this.reducesMP >= this.minReduceScale) ? this.reducesMP : this.minReduceScale;
            if ((this.Image_MP_Reduce.localScale.x - this.reducesMP) < this.Image_MPFG.localScale.x)
            {
                this.reducesMP = this.Image_MP_Reduce.localScale.x - this.Image_MPFG.localScale.x;
                this.Image_MP_Reduce.localScale = this.Image_MPFG.localScale;
                this.bReducingMP = false;
                return;
            }
            this.Image_MP_Reduce.localScale = new Vector3(this.Image_MP_Reduce.localScale.x - this.reducesMP, 1f, 1f);
        }
        this.LineAnimation();
    }

    public void UpdateHP()
    {
        if (this.Image_Fg.localScale.x > this.entity.m_EntityData.GetHPPercent())
        {
            this.bReducingHP = true;
            this.LineAnimationStart();
        }
        this.Image_Fg.localScale = new Vector3(this.entity.m_EntityData.GetHPPercent(), this.Image_Fg.localScale.y, this.Image_Fg.localScale.z);
        this.UpdateHPText();
        this.UpdateShield();
    }

    private void UpdateHPText()
    {
        if (this.Text_HP != null)
        {
            this.Text_HP.text = this.entity.m_EntityData.CurrentHP.ToString();
        }
    }

    public void UpdateShield()
    {
        long maxHP = this.entity.m_EntityData.MaxHP;
        float x = ((float) this.entity.m_EntityData.Shield_CurrentHitValue) / ((float) maxHP);
        this.Image_Fg_Blue.localScale = new Vector3(x, this.Image_Fg_Blue.localScale.y, this.Image_Fg_Blue.localScale.z);
    }
}

