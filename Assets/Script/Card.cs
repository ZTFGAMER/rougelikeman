using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

  public enum CardType
  {
    Warrior,
    Archor,
    Berserker,
    FootMan
  };

  public int m_HP;
  public int m_Cost;
  public int m_ATK;
  public bool m_IsAlive;
  public bool m_IsOnBattleGround;
  public int m_BattleRow;
  public int m_BattleColumn;

  // Use this for initialization
  void Start () {
		
	}

  public void InitType(CardType type)
  {
    switch (type)
    {
      case CardType.Warrior:
        m_Cost = 1;
        m_HP = 3;
        m_ATK = 3;
        break;
      case CardType.Archor:
        m_Cost = 1;
        m_HP = 1;
        m_ATK = 5;
        break;
      case CardType.Berserker:
        m_Cost = 2;
        m_HP = 2;
        m_ATK = 4;
        break;
      case CardType.FootMan:
        m_Cost = 1;
        m_HP = 6;
        m_ATK = 0;
        break;
      default:
        break;
    }
  }

  public void GetHurt(int damage)
  {
    if (m_HP <= damage)
    {
      m_IsAlive = false;
      m_HP = 0;
    }
    else
    {
      m_HP -= damage;
    }
  }

  public void SetPlace(int row,int column)
  {
    m_IsOnBattleGround = true;
    m_BattleColumn = column;
    m_BattleRow = row;
  }

  public bool GetPlace(int row, int column)
  {
    return row == m_BattleRow && column == m_BattleColumn && m_IsOnBattleGround;
  }

  // Update is called once per frame
  void Update () {
		
	}
}
