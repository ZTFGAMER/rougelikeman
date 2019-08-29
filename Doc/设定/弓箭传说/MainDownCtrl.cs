using Dxx.Util;
using PureMVC.Patterns;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainDownCtrl : MonoBehaviour
{
    private const int PageCount = 5;
    private GameObject[] locksimage = new GameObject[5];
    private GameObject[] images = new GameObject[5];
    private ButtonCtrl[] buttons = new ButtonCtrl[5];
    private Text[] texts = new Text[5];
    private bool[] locks = new bool[] { true, true, true, true, true, false, false, false };
    private RedNodeCtrl[] mReds = new RedNodeCtrl[5];
    private ScrollRectBase mScrollRect;
    private Transform bottomline;
    private bool bInit;

    private void Awake()
    {
        this.init();
    }

    public bool GetLock(int index) => 
        this.locks[index];

    private void init()
    {
        if (!this.bInit)
        {
            this.bInit = true;
            for (int i = 0; i < 5; i++)
            {
                if (i != 2)
                {
                    object[] objArray1 = new object[] { i };
                    this.locksimage[i] = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button/fg/Lock", objArray1)).gameObject;
                    object[] objArray2 = new object[] { i };
                    this.images[i] = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button/fg/Image", objArray2)).gameObject;
                    object[] objArray3 = new object[] { i };
                    this.buttons[i] = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button", objArray3)).GetComponent<ButtonCtrl>();
                    this.locks[i] = true;
                }
                else
                {
                    this.locks[i] = false;
                }
                object[] args = new object[] { i };
                this.texts[i] = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button/fg/Text", args)).GetComponent<Text>();
                object[] objArray5 = new object[] { i };
                Transform transform = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button/fg/RedNode", objArray5));
                if (transform != null)
                {
                    this.mReds[i] = transform.GetComponent<RedNodeCtrl>();
                }
            }
            float bottomHeight = PlatformHelper.GetBottomHeight();
            (base.transform as RectTransform).anchoredPosition = new Vector2(0f, bottomHeight);
            this.bottomline = base.transform.Find("Bottom");
            if (this.bottomline != null)
            {
                RectTransform bottomline = this.bottomline as RectTransform;
                bottomline.anchoredPosition = new Vector2(0f, -bottomHeight / GameLogic.WidthScaleAll);
            }
        }
    }

    public void OnLanguageChange()
    {
        this.texts[0].text = GameLogic.Hold.Language.GetLanguageByTID("Main_Shop", Array.Empty<object>());
        this.texts[1].text = GameLogic.Hold.Language.GetLanguageByTID("Main_Equip", Array.Empty<object>());
        this.texts[2].text = GameLogic.Hold.Language.GetLanguageByTID("Main_Battle", Array.Empty<object>());
        this.texts[3].text = GameLogic.Hold.Language.GetLanguageByTID("Main_Talent", Array.Empty<object>());
        this.texts[4].text = GameLogic.Hold.Language.GetLanguageByTID("设置_标题", Array.Empty<object>());
    }

    public void SetRedCount(int index, int count)
    {
        this.init();
        if (this.GetLock(index))
        {
            count = 0;
        }
        if (this.mReds[index] != null)
        {
            this.mReds[index].Value = count;
        }
    }

    public void SetRedNodeType(int index, RedNodeType type)
    {
        this.init();
        if (this.mReds[index] != null)
        {
            this.mReds[index].SetType(type);
        }
    }

    public void SetScrollRect(ScrollRectBase scroll)
    {
        this.mScrollRect = scroll;
    }

    private void Start()
    {
        this.mReds[0].SetType(RedNodeType.eRedCount);
        this.mReds[3].SetType(RedNodeType.eGreenUp);
    }

    public void UpdateLock(int index)
    {
        this.init();
        bool flag = false;
        switch (index)
        {
            case 0:
                break;

            case 1:
                flag = GameLogic.Hold.Guide.mEquip.process == 0;
                break;

            case 3:
                flag = GameLogic.Hold.Guide.mCard.process == 0;
                break;

            case 4:
                flag = false;
                break;

            default:
                flag = false;
                break;
        }
        this.UpdateLock(index, flag);
        this.mScrollRect.SetLocks(this.locks);
    }

    private void UpdateLock(int index, bool _lock)
    {
        this.init();
        if (index != 2)
        {
            if ((this.locksimage[index] != null) && (this.locksimage[index].activeSelf != _lock))
            {
                this.locksimage[index].SetActive(_lock);
            }
            if ((this.images[index] != null) && (this.images[index].activeSelf == _lock))
            {
                this.images[index].SetActive(!_lock);
            }
            this.locks[index] = _lock;
            if (this.buttons[index] != null)
            {
                this.buttons[index].SetEnable(!_lock);
            }
        }
    }

    public void UpdateUI()
    {
        bool flag = false;
        for (int i = 0; i < 5; i++)
        {
            bool flag2 = false;
            switch (i)
            {
                case 0:
                    break;

                case 1:
                    flag2 = GameLogic.Hold.Guide.mEquip.process == 0;
                    Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
                    break;

                case 3:
                    flag2 = GameLogic.Hold.Guide.mCard.process == 0;
                    Facade.Instance.SendNotification("MainUI_CardRedCountUpdate");
                    break;

                case 4:
                    flag2 = false;
                    break;

                default:
                    flag2 = false;
                    break;
            }
            this.UpdateLock(i, flag2);
            if (flag2)
            {
                flag = true;
            }
        }
        this.mScrollRect.DragDisableForce = false;
        this.mScrollRect.SetLocks(this.locks);
    }
}

