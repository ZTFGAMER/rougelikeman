using UnityEngine;

/// <summary>
/// 战斗计算器 - 处理战斗中的伤害计算逻辑
/// Battle Calculator - Handles damage calculation logic in battles
/// </summary>
public class BattleCalculator
{
    private const int GRID_ROWS = 3;
    private const int GRID_COLUMNS = 3;

    /// <summary>
    /// 计算某一列某一行的攻击伤害
    /// Calculate attack damage for a specific column and row
    /// </summary>
    /// <param name="attackArea">攻击方区域</param>
    /// <param name="defendArea">防御方区域</param>
    /// <param name="column">当前列</param>
    /// <param name="row">当前行</param>
    /// <returns>穿透到玩家/敌人的伤害</returns>
    public int CalculateColumnRowDamage(CardArea attackArea, CardArea defendArea, int column, int row)
    {
        int totalAttack = 0;
        Card.HurtEffect effectType = Card.HurtEffect.Normal;

        // 收集攻击方该列该行的所有攻击力
        for (int i = 0; i < GRID_ROWS * GRID_COLUMNS; i++)
        {
            BattleCube cube = attackArea.transform.GetChild(i).GetComponent<BattleCube>();
            if (cube != null && cube.m_Column == column && cube.m_Row == row && cube.transform.childCount > 0)
            {
                Card attackCard = cube.transform.GetChild(0).GetComponent<Card>();
                totalAttack += attackCard.m_CurrentATK;

                // 使用优先级最高的伤害效果
                if (attackCard.m_HurtEffect > effectType)
                {
                    effectType = attackCard.m_HurtEffect;
                }
            }
        }

        // 根据伤害效果类型分配伤害
        return DistributeDamage(defendArea, totalAttack, effectType, column);
    }

    /// <summary>
    /// 分配伤害给防御方
    /// Distribute damage to defenders
    /// </summary>
    private int DistributeDamage(CardArea defendArea, int damage, Card.HurtEffect effectType, int column)
    {
        if (damage <= 0) return 0;

        switch (effectType)
        {
            case Card.HurtEffect.Backstab:
                // 背刺：从后往前攻击
                return DistributeBackstabDamage(defendArea, damage, column);

            case Card.HurtEffect.Penetrate:
                // 贯穿：对该列所有单位造成伤害，不穿透到玩家
                return DistributePenetrateDamage(defendArea, damage, column);

            case Card.HurtEffect.Puncture:
                // 穿刺：无视护盾（当前版本未实现）
                return DistributeNormalDamage(defendArea, damage, column, true);

            case Card.HurtEffect.Normal:
            default:
                // 普通：从前往后攻击
                return DistributeNormalDamage(defendArea, damage, column, false);
        }
    }

    /// <summary>
    /// 普通伤害分配：从前往后
    /// Normal damage: front to back
    /// </summary>
    private int DistributeNormalDamage(CardArea defendArea, int damage, int column, bool isPuncture)
    {
        for (int i = 0; i < GRID_ROWS * GRID_COLUMNS; i++)
        {
            BattleCube cube = defendArea.transform.GetChild(i).GetComponent<BattleCube>();
            if (cube != null && cube.m_Column == column && cube.transform.childCount > 0)
            {
                Card defendCard = cube.transform.GetChild(0).GetComponent<Card>();
                if (isPuncture)
                {
                    damage = defendCard.GetHurtByPuncture(damage);
                }
                else
                {
                    damage = defendCard.GetHurt(damage);
                }

                if (damage <= 0) break;
            }
        }
        return damage;
    }

    /// <summary>
    /// 背刺伤害分配：从后往前
    /// Backstab damage: back to front
    /// </summary>
    private int DistributeBackstabDamage(CardArea defendArea, int damage, int column)
    {
        for (int i = GRID_ROWS * GRID_COLUMNS - 1; i >= 0; i--)
        {
            BattleCube cube = defendArea.transform.GetChild(i).GetComponent<BattleCube>();
            if (cube != null && cube.m_Column == column && cube.transform.childCount > 0)
            {
                Card defendCard = cube.transform.GetChild(0).GetComponent<Card>();
                damage = defendCard.GetHurt(damage);

                if (damage <= 0) break;
            }
        }
        return damage;
    }

    /// <summary>
    /// 贯穿伤害分配：对所有单位造成伤害
    /// Penetrate damage: damage all units in column
    /// </summary>
    private int DistributePenetrateDamage(CardArea defendArea, int damage, int column)
    {
        for (int i = 0; i < GRID_ROWS * GRID_COLUMNS; i++)
        {
            BattleCube cube = defendArea.transform.GetChild(i).GetComponent<BattleCube>();
            if (cube != null && cube.m_Column == column && cube.transform.childCount > 0)
            {
                Card defendCard = cube.transform.GetChild(0).GetComponent<Card>();
                defendCard.GetHurt(damage);
            }
        }
        // 贯穿不对玩家造成伤害
        return 0;
    }

    /// <summary>
    /// 预计算战斗伤害（用于显示预期伤害）
    /// Pre-calculate battle damage (for displaying expected damage)
    /// </summary>
    public int CalculateExpectedDamage(CardArea attackArea, CardArea defendArea)
    {
        int totalDamage = 0;

        for (int i = 0; i < GRID_ROWS * GRID_COLUMNS; i++)
        {
            BattleCube attackCube = attackArea.transform.GetChild(i).GetComponent<BattleCube>();
            if (attackCube != null && attackCube.transform.childCount > 0)
            {
                Card attackCard = attackCube.transform.GetChild(0).GetComponent<Card>();
                int damage = attackCard.m_CurrentATK;
                Card.HurtEffect effectType = attackCard.m_HurtEffect;

                // 根据效果类型计算预期伤害
                damage = CalculateExpectedDamageForCard(defendArea, damage, effectType, attackCube.m_Column);
                totalDamage += damage;
            }
        }

        return totalDamage;
    }

    /// <summary>
    /// 计算单张卡牌的预期伤害
    /// Calculate expected damage for a single card
    /// </summary>
    private int CalculateExpectedDamageForCard(CardArea defendArea, int damage, Card.HurtEffect effectType, int column)
    {
        if (effectType == Card.HurtEffect.Backstab)
        {
            // 背刺：从后往前预计算
            for (int j = GRID_ROWS * GRID_COLUMNS - 1; j >= 0; j--)
            {
                BattleCube defendCube = defendArea.transform.GetChild(j).GetComponent<BattleCube>();
                if (defendCube != null && defendCube.m_Column == column && defendCube.transform.childCount > 0)
                {
                    Card defendCard = defendCube.transform.GetChild(0).GetComponent<Card>();
                    damage = defendCard.GetHurtPre(damage);
                    if (damage <= 0) break;
                }
            }
        }
        else
        {
            // 普通/穿刺/贯穿：从前往后预计算
            for (int j = 0; j < GRID_ROWS * GRID_COLUMNS; j++)
            {
                BattleCube defendCube = defendArea.transform.GetChild(j).GetComponent<BattleCube>();
                if (defendCube != null && defendCube.m_Column == column && defendCube.transform.childCount > 0)
                {
                    Card defendCard = defendCube.transform.GetChild(0).GetComponent<Card>();

                    if (effectType == Card.HurtEffect.Normal)
                    {
                        damage = defendCard.GetHurtPre(damage);
                    }
                    else if (effectType == Card.HurtEffect.Penetrate)
                    {
                        defendCard.GetHurtPre(damage);
                    }

                    if (damage <= 0 && effectType != Card.HurtEffect.Penetrate) break;
                }
            }

            if (effectType == Card.HurtEffect.Penetrate)
            {
                damage = 0;
            }
        }

        return damage;
    }

    /// <summary>
    /// 清除预计算的伤害显示
    /// Clear pre-calculated damage display
    /// </summary>
    public void ClearExpectedDamage(CardArea area1, CardArea area2, int specificColumn = -1)
    {
        ClearAreaExpectedDamage(area1, specificColumn);
        ClearAreaExpectedDamage(area2, specificColumn);
    }

    private void ClearAreaExpectedDamage(CardArea area, int specificColumn)
    {
        foreach (Card card in area.m_AreaList)
        {
            if (specificColumn < 0 || card.m_BattleColumn == specificColumn)
            {
                card.m_CurrentHurt = 0;
                card.m_CurrentHurtA = 0;
            }
        }
    }

    /// <summary>
    /// 清除死亡的卡牌
    /// Remove dead cards from battlefield
    /// </summary>
    public void RemoveDeadCards(CardArea area, int column)
    {
        for (int i = 0; i < GRID_ROWS; i++)
        {
            BattleCube cube = area.transform.GetChild(column + i * GRID_COLUMNS).GetComponent<BattleCube>();
            if (cube.transform.childCount > 0)
            {
                Card card = cube.transform.GetChild(0).GetComponent<Card>();
                if (card.m_IsBattleDead)
                {
                    area.m_AreaList.Remove(card);
                    Object.Destroy(card.gameObject);
                    GameEvents.CardDeath(card);
                }
            }
        }
    }
}
