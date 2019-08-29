using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class PauseUISkillIconCtrl : MonoBehaviour
{
    private Image image;
    private Text Text_SkillID;

    private void Awake()
    {
        this.image = base.transform.Find("child/Image").GetComponent<Image>();
        this.Text_SkillID = base.transform.Find("child/Text").GetComponent<Text>();
        this.Text_SkillID.gameObject.SetActive(false);
    }

    public void Init(int SkillID)
    {
        int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(SkillID).SkillIcon;
        if (skillIcon == 0)
        {
            object[] args = new object[] { SkillID };
            SdkManager.Bugly_Report("PauseUISkillIconCtrl", Utils.FormatString("Init iconid == 0   skillid:{0}", args));
        }
        this.image.set_sprite(SpriteManager.GetSkillIcon(skillIcon));
    }
}

