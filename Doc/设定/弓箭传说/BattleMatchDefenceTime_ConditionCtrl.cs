using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleMatchDefenceTime_ConditionCtrl : MonoBehaviour
{
    public Text Text_Time;
    public Text Text_Me_Name;
    public Text Text_Me_Score;
    public Text Text_Other_Name;
    public Text Text_Other_Score;
    public RectTransform Progress_BG;
    public RectTransform Progress_Me;
    public RectTransform Progress_Other;
    public RectTransform Progress_Light;
    public BattleMatchDefenceTime_InfoCtrl mInfoCtrl;
    private float allwidth;
    private float height;
    private int score_me;
    private int score_other;
    private Transform t_name;
    private Sequence seq_name;

    private void Awake()
    {
        if (this.Progress_BG != null)
        {
            this.allwidth = this.Progress_BG.sizeDelta.x;
        }
        if (this.Progress_Me != null)
        {
            this.height = this.Progress_Me.sizeDelta.y;
        }
        this.update_progress();
    }

    public bool isWin() => 
        (this.score_me > this.score_other);

    private void KillSeq()
    {
        if (this.seq_name != null)
        {
            TweenExtensions.Kill(this.seq_name, false);
            this.seq_name = null;
        }
    }

    public void SetMeName(string name)
    {
        if (this.Text_Me_Name != null)
        {
            this.Text_Me_Name.text = name;
        }
    }

    public void SetMeScore(int value)
    {
        this.score_me = value;
        if (this.Text_Me_Score != null)
        {
            this.Text_Me_Score.text = value.ToString();
        }
        this.update_progress();
    }

    public void SetOtherName(string name)
    {
        if (this.Text_Other_Name != null)
        {
            this.Text_Other_Name.text = name;
        }
    }

    public void SetOtherScore(int value)
    {
        this.score_other = value;
        if (this.Text_Other_Score != null)
        {
            this.Text_Other_Score.text = value.ToString();
        }
        this.update_progress();
    }

    public void SetTime(int time)
    {
        if (this.Text_Time != null)
        {
            this.Text_Time.text = Utils.GetSecond2String(time);
        }
    }

    public void ShowInfo(string eventname, object body)
    {
        if (this.mInfoCtrl != null)
        {
            this.mInfoCtrl.ShowInfo(eventname, body);
        }
    }

    private void update_progress()
    {
        if (((this.Progress_Me != null) && (this.Progress_Other != null)) && (this.Progress_Light != null))
        {
            if ((this.score_me == 0) && (this.score_other == 0))
            {
                this.Progress_Me.sizeDelta = new Vector2(this.allwidth / 2f, this.height);
                this.Progress_Other.sizeDelta = new Vector2(this.allwidth / 2f, this.height);
                this.Progress_Light.anchoredPosition = new Vector2(0f, 0f);
            }
            else if (this.score_other == 0)
            {
                this.Progress_Me.sizeDelta = new Vector2(this.allwidth, this.height);
                this.Progress_Other.sizeDelta = new Vector2(0f, this.height);
                this.Progress_Light.anchoredPosition = new Vector2(this.allwidth / 2f, 0f);
            }
            else
            {
                float num = this.score_me + this.score_other;
                float num2 = ((float) this.score_me) / num;
                float x = num2 * this.allwidth;
                this.Progress_Me.sizeDelta = new Vector2(x, this.height);
                this.Progress_Other.sizeDelta = new Vector2(this.allwidth - x, this.height);
                this.Progress_Light.anchoredPosition = new Vector2(x - (this.allwidth / 2f), 0f);
                Transform transform = null;
                if (this.score_other > this.score_me)
                {
                    transform = this.Text_Other_Name.transform;
                }
                else if (this.score_me > this.score_other)
                {
                    transform = this.Text_Me_Name.transform;
                }
                if (transform != null)
                {
                    if (transform != this.t_name)
                    {
                        this.KillSeq();
                        if (this.t_name != null)
                        {
                            this.t_name.localScale = Vector3.one;
                            this.t_name.localRotation = Quaternion.identity;
                        }
                        this.t_name = transform;
                        this.t_name.localScale = Vector3.one * 1.35f;
                        this.seq_name = DOTween.Sequence();
                        TweenSettingsExtensions.Append(this.seq_name, ShortcutExtensions.DOScale(this.t_name, Vector3.one * 1.5f, 0.1f));
                        TweenSettingsExtensions.Join(this.seq_name, ShortcutExtensions.DOShakeRotation(this.t_name, 0.1f, 5f, 10, 90f, true));
                        TweenSettingsExtensions.SetEase<Sequence>(this.seq_name, 6);
                        TweenSettingsExtensions.SetLoops<Sequence>(this.seq_name, -1, 1);
                        TweenSettingsExtensions.SetUpdate<Sequence>(this.seq_name, true);
                    }
                }
                else
                {
                    this.KillSeq();
                }
            }
        }
    }
}

