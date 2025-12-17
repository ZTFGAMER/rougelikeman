using UnityEngine;

/// <summary>
/// 卡牌数据定义 - 存储卡牌的基础属性
/// Card Data Definition - Stores basic card properties
/// </summary>
[System.Serializable]
public class CardData
{
    public string cardName;
    public string animationName;
    public int cost;
    public int hp;
    public int attack;
    public Card.CardType cardType;
    public Card.HurtEffect hurtEffect;
    public bool isEnemy;

    public CardData(string name, string animName, int cost, int hp, int attack,
                    Card.CardType type = Card.CardType.Character,
                    Card.HurtEffect effect = Card.HurtEffect.Normal,
                    bool isEnemy = false)
    {
        this.cardName = name;
        this.animationName = animName;
        this.cost = cost;
        this.hp = hp;
        this.attack = attack;
        this.cardType = type;
        this.hurtEffect = effect;
        this.isEnemy = isEnemy;
    }

    /// <summary>
    /// 创建卡牌数据的副本
    /// Create a copy of card data
    /// </summary>
    public CardData Clone()
    {
        return new CardData(cardName, animationName, cost, hp, attack, cardType, hurtEffect, isEnemy);
    }
}
