using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuideManager
{
    private int guideStep;
    private GameObject currentobj;
    private bool bBattleNeedGuide = true;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private GuideCard <mCard>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private GuideEquip <mEquip>k__BackingField;

    public void Card_DeInit()
    {
        if (this.mCard != null)
        {
            this.mCard.DeInit();
        }
    }

    private void Card_Init()
    {
        if (this.mCard == null)
        {
            this.mCard = new GuideCard();
        }
        this.mCard.Init();
    }

    public void DeInit()
    {
        this.RemoveLastGuide();
        this.Card_DeInit();
        this.Equip_DeInit();
    }

    public void Equip_DeInit()
    {
        if (this.mEquip != null)
        {
            this.mEquip.DeInit();
        }
    }

    private void Equip_Init()
    {
        if (this.mEquip == null)
        {
            this.mEquip = new GuideEquip();
        }
        this.mEquip.Init();
    }

    public bool GetFlowerAttack()
    {
        if (this.bBattleNeedGuide && (GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID() < 3))
        {
            return false;
        }
        return true;
    }

    private GameObject GetGuideObj(int index)
    {
        object[] args = new object[] { index };
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("Game/Map/Guide/Guide{0}", args)));
        MeshRenderer[] componentsInChildren = obj2.GetComponentsInChildren<MeshRenderer>();
        int num = 0;
        int length = componentsInChildren.Length;
        while (num < length)
        {
            componentsInChildren[num].sortingLayerName = "MapBG";
            num++;
        }
        object[] objArray2 = new object[] { index };
        obj2.transform.Find("text_content").GetComponent<TextMesh>().text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("新手引导_{0}", objArray2), Array.Empty<object>());
        return obj2;
    }

    public bool GetNeedGuide() => 
        this.bBattleNeedGuide;

    public void GuideBattleNext()
    {
        if (this.GetNeedGuide())
        {
            this.RemoveLastGuide();
            this.guideStep++;
            switch (this.guideStep)
            {
                case 1:
                case 2:
                case 3:
                    this.currentobj = this.GetGuideObj(this.guideStep);
                    GameLogic.Release.Mode.RoomGenerate.AddGuildToMap(this.currentobj);
                    break;
            }
            if (this.guideStep == 3)
            {
                PlayerPrefsEncrypt.SetBool("guide_battle", true);
                this.bBattleNeedGuide = false;
                LocalSave.Instance.SaveExtra.SetGuideBattleProcess(1);
            }
        }
    }

    public void Init()
    {
        if (this.bBattleNeedGuide && ((!LocalSave.Instance.GetNoCard() || (LocalSave.Instance.GetHaveEquips(true).Count > 1)) || ((LocalSave.Instance.GetGold() > 100L) || PlayerPrefsEncrypt.GetBool("guide_battle", false))))
        {
            this.bBattleNeedGuide = false;
            LocalSave.Instance.SaveExtra.SetGuideBattleProcess(2);
        }
        this.Card_Init();
        this.Equip_Init();
    }

    private void RemoveLastGuide()
    {
        if (this.currentobj != null)
        {
            Object.Destroy(this.currentobj);
            this.currentobj = null;
        }
    }

    public GuideCard mCard { get; private set; }

    public GuideEquip mEquip { get; private set; }

    public class GuideCard : GuideManager.GuideUIBase
    {
        protected override bool GetCanStartGuide() => 
            LocalSave.Instance.GetNoCard();

        protected override void OnInit()
        {
            if (base.process != 3)
            {
                base.process = 0;
                if (!this.GetCanStartGuide())
                {
                    base.process = 3;
                }
                base.guidecount = 2;
            }
        }
    }

    public class GuideData
    {
        public int index;
        public RectTransform t;

        public override string ToString()
        {
            object[] args = new object[] { this.index, this.t.name };
            return Utils.FormatString("index:{0} t:{1}", args);
        }
    }

    public class GuideEquip : GuideManager.GuideUIBase
    {
        public override bool CheckGuide() => 
            base.CheckGuide();

        protected override bool GetCanStartGuide() => 
            true;

        protected override void OnInit()
        {
            base.guidecount = 3;
            if (base.process != 3)
            {
                base.process = 0;
                if (LocalSave.Instance.GetHaveEquips(true).Count > 1)
                {
                    base.process = 3;
                }
            }
        }
    }

    public abstract class GuideUIBase
    {
        public int process;
        protected int guidecount;
        protected Dictionary<int, GuideManager.GuideData> mGuideList = new Dictionary<int, GuideManager.GuideData>();
        protected int mGuideIndex;
        protected Action mGuideUpdate;
        protected GuideOneCtrl _GuideOneCtrl;

        protected GuideUIBase()
        {
        }

        private void CheckBug()
        {
            if (this.guidecount == 0)
            {
                object[] args = new object[] { base.GetType().ToString() };
                SdkManager.Bugly_Report("GuideManager_Equip", Utils.FormatString("GuideUIBase.Init guidecount == 0 in [{0}]", args));
            }
        }

        public virtual bool CheckGuide()
        {
            if (WindowUI.IsWindowOpened(WindowID.WindowID_Guide))
            {
                return false;
            }
            this.CheckBug();
            if (this.process != 1)
            {
                return false;
            }
            this.process = 2;
            WindowUI.ShowWindow(WindowID.WindowID_Guide);
            this.GoNext(this.mGuideList[0]);
            return true;
        }

        public virtual void CurrentOver(int index)
        {
            if ((index == (this.guidecount - 1)) && (this.process == 2))
            {
                this.show_guide(false);
                this.process = 3;
                this.OnGuideEnd();
                WindowUI.CloseWindow(WindowID.WindowID_Guide);
            }
            else if (!this.IsGuideOver())
            {
                this.show_guide(false);
            }
        }

        public void DeInit()
        {
            if (this._GuideOneCtrl != null)
            {
                this.show_guide(false);
                Object.Destroy(this._GuideOneCtrl.gameObject);
                this._GuideOneCtrl = null;
            }
            this.OnDeInit();
        }

        protected abstract bool GetCanStartGuide();
        public void GoNext(GuideManager.GuideData data)
        {
            if (!this.mGuideList.ContainsKey(data.index))
            {
                this.mGuideList.Add(data.index, data);
            }
            else if (this.mGuideList[data.index].t == null)
            {
                this.mGuideList[data.index] = data;
            }
            if (this.IsGuiding())
            {
                this.show_guide(true);
                this.mGuideOneCtrl.Init(data.t);
                if (this.mGuideUpdate != null)
                {
                    this.mGuideUpdate();
                }
            }
        }

        public void GoNext(int index, RectTransform t)
        {
            GuideManager.GuideData data = new GuideManager.GuideData {
                index = index,
                t = t
            };
            this.GoNext(data);
        }

        public void Init()
        {
            this.OnInit();
            this.CheckBug();
        }

        public bool IsGuideOver() => 
            ((this.process == 0) || (this.process == 3));

        public bool IsGuiding() => 
            (this.process == 2);

        protected virtual void OnDeInit()
        {
        }

        protected virtual void OnGuideEnd()
        {
        }

        protected virtual void OnInit()
        {
        }

        private void show_guide(bool value)
        {
            if (this.mGuideOneCtrl != null)
            {
                this.mGuideOneCtrl.gameObject.SetActive(value);
            }
            EventSystemCtrl.Instance.SetDragEnable(!value);
        }

        public void StartGuide()
        {
            if ((this.process == 0) && this.GetCanStartGuide())
            {
                this.process = 1;
                WindowUI.ShowMask(true);
                WindowUI.ShowMask(false);
            }
        }

        protected GuideOneCtrl mGuideOneCtrl
        {
            get
            {
                if (this._GuideOneCtrl == null)
                {
                    GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/GuideUI/guide"));
                    this._GuideOneCtrl = obj2.GetComponent<GuideOneCtrl>();
                    RectTransform transform = obj2.transform as RectTransform;
                    transform.SetParent(GameNode.m_FrontMask);
                    transform.localPosition = Vector3.zero;
                    transform.anchoredPosition = Vector3.zero;
                    transform.localScale = Vector3.one;
                    transform.localRotation = Quaternion.identity;
                }
                return this._GuideOneCtrl;
            }
        }
    }
}

