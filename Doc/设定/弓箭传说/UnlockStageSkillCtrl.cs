using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class UnlockStageSkillCtrl : MonoBehaviour
{
    public GameObject child;
    public GameObject copyitem;
    public Text Text_SkillContent;
    private const int LineCount = 5;
    private const float WidthOne = 145f;
    private const float HeightOne = 145f;
    private LocalUnityObjctPool mPool;

    private void Awake()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<UnlockStageSkillOneCtrl>(this.copyitem);
        this.copyitem.SetActive(false);
    }

    public void DeInit()
    {
        this.mPool.Collect<UnlockStageSkillOneCtrl>();
    }

    public int GetUnlockSkillCount(int stage) => 
        LocalModelManager.Instance.Skill_slotin.GetSkillsByStage(stage).Count;

    public void Init(Sequence seq, int stage)
    {
        <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
            $this = this
        };
        this.mPool.Collect<UnlockStageSkillOneCtrl>();
        storey.list = LocalModelManager.Instance.Skill_slotin.GetSkillsByStage(stage);
        int num = MathDxx.Clamp(storey.list.Count, 0, 5);
        storey.startx = (-(num - 1) * 145f) / 2f;
        float num2 = storey.list.Count * 0.1f;
        int num3 = 0;
        int count = storey.list.Count;
        while (num3 < count)
        {
            <Init>c__AnonStorey1 storey2 = new <Init>c__AnonStorey1 {
                <>f__ref$0 = storey,
                index = num3
            };
            TweenSettingsExtensions.AppendCallback(seq, new TweenCallback(storey2, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(seq, 0.05f);
            num3++;
        }
        this.Text_SkillContent.text = GameLogic.Hold.Language.GetLanguageByTID("UnlockStage_Skill", Array.Empty<object>());
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey0
    {
        internal UnlockStageSkillOneCtrl ctrl;
        internal float startx;
        internal List<int> list;
        internal UnlockStageSkillCtrl $this;
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey1
    {
        internal int index;
        internal UnlockStageSkillCtrl.<Init>c__AnonStorey0 <>f__ref$0;

        internal void <>m__0()
        {
            this.<>f__ref$0.ctrl = this.<>f__ref$0.$this.mPool.DeQueue<UnlockStageSkillOneCtrl>();
            this.<>f__ref$0.ctrl.transform.SetParentNormal(this.<>f__ref$0.$this.child);
            Vector3 vector = new Vector3(this.<>f__ref$0.startx + ((this.index % 5) * 0x91), (float) (-(this.index / 5) * 0x91), 0f);
            this.<>f__ref$0.ctrl.transform.localPosition = vector - new Vector3(0f, 50f, 0f);
            this.<>f__ref$0.ctrl.Init(this.<>f__ref$0.list[this.index]);
            Sequence sequence = DOTween.Sequence();
            this.<>f__ref$0.ctrl.mCanvasGroup.alpha = 0f;
            TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.<>f__ref$0.ctrl.mCanvasGroup, 1f, 0.2f));
            TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOLocalMoveY(this.<>f__ref$0.ctrl.transform, vector.y, 0.2f, false));
        }
    }
}

