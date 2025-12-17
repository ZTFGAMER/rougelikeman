using UnityEngine;

/// <summary>
/// AI控制器 - 管理敌人的AI行为
/// AI Controller - Manages enemy AI behavior
/// </summary>
public class AIController
{
    private CardArea enemyBattleArea;
    private CardArea enemyDeckArea;
    private int gridSize = 9;

    public AIController(CardArea enemyBattleArea, CardArea enemyDeckArea)
    {
        this.enemyBattleArea = enemyBattleArea;
        this.enemyDeckArea = enemyDeckArea;
    }

    /// <summary>
    /// 执行AI回合 - 在战场上放置卡牌
    /// Execute AI turn - Place cards on battlefield
    /// </summary>
    /// <param name="level">当前关卡等级，决定放置卡牌数量</param>
    public void ExecuteAITurn(int level)
    {
        if (enemyDeckArea.m_AreaList.Count == 0)
        {
            Debug.LogWarning("Enemy deck is empty, cannot place cards");
            return;
        }

        int cardsToPlace = Mathf.Min(level, gridSize);

        for (int i = 0; i < cardsToPlace; i++)
        {
            PlaceRandomCard();
        }
    }

    /// <summary>
    /// 随机放置一张卡牌到空位
    /// Place a random card to an empty slot
    /// </summary>
    private void PlaceRandomCard()
    {
        // 尝试多次寻找空位
        int attempts = 0;
        int maxAttempts = gridSize * 2;

        while (attempts < maxAttempts)
        {
            int randomIndex = Random.Range(0, gridSize);
            BattleCube cube = enemyBattleArea.transform.GetChild(randomIndex).GetComponent<BattleCube>();

            if (cube != null && cube.transform.childCount == 0)
            {
                // 找到空位，放置卡牌
                PlaceCardAtCube(cube);
                return;
            }

            attempts++;
        }

        Debug.LogWarning("No empty slots available for AI to place card");
    }

    /// <summary>
    /// 在指定格子放置卡牌
    /// Place card at specified cube
    /// </summary>
    private void PlaceCardAtCube(BattleCube cube)
    {
        if (enemyDeckArea.m_AreaList.Count == 0) return;

        // 从牌库随机选择一张卡
        int randomCardIndex = Random.Range(0, enemyDeckArea.m_AreaList.Count);
        Card templateCard = enemyDeckArea.m_AreaList[randomCardIndex];

        // 创建卡牌实例
        GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
        Card newCard = Object.Instantiate(toInstantiate, cube.transform).GetComponent<Card>();

        // 初始化卡牌
        newCard.gameObject.transform.position = cube.gameObject.transform.position;
        newCard.InitByClone(templateCard);
        newCard.m_IsInBattleGround = true;
        newCard.m_BattleRow = cube.m_Row;
        newCard.m_BattleColumn = cube.m_Column;

        // 禁用卡牌的点击，使其可穿透
        newCard.SetRaycastTarget(false);

        // 添加到战场区域
        enemyBattleArea.m_AreaList.Add(newCard);

        GameEvents.CardPlaced(newCard, cube.m_Row, cube.m_Column);
    }

    /// <summary>
    /// 更新战场区域引用
    /// Update battlefield area reference
    /// </summary>
    public void SetBattleArea(CardArea battleArea)
    {
        this.enemyBattleArea = battleArea;
    }

    /// <summary>
    /// 更新牌库区域引用
    /// Update deck area reference
    /// </summary>
    public void SetDeckArea(CardArea deckArea)
    {
        this.enemyDeckArea = deckArea;
    }
}
