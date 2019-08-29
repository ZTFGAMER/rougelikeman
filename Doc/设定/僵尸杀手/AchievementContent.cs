using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AchievementContent : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text currentValueText;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject progress;
    [SerializeField]
    private GameObject completed;
    [SerializeField]
    private Text rewardText;
    [SerializeField]
    private OverlayParticle claimParticle;
    [NonSerialized]
    public ClaimType claimType;
    private int ID;
    private string description;

    public void ClaimReward()
    {
        SaveData.AchievementsCompleted item = new SaveData.AchievementsCompleted {
            typeID = DataLoader.Instance.achievements[this.ID].type,
            localID = DataLoader.Instance.achievements[this.ID].ID
        };
        DataLoader.playerData.achievementsCompleted.Add(item);
        DataLoader.Instance.RefreshMoney((double) DataLoader.Instance.achievements[this.ID].reward, true);
        this.claimParticle.Play();
        this.button.gameObject.SetActive(false);
        this.progress.gameObject.SetActive(false);
        this.completed.SetActive(true);
        DataLoader.gui.UpdateMenuContent();
        SoundManager.Instance.PlaySound(SoundManager.Instance.claimSound, -1f);
        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
            { 
                "ID",
                this.ID.ToString()
            }
        };
        AnalyticsManager.instance.LogEvent("AchievementClaim", eventParameters);
    }

    public void GetCorrectContentValues()
    {
        int currentLevel = 0;
        switch (DataLoader.Instance.achievements[this.ID].type)
        {
            case 1:
            {
                SaveData.HeroData data = DataLoader.playerData.heroData[DataLoader.Instance.achievements[this.ID].ID];
                currentLevel = !data.isOpened ? 0 : 1;
                break;
            }
            case 2:
                for (int i = 0; i < DataLoader.playerData.heroData.Count; i++)
                {
                    SaveData.HeroData data2 = DataLoader.playerData.heroData[i];
                    currentLevel += data2.pickedUpCount;
                }
                break;

            case 3:
            {
                SaveData.HeroData data3 = DataLoader.playerData.heroData[DataLoader.Instance.achievements[this.ID].ID - 1];
                currentLevel = data3.currentLevel;
                break;
            }
            case 4:
                for (int i = 0; i < DataLoader.playerData.heroData.Count; i++)
                {
                    SaveData.HeroData data4 = DataLoader.playerData.heroData[i];
                    currentLevel += data4.diedCount;
                }
                break;

            case 5:
            {
                SaveData.ZombieData data5 = DataLoader.playerData.zombieData[DataLoader.Instance.achievements[this.ID].ID - 1];
                currentLevel = data5.totalTimesKilled;
                break;
            }
            case 6:
                for (int i = 0; i < DataLoader.playerData.zombieData.Count; i++)
                {
                    SaveData.ZombieData data6 = DataLoader.playerData.zombieData[i];
                    currentLevel += data6.killedByCapsule;
                }
                break;

            case 7:
                currentLevel = (int) DataLoader.playerData.totalDamage;
                break;
        }
        this.rewardText.text = $"{DataLoader.Instance.achievements[this.ID].reward:N0}";
        this.fillImage.fillAmount = ((float) currentLevel) / ((float) DataLoader.Instance.achievements[this.ID].count);
        this.currentValueText.text = currentLevel + "/" + DataLoader.Instance.achievements[this.ID].count;
        if (Enumerable.Any<SaveData.AchievementsCompleted>(DataLoader.playerData.achievementsCompleted, a => (a.typeID == DataLoader.Instance.achievements[this.ID].type) && (a.localID == DataLoader.Instance.achievements[this.ID].ID)))
        {
            this.button.gameObject.SetActive(false);
            this.progress.gameObject.SetActive(false);
            this.completed.SetActive(true);
            this.rewardText.gameObject.SetActive(false);
            this.claimType = ClaimType.CLAIMED;
        }
        else if (this.fillImage.fillAmount >= 1f)
        {
            this.claimType = ClaimType.READY;
            this.progress.gameObject.SetActive(false);
            this.button.gameObject.SetActive(true);
            DataLoader.gui.achievementsPanel.newAchievementsCount++;
        }
        else
        {
            this.claimType = ClaimType.NOTREADY;
        }
    }

    public float GetProgressPercentage() => 
        this.fillImage.fillAmount;

    public void MarkComleted()
    {
        SaveData.AchievementsCompleted item = new SaveData.AchievementsCompleted {
            typeID = DataLoader.Instance.achievements[this.ID].type,
            localID = DataLoader.Instance.achievements[this.ID].ID
        };
        DataLoader.playerData.achievementsCompleted.Add(item);
        this.button.gameObject.SetActive(false);
        this.progress.gameObject.SetActive(false);
        this.completed.SetActive(true);
        this.UpdateContent();
    }

    public void SetContent(int ID)
    {
        this.ID = ID;
        this.SetLocalizedText();
        this.icon.set_sprite(DataLoader.Instance.achievements[ID].icon);
        this.GetCorrectContentValues();
        this.SetLocalizedText();
    }

    public void SetLocalizedText()
    {
        if (this.descriptionText != null)
        {
            this.descriptionText.text = LanguageManager.instance.GetLocalizedText(DataLoader.Instance.achievements[this.ID].description);
            this.descriptionText.set_font(LanguageManager.instance.currentLanguage.font);
        }
        if (this.nameText != null)
        {
            this.nameText.text = LanguageManager.instance.GetLocalizedText(DataLoader.Instance.achievements[this.ID].name);
            this.nameText.set_font(LanguageManager.instance.currentLanguage.font);
        }
    }

    public void UpdateContent()
    {
        this.GetCorrectContentValues();
    }

    public void UpdateLocalizaton()
    {
        this.SetLocalizedText();
    }

    public enum ClaimType
    {
        NOTREADY,
        READY,
        CLAIMED
    }
}

