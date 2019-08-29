using DG.Tweening;
using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class BattleMatchDefenceTime_InfoCtrl : MonoBehaviour
{
    private const string aniname = "Card_InfoShow";
    public GameObject child;
    public Animation ani;
    public Text Text_Content;
    public Image Image_Icon;
    private Sequence seq;

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    private void show(bool value)
    {
        if (value)
        {
            this.ani["Card_InfoShow"].time = 0f;
            this.ani["Card_InfoShow"].speed = 1f;
        }
        else
        {
            this.ani["Card_InfoShow"].time = this.ani["Card_InfoShow"].clip.length;
            this.ani["Card_InfoShow"].speed = -1f;
        }
        this.ani.Play("Card_InfoShow");
    }

    public void ShowInfo(string eventname, object body = null)
    {
        this.show(true);
        if (eventname != null)
        {
            if (eventname != "MatchDefenceTime_other_learn_skill")
            {
                if (eventname == "MatchDefenceTime_other_dead")
                {
                    this.Image_Icon.enabled = false;
                    this.Text_Content.get_rectTransform().anchoredPosition = new Vector2(0f, 0f);
                    this.Text_Content.text = Utils.FormatString("死了", Array.Empty<object>());
                }
                else if (eventname == "MatchDefenceTime_other_reborn")
                {
                    this.Image_Icon.enabled = false;
                    this.Text_Content.get_rectTransform().anchoredPosition = new Vector2(0f, 0f);
                    this.Text_Content.text = Utils.FormatString("复活了", Array.Empty<object>());
                }
            }
            else
            {
                this.Image_Icon.enabled = true;
                this.Text_Content.text = Utils.FormatString("学习了", Array.Empty<object>());
                int skillid = (int) body;
                this.Image_Icon.set_sprite(SpriteManager.GetSkillIconByID(skillid));
                float num2 = 0f;
                float num3 = (this.Text_Content.preferredWidth + this.Image_Icon.get_rectTransform().sizeDelta.x) + num2;
                float x = (-num3 / 2f) + (this.Text_Content.preferredWidth / 2f);
                float num5 = ((x + (this.Text_Content.preferredWidth / 2f)) + (this.Image_Icon.get_rectTransform().sizeDelta.x / 2f)) + num2;
                this.Text_Content.get_rectTransform().anchoredPosition = new Vector2(x, 0f);
                this.Image_Icon.get_rectTransform().anchoredPosition = new Vector2(num5, 0f);
            }
        }
        this.KillSequence();
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, 3f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<ShowInfo>m__0));
    }
}

