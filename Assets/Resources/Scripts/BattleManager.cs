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

        // 初始化玩家和敌人
        InitPlayer(player, "圣骑士", 35, 3, true, "曹操(骑马)");
        InitPlayer(enemy, "野蛮人", 30, 3, false, "陆逊");
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
    /// 初始化玩家卡组
    /// Initialize player deck
    /// </summary>
    void InitializePlayerDeck()
    {
        CardArea dropArea = cardSystem.GetCardArea(PLAYER_DROP_AREA);

        // 添加士兵卡
        for (int i = 0; i < 3; i++)
        {
            dropArea.InitCard(false, "士兵", "近卫兵", 1, 3, 3);
        }

        // 添加弓箭手卡
        for (int i = 0; i < 3; i++)
        {
            dropArea.InitCard(false, "弓箭手", "黄忠(骑马)", 1, 1, 4, Card.CardType.Character, Card.HurtEffect.Backstab);
        }

        // 添加盾手卡
        for (int i = 0; i < 3; i++)
        {
            dropArea.InitCard(false, "盾手", "曹仁", 1, 5, 1);
        }

        // 添加冲击手卡
        dropArea.InitCard(false, "冲击手", "徐晃", 2, 3, 3, Card.CardType.Character, Card.HurtEffect.Penetrate);

        // 添加指挥官卡
        dropArea.InitCard(false, "指挥官", "夏侯敦(骑马)", 2, 2, 2);

        // 添加魔法卡
        dropArea.InitCard(false, "冲锋", "magiccross", 1, 0, 0, Card.CardType.Magic);
        dropArea.InitCard(false, "爆发", "magiccolumn", 0, 0, 0, Card.CardType.Magic);
        dropArea.InitCard(false, "坚守", "magicrow", 1, 0, 0, Card.CardType.Magic);
        dropArea.InitCard(false, "巨盾", "magicall", 2, 0, 0, Card.CardType.Magic);
    }

    /// <summary>
    /// 初始化敌人卡组
    /// Initialize enemy deck
    /// </summary>
    void InitializeEnemyDeck()
    {
        CardArea deckArea = cardSystem.GetCardArea(ENEMY_DECK_AREA);

        // 添加蛮族勇士卡
        for (int i = 0; i < 3; i++)
        {
            deckArea.InitCard(true, "蛮族勇士", "黄盖", 1, 2, 2);
        }

        // 添加蛮族刺客卡
        for (int i = 0; i < 2; i++)
        {
            deckArea.InitCard(true, "蛮族刺客", "甘宁", 1, 1, 3, Card.CardType.Character, Card.HurtEffect.Backstab);
        }

        // 添加蛮族巫师卡
        deckArea.InitCard(true, "蛮族巫师", "谋士", 1, 1, 2, Card.CardType.Character, Card.HurtEffect.Penetrate);
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

        // 检查格子是否已被占用
        foreach (Card card in playerBattle.m_AreaList)
        {
            if (card.m_BattleColumn == cube.m_Column && card.m_BattleRow == cube.m_Row)
                return;
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

        GameEvents.CardSelected(card);
    }

    /// <summary>
    /// 弃牌按钮点击
    /// Drop button click
    /// </summary>
    public void OnDropButtonClick()
    {
        if (bPushCard)
        {
            player.m_CurrentCost = player.m_Cost;
            bDropCard = true;
        }
    }
}
