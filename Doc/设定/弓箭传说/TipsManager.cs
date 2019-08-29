using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TipsManager : CInstance<TipsManager>
{
    private int tipsCount;
    private Queue<TipsData> mCacheList = new Queue<TipsData>();

    public void Cache(GameObject o)
    {
        GameLogic.EffectCache(o);
    }

    public void CanShowNext()
    {
        if (this.tipsCount > 0)
        {
            this.tipsCount--;
            if (this.mCacheList.Count > 0)
            {
                TipsData data = this.mCacheList.Dequeue();
                this.ShowMust(data.value1, data.value2);
            }
        }
    }

    public void Clear()
    {
        GameNode.m_Tips.DestroyChildren();
        this.tipsCount = 0;
        this.mCacheList.Clear();
    }

    private TipsCtrl Get()
    {
        GameObject obj2 = GameLogic.EffectGet("Game/UI/TipsOne");
        obj2.transform.SetParent(GameNode.m_Tips);
        RectTransform transform = obj2.transform as RectTransform;
        transform.localScale = Vector3.one;
        transform.anchoredPosition = new Vector2(0f, 0f);
        return obj2.GetComponent<TipsCtrl>();
    }

    public void Show(string value1, string value2 = "")
    {
        if (this.tipsCount == 0)
        {
            this.ShowMust(value1, value2);
        }
        else
        {
            TipsData item = new TipsData {
                value1 = value1,
                value2 = value2
            };
            this.mCacheList.Enqueue(item);
        }
        this.tipsCount++;
    }

    private void ShowMust(string value1, string value2)
    {
        this.Get().Init(value1, value2);
    }

    public void ShowSkill(int skillId)
    {
        string skillName = GameLogic.Hold.Language.GetSkillName(skillId);
        string skillContent = GameLogic.Hold.Language.GetSkillContent(skillId);
        this.Show(skillName, skillContent);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct TipsData
    {
        public string value1;
        public string value2;
    }
}

