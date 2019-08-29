using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyFlyCtrl
{
    private ActionBasic action = new ActionBasic();
    private static Dictionary<CurrencyType, CurrencyFlyData> mList;

    static CurrencyFlyCtrl()
    {
        Dictionary<CurrencyType, CurrencyFlyData> dictionary = new Dictionary<CurrencyType, CurrencyFlyData>();
        CurrencyFlyData data = new CurrencyFlyData {
            path = "CurrencyFly_Gold",
            range = ((float) GameLogic.Width) / 7f
        };
        dictionary.Add(CurrencyType.Gold, data);
        data = new CurrencyFlyData {
            path = "CurrencyFly_Diamond",
            range = ((float) GameLogic.Width) / 7f
        };
        dictionary.Add(CurrencyType.Diamond, data);
        data = new CurrencyFlyData {
            path = "CurrencyFly_LevelExp",
            range = ((float) GameLogic.Width) / 7f
        };
        dictionary.Add(CurrencyType.LevelExp, data);
        data = new CurrencyFlyData {
            path = "CurrencyFly_Gold",
            range = ((float) GameLogic.Width) / 7f
        };
        dictionary.Add(CurrencyType.BattleGold, data);
        data = new CurrencyFlyData {
            path = "CurrencyFly_Key",
            range = ((float) GameLogic.Width) / 7f
        };
        dictionary.Add(CurrencyType.Key, data);
        data = new CurrencyFlyData {
            path = "CurrencyFly_Diamond",
            range = ((float) GameLogic.Width) / 7f
        };
        dictionary.Add(CurrencyType.BattleDiamond, data);
        mList = dictionary;
    }

    public void DeInit()
    {
        this.action.DeInit();
    }

    public static void GetCurrency(CurrencyType type, int count, Vector3 startpos)
    {
        CurrencyGetStruct body = new CurrencyGetStruct {
            type = type,
            count = count,
            startpos = startpos
        };
        Facade.Instance.SendNotification("GetCurrency", body);
    }

    private GameObject GetGameobject(string name, Transform parent, Vector3 startpos)
    {
        object[] args = new object[] { "UIPanel/CurrencyUI/", name };
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.GetString(args)));
        obj2.transform.SetParent(parent);
        obj2.transform.position = startpos;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.rotation = Quaternion.identity;
        Animation componentInChildren = obj2.GetComponentInChildren<Animation>();
        if (componentInChildren != null)
        {
            componentInChildren["CurrencyFlyRotate"].time = GameLogic.Random(0f, componentInChildren["CurrencyFlyRotate"].length);
        }
        return obj2;
    }

    private static Transform GetObject(string path, Vector3 startpos, Transform parent)
    {
        object[] args = new object[] { path };
        GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/CurrencyUI/{0}", args)));
        child.SetParentNormal(parent);
        RectTransform transform = child.transform as RectTransform;
        transform.position = startpos;
        return transform;
    }

    public static void PlayFlyAnimation(CurrencyType type, long allcount, Vector3 startpos, Vector3 endpos, Action<long> OnOverOne, Action onFinish, bool mask)
    {
        <PlayFlyAnimation>c__AnonStorey2 storey = new <PlayFlyAnimation>c__AnonStorey2 {
            allcount = allcount,
            type = type,
            OnOverOne = OnOverOne,
            mask = mask,
            onFinish = onFinish
        };
        if ((storey.allcount != 0L) && mList.ContainsKey(storey.type))
        {
            startpos = new Vector3(startpos.x, startpos.y, 0f);
            endpos = new Vector3(endpos.x, endpos.y, 0f);
            storey.count = (int) storey.allcount;
            if (storey.count > 20)
            {
                storey.count = 20;
            }
            storey.beforecount = 0L;
            float range = mList[storey.type].range;
            Vector3 vector = endpos - startpos;
            float num2 = Utils.getAngle(new Vector2(vector.x, vector.y));
            if (storey.mask)
            {
                WindowUI.ShowMask(true);
            }
            for (int i = 0; i < storey.count; i++)
            {
                <PlayFlyAnimation>c__AnonStorey1 storey2 = new <PlayFlyAnimation>c__AnonStorey1 {
                    <>f__ref$2 = storey,
                    index = i
                };
                if (!mList.ContainsKey(storey.type))
                {
                    object[] args = new object[] { storey.type };
                    SdkManager.Bugly_Report("CurrencyFlyCtrl", Utils.FormatString("PlayFlyAnimation {0} is not int mList.", args));
                }
                storey2.t = GetObject(mList[storey.type].path, startpos, GameNode.m_Front3);
                Vector3 vector2 = new Vector3(GameLogic.Random(-range, range), GameLogic.Random(-range, range), 0f);
                Vector3 vector3 = startpos + vector2;
                Sequence sequence = DOTween.Sequence();
                TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
                CanvasGroup component = storey2.t.GetComponent<CanvasGroup>();
                component.alpha = 0f;
                TweenSettingsExtensions.AppendInterval(sequence, storey2.index * 0.03f);
                TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOMove(storey2.t, vector3, 0.2f, false), true));
                TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions46.DOFade(component, 1f, 0.4f), true));
                TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOMove(storey2.t, endpos, 0.7f, false), true), 0x1a), new TweenCallback(storey2, this.<>m__0)));
                if (storey2.index == (storey.count - 1))
                {
                    TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey2, this.<>m__1));
                }
            }
        }
    }

    public static void PlayGet(CurrencyType type, long allcount, Action<long> OnOverOne = null, Action onFinish = null, bool mask = true)
    {
        PlayGet(type, allcount, new Vector3(((float) GameLogic.Width) / 2f, ((float) GameLogic.Height) / 2f, 0f), OnOverOne, onFinish, mask);
    }

    public static void PlayGet(CurrencyType type, long allcount, Vector3 startpos, Action<long> OnOverOne = null, Action onFinish = null, bool mask = true)
    {
        if (allcount > 0L)
        {
            IMediator mediator = Facade.Instance.RetrieveMediator("CurrencyModuleMediator");
            if (mediator != null)
            {
                Vector3 endpos = new Vector3((float) GameLogic.Width, (float) GameLogic.Height, 0f);
                if (type != CurrencyType.Gold)
                {
                    if (type == CurrencyType.Diamond)
                    {
                        endpos = (Vector3) mediator.GetEvent("GetEvent_GetDiamondPosition");
                    }
                    else if (type == CurrencyType.Key)
                    {
                        endpos = (Vector3) mediator.GetEvent("GetEvent_GetKeyPosition");
                    }
                }
                else
                {
                    endpos = (Vector3) mediator.GetEvent("GetEvent_GetGoldPosition");
                }
                PlayGet(type, allcount, startpos, endpos, OnOverOne, onFinish, mask);
            }
        }
    }

    public static void PlayGet(CurrencyType type, long allcount, Vector3 startpos, Vector3 endpos, Action<long> OnOverOne, Action onFinish, bool mask)
    {
        if (allcount > 0L)
        {
            PlayFlyAnimation(type, allcount, startpos, endpos, OnOverOne, onFinish, mask);
        }
    }

    public static void PlayKeyUse(long count, Vector3 startpos, Vector3 endpos, Action onFinish)
    {
        <PlayKeyUse>c__AnonStorey0 storey = new <PlayKeyUse>c__AnonStorey0 {
            onFinish = onFinish
        };
        WindowUI.ShowMask(true);
        storey.t = GetObject("CurrencyFly_Key", startpos, GameNode.m_FrontEvent);
        TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOMove(storey.t, endpos, 1f, false), true), 6), new TweenCallback(storey, this.<>m__0));
    }

    public void UseAction(string typename, Transform parent, Vector3 startpos, Vector3 endpos, long count, Action callback)
    {
        if ((parent != null) && (this.action.GetActionCount() <= 0))
        {
            this.action.Init(false);
            ActionBasic.ActionParallel parallel = new ActionBasic.ActionParallel();
            List<ActionBasic.ActionBase> list = new List<ActionBasic.ActionBase>();
            if (count < 1L)
            {
                count = 1L;
            }
            else if (count > 10L)
            {
                count = 10L;
            }
            for (int i = 0; i < count; i++)
            {
                CurrencyUseAction item = new CurrencyUseAction {
                    gameobject = this.GetGameobject(typename, parent, startpos),
                    endpos = endpos
                };
                list.Add(item);
            }
            parallel.list = list;
            this.action.AddAction(parallel);
            if (callback != null)
            {
                this.action.AddActionDelegate(callback);
            }
        }
    }

    public static void UseCurrency(CurrencyType type, long count, Vector3 endpos, Action callback)
    {
        CurrencyUseStruct body = new CurrencyUseStruct {
            type = type,
            count = count,
            endpos = endpos,
            callback = callback
        };
        Facade.Instance.SendNotification("UseCurrency", body);
    }

    [CompilerGenerated]
    private sealed class <PlayFlyAnimation>c__AnonStorey1
    {
        internal Transform t;
        internal int index;
        internal CurrencyFlyCtrl.<PlayFlyAnimation>c__AnonStorey2 <>f__ref$2;

        internal void <>m__0()
        {
            Object.Destroy(this.t.gameObject);
            long num = (long) ((((float) this.<>f__ref$2.allcount) / ((float) this.<>f__ref$2.count)) * (this.index + 1f));
            long diamond = num - this.<>f__ref$2.beforecount;
            this.<>f__ref$2.beforecount = num;
            if (this.<>f__ref$2.type != CurrencyType.Gold)
            {
                if (this.<>f__ref$2.type == CurrencyType.Diamond)
                {
                    LocalSave.Instance.Modify_ShowDiamond(diamond);
                }
                else if (this.<>f__ref$2.type == CurrencyType.Key)
                {
                    LocalSave.Instance.Modify_Key(diamond, true);
                }
            }
            else
            {
                LocalSave.Instance.Modify_ShowGold(diamond);
            }
            if (this.<>f__ref$2.OnOverOne != null)
            {
                this.<>f__ref$2.OnOverOne(diamond);
            }
        }

        internal void <>m__1()
        {
            if (this.<>f__ref$2.mask)
            {
                WindowUI.ShowMask(false);
            }
            if (this.<>f__ref$2.onFinish != null)
            {
                this.<>f__ref$2.onFinish();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayFlyAnimation>c__AnonStorey2
    {
        internal long allcount;
        internal int count;
        internal long beforecount;
        internal CurrencyType type;
        internal Action<long> OnOverOne;
        internal bool mask;
        internal Action onFinish;
    }

    [CompilerGenerated]
    private sealed class <PlayKeyUse>c__AnonStorey0
    {
        internal Transform t;
        internal Action onFinish;

        internal void <>m__0()
        {
            WindowUI.ShowMask(false);
            Object.Destroy(this.t.gameObject);
            if (this.onFinish != null)
            {
                this.onFinish();
            }
        }
    }

    private class CurrencyFlyData
    {
        public string path;
        public float range;
    }

    public class CurrencyGetStruct
    {
        public CurrencyType type;
        public long count;
        public Vector3 startpos;
    }

    public class CurrencyUseAction : ActionBasic.ActionUIBase
    {
        public GameObject gameobject;
        public Vector3 endpos;
        private Image image;
        private Vector3 startpos;
        private Vector3 startoffsetpos;
        private float offsets = 70f;
        private float offsettime;
        private float flytime;
        private float alphastarttime;
        private float starttime;
        private AnimationCurve curve;
        private bool bMoveOffset = true;

        protected override void OnInit()
        {
            this.image = this.gameobject.GetComponentInChildren<Image>();
            this.alphastarttime = GameLogic.Random((float) 0f, (float) 0.2f);
            this.offsettime = GameLogic.Random((float) 0.5f, (float) 0.8f);
            this.flytime = GameLogic.Random((float) 0.6f, (float) 0.9f);
            this.startpos = this.gameobject.transform.position;
            this.startoffsetpos = this.startpos + new Vector3(GameLogic.Random(-this.offsets, this.offsets), GameLogic.Random(-this.offsets, this.offsets), 0f);
            this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186a8);
            this.starttime = Updater.AliveTime;
            this.bMoveOffset = true;
        }

        protected override void OnUpdate()
        {
            if (this.gameobject == null)
            {
                base.End();
            }
            else if (this.bMoveOffset)
            {
                this.UpdateAlpha();
                this.UpdateOffset();
            }
            else
            {
                this.UpdateFly();
            }
        }

        private void UpdateAlpha()
        {
            float num = Updater.AliveTime - this.starttime;
            if (num > this.alphastarttime)
            {
                float num2 = num - this.alphastarttime;
                this.image.set_color(new Color(this.image.get_color().r, this.image.get_color().g, this.image.get_color().b, num2 / 0.3f));
            }
        }

        private void UpdateFly()
        {
            float num = Updater.AliveTime - this.starttime;
            float num2 = num / this.flytime;
            num2 = Mathf.Clamp01(num2);
            this.gameobject.transform.position = ((Vector3) (this.curve.Evaluate(num2) * (this.endpos - this.startoffsetpos))) + this.startoffsetpos;
            if (num2 == 1f)
            {
                if (this.gameobject != null)
                {
                    Object.Destroy(this.gameobject);
                }
                base.End();
            }
        }

        private void UpdateOffset()
        {
            float num = Updater.AliveTime - this.starttime;
            float num2 = num / (this.offsettime - 0.1f);
            num2 = Mathf.Clamp01(num2);
            this.gameobject.transform.position = this.startpos + ((this.startoffsetpos - this.startpos) * this.curve.Evaluate(num2));
            if (num >= this.offsettime)
            {
                this.starttime = Updater.AliveTime;
                this.bMoveOffset = false;
            }
        }
    }

    public class CurrencyUseStruct
    {
        public CurrencyType type;
        public long count;
        public Vector3 endpos;
        public Action callback;
    }
}

