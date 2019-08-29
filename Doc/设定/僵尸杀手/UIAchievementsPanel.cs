using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievementsPanel : UIBaseScrollPanel
{
    [HideInInspector]
    public AchievementContent[] achievementsContent;
    [HideInInspector]
    public int newAchievementsCount;
    [SerializeField]
    private GameObject newAchievementsObj;
    [SerializeField]
    private Text newAchievementsCountText;
    [CompilerGenerated]
    private static Func<AchievementContent, float> <>f__am$cache0;

    public override void CreateCells()
    {
        RectTransform[] transformArray = UIController.instance.CreareScrollContent(base.cellPrefab, base.scrollRect, DataLoader.Instance.achievements.Count, 0f, base.distanceBetweenCells, false);
        this.achievementsContent = new AchievementContent[transformArray.Length];
        for (int i = 0; i < transformArray.Length; i++)
        {
            this.achievementsContent[i] = transformArray[i].GetComponent<AchievementContent>();
            this.achievementsContent[i].SetContent(i);
        }
        base.cellPrefab.gameObject.SetActive(false);
    }

    public void MarkAchievementsCompleted()
    {
        DataLoader.Instance.cloudSaveLoaded = false;
        for (int i = 0; i < this.achievementsContent.Length; i++)
        {
            if (this.achievementsContent[i].claimType == AchievementContent.ClaimType.READY)
            {
                this.achievementsContent[i].MarkComleted();
            }
        }
    }

    public void Sort()
    {
        List<RectTransform> list = new List<RectTransform>();
        int num = -1;
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = a => a.GetProgressPercentage();
        }
        AchievementContent[] contentArray = Enumerable.OrderByDescending<AchievementContent, float>(this.achievementsContent, <>f__am$cache0).ToArray<AchievementContent>();
        for (int i = contentArray.Length - 1; i >= 0; i--)
        {
            AchievementContent.ClaimType claimType = contentArray[i].claimType;
            if (claimType != AchievementContent.ClaimType.READY)
            {
                if (claimType == AchievementContent.ClaimType.NOTREADY)
                {
                    goto Label_007F;
                }
                if (claimType == AchievementContent.ClaimType.CLAIMED)
                {
                    goto Label_0095;
                }
            }
            else
            {
                list.Insert(0, contentArray[i].rectTransform);
                num++;
            }
            continue;
        Label_007F:
            list.Insert(num + 1, contentArray[i].rectTransform);
            continue;
        Label_0095:
            list.Add(contentArray[i].rectTransform);
        }
        float y = base.cellPrefab.rect.y;
        for (int j = 0; j < list.Count; j++)
        {
            list[j].anchoredPosition = new Vector2(base.cellPrefab.anchoredPosition.x, y);
            y -= (base.cellPrefab.rect.height * base.cellPrefab.localScale.y) + base.distanceBetweenCells;
        }
    }

    public override void UpdateAllContent()
    {
        this.newAchievementsObj.SetActive(false);
        this.newAchievementsCount = 0;
        for (int i = 0; i < this.achievementsContent.Length; i++)
        {
            this.achievementsContent[i].UpdateContent();
        }
        this.newAchievementsCountText.text = this.newAchievementsCount.ToString();
        this.newAchievementsObj.SetActive(this.newAchievementsCount > 0);
    }

    public void UpdateLocalization()
    {
        if (this.achievementsContent != null)
        {
            for (int i = 0; i < this.achievementsContent.Length; i++)
            {
                this.achievementsContent[i].UpdateLocalizaton();
            }
        }
    }
}

