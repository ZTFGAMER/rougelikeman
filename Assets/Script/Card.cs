using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

  public enum CardType
  {
    Character,
    Magic,
    Trap,
  };
  public enum CardSpace
  {
    PlayerDeck,
    PlayerHand,
    PlayerDrop,
    PlayerGround,
    EnemyGround,
    EnemyPre,
  };
  public enum HurtEffect
  {
    Normal,//常规
    Penetrate,//贯穿，对所有排上的敌人造成伤害
    Backstab,//从最后的敌人开始计算伤害
    Puncture,//无视护盾造成伤害
  };

  public string m_CardName;
  public int m_HP;
  public int m_Cost;
  public int m_ATK;
  public CardType m_CardType;
  public CardSpace m_CardSpace;
  public HurtEffect m_HurtEffect;
  public bool m_IsAlive;
  public int m_BattleRow;
  public int m_BattleColumn;
  public int m_CurrentHP;
  public int m_CurrentCost;
  public int m_CurrentATK;
  public int m_CurrentArmor;
  public int m_CurrentOrder;

  // Use this for initialization
  void Start () {
		
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
    m_BattleColumn = column;
    m_BattleRow = row;
  }

  public bool GetPlace(int row, int column)
  {
    return row == m_BattleRow && column == m_BattleColumn;
  }

  // Update is called once per frame
  void Update () {
		
	}
}
