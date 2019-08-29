using PureMVC.Patterns;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSkillOneCtrl : MonoBehaviour
{
    private int skillid;
    private Text text;
    private Image image;
    private int index;
    private RectTransform rectt;
    private float allspeed;
    private int endindex;
    private float endposy;
    private bool bStart;
    private Action mRandomName;
    private int mColumn;
    private int mCount;
    private const float time = 0.1f;
    private float movetime;
    private bool bLast;
    private bool bRevert;
    private int mRevertState;
    private float mEndPosY;

    public void AddAction(int column, int count, Action randomname)
    {
        this.mRandomName = randomname;
        this.mColumn = column;
        this.mCount = count;
        this.bStart = true;
        this.bLast = false;
        this.bRevert = false;
        this.mRevertState = 0;
        this.movetime = 0f;
        this.mEndPosY = 0f;
        this.endindex = (this.mCount + this.index) % 3;
        this.endposy = (1 - this.endindex) * 180;
    }

    private void Awake()
    {
        this.index = int.Parse(base.name);
        this.image = base.transform.GetComponent<Image>();
        this.rectt = base.transform as RectTransform;
    }

    public void Init(int skillid, Text text)
    {
        this.allspeed = 0f;
        this.skillid = skillid;
        int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(skillid).SkillIcon;
        this.image.set_sprite(SpriteManager.GetSkillIcon(skillIcon));
        this.text = text;
        this.Modify();
    }

    private void Modify()
    {
        float y = (1 - this.index) * 180;
        this.rectt.anchoredPosition = new Vector2(this.rectt.anchoredPosition.x, y);
    }

    private void ModifyPositionY()
    {
        while (this.rectt.anchoredPosition.y <= -180f)
        {
            this.rectt.anchoredPosition = new Vector2(this.rectt.anchoredPosition.x, this.rectt.anchoredPosition.y + 540f);
        }
    }

    public void OnClick()
    {
        if (this.endindex == 1)
        {
            GameLogic.Self.LearnSkill(this.skillid);
            LocalSave.Instance.BattleIn_UpdateLearnSkill(this.skillid);
            Facade.Instance.SendNotification("BATTLE_CHOOSESKILL_SKILL_CHOOSE", this.skillid);
        }
    }

    private void Update()
    {
        this.ModifyPositionY();
        float num = (Time.unscaledDeltaTime / 0.1f) * 180f;
        if (this.bStart)
        {
            float y = this.rectt.anchoredPosition.y;
            float num3 = this.rectt.anchoredPosition.y - num;
            this.rectt.anchoredPosition = new Vector2(this.rectt.anchoredPosition.x, num3);
            this.ModifyPositionY();
            if ((y * num3) <= 0f)
            {
                GameLogic.Hold.Sound.PlayUI(0xf4245);
            }
            this.allspeed += num;
            while (this.allspeed >= 180f)
            {
                this.allspeed -= 180f;
                this.mCount--;
            }
            if ((Time.frameCount % 3) == 0)
            {
                this.mRandomName();
            }
            if (this.mCount <= 0)
            {
                this.bStart = false;
                this.bLast = true;
            }
        }
        else if (this.bLast)
        {
            if (this.endindex == 1)
            {
                this.bLast = false;
                this.bRevert = true;
                this.text.text = GameLogic.Hold.Language.GetSkillName(this.skillid);
                GameLogic.Hold.Sound.PlayUI(0xf4246);
                if (this.mColumn == 2)
                {
                    Facade.Instance.SendNotification("BATTLE_CHOOSESKILL_ACTION_END");
                }
            }
            else
            {
                this.bLast = false;
                this.rectt.anchoredPosition = new Vector2(this.rectt.anchoredPosition.x, this.endposy);
            }
        }
        else if (this.bRevert)
        {
            if (this.mRevertState == 0)
            {
                this.rectt.anchoredPosition = new Vector2(this.rectt.anchoredPosition.x, this.rectt.anchoredPosition.y - num);
                if (this.rectt.anchoredPosition.y < (this.endposy - 54f))
                {
                    this.mRevertState = 1;
                }
            }
            else if (this.mRevertState == 1)
            {
                num *= 0.5f;
                float y = this.rectt.anchoredPosition.y + num;
                if ((y >= this.endposy) || (num < 0.001f))
                {
                    y = this.endposy;
                    this.bRevert = false;
                    this.mRevertState = 0;
                }
                this.rectt.anchoredPosition = new Vector2(this.rectt.anchoredPosition.x, y);
            }
        }
    }
}

