using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌系统 - 管理卡牌的抽取、弃置、洗牌等操作
/// Card System - Manages card drawing, discarding, shuffling operations
/// </summary>
public class CardSystem
{
    private Transform recycleTransform;
    private Dictionary<string, CardArea> cardAreas;

    public CardSystem(Transform recycleTransform)
    {
        this.recycleTransform = recycleTransform;
        this.cardAreas = new Dictionary<string, CardArea>();
    }

    /// <summary>
    /// 注册卡牌区域
    /// Register card area
    /// </summary>
    public void RegisterCardArea(string name, CardArea area)
    {
        if (!cardAreas.ContainsKey(name))
        {
            cardAreas.Add(name, area);
        }
    }

    /// <summary>
    /// 获取卡牌区域
    /// Get card area by name
    /// </summary>
    public CardArea GetCardArea(string name)
    {
        if (cardAreas.ContainsKey(name))
        {
            return cardAreas[name];
        }
        Debug.LogWarning($"CardArea '{name}' not found!");
        return null;
    }

    /// <summary>
    /// 初始化卡牌数据
    /// Initialize card from data
    /// </summary>
    public Card CreateCard(CardData data)
    {
        GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
        Card card = Object.Instantiate(toInstantiate, recycleTransform).GetComponent<Card>();

        card.m_IsEnemy = data.isEnemy;
        card.ChangeHPAndATKLine();
        card.m_CardName = data.cardName;
        card.m_HurtEffect = data.hurtEffect;
        card.ChangeHP(data.hp);
        card.ChangeATK(data.attack);
        card.m_Cost = data.cost;
        card.m_CardType = data.cardType;
        card.InitAnimation(data.animationName);
        card.PrepareForBattle();

        return card;
    }

    /// <summary>
    /// 抽牌逻辑
    /// Draw cards logic
    /// </summary>
    public void DrawCards(string dropAreaName, string deckAreaName, string handAreaName, int count)
    {
        CardArea dropArea = GetCardArea(dropAreaName);
        CardArea deckArea = GetCardArea(deckAreaName);
        CardArea handArea = GetCardArea(handAreaName);

        if (dropArea == null || deckArea == null || handArea == null) return;

        int drawn = 0;
        while (drawn < count)
        {
            // 如果牌库为空，从弃牌堆洗牌
            if (deckArea.m_AreaList.Count == 0)
            {
                Shuffle(dropArea, deckArea);
            }

            if (deckArea.m_AreaList.Count >= 1)
            {
                Card card = deckArea.m_AreaList[0];
                ChangeCardArea(handArea, card);
                handArea.m_AreaList.Add(card);
                deckArea.m_AreaList.Remove(card);
                drawn++;

                GameEvents.CardDrawn(card);
            }
            else
            {
                // 没有更多卡牌可抽
                break;
            }
        }

        UpdateCardAreaCount(deckArea);
        UpdateCardAreaCount(handArea);
    }

    /// <summary>
    /// 弃置手牌
    /// Discard hand cards
    /// </summary>
    public void DiscardCards(string dropAreaName, string handAreaName)
    {
        CardArea dropArea = GetCardArea(dropAreaName);
        CardArea handArea = GetCardArea(handAreaName);

        if (dropArea == null || handArea == null) return;

        for (int i = handArea.m_AreaList.Count - 1; i >= 0; i--)
        {
            Card card = handArea.m_AreaList[i];
            ChangeCardArea(dropArea, card);
            dropArea.m_AreaList.Add(card);
            handArea.m_AreaList.Remove(card);

            GameEvents.CardDiscarded(card);
        }

        UpdateCardAreaCount(dropArea);
        UpdateCardAreaCount(handArea);
    }

    /// <summary>
    /// 洗牌 - 将一个区域的牌随机移动到另一个区域
    /// Shuffle - Randomly move cards from one area to another
    /// </summary>
    public void Shuffle(CardArea fromArea, CardArea toArea)
    {
        int countNum = fromArea.m_AreaList.Count;
        int originalToCount = toArea.m_AreaList.Count;

        while (fromArea.m_AreaList.Count > 0 && toArea.m_AreaList.Count < countNum + originalToCount)
        {
            int index = Random.Range(0, fromArea.m_AreaList.Count);
            Card card = fromArea.m_AreaList[index];

            if (!toArea.m_AreaList.Contains(card))
            {
                ChangeCardArea(toArea, card);
                toArea.m_AreaList.Add(card);
                fromArea.m_AreaList.Remove(card);
            }
        }

        UpdateCardAreaCount(fromArea);
        UpdateCardAreaCount(toArea);
    }

    /// <summary>
    /// 改变卡牌所属区域
    /// Change card parent area
    /// </summary>
    public void ChangeCardArea(CardArea newArea, Card card)
    {
        if (!newArea.m_CardAreaName.Contains("Hand"))
        {
            card.transform.SetParent(recycleTransform);
        }
        else
        {
            card.transform.SetParent(newArea.transform);
        }
    }

    /// <summary>
    /// 更新区域卡牌数量显示
    /// Update card area count display
    /// </summary>
    public void UpdateCardAreaCount(CardArea area)
    {
        if (area.m_TextCount != null)
        {
            area.m_TextCount.text = area.m_AreaList.Count.ToString();
        }
    }

    /// <summary>
    /// 更新所有卡牌区域的计数
    /// Update all card area counts
    /// </summary>
    public void UpdateAllCardAreaCounts()
    {
        foreach (var area in cardAreas.Values)
        {
            UpdateCardAreaCount(area);
        }
    }

    /// <summary>
    /// 获取所有卡牌区域列表
    /// Get all card areas
    /// </summary>
    public List<CardArea> GetAllCardAreas()
    {
        return new List<CardArea>(cardAreas.Values);
    }
}
