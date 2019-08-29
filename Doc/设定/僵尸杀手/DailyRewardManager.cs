using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class DailyRewardManager : MonoBehaviour
{
    private DateTime currentDate;
    [SerializeField]
    private List<DailyRewardContent> dailyContent = new List<DailyRewardContent>();
    [SerializeField]
    private DailyRewardContent dayPrefab;
    [SerializeField]
    private Transform parent;
    private StreakType type;

    private void ActivateCurrentDay()
    {
        int num2;
        int num = 0;
        if (this.type == StreakType.OnStreak)
        {
            num2 = (DataLoader.playerData.totalDaysInRow % 7) + 1;
            num = DataLoader.playerData.totalDaysInRow / 7;
        }
        else
        {
            num2 = ((DataLoader.playerData.totalDaysInRow + 1) % 7) + 1;
            num = (DataLoader.playerData.totalDaysInRow + 1) / 7;
        }
        DataLoader.gui.popUpsPanel.dailyHeaderText.text = (num + 1).ToString();
        DataLoader.Instance.currentDayInRow = (num2 + (7 * (num % 2))) - 1;
        int currentDayInRow = DataLoader.Instance.currentDayInRow;
        currentDayInRow = (currentDayInRow < 7) ? currentDayInRow : (currentDayInRow - 7);
        for (int i = 0; i < this.dailyContent.Count; i++)
        {
            if ((currentDayInRow == 0) && (i == 0))
            {
                if (this.type == StreakType.OnStreak)
                {
                    this.dailyContent[0].SetDay(DailyContentType.Inactive);
                }
                else
                {
                    this.dailyContent[0].SetDay(DailyContentType.Active);
                }
            }
            else
            {
                int num6 = (this.type == StreakType.OnStreak) ? (currentDayInRow + 1) : currentDayInRow;
                if (i < num6)
                {
                    this.dailyContent[i].SetDay(DailyContentType.Inactive);
                }
                else if (i == num6)
                {
                    if (this.type == StreakType.OnStreak)
                    {
                        this.dailyContent[i].SetDay(DailyContentType.Next);
                    }
                    else
                    {
                        this.dailyContent[i].SetDay(DailyContentType.Active);
                    }
                }
                else
                {
                    this.dailyContent[i].SetDay(DailyContentType.Next);
                }
            }
        }
        if (num > 0)
        {
            DataLoader.gui.dailyPresent.set_sprite(DataLoader.gui.multiplyImages.dailyPresent[MultiplyImages.GetDailyPresentSpriteID(DataLoader.Instance.dailyBonus[currentDayInRow + 7].type)]);
        }
        else
        {
            DataLoader.gui.dailyPresent.set_sprite(DataLoader.gui.multiplyImages.dailyPresent[MultiplyImages.GetDailyPresentSpriteID(DataLoader.Instance.dailyBonus[currentDayInRow].type)]);
        }
    }

    public void ActivateDailyReward(bool debug = true)
    {
        this.FillContent();
        if (TimeManager.gotDateTime)
        {
            this.currentDate = TimeManager.CurrentDateTime;
            this.type = this.CanGetReward(debug);
            this.ActivateCurrentDay();
        }
        else
        {
            this.type = StreakType.Unknown;
            DataLoader.gui.OpenDaily(false);
        }
    }

    public StreakType CanGetReward(bool debug = true)
    {
        if (PlayerPrefs.HasKey(StaticConstants.DailyRewardKey))
        {
            DateTime time = new DateTime(Convert.ToInt64(PlayerPrefs.GetString(StaticConstants.DailyRewardKey)), DateTimeKind.Utc);
            int totalDays = (int) this.currentDate.Subtract(time).TotalDays;
            if (totalDays == DataLoader.playerData.totalDaysInRow)
            {
                if (debug)
                {
                    Debug.Log("Can't claim reward today. Total days: " + (DataLoader.playerData.totalDaysInRow + 1));
                }
                DataLoader.gui.OpenDaily(false);
                return StreakType.OnStreak;
            }
            if (totalDays == (DataLoader.playerData.totalDaysInRow + 1))
            {
                if (debug)
                {
                    Debug.Log("You can claim reward. Total days: " + (DataLoader.playerData.totalDaysInRow + 1));
                }
                DataLoader.gui.OpenDaily(true);
                return StreakType.GetToday;
            }
            if (debug)
            {
                Debug.Log("Streak ended. You can claim reward. Total days: " + (DataLoader.playerData.totalDaysInRow + 1));
            }
            DataLoader.Instance.SetTotalDays(-1, false);
            DataLoader.gui.OpenDaily(true);
            return StreakType.StreakEnded;
        }
        Debug.Log("First enter. You can claim reward.");
        DataLoader.gui.OpenDaily(true);
        return StreakType.StreakEnded;
    }

    public void ClaimReward()
    {
        if ((this.type == StreakType.GetToday) || (this.type == StreakType.StreakEnded))
        {
            DataLoader.Instance.SetTotalDays(DataLoader.playerData.totalDaysInRow + 1, false);
            if (this.type == StreakType.StreakEnded)
            {
                PlayerPrefs.SetString(StaticConstants.DailyRewardKey, this.currentDate.Date.Ticks.ToString());
            }
            this.dailyContent[(DataLoader.Instance.currentDayInRow < 7) ? DataLoader.Instance.currentDayInRow : (DataLoader.Instance.currentDayInRow - 7)].ActivateReward();
            this.type = StreakType.OnStreak;
            this.ActivateDailyReward(false);
            Debug.Log("Got Daily reward");
        }
    }

    private void FillContent()
    {
        float num = 15f;
        Vector2 sizeDelta = this.dayPrefab.rect.sizeDelta;
        if (this.dailyContent.Count == 0)
        {
            for (int i = 0; i < 7; i++)
            {
                this.dailyContent.Add(UnityEngine.Object.Instantiate<DailyRewardContent>(this.dayPrefab, this.parent));
                this.dailyContent.Last<DailyRewardContent>().rect.anchoredPosition = new Vector2(0f, (-(num + this.dayPrefab.rect.sizeDelta.y) * i) - num);
                this.dailyContent.Last<DailyRewardContent>().SetClaim(() => this.ClaimReward());
            }
        }
        this.UpdateDailyRewardContent();
    }

    public void UpdateDailyRewardContent()
    {
        if (this.type == StreakType.OnStreak)
        {
            for (int i = 0; i < this.dailyContent.Count; i++)
            {
                this.dailyContent[i].SetContent(((DataLoader.playerData.totalDaysInRow + 1) <= 7) ? i : (i + 7));
                this.dailyContent[i].ActivateLigth(true);
            }
        }
        else
        {
            for (int i = 0; i < this.dailyContent.Count; i++)
            {
                this.dailyContent[i].SetContent(((DataLoader.playerData.totalDaysInRow + 1) < 7) ? i : (i + 7));
                this.dailyContent[i].ActivateLigth(false);
            }
        }
        this.ActivateCurrentDay();
    }

    public enum StreakType
    {
        GetToday,
        OnStreak,
        StreakEnded,
        Unknown
    }
}

