using UnityEngine;

/// <summary>
/// 回合管理器 - 管理游戏回合流程
/// Turn Manager - Manages game turn flow
/// </summary>
public class TurnManager
{
    /// <summary>
    /// 回合阶段枚举
    /// Turn phase enumeration
    /// </summary>
    public enum TurnPhase
    {
        DrawPhase,      // 抽牌阶段
        PlacePhase,     // 放置卡牌阶段
        BattlePhase,    // 战斗阶段
        EndPhase        // 结束阶段
    }

    private TurnPhase currentPhase;
    private bool isPlayerTurn;

    public TurnPhase CurrentPhase => currentPhase;
    public bool IsPlayerTurn => isPlayerTurn;

    public TurnManager()
    {
        currentPhase = TurnPhase.DrawPhase;
        isPlayerTurn = true;
    }

    /// <summary>
    /// 开始新回合
    /// Start new turn
    /// </summary>
    public void StartNewTurn()
    {
        currentPhase = TurnPhase.DrawPhase;
        GameEvents.TurnStart();
        GameEvents.DrawPhaseStart();
    }

    /// <summary>
    /// 切换到放置阶段
    /// Switch to place phase
    /// </summary>
    public void EnterPlacePhase()
    {
        currentPhase = TurnPhase.PlacePhase;
        GameEvents.PlacePhaseStart();
    }

    /// <summary>
    /// 切换到战斗阶段
    /// Switch to battle phase
    /// </summary>
    public void EnterBattlePhase()
    {
        currentPhase = TurnPhase.BattlePhase;
        GameEvents.BattleStart();
    }

    /// <summary>
    /// 切换到结束阶段
    /// Switch to end phase
    /// </summary>
    public void EnterEndPhase()
    {
        currentPhase = TurnPhase.EndPhase;
        GameEvents.BattleEnd();
    }

    /// <summary>
    /// 结束当前回合
    /// End current turn
    /// </summary>
    public void EndTurn()
    {
        GameEvents.TurnEnd();
        isPlayerTurn = !isPlayerTurn;
    }

    /// <summary>
    /// 检查是否可以进入下一阶段
    /// Check if can proceed to next phase
    /// </summary>
    public bool CanProceedToNextPhase()
    {
        switch (currentPhase)
        {
            case TurnPhase.DrawPhase:
                return true; // 抽牌完成后可以进入放置阶段
            case TurnPhase.PlacePhase:
                return true; // 玩家确认后可以进入战斗阶段
            case TurnPhase.BattlePhase:
                return true; // 战斗结束后进入结束阶段
            case TurnPhase.EndPhase:
                return true; // 结束阶段后开始新回合
            default:
                return false;
        }
    }

    /// <summary>
    /// 重置回合管理器
    /// Reset turn manager
    /// </summary>
    public void Reset()
    {
        currentPhase = TurnPhase.DrawPhase;
        isPlayerTurn = true;
    }
}
