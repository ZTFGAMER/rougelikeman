using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class StageListOneCtrl : MonoBehaviour
{
    public GameObject commonparent;
    public GameObject commingparent;
    public Text Text_CommingSoon;
    public Text Text_Content;
    public Text Text_Stage;
    public Text Text_Info;
    public Text Text_Level;
    public GameObject stageparent;
    public StageListSkillsCtrl mSkillsCtrl;
    private GameObject stageitem;
    private int stageId;
    private bool bCommingSoon;

    public void Init(int stage, bool unlock)
    {
        this.mSkillsCtrl.gameObject.SetActive(false);
        int num = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter_Hero();
        this.Text_Content.text = string.Empty;
        this.bCommingSoon = stage > num;
        this.commonparent.SetActive(!this.bCommingSoon);
        this.commingparent.SetActive(this.bCommingSoon);
        if (!this.bCommingSoon)
        {
            this.stageId = stage;
            object[] args = new object[] { stage };
            this.Text_Stage.text = GameLogic.Hold.Language.GetLanguageByTID("ChapterIndex_x", args);
            object[] objArray2 = new object[] { stage };
            this.Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("ChapterInfo_{0}", objArray2), Array.Empty<object>());
            int currentMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(stage);
            string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("stagelist_stagelength", Array.Empty<object>());
            object[] objArray3 = new object[] { languageByTID, currentMaxLevel };
            this.Text_Level.text = Utils.FormatString("{0} : {1}", objArray3);
            List<int> skillsByStage = LocalModelManager.Instance.Skill_slotin.GetSkillsByStage(stage);
            this.InitStage();
        }
        else
        {
            this.Text_CommingSoon.set_color(LocalModelManager.Instance.Stage_Level_stagechapter.GetChapterColor(stage));
            if (num == 6)
            {
                this.Text_CommingSoon.text = GameLogic.Hold.Language.GetLanguageByTID("stagelist_hero", Array.Empty<object>());
            }
            else
            {
                this.Text_CommingSoon.text = GameLogic.Hold.Language.GetLanguageByTID("Main_CommingSoon", Array.Empty<object>());
            }
        }
    }

    private void InitStage()
    {
        if (this.stageitem != null)
        {
            Object.Destroy(this.stageitem);
        }
        object[] args = new object[] { this.stageId };
        this.stageitem = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/MainUI/Stage/Stage{0:D2}", args)));
        this.stageitem.SetParentNormal(this.stageparent);
        Image[] componentsInChildren = this.stageitem.GetComponentsInChildren<Image>();
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            componentsInChildren[index].set_material((Material) null);
            index++;
        }
    }
}

