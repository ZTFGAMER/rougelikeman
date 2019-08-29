using PureMVC.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MatchDefenceTimeUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Match;
    public Text Text_Match;
    public GameObject match_obj;

    private void InitUI()
    {
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.Button_Match.onClick = delegate {
            switch (Singleton<MatchDefenceTimeSocketCtrl>.Instance.State)
            {
                case MatchDefenceTimeSocketCtrl.ConnectState.eConnected:
                    this.StopMatch();
                    Singleton<MatchDefenceTimeSocketCtrl>.Instance.Close();
                    break;

                case MatchDefenceTimeSocketCtrl.ConnectState.eClose:
                    this.StartMatch();
                    Singleton<MatchDefenceTimeSocketCtrl>.Instance.Connect();
                    break;
            }
        };
        RectTransform parent = this.Button_Match.transform.parent as RectTransform;
        parent.anchoredPosition = new Vector2(0f, GameLogic.Height * 0.23f);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.StopMatch();
        this.InitUI();
    }

    private void StartMatch()
    {
        this.match_obj.SetActive(true);
        this.Text_Match.text = "取消匹配";
    }

    private void StopMatch()
    {
        this.match_obj.SetActive(false);
        this.Text_Match.text = "匹配";
    }
}

