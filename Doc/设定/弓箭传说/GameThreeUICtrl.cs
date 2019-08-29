using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameThreeUICtrl : MonoBehaviour
{
    private List<Transform> list = new List<Transform>();
    private List<Animation> animationlist = new List<Animation>();
    private List<Transform> shadowlist = new List<Transform>();
    private List<ButtonCtrl> buttonlist = new List<ButtonCtrl>();
    private List<Vector3> listpos = new List<Vector3>();
    private GameObject sieve;
    private GameThreeActionCtrl ctrl;
    private Text textcontentcount;
    private int count;
    private Action mCallback;
    private ActionBasic action1 = new ActionBasic();

    private void ActionEnd()
    {
        this.ButtonEnable(true);
        this.CupRotatePlay(true);
    }

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            <Awake>c__AnonStorey0 storey = new <Awake>c__AnonStorey0 {
                $this = this
            };
            object[] args = new object[] { "child/three/", i };
            this.list.Add(base.transform.Find(Utils.GetString(args)));
            this.animationlist.Add(this.list[i].GetComponent<Animation>());
            object[] objArray2 = new object[] { "child/three/shadow", i };
            this.shadowlist.Add(base.transform.Find(Utils.GetString(objArray2)));
            storey.index = i;
            this.buttonlist.Add(this.list[storey.index].Find("cup").GetComponent<ButtonCtrl>());
            this.buttonlist[storey.index].onClick = new Action(storey.<>m__0);
            this.listpos.Add(this.list[i].localPosition);
        }
        this.ButtonEnable(false);
        this.textcontentcount = base.transform.Find("Title/Text_Content_Count").GetComponent<Text>();
        this.sieve = base.transform.Find("child/three/sieve").gameObject;
        this.ctrl = new GameThreeActionCtrl();
        this.ctrl.Init(true);
        this.action1.Init(true);
    }

    private void ButtonEnable(bool enable)
    {
        for (int i = 0; i < 3; i++)
        {
            this.buttonlist[i].enabled = enable;
        }
    }

    private void CupRotatePlay(bool play)
    {
        if (play)
        {
            for (int i = 0; i < 3; i++)
            {
                this.animationlist[i].Play("CupRotate");
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                this.animationlist[i].Stop();
            }
        }
    }

    private void DoAction()
    {
        this.action1.ActionClear();
        ActionBasic.ActionWaitIgnoreTime action = new ActionBasic.ActionWaitIgnoreTime {
            waitTime = 0.6f
        };
        this.action1.AddAction(action);
        ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
            action = new Action(this.DoActionMust)
        };
        this.action1.AddAction(delegate2);
    }

    private void DoActionMust()
    {
        for (int i = 0; i < 3; i++)
        {
            this.list[i].localPosition = this.listpos[i];
        }
        this.sieve.transform.localPosition = Vector3.zero;
        this.ctrl.DoAction(this.list, this.shadowlist, this.sieve, new Action(this.ActionEnd));
    }

    public void DoAllActions()
    {
        this.DoAction();
    }

    private void OnClick(Transform t)
    {
        this.ButtonEnable(false);
        this.CupRotatePlay(false);
        this.ctrl.OnClickOne(t, this.sieve.transform, new Action<bool>(this.OnClickEnd));
    }

    private void OnClickEnd(bool result)
    {
        object[] args = new object[] { "result ", result };
        Debugger.Log(Utils.GetString(args));
        this.count--;
        this.UpdateCountUI();
        if (this.count > 0)
        {
            this.DoAction();
        }
        else
        {
            this.mCallback();
        }
    }

    public void RemoveAction()
    {
        this.ctrl.DeInit();
        this.action1.DeInit();
    }

    public void SetCount(int count)
    {
        this.count = count;
        this.UpdateCountUI();
    }

    public void SetEndCallback(Action callback)
    {
        this.mCallback = callback;
    }

    public void UpdateCountUI()
    {
        object[] args = new object[] { GameLogic.Hold.Language.GetLanguageByTID("剩余次数", Array.Empty<object>()), " : ", this.count };
        this.textcontentcount.text = Utils.GetString(args);
    }

    [CompilerGenerated]
    private sealed class <Awake>c__AnonStorey0
    {
        internal int index;
        internal GameThreeUICtrl $this;

        internal void <>m__0()
        {
            this.$this.OnClick(this.$this.list[this.index]);
        }
    }
}

