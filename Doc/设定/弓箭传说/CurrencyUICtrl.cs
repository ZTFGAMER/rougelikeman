using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Key;
    public ButtonCtrl Button_Gold;
    public ButtonCtrl Button_Diamond;
    public CurrencyLevelCtrl mLevelCtrl;
    public Text Text_UseKey;
    public CanvasGroup mUseKey;
    public Transform Tran_Key;
    public Image Image_Key;
    public Image Image_Gold;
    public Image Image_Diamond;
    public Text Text_Gold;
    public Text Text_Diamond;
    public Text Text_Time;
    public Animation keyrotate;
    public ProgressTextCtrl mProgressCtrl;
    private static Dictionary<CurrencyType, string> mCurrencyPathList;
    private long mKeyStartTime;
    private int PerKeyTime;
    private CurrencyFlyCtrl mFlyCtrl;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;
    [CompilerGenerated]
    private static Action <>f__am$cache2;

    static CurrencyUICtrl()
    {
        Dictionary<CurrencyType, string> dictionary = new Dictionary<CurrencyType, string> {
            { 
                CurrencyType.Gold,
                "CurrencyFly_Gold"
            },
            { 
                CurrencyType.Diamond,
                "CurrencyFly_Diamond"
            },
            { 
                CurrencyType.Key,
                "CurrencyFly_Key"
            }
        };
        mCurrencyPathList = dictionary;
    }

    private Vector3 GetUseStartPos(CurrencyType type)
    {
        if (type != CurrencyType.Gold)
        {
            if (type == CurrencyType.Diamond)
            {
                if (this.Image_Diamond != null)
                {
                    return this.Image_Diamond.transform.position;
                }
            }
            else if ((type == CurrencyType.Key) && (this.Image_Key != null))
            {
                return this.Image_Key.transform.position;
            }
        }
        else
        {
            return this.Image_Gold?.transform.position;
        }
        throw new Exception("currencyui dont have CurrencyType." + type + " in CurrencyUICtrl.cs");
    }

    private void InitUI()
    {
        this.UpdateCurrency();
    }

    protected override void OnClose()
    {
        if (this.mFlyCtrl != null)
        {
            this.mFlyCtrl.DeInit();
        }
    }

    public override object OnGetEvent(string eventName)
    {
        if (eventName != null)
        {
            if (eventName == "GetEvent_GetGoldPosition")
            {
                return this.GetUseStartPos(CurrencyType.Gold);
            }
            if (eventName == "GetEvent_GetDiamondPosition")
            {
                return this.GetUseStartPos(CurrencyType.Diamond);
            }
            if (eventName == "GetEvent_GetKeyPosition")
            {
                return this.GetUseStartPos(CurrencyType.Key);
            }
        }
        return null;
    }

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name != null)
        {
            if (name == "PUB_UI_UPDATE_CURRENCY")
            {
                this.UpdateCurrency();
            }
            else if (name == "CurrencyKeyRotate")
            {
                if (this.keyrotate != null)
                {
                    this.keyrotate.Play("KeyRotate");
                }
            }
            else if (name == "UseCurrencyKey")
            {
                this.SetUseKeyShow(true);
                if (this.mUseKey != null)
                {
                    this.mUseKey.alpha = 1f;
                    this.mUseKey.transform.localPosition = Vector3.zero;
                    TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions46.DOFade(this.mUseKey, 0.8f, 1f)), ShortcutExtensions.DOLocalMoveY(this.mUseKey.transform, -100f, 1f, false)), new TweenCallback(this, this.<OnHandleNotification>m__3));
                }
            }
            else if (name == "UseCurrency")
            {
                CurrencyFlyCtrl.CurrencyUseStruct struct2 = (CurrencyFlyCtrl.CurrencyUseStruct) body;
                if (this.mFlyCtrl == null)
                {
                    this.mFlyCtrl = new CurrencyFlyCtrl();
                }
                this.mFlyCtrl.UseAction(mCurrencyPathList[struct2.type], base.transform, this.GetUseStartPos(struct2.type), struct2.endpos, struct2.count, struct2.callback);
            }
            else if (name == "GetCurrency")
            {
                CurrencyFlyCtrl.CurrencyGetStruct struct3 = (CurrencyFlyCtrl.CurrencyGetStruct) body;
                if (this.mFlyCtrl == null)
                {
                    this.mFlyCtrl = new CurrencyFlyCtrl();
                }
                this.mFlyCtrl.UseAction(mCurrencyPathList[struct3.type], base.transform, struct3.startpos, this.GetUseStartPos(struct3.type), struct3.count, null);
            }
        }
    }

    protected override void OnInit()
    {
        this.SetUseKeyShow(false);
        object[] args = new object[] { GameConfig.GetModeLevelKey() };
        this.Text_UseKey.text = Utils.FormatString("-{0}", args);
        (base.transform as RectTransform).anchoredPosition = new Vector3(0f, PlatformHelper.GetFringeHeight(), 0f);
        this.PerKeyTime = GameConfig.GetKeyRecoverTime();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate {
                KeyBuyUICtrl.SetSource(KeyBuySource.ECURRENCY);
                WindowUI.ShowWindow(WindowID.WindowID_KeyBuy);
            };
        }
        this.Button_Key.onClick = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneGold");
        }
        this.Button_Gold.onClick = <>f__am$cache1;
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = () => Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneDiamond");
        }
        this.Button_Diamond.onClick = <>f__am$cache2;
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void SetUseKeyShow(bool value)
    {
        if (this.mUseKey != null)
        {
            this.mUseKey.gameObject.SetActive(value);
        }
    }

    private void Update()
    {
        if (this.Text_Time != null)
        {
            if (LocalSave.Instance.IsKeyMax())
            {
                if (this.Text_Time.text != string.Empty)
                {
                    this.Text_Time.text = string.Empty;
                }
            }
            else
            {
                long currentTime = Utils.CurrentTime;
                long key = (long) (((float) (currentTime - this.mKeyStartTime)) / ((float) this.PerKeyTime));
                if (key > 0L)
                {
                    LocalSave.Instance.Modify_Key(key, false);
                    this.mKeyStartTime += key * this.PerKeyTime;
                    LocalSave.Instance.SetKeyTime(this.mKeyStartTime);
                    this.UpdateCurrency();
                }
                else
                {
                    int second = this.PerKeyTime - ((int) (currentTime - this.mKeyStartTime));
                    string str = Utils.GetSecond2String(second);
                    this.Text_Time.text = str;
                }
            }
        }
    }

    private void UpdateCurrency()
    {
        this.mKeyStartTime = LocalSave.Instance.GetKeyTime();
        LocalSave.UserInfo userInfo = LocalSave.Instance.GetUserInfo();
        int maxKeyCount = GameConfig.GetMaxKeyCount();
        this.mProgressCtrl.max = maxKeyCount;
        this.mProgressCtrl.current = userInfo.Key;
        this.mLevelCtrl.UpdateUI();
        if (this.Text_Gold != null)
        {
            this.Text_Gold.text = userInfo.Show_Gold.ToString();
        }
        if (this.Text_Diamond != null)
        {
            this.Text_Diamond.text = userInfo.Show_Diamond.ToString();
        }
    }
}

