using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class UnlockStageSkillOneCtrl : MonoBehaviour
{
    public Image Image_Icon;
    public CanvasGroup mCanvasGroup;

    public void Init(int skillid)
    {
        int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(skillid).SkillIcon;
        this.Image_Icon.set_sprite(SpriteManager.GetSkillIcon(skillIcon));
    }
}

