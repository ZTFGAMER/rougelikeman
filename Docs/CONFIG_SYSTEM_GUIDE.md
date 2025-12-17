# 配置系统指南 / Configuration System Guide

## 概述 / Overview

本项目已将所有初始化数据从硬编码转换为基于JSON的配置文件系统。这使得游戏数据的修改和维护变得更加简单，无需修改代码即可调整游戏平衡性。

All initialization data has been migrated from hard-coded values to a JSON-based configuration system. This makes game data modification and maintenance much simpler, allowing balance adjustments without code changes.

## 配置文件结构 / Configuration Files Structure

```
Assets/Resources/Config/
├── cards.json          # 所有卡牌数据 / All card data
├── gameConfig.json     # 游戏配置和常量 / Game config and constants
└── decks.json          # 卡组配置 / Deck configurations
```

---

## 1. cards.json - 卡牌数据库

包含游戏中所有21张卡牌的完整属性数据。

Contains complete attribute data for all 21 cards in the game.

### 结构 / Structure

```json
{
  "cards": [
    {
      "id": "soldier",                    // 唯一ID / Unique ID
      "name": "士兵",                     // 显示名称 / Display name
      "animationName": "近卫兵",           // 动画名称 / Animation name
      "cost": 1,                          // 能量消耗 / Energy cost
      "hp": 3,                            // 生命值 / Health points
      "attack": 3,                        // 攻击力 / Attack power
      "cardType": "Character",            // 卡牌类型 / Card type
      "hurtEffect": "Normal",             // 伤害效果 / Hurt effect
      "faction": "player",                // 阵营 / Faction
      "description": "基础步兵单位"        // 描述 / Description
    }
  ]
}
```

### 卡牌类型 / Card Types

- **Character**: 角色卡，显示ATK/HP
- **Magic**: 魔法卡，不显示ATK/HP
- **Trap**: 陷阱卡（未实现）

### 伤害效果 / Hurt Effects

- **Normal** (1): 常规伤害，从前往后逐个攻击
- **Backstab** (2): 背刺伤害，从后往前攻击
- **Penetrate** (8): 贯穿伤害，攻击该列所有单位
- **Puncture** (4): 穿刺伤害，无视护盾（未实现）

### 阵营 / Factions

- **player**: 玩家卡牌
- **enemy**: 敌人卡牌

### 当前卡牌列表 / Current Card List

**玩家卡牌 (9种):**
- soldier (士兵) x3
- archer (弓箭手) x3
- shield_guard (盾手) x3
- striker (冲击手) x1
- commander (指挥官) x1
- charge (冲锋) x1
- burst (爆发) x1
- defend (坚守) x1
- giant_shield (巨盾) x1

**敌人卡牌 (3种):**
- barbarian_warrior (蛮族勇士) x3
- barbarian_assassin (蛮族刺客) x2
- barbarian_shaman (蛮族巫师) x1

---

## 2. gameConfig.json - 游戏配置

包含游戏常量、玩家/敌人配置、卡牌区域名称等。

Contains game constants, player/enemy config, card area names, etc.

### 结构 / Structure

```json
{
  "gameConstants": {
    "playerInitialHP": 35,              // 玩家初始血量
    "playerInitialEnergy": 3,           // 玩家初始能量
    "enemyInitialHP": 30,               // 敌人初始血量
    "enemyInitialEnergy": 3,            // 敌人初始能量
    "drawCardCount": 5,                 // 每回合抽牌数
    "battleGridRows": 3,                // 战场行数
    "battleGridColumns": 3              // 战场列数
  },
  "playerConfig": {
    "name": "圣骑士",                   // 玩家名称
    "animationName": "曹操(骑马)",       // 动画名称
    "initialHP": 35,                    // 初始生命值
    "initialEnergy": 3                  // 初始能量
  },
  "enemyConfig": {
    "name": "野蛮人",                   // 敌人名称
    "animationName": "陆逊",             // 动画名称
    "initialHP": 30,                    // 初始生命值
    "initialEnergy": 3                  // 初始能量
  },
  "cardAreaNames": {
    "playerHandArea": "PlayerHandArea",
    "playerBattleArea": "PlayerBattleArea",
    "playerDeckArea": "PlayerDeckArea",
    "playerDropArea": "PlayerDropArea",
    "enemyHandArea": "EnemyHandArea",
    "enemyBattleArea": "EnemyBattleArea",
    "enemyDeckArea": "EnemyDeckArea",
    "enemyDropArea": "EnemyDropArea"
  }
}
```

---

## 3. decks.json - 卡组配置

定义玩家和敌人的初始卡组组成。

Defines the initial deck composition for player and enemy.

### 结构 / Structure

```json
{
  "decks": {
    "playerStarterDeck": {
      "name": "圣骑士起始卡组",
      "description": "玩家初始卡组",
      "cards": [
        {
          "cardId": "soldier",          // 卡牌ID
          "count": 3                    // 数量
        }
      ],
      "totalCards": 15                  // 总卡牌数
    },
    "enemyStarterDeck": {
      "name": "野蛮人卡组",
      "description": "敌人初始卡组",
      "cards": [
        {
          "cardId": "barbarian_warrior",
          "count": 3
        }
      ],
      "totalCards": 6
    }
  }
}
```

---

## 代码架构 / Code Architecture

### 新增类 / New Classes

#### 1. CardDatabase.cs
- **位置**: `Assets/Resources/Scripts/Data/CardDatabase.cs`
- **功能**:
  - 从 cards.json 加载所有卡牌数据
  - 提供通过ID查询卡牌配置的接口
  - 创建 CardData 实例
- **使用方式**:
```csharp
// 加载卡牌数据库
CardDatabase.Instance.LoadCardsFromJson();

// 获取卡牌配置
CardConfigData config = CardDatabase.Instance.GetCardConfig("soldier");

// 创建CardData实例
CardData cardData = CardDatabase.Instance.CreateCardData("soldier");
```

#### 2. ConfigManager.cs
- **位置**: `Assets/Resources/Scripts/Data/ConfigManager.cs`
- **功能**:
  - 从 gameConfig.json 加载游戏配置
  - 从 decks.json 加载卡组配置
  - 提供统一的配置访问接口
- **使用方式**:
```csharp
// 加载所有配置
ConfigManager.Instance.LoadAllConfigs();

// 获取游戏配置
GameConfig config = ConfigManager.Instance.GetGameConfig();

// 获取卡组数据
DeckData deck = ConfigManager.Instance.GetDeck("playerStarterDeck");
```

### 修改的类 / Modified Classes

#### 1. BattleManager.cs
- **修改内容**:
  - Start()中添加配置加载
  - InitBattleData()使用配置文件初始化玩家/敌人
  - InitializePlayerDeck()从配置文件加载玩家卡组
  - InitializeEnemyDeck()从配置文件加载敌人卡组

**关键代码**:
```csharp
void Start()
{
    // 加载所有配置文件
    ConfigManager.Instance.LoadAllConfigs();

    InitializeSystems();
    InitBattleData();
    InitBattleGround();
}

void InitializePlayerDeck()
{
    CardArea dropArea = cardSystem.GetCardArea(PLAYER_DROP_AREA);
    DeckData deckData = ConfigManager.Instance.GetDeck("playerStarterDeck");

    foreach (DeckCardEntry entry in deckData.cards)
    {
        CardData cardData = CardDatabase.Instance.CreateCardData(entry.cardId);
        for (int i = 0; i < entry.count; i++)
        {
            dropArea.InitCardFromData(cardData);
        }
    }
}
```

#### 2. CardArea.cs
- **新增方法**: `InitCardFromData(CardData cardData)`
- **功能**: 使用CardData实例初始化卡牌

```csharp
public void InitCardFromData(CardData cardData)
{
    GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
    Card card = Instantiate(toInstantiate, battlemanager.transform.Find("Recycle")).GetComponent<Card>();
    card.battleManager = battlemanager;
    card.m_IsEnemy = cardData.isEnemy;
    card.ChangeHPAndATKLine();
    card.m_CardName = cardData.cardName;
    card.m_HurtEffect = cardData.hurtEffect;
    card.ChangeHP(cardData.hp);
    card.ChangeATK(cardData.attack);
    card.m_Cost = cardData.cost;
    card.m_CardType = cardData.cardType;
    card.InitAnimation(cardData.animationName);
    card.PrepareForBattle();
    this.m_AreaList.Add(card);
}
```

#### 3. Constant.cs
- **修改内容**: 添加注释说明常量已迁移到配置文件
- **状态**: 保留作为默认值/回退值

---

## 使用指南 / Usage Guide

### 添加新卡牌 / Adding New Cards

1. 在 `cards.json` 中添加新卡牌数据：
```json
{
  "id": "new_card",
  "name": "新卡牌",
  "animationName": "动画名称",
  "cost": 2,
  "hp": 4,
  "attack": 4,
  "cardType": "Character",
  "hurtEffect": "Normal",
  "faction": "player",
  "description": "描述文本"
}
```

2. 在 `decks.json` 中将卡牌添加到卡组：
```json
{
  "cardId": "new_card",
  "count": 2
}
```

3. 无需修改任何代码！

### 修改游戏平衡 / Modifying Game Balance

**修改玩家/敌人初始属性**:
- 编辑 `gameConfig.json` 中的 `playerConfig` 或 `enemyConfig`

**修改卡牌属性**:
- 编辑 `cards.json` 中对应卡牌的数值

**修改卡组组成**:
- 编辑 `decks.json` 中的卡牌数量

### 创建新卡组 / Creating New Decks

1. 在 `decks.json` 中添加新卡组：
```json
"newDeck": {
  "name": "新卡组",
  "description": "描述",
  "cards": [
    {"cardId": "soldier", "count": 5}
  ],
  "totalCards": 5
}
```

2. 在 `ConfigManager.cs` 的 `GetDeck()` 方法中添加对应的case：
```csharp
case "newDeck":
    return deckConfig.decks.newDeck;
```

---

## 调试和验证 / Debugging and Validation

### 日志输出 / Log Output

系统会在控制台输出加载信息：

```
CardDatabase: Loaded 12 cards from JSON
ConfigManager: Game config loaded successfully
ConfigManager: Deck config loaded successfully
BattleManager: Player deck initialized with 15 cards
BattleManager: Enemy deck initialized with 6 cards
```

### 验证JSON格式 / Validating JSON Format

使用命令行验证JSON文件格式：
```bash
python3 -m json.tool cards.json
python3 -m json.tool gameConfig.json
python3 -m json.tool decks.json
```

### 常见问题 / Common Issues

**问题**: 卡牌无法加载
- 检查 cardId 是否与 cards.json 中的 id 字段匹配
- 检查 JSON 格式是否正确

**问题**: 配置未生效
- 确认配置文件位于 `Assets/Resources/Config/` 目录
- 确认 Unity 已重新导入配置文件（查看 .meta 文件）

**问题**: 枚举值解析错误
- 确认 cardType 值为: Character, Magic, 或 Trap
- 确认 hurtEffect 值为: Normal, Backstab, Penetrate, 或 Puncture

---

## 迁移对比 / Migration Comparison

### 迁移前 (Hard-coded) / Before Migration

```csharp
// 硬编码在 BattleManager.cs 中
void InitializePlayerDeck()
{
    CardArea dropArea = cardSystem.GetCardArea(PLAYER_DROP_AREA);

    for (int i = 0; i < 3; i++)
    {
        dropArea.InitCard(false, "士兵", "近卫兵", 1, 3, 3);
    }

    for (int i = 0; i < 3; i++)
    {
        dropArea.InitCard(false, "弓箭手", "黄忠(骑马)", 1, 1, 4,
                         Card.CardType.Character, Card.HurtEffect.Backstab);
    }
    // ... 更多硬编码
}
```

**问题**:
- 修改数值需要编辑代码
- 难以维护和扩展
- 无法热更新
- 策划人员无法独立调整

### 迁移后 (Config-based) / After Migration

```csharp
// 从配置文件加载
void InitializePlayerDeck()
{
    CardArea dropArea = cardSystem.GetCardArea(PLAYER_DROP_AREA);
    DeckData deckData = ConfigManager.Instance.GetDeck("playerStarterDeck");

    foreach (DeckCardEntry entry in deckData.cards)
    {
        CardData cardData = CardDatabase.Instance.CreateCardData(entry.cardId);
        for (int i = 0; i < entry.count; i++)
        {
            dropArea.InitCardFromData(cardData);
        }
    }
}
```

**优势**:
- 修改数值只需编辑JSON文件
- 易于维护和扩展
- 支持热更新（可选）
- 策划人员可独立调整平衡性
- 数据和逻辑分离

---

## 性能考虑 / Performance Considerations

- 配置文件在游戏启动时一次性加载到内存
- 使用字典进行O(1)时间复杂度的卡牌查询
- CardData采用浅拷贝，避免重复解析

---

## 未来扩展 / Future Extensions

### 可能的增强 / Possible Enhancements

1. **关卡配置系统**
   - 创建 levels.json 定义多个关卡
   - 每个关卡有不同的敌人配置

2. **技能/效果配置**
   - 将特殊效果规则移到配置文件
   - 支持动态加载新技能

3. **本地化支持**
   - 支持多语言配置文件
   - 根据系统语言加载对应配置

4. **热更新支持**
   - 从服务器下载最新配置
   - 动态重载配置无需重启游戏

5. **配置验证工具**
   - 编辑器工具验证配置完整性
   - 自动检测配置错误

---

## 文件清单 / File Checklist

### 新增文件 / New Files
- ✅ `Assets/Resources/Config/cards.json`
- ✅ `Assets/Resources/Config/gameConfig.json`
- ✅ `Assets/Resources/Config/decks.json`
- ✅ `Assets/Resources/Scripts/Data/CardDatabase.cs`
- ✅ `Assets/Resources/Scripts/Data/ConfigManager.cs`
- ✅ `CONFIG_SYSTEM_GUIDE.md` (本文件)

### 修改文件 / Modified Files
- ✅ `Assets/Resources/Scripts/BattleManager.cs`
- ✅ `Assets/Resources/Scripts/CardArea.cs`
- ✅ `Assets/Resources/Scripts/Constant.cs`

### 保留文件 / Preserved Files
- ✅ `Assets/Resources/Scripts/Data/CardData.cs` (未修改)
- ✅ `Assets/Resources/Scripts/Card.cs` (未修改)
- ✅ 所有系统类 (未修改)

---

## 总结 / Summary

本次重构成功将所有初始化数据表格化，实现了数据与代码的分离。现在可以通过修改JSON配置文件来调整游戏平衡性，无需修改任何代码。这为后续的游戏开发和维护提供了极大的便利性。

This refactoring successfully tabularized all initialization data, achieving separation between data and code. Game balance can now be adjusted by modifying JSON configuration files without touching any code. This provides great convenience for future game development and maintenance.

---

**创建日期 / Created**: 2025-12-17
**版本 / Version**: 1.0
**作者 / Author**: Claude Code
