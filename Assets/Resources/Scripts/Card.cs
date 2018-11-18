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
    Normal = 1,//常规
    Backstab = 2,//从最后的敌人开始计算伤害
    Puncture = 4,//无视护盾造成伤害
    Penetrate = 8,//贯穿，对所有排上的敌人造成伤害
  };

  public string m_CardName;
  public int m_HP;
  public int m_Cost;
  public int m_ATK;
  public int m_Armor;
  public CardType m_CardType;
  public CardSpace m_CardSpace;
  public HurtEffect m_HurtEffect;

  public bool m_IsSelected;
  public bool m_IsInBattleGround;
  public int m_BattleRow;
  public int m_BattleColumn;
  public bool m_IsBattleDead;

  public int m_CurrentHP;
  public int m_CurrentCost;
  public int m_CurrentATK;
  public int m_CurrentArmor;
  public int m_CurrentOrder;
  public Text m_TextHP;
  public Text m_TextATK;
  public Text m_TextCost;
  public Text m_TextCardName;
  public GameObject m_ObjectCardSelect;

  public BattleManager battleManager;

  // Use this for initialization
  void Start () {
		
	}

  public void InitByClone(Card clonecard,BattleManager battlem)
  {
    m_CardName = clonecard.m_CardName;
    m_HP=clonecard.m_HP ;
    m_ATK=clonecard.m_ATK ;
    m_Cost=clonecard.m_Cost ;
    m_CardType=clonecard.m_CardType ;
    m_HurtEffect=clonecard.m_HurtEffect ;
    this.battleManager = battlem;
    PrepareForBattle();
  }

  public void PrepareForBattle()
  {
    m_CurrentHP = m_HP;
    m_CurrentCost = m_Cost;
    m_CurrentATK = m_ATK;
    m_IsSelected = false;
    m_IsInBattleGround = false;
    m_IsBattleDead = false;
  }

  public void ReadyToBattle()
  {
    if(!m_IsInBattleGround)
      battleManager.SelectHandCard(this);
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

  public int GetHurt(int hurt)
  {
    if (hurt >= m_CurrentArmor + m_CurrentHP)
    {
      m_IsBattleDead = true;
      return hurt - m_CurrentArmor - m_CurrentHP;
    }
    else
    {
      m_CurrentHP -= Mathf.Max(0, hurt - m_CurrentArmor);
      return 0;
    }
  }
  public int GetHurtByPuncture(int hurt)
  {
    if (hurt >= m_CurrentHP)
    {
      m_IsBattleDead = true;
      return hurt - m_CurrentHP;
    }
    else
    {
      m_CurrentHP -=  hurt;
      return 0;
    }
  }

  // Update is called once per frame
  void Update () {
      ShowCard();
	}

  void ShowCard()
  {
    m_TextCost.text = m_CurrentCost.ToString();
    m_TextCardName.text = m_CardName;
    if (m_CardType == CardType.Character)
    {
      m_TextATK.text = m_CurrentATK.ToString();
      m_TextHP.text = m_CurrentHP.ToString();
    }
    else
    {
      m_TextATK.transform.parent.gameObject.SetActive(false);
      m_TextHP.transform.parent.gameObject.SetActive(false);
    }
    if (m_ObjectCardSelect != null)
    {
      m_ObjectCardSelect.SetActive(m_IsSelected);
    }
  }
}
