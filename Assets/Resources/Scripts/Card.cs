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
  public int m_HP = 0;
  public int m_Cost;
  public int m_ATK = 0;
  public int m_Armor;
  public CardType m_CardType;
  public CardSpace m_CardSpace;
  public HurtEffect m_HurtEffect;

  public bool m_IsSelected;
  public bool m_IsInBattleGround;
  public int m_BattleRow;
  public int m_BattleColumn;
  public bool m_IsBattleDead;
  public float m_DeadAnimTime = 0f;

  public int m_CurrentHP;
  public int m_CurrentCost;
  public int m_CurrentATK;
  public int m_CurrentArmor;
  public int m_CurrentOrder;
  public int m_CurrentHurt = 0;
  public Text m_TextHP;
  public Text m_TextATK;
  public Text m_TextCost;
  public Text m_TextCardName;
  public GameObject m_HPLine;
  public GameObject m_ATKLine;
  public GameObject m_ObjectCardSelect;

  public UGUISpriteAnimation animationConfig;

  public BattleManager battleManager;

  // Use this for initialization
  void Start() {

  }
  public void InitByClone(Card clonecard, BattleManager battlem)
  {
    m_CardName = clonecard.m_CardName;
    ChangeHP(clonecard.m_HP);
    ChangeATK(clonecard.m_ATK);
    m_Cost = clonecard.m_Cost;
    m_CurrentHurt = 0;
    m_CardType = clonecard.m_CardType;
    m_HurtEffect = clonecard.m_HurtEffect;
    InitAnimation(clonecard.animationConfig.m_SpiteName);
    this.battleManager = battlem;
    PrepareForBattle();
  }
  public void ChangeHP(int delta)
  {
    m_HP = Mathf.Max(0,m_HP + delta);
    if (delta != 0)
    {
      ChangeCardLine(m_HPLine, m_HP, delta);
    }
  }
  public void ChangeATK(int delta)
  {
    m_ATK = Mathf.Max(0, m_ATK + delta);
    if (delta != 0)
    {
      ChangeCardLine(m_ATKLine, m_ATK, delta);
    }
  }
  void ChangeCardLine(GameObject line, int total, int delta)
  {
    if (delta == 0)
    {
      return;
    }
    else if (delta > 0)
    {
      for (int i = 0; i < delta; i++)
      {
        GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HPLine");
        Instantiate(toInstantiate, line.transform.Find("TotalContent"));
      }
    }
    else if(delta <= total)
    {
      for (int i = 0; i > delta; i--)
      {
        Destroy(line.transform.Find("TotalContent").GetChild(0).gameObject);
      }
    }
  }
  public void InitAnimation(string animname)
  {
    this.animationConfig = this.transform.Find("HandCardAnim").GetComponent<UGUISpriteAnimation>();
    this.animationConfig.m_SpiteName = animname;
    this.animationConfig.InitFrame(animname);
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
    if (!m_IsInBattleGround)
      battleManager.SelectHandCard(this);
  }
  public void SetPlace(int row, int column)
  {
    m_BattleColumn = column;
    m_BattleRow = row;
  }
  public bool GetPlace(int row, int column)
  {
    return row == m_BattleRow && column == m_BattleColumn;
  }
  public int GetHurtPre(int hurt)
  {
    if (hurt >= m_CurrentHP)
    {
      hurt -= m_CurrentHP;
      m_CurrentHurt += m_CurrentHP;
      return hurt;
    }
    else
    {
      m_CurrentHurt += hurt;
      return 0;
    }
  }
  public int GetHurt(int hurt)
  {
    if (hurt >= m_CurrentHP)
    {
      m_IsBattleDead = true;
      hurt -= m_CurrentHP;
      m_CurrentHP = 0;
      return hurt;
    }
    else
    {
      m_CurrentHP -= hurt;
      return 0;
    }
  }
  public int GetHurtByPuncture(int hurt)
  {
    if (hurt >= m_CurrentHP)
    {
      m_IsBattleDead = true;
      hurt -= m_CurrentHP;
      m_CurrentHP = 0;
      return hurt;
    }
    else
    {
      m_CurrentHP -= hurt;
      return 0;
    }
  }
  public void Tick() {
    ShowCard();
    if (m_IsInBattleGround)
    { 
      UpdateCardLine(m_HPLine, m_HP, m_CurrentHP,m_CurrentHurt,Color.red,Color.green,Color.yellow);
      //UpdateCardLine(m_ATKLine, m_ATK, m_CurrentATK,Color.red,Color.red);
    }
  }
  void UpdateCardLine(GameObject line, int total, int delta,int hurt,Color totalc,Color deltac,Color hurtc)
  {
    if (total > 0)
    {
      line.transform.Find("TotalContent").GetComponent<GridLayoutGroup>().cellSize = new Vector2(20f, 150f / total);
      for (int i = 0; i < total; i++)
      {

        if (i < delta - hurt)
        {
          line.transform.Find("TotalContent").GetChild(total - i - 1).GetComponent<Image>().color = deltac;
        }
        else if (i < delta)
        {
          line.transform.Find("TotalContent").GetChild(total - i - 1).GetComponent<Image>().color = hurtc;
        }
        else
        {
          line.transform.Find("TotalContent").GetChild(total - i - 1).GetComponent<Image>().color = totalc;
        }
      }
    }
  }
  void ShowCard()
  {
    m_TextCost.text = m_CurrentCost.ToString();
    m_TextCardName.text = m_CardName;
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_Cost"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_Cost").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_Name"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_Name").gameObject.SetActive(!m_IsInBattleGround);
    }
    //if (this.gameObject.transform.Find("HandCardAnim/HandCard_ATK"))
    //{
    //  this.gameObject.transform.Find("HandCardAnim/HandCard_ATK").gameObject.SetActive(!m_IsInBattleGround);
    //}
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_HP"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_HP").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (m_HPLine != null)
    {
      m_HPLine.SetActive(m_IsInBattleGround);
    }
    if (m_ATKLine != null)
    {
      //m_ATKLine.SetActive(m_IsInBattleGround);
      m_ATKLine.SetActive(false);
    }
    if (m_CardType == CardType.Character)
    {
      m_TextATK.text = m_ATK.ToString();
      m_TextHP.text = m_HP.ToString();
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
