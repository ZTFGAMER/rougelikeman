using DG.Tweening;
using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class GameTurnTableOneCtrl : MonoBehaviour
{
    public Transform child;
    public Image Image_Icon;
    public Text Text_Value;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TurnTableData <mData>k__BackingField;

    private void Awake()
    {
    }

    public void Init(TurnTableData data)
    {
        this.mData = data;
        this.child.localScale = Vector3.one;
        this.Text_Value.text = string.Empty;
        if (LocalSave.QualityColors.ContainsKey(data.quality))
        {
            this.Text_Value.set_color(LocalSave.QualityColors[data.quality]);
        }
        switch (data.type)
        {
            case TurnTableType.ExpBig:
                this.Image_Icon.set_sprite(SpriteManager.GetBattle("GameTurn_ExpBig"));
                break;

            case TurnTableType.ExpSmall:
                this.Image_Icon.set_sprite(SpriteManager.GetBattle("GameTurn_ExpSmall"));
                break;

            case TurnTableType.PlayerSkill:
            {
                int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById((int) data.value).SkillIcon;
                this.Image_Icon.set_sprite(SpriteManager.GetSkillIcon(skillIcon));
                break;
            }
            case TurnTableType.EventSkill:
            {
                Room_eventgameturn beanById = LocalModelManager.Instance.Room_eventgameturn.GetBeanById((int) data.value);
                int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(beanById.GetID).SkillIcon;
                this.Image_Icon.set_sprite(SpriteManager.GetSkillIcon(skillIcon));
                break;
            }
            case TurnTableType.HPAdd:
                this.Image_Icon.set_sprite(SpriteManager.GetSkillIcon((int) data.value));
                break;

            case TurnTableType.Empty:
                this.Image_Icon.set_sprite(SpriteManager.GetBattle("GameTurn_Empty"));
                break;

            case TurnTableType.Gold:
            {
                this.Image_Icon.set_sprite(SpriteManager.GetUICommon("Currency_Gold"));
                object[] args = new object[] { data.value };
                this.Text_Value.text = Utils.FormatString("x{0}", args);
                break;
            }
            case TurnTableType.Diamond:
            {
                this.Image_Icon.set_sprite(SpriteManager.GetUICommon("Currency_Diamond"));
                object[] args = new object[] { data.value };
                this.Text_Value.text = Utils.FormatString("x{0}", args);
                break;
            }
            case TurnTableType.Get:
            {
                Sequence sequence = DOTween.Sequence();
                TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
                TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1.3f, 0.18f));
                TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1f, 0.18f));
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<Init>m__0));
                break;
            }
            case TurnTableType.Reward_Gold2:
            case TurnTableType.Reward_Gold3:
            case TurnTableType.Reward_Item2:
            case TurnTableType.Reward_Item3:
            case TurnTableType.Reward_All2:
            case TurnTableType.Reward_All3:
                this.Image_Icon.set_sprite(SpriteManager.GetBattle(data.type.ToString()));
                break;
        }
    }

    public TurnTableData mData { get; private set; }
}

