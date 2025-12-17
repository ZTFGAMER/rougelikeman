using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配置管理器 - 从JSON加载和管理游戏配置
/// Config Manager - Loads and manages game configuration from JSON
/// </summary>
public class ConfigManager
{
    private static ConfigManager instance;
    private GameConfig gameConfig;
    private DeckConfig deckConfig;
    private bool isGameConfigLoaded = false;
    private bool isDeckConfigLoaded = false;

    public static ConfigManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConfigManager();
            }
            return instance;
        }
    }

    private ConfigManager()
    {
    }

    /// <summary>
    /// 从JSON文件加载游戏配置
    /// Load game config from JSON file
    /// </summary>
    public void LoadGameConfig()
    {
        if (isGameConfigLoaded)
        {
            Debug.Log("ConfigManager: Game config already loaded");
            return;
        }

        try
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Config/gameConfig");
            if (jsonFile == null)
            {
                Debug.LogError("ConfigManager: Failed to load gameConfig.json from Resources/Config/");
                return;
            }

            gameConfig = JsonUtility.FromJson<GameConfig>(jsonFile.text);
            if (gameConfig == null)
            {
                Debug.LogError("ConfigManager: Failed to parse gameConfig.json");
                return;
            }

            isGameConfigLoaded = true;
            Debug.Log("ConfigManager: Game config loaded successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"ConfigManager: Error loading game config: {e.Message}");
        }
    }

    /// <summary>
    /// 从JSON文件加载卡组配置
    /// Load deck config from JSON file
    /// </summary>
    public void LoadDeckConfig()
    {
        if (isDeckConfigLoaded)
        {
            Debug.Log("ConfigManager: Deck config already loaded");
            return;
        }

        try
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Config/decks");
            if (jsonFile == null)
            {
                Debug.LogError("ConfigManager: Failed to load decks.json from Resources/Config/");
                return;
            }

            deckConfig = JsonUtility.FromJson<DeckConfig>(jsonFile.text);
            if (deckConfig == null || deckConfig.decks == null)
            {
                Debug.LogError("ConfigManager: Failed to parse decks.json");
                return;
            }

            isDeckConfigLoaded = true;
            Debug.Log("ConfigManager: Deck config loaded successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"ConfigManager: Error loading deck config: {e.Message}");
        }
    }

    /// <summary>
    /// 获取游戏配置
    /// Get game config
    /// </summary>
    public GameConfig GetGameConfig()
    {
        if (!isGameConfigLoaded)
        {
            Debug.LogWarning("ConfigManager: Game config not loaded yet, loading now...");
            LoadGameConfig();
        }
        return gameConfig;
    }

    /// <summary>
    /// 获取卡组配置
    /// Get deck config by ID
    /// </summary>
    public DeckData GetDeck(string deckId)
    {
        if (!isDeckConfigLoaded)
        {
            Debug.LogWarning("ConfigManager: Deck config not loaded yet, loading now...");
            LoadDeckConfig();
        }

        if (deckConfig?.decks != null)
        {
            switch (deckId)
            {
                case "playerStarterDeck":
                    return deckConfig.decks.playerStarterDeck;
                case "enemyStarterDeck":
                    return deckConfig.decks.enemyStarterDeck;
                default:
                    Debug.LogError($"ConfigManager: Deck with ID '{deckId}' not found");
                    return null;
            }
        }

        return null;
    }

    /// <summary>
    /// 加载所有配置
    /// Load all configurations
    /// </summary>
    public void LoadAllConfigs()
    {
        LoadGameConfig();
        LoadDeckConfig();
        CardDatabase.Instance.LoadCardsFromJson();
    }
}

/// <summary>
/// 游戏配置JSON结构
/// Game config JSON structure
/// </summary>
[Serializable]
public class GameConfig
{
    public GameConstants gameConstants;
    public PlayerConfig playerConfig;
    public PlayerConfig enemyConfig;
    public CardAreaNames cardAreaNames;
}

[Serializable]
public class GameConstants
{
    public int playerInitialHP;
    public int playerInitialEnergy;
    public int enemyInitialHP;
    public int enemyInitialEnergy;
    public int drawCardCount;
    public int battleGridRows;
    public int battleGridColumns;
}

[Serializable]
public class PlayerConfig
{
    public string name;
    public string animationName;
    public int initialHP;
    public int initialEnergy;
}

[Serializable]
public class CardAreaNames
{
    public string playerHandArea;
    public string playerBattleArea;
    public string playerDeckArea;
    public string playerDropArea;
    public string enemyHandArea;
    public string enemyBattleArea;
    public string enemyDeckArea;
    public string enemyDropArea;
}

/// <summary>
/// 卡组配置JSON结构
/// Deck config JSON structure
/// </summary>
[Serializable]
public class DeckConfig
{
    public DecksWrapper decks;
}

[Serializable]
public class DecksWrapper
{
    public DeckData playerStarterDeck;
    public DeckData enemyStarterDeck;
}

[Serializable]
public class DeckData
{
    public string name;
    public string description;
    public List<DeckCardEntry> cards;
    public int totalCards;
}

[Serializable]
public class DeckCardEntry
{
    public string cardId;
    public int count;
}
