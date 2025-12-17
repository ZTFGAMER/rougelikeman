using System;
using UnityEngine;

/// <summary>
/// 游戏事件系统 - 用于解耦各个系统之间的依赖
/// Game Events System - Decouples dependencies between systems
/// </summary>
public static class GameEvents
{
    // 卡牌相关事件 / Card Events
    public static event Action<Card> OnCardSelected;
    public static event Action<Card, int, int> OnCardPlaced;
    public static event Action<Card> OnCardDrawn;
    public static event Action<Card> OnCardDiscarded;
    public static event Action<Card> OnCardDeath;

    // 战斗相关事件 / Battle Events
    public static event Action OnBattleStart;
    public static event Action OnBattleEnd;
    public static event Action<int> OnColumnAttack;
    public static event Action<Player, int> OnPlayerDamaged;
    public static event Action<Player> OnPlayerDeath;

    // 回合相关事件 / Turn Events
    public static event Action OnTurnStart;
    public static event Action OnTurnEnd;
    public static event Action OnDrawPhaseStart;
    public static event Action OnPlacePhaseStart;

    // 关卡相关事件 / Stage Events
    public static event Action<int> OnStageComplete;
    public static event Action OnGameOver;

    // 触发事件的方法 / Methods to trigger events
    public static void CardSelected(Card card) => OnCardSelected?.Invoke(card);
    public static void CardPlaced(Card card, int row, int column) => OnCardPlaced?.Invoke(card, row, column);
    public static void CardDrawn(Card card) => OnCardDrawn?.Invoke(card);
    public static void CardDiscarded(Card card) => OnCardDiscarded?.Invoke(card);
    public static void CardDeath(Card card) => OnCardDeath?.Invoke(card);

    public static void BattleStart() => OnBattleStart?.Invoke();
    public static void BattleEnd() => OnBattleEnd?.Invoke();
    public static void ColumnAttack(int column) => OnColumnAttack?.Invoke(column);
    public static void PlayerDamaged(Player player, int damage) => OnPlayerDamaged?.Invoke(player, damage);
    public static void PlayerDeath(Player player) => OnPlayerDeath?.Invoke(player);

    public static void TurnStart() => OnTurnStart?.Invoke();
    public static void TurnEnd() => OnTurnEnd?.Invoke();
    public static void DrawPhaseStart() => OnDrawPhaseStart?.Invoke();
    public static void PlacePhaseStart() => OnPlacePhaseStart?.Invoke();

    public static void StageComplete(int level) => OnStageComplete?.Invoke(level);
    public static void GameOver() => OnGameOver?.Invoke();

    /// <summary>
    /// 清除所有事件订阅 - 用于场景切换时避免内存泄漏
    /// Clear all event subscriptions - Used when switching scenes to avoid memory leaks
    /// </summary>
    public static void ClearAll()
    {
        OnCardSelected = null;
        OnCardPlaced = null;
        OnCardDrawn = null;
        OnCardDiscarded = null;
        OnCardDeath = null;
        OnBattleStart = null;
        OnBattleEnd = null;
        OnColumnAttack = null;
        OnPlayerDamaged = null;
        OnPlayerDeath = null;
        OnTurnStart = null;
        OnTurnEnd = null;
        OnDrawPhaseStart = null;
        OnPlacePhaseStart = null;
        OnStageComplete = null;
        OnGameOver = null;
    }
}
