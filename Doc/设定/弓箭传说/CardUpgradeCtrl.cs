using System;
using UnityEngine;
using UnityEngine.UI;

public class CardUpgradeCtrl : MonoBehaviour
{
    public Text Text_UpgradeCount;
    public CardUILevelLimitCtrl mLevelLimitCtrl;
    public ButtonCtrl Button_Upgrade;
    public GoldTextCtrl mGoldCtrl;
    public Text Text_UpgradeContent;

    public void OnLanguageChange()
    {
        this.Text_UpgradeContent.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Upgrade", Array.Empty<object>());
    }

    public void UpdateUpgrade()
    {
        object[] args = new object[] { LocalSave.Instance.Card_GetRandomCount() };
        this.Text_UpgradeCount.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_UpgradeCount", args);
        if (!LocalSave.Instance.Card_GetAllMax())
        {
            int level = LocalSave.Instance.Card_GetNeedLevel();
            if (LocalSave.Instance.GetLevel() < level)
            {
                this.mLevelLimitCtrl.Show(true);
                this.mLevelLimitCtrl.Init(level);
                this.Button_Upgrade.gameObject.SetActive(false);
            }
            else
            {
                this.mLevelLimitCtrl.Show(false);
                this.Button_Upgrade.gameObject.SetActive(true);
                int num2 = LocalSave.Instance.Card_GetRandomGold();
                this.mGoldCtrl.SetCurrencyType(CurrencyType.Gold);
                this.mGoldCtrl.SetValue(num2);
                this.Button_Upgrade.SetEnable(true);
            }
        }
        else
        {
            this.mLevelLimitCtrl.Show(false);
            this.Button_Upgrade.gameObject.SetActive(false);
        }
    }
}

