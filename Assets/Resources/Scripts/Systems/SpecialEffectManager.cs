using UnityEngine;

/// <summary>
/// 特殊效果管理器 - 处理特殊卡牌的效果
/// Special Effect Manager - Handles special card effects
/// </summary>
public class SpecialEffectManager
{
    /// <summary>
    /// 应用卡牌放置时的特殊效果
    /// Apply special effects when card is placed
    /// </summary>
    /// <param name="placedCard">被放置的卡牌</param>
    /// <param name="battleArea">战场区域</param>
    public void ApplyCardPlacementEffects(Card placedCard, CardArea battleArea)
    {
        if (placedCard == null || battleArea == null) return;

        switch (placedCard.m_CardName)
        {
            case "指挥官":
                ApplyCommanderEffect(placedCard, battleArea);
                break;

            case "冲锋":
                ApplyCrossMagicEffect(placedCard, battleArea);
                break;

            case "爆发":
                ApplyColumnMagicEffect(placedCard, battleArea);
                break;

            case "坚守":
                ApplyRowMagicEffect(placedCard, battleArea);
                break;

            case "巨盾":
                ApplyAllMagicEffect(placedCard, battleArea);
                break;
        }
    }

    /// <summary>
    /// 指挥官效果：根据场上友军数量增加攻击力和生命值
    /// Commander effect: Increase ATK and HP based on allies count
    /// </summary>
    private void ApplyCommanderEffect(Card card, CardArea battleArea)
    {
        int allyCount = battleArea.m_AreaList.Count;
        card.m_CurrentATK += allyCount;
        card.m_CurrentHP += allyCount;
        card.ChangeHP(allyCount);
        card.ChangeATK(allyCount);
    }

    /// <summary>
    /// 冲锋效果：同行同列的友军攻击力+2
    /// Cross Magic: Allies in same row or column gain +2 ATK
    /// </summary>
    private void ApplyCrossMagicEffect(Card magicCard, CardArea battleArea)
    {
        foreach (Card allyCard in battleArea.m_AreaList)
        {
            if (allyCard.m_CardType == Card.CardType.Character &&
                (magicCard.m_BattleRow == allyCard.m_BattleRow ||
                 magicCard.m_BattleColumn == allyCard.m_BattleColumn))
            {
                allyCard.m_CurrentATK += 2;
                allyCard.ChangeATK(2);
            }
        }
    }

    /// <summary>
    /// 爆发效果：同列友军攻击力x3，生命值变为1
    /// Column Magic: Allies in same column get ATK x3, HP becomes 1
    /// </summary>
    private void ApplyColumnMagicEffect(Card magicCard, CardArea battleArea)
    {
        foreach (Card allyCard in battleArea.m_AreaList)
        {
            if (allyCard.m_CardType == Card.CardType.Character &&
                magicCard.m_BattleColumn == allyCard.m_BattleColumn)
            {
                allyCard.m_CurrentATK *= 3;
                allyCard.m_CurrentHP = 1;
                allyCard.ChangeHP(1 - allyCard.m_HP);
                allyCard.ChangeATK(allyCard.m_ATK * 2);
            }
        }
    }

    /// <summary>
    /// 坚守效果：同行友军生命值x2
    /// Row Magic: Allies in same row get HP x2
    /// </summary>
    private void ApplyRowMagicEffect(Card magicCard, CardArea battleArea)
    {
        foreach (Card allyCard in battleArea.m_AreaList)
        {
            if (allyCard.m_CardType == Card.CardType.Character &&
                magicCard.m_BattleRow == allyCard.m_BattleRow)
            {
                allyCard.m_CurrentHP *= 2;
                allyCard.ChangeHP(allyCard.m_HP);
            }
        }
    }

    /// <summary>
    /// 巨盾效果：所有友军生命值+5
    /// All Magic: All allies gain +5 HP
    /// </summary>
    private void ApplyAllMagicEffect(Card magicCard, CardArea battleArea)
    {
        foreach (Card allyCard in battleArea.m_AreaList)
        {
            if (allyCard.m_CardType == Card.CardType.Character)
            {
                allyCard.m_CurrentHP += 5;
                allyCard.ChangeHP(5);
            }
        }
    }

    /// <summary>
    /// 检查卡牌是否为魔法卡
    /// Check if card is a magic card
    /// </summary>
    public bool IsMagicCard(Card card)
    {
        return card.m_CardType == Card.CardType.Magic;
    }

    /// <summary>
    /// 检查卡牌是否有特殊效果
    /// Check if card has special effects
    /// </summary>
    public bool HasSpecialEffect(Card card)
    {
        string[] specialCards = { "指挥官", "冲锋", "爆发", "坚守", "巨盾" };
        foreach (string specialName in specialCards)
        {
            if (card.m_CardName == specialName)
                return true;
        }
        return false;
    }
}
