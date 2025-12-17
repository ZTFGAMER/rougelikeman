using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗管理器（重构版）- 作为协调者管理各个系统
/// Battle Manager (Refactored) - Coordinates all systems
/// </summary>
public class BattleManager : MonoBehaviour
{
    // 系统引用 / System References
    private CardSystem cardSystem;
    private BattleCalculator battleCalculator;
    private AIController aiController;
    private TurnManager turnManager;
    private SpecialEffectManager specialEffectManager;

    // 玩家和敌人 / Player and Enemy
    public Player player;
    public Player enemy;

    // 状态标志 / State Flags
    public bool bDrawCard = true;
    public bool bPushCard = false;
    public bool bDropCard = false;
    public bool bShowAttack = false;
    public int iDrawCardCount = 0;
    public Card cSelectCard;
    public Card cSelectedBattleCard;  // 选中的场上单位
    public BattleCube cSelectedBattleCube;  // 选中单位所在的格子
    public Card cCurrentDetailCard;  // 当前显示详情的卡牌（全局唯一）
    public int Level = 1;

    // 卡牌区域名称常量 / Card Area Name Constants
    private const string PLAYER_HAND_AREA = "PlayerHandArea";
    private const string PLAYER_BATTLE_AREA = "PlayerBattleArea";
    private const string PLAYER_DECK_AREA = "PlayerDeckArea";
    private const string PLAYER_DROP_AREA = "PlayerDropArea";
    private const string ENEMY_HAND_AREA = "EnemyHandArea";
    private const string ENEMY_BATTLE_AREA = "EnemyBattleArea";
    private const string ENEMY_DECK_AREA = "EnemyDeckArea";
    private const string ENEMY_DROP_AREA = "EnemyDropArea";

    void Start()
    {
        // 加载所有配置文件
        ConfigManager.Instance.LoadAllConfigs();

        InitializeSystems();
        InitBattleData();
        InitBattleGround();
    }

    /// <summary>
    /// 初始化所有系统
    /// Initialize all systems
    /// </summary>
    void InitializeSystems()
    {
        Transform recycleTransform = this.transform.Find("Recycle") ?? this.transform;

        cardSystem = new CardSystem(recycleTransform);
        battleCalculator = new BattleCalculator();
        turnManager = new TurnManager();
        specialEffectManager = new SpecialEffectManager();
    }

    /// <summary>
    /// 初始化战斗数据
    /// Initialize battle data
    /// </summary>
    void InitBattleData()
    {
        // 初始化卡牌区域
        InitCardAreaList(PLAYER_HAND_AREA);
        InitCardAreaList(ENEMY_HAND_AREA);
        InitCardAreaList(PLAYER_BATTLE_AREA);
        InitCardAreaList(ENEMY_BATTLE_AREA);
        InitCardAreaList(PLAYER_DECK_AREA);
        InitCardAreaList(ENEMY_DECK_AREA);
        InitCardAreaList(PLAYER_DROP_AREA);
        InitCardAreaList(ENEMY_DROP_AREA);

        // 初始化AI控制器
        aiController = new AIController(
            cardSystem.GetCardArea(ENEMY_BATTLE_AREA),
            cardSystem.GetCardArea(ENEMY_DECK_AREA)
        );

        // 从配置文件初始化玩家和敌人
        GameConfig config = ConfigManager.Instance.GetGameConfig();
        InitPlayer(player, config.playerConfig.name, config.playerConfig.initialHP,
                  config.playerConfig.initialEnergy, true, config.playerConfig.animationName);
        InitPlayer(enemy, config.enemyConfig.name, config.enemyConfig.initialHP,
                  config.enemyConfig.initialEnergy, false, config.enemyConfig.animationName);
        player.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunBack);
        enemy.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);

        // 初始化玩家卡组
        InitializePlayerDeck();

        // 初始化敌人卡组
        InitializeEnemyDeck();

        bDrawCard = true;
    }

    /// <summary>
    /// 初始化卡牌区域列表
    /// Initialize card area list
    /// </summary>
    void InitCardAreaList(string name)
    {
        Transform areaTransform = this.transform.Find(name);
        if (areaTransform)
        {
            CardArea cardArea = areaTransform.GetComponent<CardArea>();
            if (!cardArea)
            {
                cardArea = areaTransform.gameObject.AddComponent<CardArea>();
            }

            cardArea.m_CardAreaName = name;
            cardArea.battlemanager = this;
            cardArea.m_AreaList = new List<Card>();

            // 更新计数显示
            if (areaTransform.Find("Text_Count"))
            {
                cardArea.m_TextCount = areaTransform.Find("Text_Count").GetComponent<Text>();
            }

            cardSystem.RegisterCardArea(name, cardArea);
        }
    }

    /// <summary>
    /// 初始化玩家
    /// Initialize player
    /// </summary>
    void InitPlayer(Player p, string name, int hp, int cost, bool isplayer, string animname)
    {
        p.m_PlayerName = name;
        p.m_HP = hp;
        p.m_Cost = cost;
        p.m_BattleCardList = new List<Card>();
        p.m_IsPlayer = isplayer;
        p.battleManager = this;
        p.InitAnimation(animname);
    }

    /// <summary>
    /// 初始化玩家卡组（从配置文件）
    /// Initialize player deck (from config file)
    /// </summary>
    void InitializePlayerDeck()
    {
        CardArea dropArea = cardSystem.GetCardArea(PLAYER_DROP_AREA);
        DeckData deckData = ConfigManager.Instance.GetDeck("playerStarterDeck");

        if (deckData == null)
        {
            Debug.LogError("BattleManager: Failed to load player starter deck");
            return;
        }

        foreach (DeckCardEntry entry in deckData.cards)
        {
            CardData cardData = CardDatabase.Instance.CreateCardData(entry.cardId);
            if (cardData != null)
            {
                for (int i = 0; i < entry.count; i++)
                {
                    dropArea.InitCardFromData(cardData);
                }
            }
            else
            {
                Debug.LogError($"BattleManager: Failed to create card with ID '{entry.cardId}'");
            }
        }

        Debug.Log($"BattleManager: Player deck initialized with {dropArea.m_AreaList.Count} cards");
    }

    /// <summary>
    /// 初始化敌人卡组（从配置文件）
    /// Initialize enemy deck (from config file)
    /// </summary>
    void InitializeEnemyDeck()
    {
        CardArea deckArea = cardSystem.GetCardArea(ENEMY_DECK_AREA);
        DeckData deckData = ConfigManager.Instance.GetDeck("enemyStarterDeck");

        if (deckData == null)
        {
            Debug.LogError("BattleManager: Failed to load enemy starter deck");
            return;
        }

        foreach (DeckCardEntry entry in deckData.cards)
        {
            CardData cardData = CardDatabase.Instance.CreateCardData(entry.cardId);
            if (cardData != null)
            {
                for (int i = 0; i < entry.count; i++)
                {
                    deckArea.InitCardFromData(cardData);
                }
            }
            else
            {
                Debug.LogError($"BattleManager: Failed to create card with ID '{entry.cardId}'");
            }
        }

        Debug.Log($"BattleManager: Enemy deck initialized with {deckArea.m_AreaList.Count} cards");
    }

    /// <summary>
    /// 初始化战场格子
    /// Initialize battle cubes
    /// </summary>
    void InitBattleCube(CardArea area, int row = 1, int column = 1)
    {
        for (int i = 0; i < row * column; i++)
        {
            GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/BattleCube");
            BattleCube cube = Instantiate(toInstantiate, area.transform).GetComponent<BattleCube>();
            cube.m_Row = i / column;
            cube.m_Column = i % column;
            cube.m_IsPlayer = area.m_CardAreaName.Contains("Player");
            cube.battlemanager = this;
        }
    }

    /// <summary>
    /// 初始化战场
    /// Initialize battle ground
    /// </summary>
    void InitBattleGround()
    {
        player.PrepareForBattle();
        enemy.PrepareForBattle();
        InitBattleCube(cardSystem.GetCardArea(PLAYER_BATTLE_AREA), 3, 3);
        InitBattleCube(cardSystem.GetCardArea(ENEMY_BATTLE_AREA), 3, 3);
    }

    /// <summary>
    /// 主循环更新
    /// Main tick update
    /// </summary>
    public void Tick()
    {
        // 抽牌阶段
        if (bDrawCard)
        {
            aiController.ExecuteAITurn(Level);
            cardSystem.DrawCards(PLAYER_DROP_AREA, PLAYER_DECK_AREA, PLAYER_HAND_AREA, player.m_DrawCardCount);
            bDrawCard = false;
            bPushCard = true;
            UpdatePreCalculation();
        }

        // 弃牌并开始战斗
        if (bDropCard)
        {
            cardSystem.DiscardCards(PLAYER_DROP_AREA, PLAYER_HAND_AREA);
            StartCoroutine(UpdateBattle());
            bDropCard = false;
        }

        // 更新所有卡牌区域
        UpdateAllCardAreas();

        // 更新玩家和敌人
        player.Tick();
        enemy.Tick();
    }

    /// <summary>
    /// 更新所有卡牌区域
    /// Update all card areas
    /// </summary>
    void UpdateAllCardAreas()
    {
        foreach (CardArea area in cardSystem.GetAllCardAreas())
        {
            cardSystem.UpdateCardAreaCount(area);

            if (!bShowAttack)
            {
                UpdateCardAnimation(area);
            }

            UpdateCardTick(area);
        }
    }

    /// <summary>
    /// 更新卡牌Tick
    /// Update card tick
    /// </summary>
    void UpdateCardTick(CardArea area)
    {
        foreach (Card card in area.m_AreaList)
        {
            card.Tick();
        }
    }

    /// <summary>
    /// 更新卡牌动画状态
    /// Update card animation state
    /// </summary>
    void UpdateCardAnimation(CardArea area)
    {
        UGUISpriteAnimation.AnimState animState = UGUISpriteAnimation.AnimState.RunBack;

        if (area.m_CardAreaName.Contains("Enemy"))
        {
            animState = UGUISpriteAnimation.AnimState.RunToward;
        }

        foreach (Card card in area.m_AreaList)
        {
            if (card.m_CardType == Card.CardType.Character && !card.m_IsBattleDead)
            {
                card.animationConfig.SetAnimState(animState);
            }
        }
    }

    /// <summary>
    /// 更新预计算（显示预期伤害）
    /// Update pre-calculation (show expected damage)
    /// </summary>
    void UpdatePreCalculation()
    {
        CardArea playerBattle = cardSystem.GetCardArea(PLAYER_BATTLE_AREA);
        CardArea enemyBattle = cardSystem.GetCardArea(ENEMY_BATTLE_AREA);

        battleCalculator.ClearExpectedDamage(playerBattle, enemyBattle);

        int enemyDamage = battleCalculator.CalculateExpectedDamage(playerBattle, enemyBattle);
        int playerDamage = battleCalculator.CalculateExpectedDamage(enemyBattle, playerBattle);

        enemy.SetCurrentHurt(enemyDamage);
        player.SetCurrentHurt(playerDamage);

        // 更新反击伤害显示
        UpdateCounterDamageDisplay(playerBattle, enemyBattle);
    }

    /// <summary>
    /// 更新反击伤害显示
    /// Update counter damage display
    /// </summary>
    void UpdateCounterDamageDisplay(CardArea playerBattle, CardArea enemyBattle)
    {
        for (int i = 0; i < 9; i++)
        {
            BattleCube playerCube = playerBattle.transform.GetChild(i).GetComponent<BattleCube>();
            if (playerCube != null && playerCube.transform.childCount > 0)
            {
                Card playerCard = playerCube.transform.GetChild(0).GetComponent<Card>();
                int counterDamage = CalculateCounterDamageForCard(enemyBattle, playerCard);
                playerCard.GetHurtAPra(counterDamage);
            }
        }
    }

    /// <summary>
    /// 计算单张卡牌受到的反击伤害
    /// Calculate counter damage for a single card
    /// </summary>
    int CalculateCounterDamageForCard(CardArea enemyBattle, Card playerCard)
    {
        int damage = 0;

        for (int i = 0; i < 9; i++)
        {
            BattleCube enemyCube = enemyBattle.transform.GetChild(i).GetComponent<BattleCube>();
            if (enemyCube != null && enemyCube.m_Column == playerCard.m_BattleColumn && enemyCube.transform.childCount > 0)
            {
                Card enemyCard = enemyCube.transform.GetChild(0).GetComponent<Card>();
                damage += enemyCard.m_CurrentATK;
            }
        }

        return damage;
    }

    /// <summary>
    /// 战斗流程协程
    /// Battle sequence coroutine
    /// </summary>
    IEnumerator UpdateBattle()
    {
        bShowAttack = true;
        CardArea playerBattle = cardSystem.GetCardArea(PLAYER_BATTLE_AREA);
        CardArea enemyBattle = cardSystem.GetCardArea(ENEMY_BATTLE_AREA);

        // 逐列进行战斗
        for (int column = 0; column < 3; column++)
        {
            // 计算每一行的伤害
            for (int row = 0; row < 3; row++)
            {
                int damageToEnemy = battleCalculator.CalculateColumnRowDamage(playerBattle, enemyBattle, column, row);
                int damageToPlayer = battleCalculator.CalculateColumnRowDamage(enemyBattle, playerBattle, column, row);

                enemy.SetCurrentHP(-damageToEnemy);
                player.SetCurrentHP(-damageToPlayer);
            }

            // 清除该列的预计算伤害
            battleCalculator.ClearExpectedDamage(playerBattle, enemyBattle, column);

            // 播放攻击动画
            yield return PlayAttackAnimation(playerBattle, enemyBattle, column);

            // 播放死亡动画
            yield return PlayDeathAnimation(playerBattle, enemyBattle, column);

            // 移除死亡的卡牌
            battleCalculator.RemoveDeadCards(playerBattle, column);
            battleCalculator.RemoveDeadCards(enemyBattle, column);
        }

        // 检查胜负
        if (enemy.m_CurrentHP <= 0)
        {
            enemy.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.Dead);
            yield return new WaitForSeconds(13 / UGUISpriteAnimation.FPS);
            NextStage();
        }

        if (player.m_CurrentHP <= 0)
        {
            player.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.Dead);
            yield return new WaitForSeconds(13 / UGUISpriteAnimation.FPS);
            Dead();
        }

        bShowAttack = false;
        bDrawCard = true;
    }

    /// <summary>
    /// 播放攻击动画
    /// Play attack animation
    /// </summary>
    IEnumerator PlayAttackAnimation(CardArea playerBattle, CardArea enemyBattle, int column)
    {
        bool hasCard = false;

        foreach (Card card in playerBattle.m_AreaList)
        {
            if (card.m_BattleColumn == column)
            {
                hasCard = true;
                card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.AttackBack);
            }
        }

        foreach (Card card in enemyBattle.m_AreaList)
        {
            if (card.m_BattleColumn == column)
            {
                hasCard = true;
                card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.AttackToward);
            }
        }

        if (hasCard)
        {
            yield return new WaitForSeconds(5 / UGUISpriteAnimation.FPS);
        }
    }

    /// <summary>
    /// 播放死亡动画
    /// Play death animation
    /// </summary>
    IEnumerator PlayDeathAnimation(CardArea playerBattle, CardArea enemyBattle, int column)
    {
        bool isCardDead = false;

        foreach (Card card in playerBattle.m_AreaList)
        {
            if (card.m_BattleColumn == column)
            {
                if (card.m_IsBattleDead)
                {
                    isCardDead = true;
                    card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.Dead);
                }
                else
                {
                    card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunBack);
                }
            }
        }

        foreach (Card card in enemyBattle.m_AreaList)
        {
            if (card.m_BattleColumn == column)
            {
                if (card.m_IsBattleDead)
                {
                    isCardDead = true;
                    card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.Dead);
                }
                else
                {
                    card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);
                }
            }
        }

        if (isCardDead)
        {
            yield return new WaitForSeconds(13 / UGUISpriteAnimation.FPS);
        }
    }

    /// <summary>
    /// 游戏结束
    /// Game over
    /// </summary>
    public void Dead()
    {
        this.transform.Find("BattleEnd").gameObject.SetActive(true);
        GameEvents.GameOver();
    }

    /// <summary>
    /// 进入下一关
    /// Next stage
    /// </summary>
    void NextStage()
    {
        CardArea playerHand = cardSystem.GetCardArea(PLAYER_HAND_AREA);
        CardArea playerDeck = cardSystem.GetCardArea(PLAYER_DECK_AREA);
        CardArea playerDrop = cardSystem.GetCardArea(PLAYER_DROP_AREA);
        CardArea playerBattle = cardSystem.GetCardArea(PLAYER_BATTLE_AREA);
        CardArea enemyBattle = cardSystem.GetCardArea(ENEMY_BATTLE_AREA);

        // 洗牌
        cardSystem.Shuffle(playerHand, playerDeck);
        cardSystem.Shuffle(playerDrop, playerDeck);

        // 恢复敌人生命
        enemy.m_CurrentHP = enemy.m_HP;

        // 清空战场
        ClearBattleField(playerBattle);
        ClearBattleField(enemyBattle);

        // 升级
        Level++;
        player.SetCurrentHP(10, false);
        enemy.m_PlayerName = "野蛮人 Lv" + Level;
        this.transform.Find("Text_Stage").GetComponent<Text>().text = "Stage 1-" + Level;
        enemy.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);

        bDrawCard = true;
        GameEvents.StageComplete(Level);
    }

    /// <summary>
    /// 清空战场
    /// Clear battlefield
    /// </summary>
    void ClearBattleField(CardArea battleArea)
    {
        for (int i = 0; i < 9; i++)
        {
            BattleCube cube = battleArea.transform.GetChild(i).GetComponent<BattleCube>();
            if (cube != null && cube.transform.childCount > 0)
            {
                Card card = cube.transform.GetChild(0).GetComponent<Card>();
                battleArea.m_AreaList.Remove(card);
                Destroy(card.gameObject);
            }
        }
    }

    /// <summary>
    /// 选择战场格子（玩家放置卡牌）
    /// Select battle cube (player places card)
    /// </summary>
    public void SelectBattleCube(BattleCube cube)
    {
        if (!bPushCard || !cube.m_IsPlayer) return;

        CardArea playerBattle = cardSystem.GetCardArea(PLAYER_BATTLE_AREA);
        CardArea playerHand = cardSystem.GetCardArea(PLAYER_HAND_AREA);
        CardArea playerDrop = cardSystem.GetCardArea(PLAYER_DROP_AREA);

        // 检查格子是否已被占用（仅对角色卡检查）
        // Check if the cube is occupied (only for Character cards)
        if (cSelectCard != null && cSelectCard.m_CardType == Card.CardType.Character)
        {
            foreach (Card card in playerBattle.m_AreaList)
            {
                if (card.m_BattleColumn == cube.m_Column && card.m_BattleRow == cube.m_Row)
                    return;
            }
        }

        // 检查是否有选中的卡牌和足够的费用
        if (cSelectCard != null && player.m_CurrentCost >= cSelectCard.m_Cost)
        {
            player.m_CurrentCost -= cSelectCard.m_Cost;
            cSelectCard.m_IsSelected = false;

            cSelectCard.m_IsInBattleGround = true;
            cSelectCard.gameObject.transform.SetParent(cube.gameObject.transform);
            cSelectCard.gameObject.transform.position = cube.gameObject.transform.position;
            cSelectCard.m_BattleRow = cube.m_Row;
            cSelectCard.m_BattleColumn = cube.m_Column;

            if (cSelectCard.m_CardType == Card.CardType.Character)
            {
                playerBattle.m_AreaList.Add(cSelectCard);

                // 禁用角色卡的点击，使其可穿透
                cSelectCard.SetRaycastTarget(false);

                // 创建副本放入弃牌堆
                GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
                Card cloneCard = Instantiate(toInstantiate, this.transform.Find("Recycle")).GetComponent<Card>();
                cloneCard.InitByClone(cSelectCard);
                playerDrop.m_AreaList.Add(cloneCard);
            }
            else
            {
                // 魔法卡立即回到弃牌堆
                cSelectCard.m_IsInBattleGround = false;
                cSelectCard.gameObject.transform.SetParent(this.transform.Find("Recycle"));
                playerDrop.m_AreaList.Add(cSelectCard);
            }

            // 应用特殊效果
            specialEffectManager.ApplyCardPlacementEffects(cSelectCard, playerBattle);

            playerHand.m_AreaList.Remove(cSelectCard);
            cSelectCard = null;

            // 隐藏所有详情
            HideAllCardDetails();

            // 更新预计算
            UpdatePreCalculation();

            GameEvents.CardPlaced(cSelectCard, cube.m_Row, cube.m_Column);
        }
    }

    /// <summary>
    /// 选择手牌
    /// Select hand card
    /// </summary>
    public void SelectHandCard(Card card)
    {
        if (!bPushCard) return;

        CardArea playerHand = cardSystem.GetCardArea(PLAYER_HAND_AREA);

        // 如果点击的是已选中的手牌，取消选中
        if (cSelectCard == card && card.m_IsSelected)
        {
            card.m_IsSelected = false;
            HideAllCardDetails();
            cSelectCard = null;
            return;
        }

        // 清除场上单位的选中状态
        ClearBattleCardSelection();

        // 取消其他卡牌的选中状态
        foreach (Card handCard in playerHand.m_AreaList)
        {
            if (handCard != card)
            {
                handCard.m_IsSelected = false;
            }
        }

        card.m_IsSelected = true;
        cSelectCard = card;

        // 显示手牌详情（全局唯一）
        ShowCardDetail(card);

        GameEvents.CardSelected(card);
    }

    /// <summary>
    /// 选择场上的卡牌（支持查看敌人）
    /// Select battle card (supports viewing enemy cards)
    /// </summary>
    public void SelectBattleCard(Card card, BattleCube cube)
    {
        if (!bPushCard) return;

        // 如果点击的是已经选中的单位，取消选中
        if (cSelectedBattleCard == card)
        {
            ClearBattleCardSelection();
            HideAllCardDetails();
            return;
        }

        // 清除之前选中的场上单位
        ClearBattleCardSelection();

        // 清除手牌的选中状态
        CardArea playerHand = cardSystem.GetCardArea(PLAYER_HAND_AREA);
        if (playerHand != null)
        {
            foreach (Card handCard in playerHand.m_AreaList)
            {
                handCard.m_IsSelected = false;
            }
        }
        cSelectCard = null;

        // 选中新的场上单位
        cSelectedBattleCard = card;
        cSelectedBattleCube = cube;
        cube.SetSelected(true);

        // 显示场上单位的详情（全局唯一）
        ShowCardDetail(card);

        GameEvents.CardSelected(card);
    }

    /// <summary>
    /// 清除场上单位的选中状态
    /// Clear battle card selection
    /// </summary>
    private void ClearBattleCardSelection()
    {
        if (cSelectedBattleCard != null)
        {
            cSelectedBattleCard.HideDetail();
            cSelectedBattleCard = null;
        }

        if (cSelectedBattleCube != null)
        {
            cSelectedBattleCube.SetSelected(false);
            cSelectedBattleCube = null;
        }
    }

    /// <summary>
    /// 显示卡牌详情（全局唯一，自动隐藏之前的）
    /// Show card detail (globally unique, auto-hide previous)
    /// </summary>
    public void ShowCardDetail(Card card)
    {
        if (card == null) return;

        // 如果之前有显示详情的卡牌，先隐藏
        if (cCurrentDetailCard != null && cCurrentDetailCard != card)
        {
            cCurrentDetailCard.HideDetail();
        }

        // 显示新的详情
        cCurrentDetailCard = card;
        card.ShowDetail();
    }

    /// <summary>
    /// 隐藏所有卡牌详情
    /// Hide all card details
    /// </summary>
    public void HideAllCardDetails()
    {
        if (cCurrentDetailCard != null)
        {
            cCurrentDetailCard.HideDetail();
            cCurrentDetailCard = null;
        }
    }

    /// <summary>
    /// 弃牌按钮点击
    /// Drop button click
    /// </summary>
    public void OnDropButtonClick()
    {
        if (bPushCard)
        {
            // 清除所有选中状态
            ClearBattleCardSelection();

            CardArea playerHand = cardSystem.GetCardArea(PLAYER_HAND_AREA);
            if (playerHand != null)
            {
                foreach (Card handCard in playerHand.m_AreaList)
                {
                    handCard.m_IsSelected = false;
                }
            }
            cSelectCard = null;

            // 隐藏所有详情
            HideAllCardDetails();

            player.m_CurrentCost = player.m_Cost;
            bDropCard = true;
        }
    }
}
