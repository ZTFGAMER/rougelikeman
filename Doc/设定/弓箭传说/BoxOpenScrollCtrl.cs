using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class BoxOpenScrollCtrl : MonoBehaviour
{
    public GameObject scrollparent;
    public GameObject itemone;
    public Action OnScrollEnd;
    private int count = 0x19;
    private float perwidth;
    private float endposx;
    private List<BoxOpenOneCtrl> mList = new List<BoxOpenOneCtrl>();
    private RectTransform listone;
    private LocalUnityObjctPool mPool;
    private int state;
    private float maxrotatetime = 2f;
    private float maxlength;
    private float maxspeed = 1500f;
    private float framedis;
    private float framealldis;
    private List<int> mEquips;
    private int currentindex;
    private int gocount;
    private int resultequipid;
    private int resultindex;
    private int resultcount;

    private void Awake()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<BoxOpenOneCtrl>(this.itemone);
        this.perwidth = (this.itemone.transform as RectTransform).sizeDelta.x;
    }

    private void DealSlow()
    {
        for (int i = 0; i < this.count; i++)
        {
            <DealSlow>c__AnonStorey0 storey = new <DealSlow>c__AnonStorey0 {
                $this = this,
                index = i
            };
            RectTransform mRectTransform = this.mList[storey.index].mRectTransform;
            TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(mRectTransform, mRectTransform.localPosition.x - (this.perwidth * 15f), 4f, false), 9), new TweenCallback(storey, this.<>m__0));
        }
    }

    private List<int> GetEquips()
    {
        List<int> list = new List<int>();
        IList<Equip_equip> allBeans = LocalModelManager.Instance.Equip_equip.GetAllBeans();
        int num = 0;
        int count = allBeans.Count;
        while (num < count)
        {
            list.Add(allBeans[num].Id);
            num++;
        }
        return list;
    }

    private int GetResultIndex()
    {
        int num = 0;
        int count = this.mEquips.Count;
        while (num < count)
        {
            if (this.mEquips[num] == this.resultequipid)
            {
                return num;
            }
            num++;
        }
        object[] args = new object[] { this.resultequipid };
        SdkManager.Bugly_Report("BoxOpenScrollCtrl", Utils.FormatString("GetResultIndex[{0}] is error.", args));
        return -1;
    }

    private void GotoNextEquip()
    {
        this.currentindex++;
        this.currentindex = this.currentindex % this.mEquips.Count;
    }

    public void Init()
    {
        this.mEquips = this.GetEquips();
        this.mEquips.RandomSort<int>();
        this.framealldis = 0f;
        this.mList.Clear();
        this.mPool.Collect<BoxOpenOneCtrl>();
        for (int i = 0; i < this.count; i++)
        {
            BoxOpenOneCtrl item = this.mPool.DeQueue<BoxOpenOneCtrl>();
            item.Init(this.mEquips[i]);
            RectTransform child = item.transform as RectTransform;
            child.name = i.ToString();
            child.SetParentNormal(this.scrollparent);
            child.anchoredPosition = new Vector2(this.perwidth * i, 0f);
            this.mList.Add(item);
        }
        this.endposx = this.perwidth * this.count;
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0f), new TweenCallback(this, this.<Init>m__0));
    }

    public void ReceiveData(Drop_DropModel.DropData transfer)
    {
        int resultindex;
        this.resultequipid = transfer.id;
        this.resultcount = transfer.count;
        this.resultindex = this.GetResultIndex();
        if ((this.resultindex > this.currentindex) && (this.resultindex < (this.currentindex + 20)))
        {
            resultindex = this.currentindex + 20;
            resultindex = resultindex % this.mEquips.Count;
            this.mEquips.RemoveAt(this.resultindex);
            this.mEquips.Insert(resultindex, this.resultequipid);
        }
        else
        {
            resultindex = this.resultindex;
        }
        this.currentindex = resultindex - 15;
        if (this.currentindex < 0)
        {
            this.currentindex += this.mEquips.Count;
        }
        this.resultindex = this.GetResultIndex();
        this.maxlength = (this.gocount + 0x17) * this.perwidth;
        this.state = 2;
    }

    private void StartScroll()
    {
        this.gocount = 0;
        this.mEquips.RandomSort<int>();
        this.currentindex = 0;
        this.framealldis = 0f;
        this.state = 1;
        for (int i = 0; i < this.count; i++)
        {
            this.mList[i].mRectTransform.anchoredPosition = new Vector2(this.perwidth * i, 0f);
            this.mList[i].Init(this.mEquips[this.currentindex]);
            this.GotoNextEquip();
        }
    }

    private void Update()
    {
        switch (this.state)
        {
            case 1:
                this.framedis = Updater.delta * this.maxspeed;
                this.framealldis += this.framedis;
                for (int i = this.mList.Count - 1; i >= 0; i--)
                {
                    this.listone = this.mList[i].mRectTransform;
                    this.listone.anchoredPosition = new Vector2(this.listone.anchoredPosition.x - this.framedis, 0f);
                    if (this.listone.anchoredPosition.x <= (-this.perwidth * 2f))
                    {
                        this.listone.anchoredPosition = new Vector2(this.listone.anchoredPosition.x + this.endposx, 0f);
                        this.mList[i].Init(this.mEquips[this.currentindex]);
                        this.gocount++;
                        this.GotoNextEquip();
                    }
                }
                break;

            case 2:
                this.framedis = Updater.delta * this.maxspeed;
                if ((this.framealldis + this.framedis) > this.maxlength)
                {
                    this.framedis = this.maxlength - this.framealldis;
                    this.state = 3;
                }
                this.framealldis += this.framedis;
                for (int i = this.mList.Count - 1; i >= 0; i--)
                {
                    this.listone = this.mList[i].mRectTransform;
                    this.listone.anchoredPosition = new Vector2(this.listone.anchoredPosition.x - this.framedis, 0f);
                    if (this.listone.anchoredPosition.x <= (-this.perwidth * 2f))
                    {
                        this.listone.anchoredPosition = new Vector2(this.listone.anchoredPosition.x + this.endposx, 0f);
                        this.mList[i].Init(this.mEquips[this.currentindex]);
                        this.GotoNextEquip();
                    }
                }
                if (this.state == 3)
                {
                    this.DealSlow();
                }
                break;

            case 0:
                return;
        }
    }

    [CompilerGenerated]
    private sealed class <DealSlow>c__AnonStorey0
    {
        internal int index;
        internal BoxOpenScrollCtrl $this;

        internal void <>m__0()
        {
            if ((this.index == (this.$this.count - 1)) && (this.$this.OnScrollEnd != null))
            {
                this.$this.OnScrollEnd();
            }
        }
    }
}

