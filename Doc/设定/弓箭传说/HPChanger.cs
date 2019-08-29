using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPChanger : MonoBehaviour
{
    private static Dictionary<HitType, float> mTimes;
    private Transform mTransform;
    private EntityBase m_Entity;
    private Vector3 entitypos;
    private Vector3 entitybodypos;
    private Text text;
    private float OffsetX;
    private float OffsetY;
    private Vector3 MovePer = new Vector3();
    private Vector3 MoveAll;
    private const int MoveCount = 14;
    private int CurrentMoveCount;
    private int FontSize;
    private float CritFontScale = 1.5f;
    private float HeadShotFontScale = 1.5f;
    private const string Ani_Normal = "HPChanger_Normal";
    private const string Ani_Crit = "HPChanger_Crit";
    private const string Ani_HeadShot = "HPChanger_HeadShot";
    private float starttime;
    private CanvasGroup mCanvasGroup;
    private HitType mHitType;
    private AnimationCurve curve_pos;
    private AnimationCurve curve_scale;
    private AnimationCurve curve_alpha;
    private Vector3 screens;
    private float percent;

    static HPChanger()
    {
        Dictionary<HitType, float> dictionary = new Dictionary<HitType, float> {
            { 
                HitType.Normal,
                0.6f
            },
            { 
                HitType.Crit,
                0.83f
            },
            { 
                HitType.HeadShot,
                1.33f
            },
            { 
                HitType.Rebound,
                0.6f
            },
            { 
                HitType.Add,
                0.6f
            },
            { 
                HitType.Block,
                0.6f
            },
            { 
                HitType.Miss,
                0.6f
            },
            { 
                HitType.HPMaxChange,
                1.33f
            }
        };
        mTimes = dictionary;
    }

    private void Awake()
    {
        this.mTransform = base.transform;
        this.mCanvasGroup = base.GetComponent<CanvasGroup>();
        this.text = this.mTransform.Find("Text").GetComponent<Text>();
        this.FontSize = this.text.fontSize;
    }

    public void Despawn()
    {
        GameLogic.EffectCache(base.gameObject);
    }

    public void Init(EntityBase entity, HitStruct hs)
    {
        this.mHitType = hs.type;
        this.curve_pos = GameLogic.GetHPChangerAnimation(this.mHitType, 0);
        this.curve_scale = GameLogic.GetHPChangerAnimation(this.mHitType, 1);
        this.curve_alpha = GameLogic.GetHPChangerAnimation(this.mHitType, 2);
        this.starttime = Updater.AliveTime;
        this.OffsetX = GameLogic.Random((float) -50f, (float) 50f);
        this.OffsetY = GameLogic.Random((float) 0f, (float) 30f);
        this.MovePer.x = this.OffsetX / 14f;
        this.MovePer.y = this.OffsetY / 14f;
        this.CurrentMoveCount = 14;
        this.MoveAll = Vector3.zero;
        this.m_Entity = entity;
        this.entitypos = entity.position;
        this.entitybodypos = entity.m_Body.HPMask.transform.localPosition;
        if (hs.element != EElementType.eNone)
        {
            this.text.set_color(EntityData.ElementData[hs.element].color);
            this.text.text = hs.real_hit.ToString();
        }
        else
        {
            switch (this.mHitType)
            {
                case HitType.Crit:
                {
                    object[] args = new object[] { hs.real_hit };
                    this.text.text = Utils.FormatString("{0}!", args);
                    this.text.set_color(Color.red);
                    this.text.fontSize = (int) (this.FontSize * this.CritFontScale);
                    return;
                }
                case HitType.HeadShot:
                    this.text.text = GameLogic.Hold.Language.GetLanguageByTID("爆头", Array.Empty<object>());
                    this.text.set_color(Color.red);
                    this.text.fontSize = (int) (this.FontSize * this.HeadShotFontScale);
                    return;

                case HitType.Add:
                {
                    object[] args = new object[] { hs.real_hit };
                    this.text.text = Utils.FormatString("+{0}", args);
                    this.text.set_color(Color.green);
                    return;
                }
                case HitType.Block:
                    this.text.text = hs.real_hit.ToString();
                    this.text.set_color(Color.gray);
                    return;

                case HitType.Miss:
                    this.text.text = "Miss";
                    this.text.set_color(Color.yellow);
                    return;

                case HitType.HPMaxChange:
                {
                    string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("生命上限", Array.Empty<object>());
                    object[] args = new object[] { languageByTID, MathDxx.GetSymbolString(hs.real_hit), MathDxx.Abs(hs.real_hit) };
                    this.text.text = Utils.FormatString("{0}{1}{2}", args);
                    this.text.set_color((hs.real_hit < 0L) ? Color.red : Color.green);
                    return;
                }
            }
            this.text.text = hs.real_hit.ToString();
            this.text.set_color(Color.white);
            this.text.fontSize = this.FontSize;
        }
    }

    private void LateUpdate()
    {
        if ((this == null) || (this.mTransform == null))
        {
            this.Despawn();
        }
        else
        {
            this.percent = (Updater.AliveTime - this.starttime) / mTimes[this.mHitType];
            this.percent = MathDxx.Clamp01(this.percent);
            if (this.percent >= 1f)
            {
                this.Despawn();
            }
            else
            {
                if (this.m_Entity != null)
                {
                    this.entitypos = this.m_Entity.position;
                }
                if ((this.m_Entity != null) && (this.m_Entity.m_Body != null))
                {
                    this.entitybodypos = this.m_Entity.m_Body.HPMask.transform.localPosition;
                }
                if (this.CurrentMoveCount > 0)
                {
                    this.CurrentMoveCount--;
                    this.MoveAll += this.MovePer;
                }
                this.screens = Utils.World2Screen(this.entitypos);
                if (this.curve_pos != null)
                {
                    this.screens.y += ((this.entitybodypos.y * 23f) * GameLogic.HeightScale) + this.curve_pos.Evaluate(this.percent);
                }
                this.mTransform.position = this.screens + this.MoveAll;
                if (this.curve_scale != null)
                {
                    this.mTransform.localScale = Vector3.one * this.curve_scale.Evaluate(this.percent);
                }
                if ((this.curve_alpha != null) && (this.mCanvasGroup != null))
                {
                    this.mCanvasGroup.alpha = this.curve_alpha.Evaluate(this.percent);
                }
            }
        }
    }
}

