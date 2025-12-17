using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌数据库 - 从JSON加载和管理所有卡牌数据
/// Card Database - Loads and manages all card data from JSON
/// </summary>
public class CardDatabase
{
    private static CardDatabase instance;
    private Dictionary<string, CardConfigData> cardConfigs;
    private bool isLoaded = false;

    public static CardDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CardDatabase();
            }
            return instance;
        }
    }

    private CardDatabase()
    {
        cardConfigs = new Dictionary<string, CardConfigData>();
    }

    /// <summary>
    /// 从JSON文件加载所有卡牌数据
    /// Load all card data from JSON file
    /// </summary>
    public void LoadCardsFromJson()
    {
        if (isLoaded)
        {
            Debug.Log("CardDatabase: Cards already loaded");
            return;
        }

        try
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Config/cards");
            if (jsonFile == null)
            {
                Debug.LogError("CardDatabase: Failed to load cards.json from Resources/Config/");
                return;
            }

            CardDatabaseJson cardDatabase = JsonUtility.FromJson<CardDatabaseJson>(jsonFile.text);
            if (cardDatabase == null || cardDatabase.cards == null)
            {
                Debug.LogError("CardDatabase: Failed to parse cards.json");
                return;
            }

            cardConfigs.Clear();
            foreach (CardConfigData cardConfig in cardDatabase.cards)
            {
                if (!string.IsNullOrEmpty(cardConfig.id))
                {
                    cardConfigs[cardConfig.id] = cardConfig;
                }
            }

            isLoaded = true;
            Debug.Log($"CardDatabase: Loaded {cardConfigs.Count} cards from JSON");
        }
        catch (Exception e)
        {
            Debug.LogError($"CardDatabase: Error loading cards: {e.Message}");
        }
    }

    /// <summary>
    /// 根据ID获取卡牌配置
    /// Get card config by ID
    /// </summary>
    public CardConfigData GetCardConfig(string cardId)
    {
        if (!isLoaded)
        {
            Debug.LogWarning("CardDatabase: Cards not loaded yet, loading now...");
            LoadCardsFromJson();
        }

        if (cardConfigs.ContainsKey(cardId))
        {
            return cardConfigs[cardId];
        }

        Debug.LogError($"CardDatabase: Card with ID '{cardId}' not found");
        return null;
    }

    /// <summary>
    /// 根据ID创建CardData实例
    /// Create CardData instance by ID
    /// </summary>
    public CardData CreateCardData(string cardId)
    {
        CardConfigData config = GetCardConfig(cardId);
        if (config == null)
        {
            return null;
        }

        Card.CardType cardType = ParseCardType(config.cardType);
        Card.HurtEffect hurtEffect = ParseHurtEffect(config.hurtEffect);
        bool isEnemy = config.faction == "enemy";

        return new CardData(
            config.name,
            config.animationName,
            config.cost,
            config.hp,
            config.attack,
            cardType,
            hurtEffect,
            isEnemy
        );
    }

    /// <summary>
    /// 解析卡牌类型字符串为枚举
    /// Parse card type string to enum
    /// </summary>
    private Card.CardType ParseCardType(string typeString)
    {
        try
        {
            return (Card.CardType)Enum.Parse(typeof(Card.CardType), typeString, true);
        }
        catch
        {
            Debug.LogWarning($"CardDatabase: Unknown card type '{typeString}', using Character as default");
            return Card.CardType.Character;
        }
    }

    /// <summary>
    /// 解析伤害效果字符串为枚举
    /// Parse hurt effect string to enum
    /// </summary>
    private Card.HurtEffect ParseHurtEffect(string effectString)
    {
        try
        {
            return (Card.HurtEffect)Enum.Parse(typeof(Card.HurtEffect), effectString, true);
        }
        catch
        {
            Debug.LogWarning($"CardDatabase: Unknown hurt effect '{effectString}', using Normal as default");
            return Card.HurtEffect.Normal;
        }
    }

    /// <summary>
    /// 获取所有卡牌ID列表
    /// Get list of all card IDs
    /// </summary>
    public List<string> GetAllCardIds()
    {
        if (!isLoaded)
        {
            LoadCardsFromJson();
        }
        return new List<string>(cardConfigs.Keys);
    }
}

/// <summary>
/// JSON反序列化类 - 卡牌数据库
/// JSON deserialization class - Card database
/// </summary>
[Serializable]
public class CardDatabaseJson
{
    public List<CardConfigData> cards;
}

/// <summary>
/// JSON反序列化类 - 单个卡牌配置
/// JSON deserialization class - Single card config
/// </summary>
[Serializable]
public class CardConfigData
{
    public string id;
    public string name;
    public string animationName;
    public int cost;
    public int hp;
    public int attack;
    public string cardType;
    public string hurtEffect;
    public string faction;
    public string description;
}
