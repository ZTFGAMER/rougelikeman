using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
  public Text m_TextHP;
  public Text m_TextATK;
  public Text m_TextCost;
  public Text m_TextCardName;

  // Use this for initialization
  void Start () {
		
	}

  public void InitByClone(Card clonecard)
  {
    m_CardName = clonecard.m_CardName;
    m_HP=clonecard.m_HP ;
    m_ATK=clonecard.m_ATK ;
    m_Cost=clonecard.m_Cost ;
    m_CardType=clonecard.m_CardType ;
    m_HurtEffect=clonecard.m_HurtEffect ;
  }

  public void PrepareForBattle()
  {
    m_CurrentHP = m_HP;
    m_CurrentCost = m_Cost;
    m_CurrentATK = m_ATK;
    m_IsAlive = false;
    m_TextCardName.text = m_CardName;
    m_TextCost.text = m_CurrentCost.ToString();
    if (m_CardType == CardType.Character)
    {
      m_TextATK.text = m_CurrentATK.ToString();
      m_TextHP.text = m_CurrentHP.ToString();
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
